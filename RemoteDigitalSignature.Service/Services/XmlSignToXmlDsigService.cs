using RemoteDigitalSignature.Service.Abstractions;
using System.Text;
using static DigitalSigning.Av.AvNative;
using System.Xml;
using System.Security.Cryptography.Xml;

namespace RemoteDigitalSignature.Service.Services
{
    public class XmlSignToXmlDsigService : IXmlSignToXmlDsigService
    {
        public async Task<XmlDocument> SignToXmlDsigAsync(string serialNumberCertificate, byte[] sourceXml, string publicKey, string password)
        {
            return await Sign(sourceXml, publicKey, password);
        }

        public async Task VerifyXmlFileToXmlDsigAsync(XmlDocument document)
        {
            var signatureNodes = document.GetElementsByTagName("ds:Signature");
            if (signatureNodes.Count == 0) { throw new Exception("В документе отсутствуют элементы характерные для ЭЦП с именем ds:Signature"); }

            foreach (var signature in signatureNodes)
            {
                if (!(signature is XmlNode signGlobalNode)) { throw new Exception("Неверная структура элемента Signature"); }
                var signatureNode = new XmlDocument { PreserveWhitespace = true };
                signatureNode.LoadXml(signGlobalNode.OuterXml);

                // Ищем значение уникального идентификатора элемента Signature
                if (signatureNode.FirstChild?.Attributes == null || signatureNode.FirstChild.Attributes.Count == 0) { throw new Exception("У объекта Signature отсутствует обязательный атрибут Id"); }
                var signatureId = string.Empty;
                foreach (var atr in signatureNode.FirstChild.Attributes)
                {
                    var attribute = (XmlAttribute)atr;
                    if (attribute.Name.Equals("Id"))
                    {
                        signatureId = attribute.Value;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(signatureId)) { throw new Exception("Объект Signature не содержит корректный Id"); }
                
                var signedInfoNodes = signatureNode.GetElementsByTagName("ds:SignedInfo");
                if (signedInfoNodes.Count == 0) { throw new Exception("В документе отсутствуют элементы характерные для ЭЦП с именем ds:SignedInfo"); }
                if (signedInfoNodes.Count != 1) { throw new Exception($"Элемент ds:SignedInfo внутри секции Signature c Id={signatureId} встречается более одного раза"); }
                
                var signInfoNode = signedInfoNodes[0]!.OuterXml;
                var signNode = new XmlDocument { PreserveWhitespace = true };
                signNode.LoadXml(signInfoNode);
                
                // Ищем все ds: Reference
                var referenceNodes = signNode.GetElementsByTagName("ds:Reference");
                if (referenceNodes.Count == 0) { throw new Exception("В документе отсутствуют элементы Reference с именем ds:Reference"); }

                foreach (var item in referenceNodes)
                {
                    if (!(item is XmlNode referNode)) { throw new Exception("Неверная структура элемента Reference"); }
                    var referenceNode = new XmlDocument { PreserveWhitespace = true };
                    referenceNode.LoadXml(referNode.OuterXml);
                    // Согласно СТБ у Reference должен иметься атрибут URL, в котором лежит ссылка на подписанный блок данных
                    if (referenceNode.FirstChild?.Attributes == null || referenceNode.FirstChild.Attributes.Count == 0) { throw new Exception($"У объекта Reference внутри Signature c Id={signatureId} отсутствует обязательный атрибут URL"); }
                    var urlValue = string.Empty;
                    foreach (var atr in referenceNode.FirstChild.Attributes)
                    {
                        var attribute = (XmlAttribute)atr;
                        if (attribute.Name.Equals("URI"))
                        {
                            urlValue = attribute.Value;
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(urlValue) || !urlValue.StartsWith("#")) { throw new Exception($"Объект Reference внутри Signature c Id={signatureId} не содержит корректный URI"); }

                    // Внутри Reference должен быть элемент ds:DigestValue, в котором лежит хеш блока данных
                    var digestValueNode = referenceNode.GetElementsByTagName("ds:DigestValue");
                    if (digestValueNode is not { Count: 1 }) { throw new Exception($"Отсутствует элемент ds:DigestValue или находится более 1 раза для Reference, который ссылается на URI {urlValue} внутри Signature c Id={signatureId}"); }
                    var digestValue = digestValueNode[0]!.InnerText;
                    if (string.IsNullOrEmpty(digestValue)) { throw new Exception($"Значение хеша данных ds:DigestValue пустой для Reference, который ссылается на URI {urlValue} внутри Signature c Id={signatureId}"); }

                    // Ищем основной блок данных, вычисляем его хеш и сравниваем со значением ds:DigestValue
                    urlValue = urlValue.Remove(0, 1);  // Убираем # из URI
                    var dataForHash = document.SelectSingleNode($"//*[@Id='{urlValue}']");
                    if (dataForHash == null) { throw new Exception($"В документе отсутствует блок данных с Id={urlValue} на который ссылается Reference внутри Signature c Id={signatureId}"); }

                    // TODO Канонизация данных поддерживается только для одного типа http://www.w3.org/2001/10/xml-exc-c14n#
                    // Канонизируем данные
                    var nodeInBytes = Encoding.UTF8.GetBytes(dataForHash.OuterXml);
                    var c14TransformData = C14NTransform(nodeInBytes);

                    // Ищем хеш канонизированных данных
                    var common = await Task.Run(() => GetHashBytes(c14TransformData.data));
                    //var str = Encoding.UTF8.GetString(c14TransformData.data);
                    
                    if (!common.State) { throw new Exception($"Ошибка получения хеша со стороны библиотеки Авест для блока данных с Id={urlValue}"); }
                    var hashXml = common.Data;
                    var sB64Hash = Convert.ToBase64String(hashXml, Base64FormattingOptions.None);

                    if (!sB64Hash.Equals(digestValue)) { throw new Exception($"Подпись не валидна для блока данных с Id={urlValue}"); }
                }
                
                // Ищу значение ЭЦП
                var signatureValue = signatureNode.GetElementsByTagName("ds:SignatureValue");
                if (signatureValue is not { Count: 1 }) { throw new Exception($"Отсутствует или встречается более одного раза элемент ds:SignatureValue внутри Signature c Id={signatureId}"); }
                var signValue = signatureValue[0]!.InnerXml;

                // Канонизируем данные SignatureValue
                var sgnInfoBytes = Encoding.Default.GetBytes(signNode.OuterXml);
                var signInfoC14TransformData = C14NTransform(sgnInfoBytes);

                //var canonicData = Encoding.UTF8.GetString(signInfoC14TransformData.data);

                // Ищем хеш канонизированных данных
                var sgnInfo = await Task.Run(() => GetHashSignInfoXml(signInfoC14TransformData.data));
                if (!sgnInfo.State) { throw new Exception($"Ошибка получения хеша со стороны библиотеки Авест для SgnInfo"); }
                var signInfoByte = sgnInfo.Data;
                //var signInfoHash = Convert.ToBase64String(signInfoByte, Base64FormattingOptions.None);
                
                // Ищем сертификат подписанта
                var certificateNode = signatureNode.GetElementsByTagName("ds:X509Certificate");
                if (certificateNode is not { Count: 1 }) { throw new Exception($"Отсутствует или встречается более одного раза элемент ds:X509Certificate внутри Signature c Id={signatureId}"); }
                //var certInByte = Encoding.UTF8.GetBytes(certificateNode[0]!.InnerXml);
                
                // Проверяем значение ЭЦП
                var signResult = VerifySmallSignForXmlDsig(signInfoByte, signValue, certificateNode[0]!.InnerXml);

                if (!signResult.Valid)
                {
                    throw new Exception($"Документ не валиден по причине {signResult.Error.Result}");
                }
            }
        }

        private (string algoritm, byte[] data) C14NTransform(byte[] sourceXml)
        {
            var stream = new MemoryStream(sourceXml);
            // Алгоритм канонизации должен быть именно таким!!!
            var xmlDsigC14NTransform = new XmlDsigExcC14NTransform(false);
            xmlDsigC14NTransform.LoadInput(stream);
            var streamType = typeof(Stream);
            var outputStream = (MemoryStream)xmlDsigC14NTransform.GetOutput(streamType);
            return new (xmlDsigC14NTransform.Algorithm, outputStream.ToArray());
        }
        
        private async Task<XmlDocument> Sign(byte[] sourceXml, string publicKey, string password)
        {
            var textXml = Encoding.UTF8.GetString(sourceXml);
            var guid = Guid.NewGuid();
            var eDocId = $"d{guid}";
            var docInstance = $"i{guid}";
            var docId = $"c{guid}";

            // Подготавливаем структуру подписанного документа
            var sSignContent = "<doc:SignedDoc xmlns:doc=\"urn:EEC:SignedData:v1.0:EDoc\">" + 
                                           $"<doc:Data Id=\"{eDocId}\">" +
                                                $"<doc:SignedContent DocInstance=\"{docInstance}\" Id=\"{docId}\">" +
                                                    textXml +
                                                "</doc:SignedContent>" +
                                           "</doc:Data>"+
                                       "</doc:SignedDoc>";
            
            // Новую структура загоняем в xml
            var newXmlDocument = new XmlDocument{PreserveWhitespace = true};
            newXmlDocument.LoadXml(sSignContent);
            // Ищем ноду с данными по docId
            var nodeForHash = newXmlDocument.SelectSingleNode($"//*[@Id='{docId}']");
            if (nodeForHash == null) { throw new Exception("Не удалось обнаружить блок данных для вычисления его хеша"); }
            

            // Делаем канонизацию по алгоритму: http://www.w3.org/2001/10/xml-exc-c14n#
            
            var nodeInBytes = Encoding.UTF8.GetBytes(nodeForHash.OuterXml);
            var c14TransformData = C14NTransform(nodeInBytes);

            // Ищем хеш канонизированных данных
            var common = await Task.Run(() => GetHashBytes(c14TransformData.data, publicKey, password));
            if (!common.State) { throw new Exception("Ошибка получения хеша подписываемы данных со стороны библиотеки Авест"); }
            var hashXml = common.Data;
            var sB64Hash = Convert.ToBase64String(hashXml, Base64FormattingOptions.None);
            
            //Создаем блок сведений ЭЦП Signature
            var signGuid = $"xmldsig-{Guid.NewGuid()}";
            var signature = $"<ds:Signature xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" Id=\"{signGuid}\">" +
                                        "<ds:SignedInfo>" +
                                            $"<ds:CanonicalizationMethod Algorithm=\"{c14TransformData.algoritm}\"></ds:CanonicalizationMethod>" +
                                            "<ds:SignatureMethod Algorithm=\"urn:EAEU:Signature:bign-with-hbelt\"></ds:SignatureMethod>" +
                                            $"<ds:Reference Id=\"{signGuid}-ref0\" URI=\"#{docId}\">" +
                                                "<ds:Transforms>" +
                                                    $"<ds:Transform Algorithm=\"{c14TransformData.algoritm}\"></ds:Transform>" +
                                                "</ds:Transforms>" +
                                                "<ds:DigestMethod Algorithm=\"urn:EAEU:Digest:belt-hash256\"/>" +
                                               $"<ds:DigestValue>{sB64Hash}</ds:DigestValue>" +
                                            "</ds:Reference>" +
                                        "</ds:SignedInfo>" +
                                        $"<ds:SignatureValue Id=\"{signGuid}-sign0\"></ds:SignatureValue>" +
                                        "<ds:KeyInfo>" +
                                            "<ds:X509Data>" +
                                                $"<ds:X509Certificate></ds:X509Certificate>" +
                                            "</ds:X509Data>" +
                                        "</ds:KeyInfo>" +
                                  "</ds:Signature>";

            // Делаем канонизацию блока сведений ЭЦП
            //var signatureInBytes = Encoding.UTF8.GetBytes(signature);
            //var c14TransformSignature = C14NTransform(signatureInBytes);
            //var canonicalizationSignature = Encoding.UTF8.GetString(c14TransformSignature.data);
            
            // блок сведений об ЭЦП загоняем в объект xml
            var newSignDataXmlDocument = new XmlDocument { PreserveWhitespace = true };
            newSignDataXmlDocument.LoadXml(signature);

            // Ищем блок SignedInfo
            var nodeForSign = newSignDataXmlDocument.GetElementsByTagName("ds:SignedInfo");
            if (nodeForSign == null) { throw new Exception("Не удалось обнаружить блок данных SignInfo для подписи"); }
            
            // Делаем канонизацию SignInfo
            var signInfoXmlDocument = new XmlDocument { PreserveWhitespace = true };
            signInfoXmlDocument.LoadXml(nodeForSign[0]!.OuterXml);
            var signInfoInBytes = Encoding.UTF8.GetBytes(signInfoXmlDocument.OuterXml);
            var c14TransformSignInfo = C14NTransform(signInfoInBytes);

            // Подписываем методом, который возвращает данные для формата XaDes
            //var signinBytes = Encoding.UTF8.GetBytes(nodeForSign.OuterXml);
            var hashSignInfo = await Task.Run(() => GetHashBytes(c14TransformSignInfo.data, publicKey, password));
            if (!hashSignInfo.State) { throw new Exception("Ошибка получения хеша со стороны библиотеки Авест для блока данных SignInfo"); }
            //var hashSignInfoBase = Convert.ToBase64String(hashSignInfo.Data);
            var xaDesSign = await Task.Run(() => SignXmlFile(hashSignInfo.Data, publicKey, password));
            if (!xaDesSign.State) { throw new Exception($"Произошла ошибка подписи блока для SignedInfo: {xaDesSign.Error.Result}");}
            
            // Ищем в ноде SignInfo значение SignatureValue и подставляем туда значение ЭЦП
            var nodeForSignatureValue = newSignDataXmlDocument.GetElementsByTagName("ds:SignatureValue");
            if (nodeForSignatureValue == null) { throw new Exception("Не удалось обнаружить блок данных SignatureValue для подписи"); }
            nodeForSignatureValue[0]!.InnerXml = xaDesSign.SignValue;

            // Ищем в ноде SignInfo значение  ds:X509Certificate и подставляем туда значение сертификата
            var certNodeValue = newSignDataXmlDocument.GetElementsByTagName("ds:X509Certificate");
            if (certNodeValue == null) { throw new Exception("Не удалось обнаружить блок данных ds:X509Certificate"); }
            certNodeValue[0]!.InnerXml = xaDesSign.Certificate;
            
            // На данной стадии блок сведений об ЭЦП готов полностью.
            // Вставляем блок сведений об ЭЦП в конечный итоговый документ
            // Ищем ноду $"<doc:Data Id=\"{eDocId}\"> куда будем вставлять новые данные
            var nodeForInsert = newXmlDocument.SelectSingleNode($"//*[@Id='{eDocId}']");
            if (nodeForInsert == null) { throw new Exception("Отсутствует элемент <doc:Data>"); }
            nodeForInsert.InnerXml = $"{newSignDataXmlDocument.OuterXml}\n{nodeForInsert.InnerXml}";
            
            return newXmlDocument;
        }
    }
}
