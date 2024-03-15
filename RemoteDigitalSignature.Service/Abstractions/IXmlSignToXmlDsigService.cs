using System.Xml;

namespace RemoteDigitalSignature.Service.Abstractions
{
    public interface IXmlSignToXmlDsigService
    {
        /// <summary>
        /// Подпись XML в формате XmlDsig
        /// </summary>
        /// <param name="serialNumberCertificate"></param>
        /// <param name="sourceXml"></param>
        /// <param name="publicKey"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<XmlDocument> SignToXmlDsigAsync(string serialNumberCertificate, byte[] sourceXml, string publicKey, string password);

        /// <summary>
        /// Метод проверки подписанной XML в формате XmlDsig
        /// </summary>
        /// <returns></returns>
        /// <param name="document"></param>
        Task VerifyXmlFileToXmlDsigAsync(XmlDocument document);
    }
}
