//using AvCryptMailCOMSystem;
//using System.Diagnostics;
//using System.Reflection;
//using System.Security.Cryptography.X509Certificates;
//using Microsoft.Win32;

using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
//using AvCryptMailCOMSystem;
using Microsoft.Win32;

namespace DigitalSigning;
//public class Certificate
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public string PublicKeyId { get; set; }

//    /// <summary>
//    /// 
//    /// </summary>
//    public string SerialNumberCertificate { get; set; }

//    /// <summary>
//    /// 
//    /// </summary>
//    public string Subject { get; set; }

//    /// <summary>
//    /// 
//    /// </summary>
//    public string OrganizationName { get; set; }

//    /// <summary>
//    /// 
//    /// </summary>
//    public string SurName { get; set; }

//    /// <summary>
//    /// 
//    /// </summary>
//    public string Name { get; set; }

//    public string Position { get; set; }

//    public string City { get; set; }

//    public string Region { get; set; }

//    public string Address { get; set; }

//    public string Email { get; set; }

//    public string Unp { get; set; }

//    /// <summary>
//    /// 
//    /// </summary>
//    public DateTime ValidityNotBefore { get; set; }

//    public DateTime ValidityNotAfter { get; set; }

//    public uint ValidityNotBeforeSec { get; set; }

//    public uint ValidityNotAfterSec { get; set; }

//    public bool IsValid { get; set; }
//}


///// <summary>
///// Тип авторизации
///// </summary>
//internal enum TypeAuthorization : uint
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    NoAuthSilent = 0x4 | 0x40,

//    /// <summary>
//    /// Без авторизации
//    /// </summary>
//    NoAuth = 0x4,

//    /// <summary>
//    /// С авторизацией
//    /// </summary>
//    Auth = 0x00
//}

///// <summary>
///// Тип блоба
///// </summary>
//internal enum BlobType
//{
//    Null,
//    Integer,
//    String,
//    Text,
//    TextNullByte,
//    Hex,
//    Base64,
//    DateTime,
//    DateTimeSec,
//    RawData,
//    LoadFile
//}

///// <summary>
///// Флаги параметров
///// </summary>
//internal enum ParamFlags : uint
//{
//    /// <summary>
//    /// Открытый ключ 
//    /// </summary>
//    AVCM_PUB_KEY_ID = 0x100F,

//    /// <summary>
//    /// 
//    /// </summary>
//    AVCM_COMMON_NAME = 0x1031,

//    /// <summary>
//    /// 
//    /// </summary>
//    AVCM_PASSWORD = 0x1030,

//    /// <summary>
//    /// 
//    /// </summary>
//    AVCM_ISSUER_AS_STRING = 0x100D,

//    /// <summary>
//    /// 
//    /// </summary>
//    AVCM_CRL_ISSUER_SUBJECT = 0x1,

//    /// <summary>
//    /// 
//    /// </summary>
//    AVCM_NEXT_UPDATE = 0x1B,
//}

///// <summary>
///// Флаги сообщений
///// </summary>
//internal enum MessageFlags : uint
//{
//    /// <summary>
//    /// Открыть для проверки подписи
//    /// </summary>
//    AVCMF_OPEN_FOR_VERIFYSIGN = 0x2000,

//    /// <summary>
//    /// Отделенная подпись
//    /// </summary>
//    AVCMF_DETACHED = 0x2000000,

//    /// <summary>
//    /// Открыть сообщение для подписи
//    /// </summary>
//    AVCMF_OPEN_FOR_SIGN = 0x1000,

//    /// <summary>
//    /// Добавить все сертификаты
//    /// </summary>
//    AVCMF_ADD_ALL_CERT = 0x80000,

//    /// <summary>
//    /// Добавить сертификат подписанта
//    /// </summary>
//    AVCMF_ADD_SIGN_CERT = 0x100000,

//    /// <summary>
//    /// Сообщение
//    /// </summary>
//    AVCMF_MESSAGE = 0x1000
//}

//public class Crl
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public string Title { get; set; }

//    /// <summary>
//    /// 
//    /// </summary>
//    public string Name { get; set; }

//    /// <summary>
//    /// 
//    /// </summary>
//    public bool IsValid { get; set; }
//}

///// <summary>
///// Класс для работы с COM объектом Avest
///// </summary>
//public class DigitalSigningService
//{
//    /// <summary>
//    /// Конструктор
//    /// </summary>
//    public DigitalSigningService()
//    {
//        try
//        {
//            if (!File.Exists(AvestLogFileName))
//            {
//                Directory.CreateDirectory(Path.GetDirectoryName(AvestLogFileName) ??
//                                          throw new InvalidOperationException("Файл лога или директории не найден"));
//                var stream = File.CreateText(AvestLogFileName);

//                stream.Close();
//            }
//        }
//        catch (Exception ex)
//        {
//            //File.AppendAllText(AvestLogFileName, $"Ошибка создания файла лога - {ex.Message}");
//            throw new Exception(ex.Message);
//        }


//        //InitAvest();
//    }


//    public List<Crl> Crls { get; set; } = new()
//    {
//        new Crl
//        {
//            Title = "Корневой удостоверяющий центр ГосСУОК",
//            Name =
//                "CN=Корневой удостоверяющий центр ГосСУОК, O=\"Республиканское унитарное предприятие \"\"Национальный центр электронных услуг\"\"\", C=BY"
//        },
//        new Crl
//        {
//            Title = "Республиканский удостоверяющий центр ГосСУОК",
//            Name =
//                "CN=Республиканский удостоверяющий центр ГосСУОК, EMAIL=rca@pki.gov.by, O=\"Республиканское унитарное предприятие \"\"Национальный центр электронных услуг\"\"\", STREET=\"пр-т Машерова, 25\", L=г. Минск, ST=Минская, C=BY"
//        }
//    };

//    /// <summary>
//    /// Главный объект авеста
//    /// </summary>
//    private IAvCryptMailSystem? _cryptSystem;

//    /// <summary>
//    /// Соединение с криптопровайдером
//    /// </summary>
//    private IAvCMConnection? _connection;

//    public string SerialCertificate { get; set; }

//    /// <summary>
//    /// Фамилия
//    /// </summary>
//    public string SurName { get; set; }

//    /// <summary>
//    /// Имя и Отчество
//    /// </summary>
//    public string Name { get; set; }

//    /// <summary>
//    /// Дата выработки ЭЦП
//    /// </summary>
//    public DateTime SignDateTime { get; set; }

//    /// <summary>
//    /// Субъект
//    /// </summary>
//    public string Subject { get; set; }

//    /// <summary>
//    /// Наименование организации
//    /// </summary>
//    public string OrganizationName { get; set; }

//    /// <summary>
//    /// Хэш значение подписи
//    /// </summary>
//    public string Hash { get; set; }

//    /// <summary>
//    /// Подпись в формате Base64
//    /// </summary>
//    public string Signature { get; set; }

//    /// <summary>
//    /// Подпись в формате Binary
//    /// </summary>
//    public byte[] SignatureBinary { get; set; }

//    /// <summary>
//    /// Подпись в формате закодированном Base64
//    /// </summary>
//    public string EncryptedSignature { get; set; }

//    /// <summary>
//    /// 
//    /// </summary>
//    public string FilePath { get; set; }

//    /// <summary>
//    /// Признак проверки валидности ЭЦП средствами Авест
//    /// <b>Avest не валидирует ЭЦП, если дата закончился срок действия сертификата личного ключа</b>
//    /// </summary>
//    public bool IsVerified { get; set; }

//    /// <summary>
//    /// Файл для логов Avest
//    /// </summary>
//    private const string AvestLogFileName = "Logs\\avest.log";

//    public string GetVersion()
//    {
//        return _cryptSystem.Version;
//    }

//    /// <summary>
//    /// Вызов хранилища сертификатов Microsoft 
//    /// </summary>
//    public void SelectCertificate()
//    {
//        var store = new X509Store("MY", StoreLocation.CurrentUser);
//        store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

//        var collection = store.Certificates;
//        var fCollection = collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
//        /*var sCollection = X509Certificate2UI.SelectFromCollection(fCollection, "Выбор сертификата",
//            "Выберите сертификат для подписи",
//            X509SelectionFlag.SingleSelection);*/

//        store.Close();
//    }

//    public bool TryCheckAvest(out IEnumerable<string> errors)
//    {
//        var messErrors = new List<string>();

//        errors = messErrors;

//        var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE");

//        var avKey = key?.OpenSubKey(@"AVEST\Avest CSP");

//        if (avKey != null)
//        {
//            var type = Type.GetTypeFromProgID("AvCryptMailCOMSystem.AvCryptMailSystem");

//            if (type != null)
//            {
//                try
//                {
//                    var avCryptMailComSystem = Activator.CreateInstance(type);

//                    if (avCryptMailComSystem != null)
//                    {
//                        return true;
//                    }

//                    messErrors.Add("Компонент AvCryptMailCOM не установлен");
//                }
//                catch
//                {
//                    messErrors.Add("Компонент AvCryptMailCOM не установлен");
//                }
//            }
//            else
//            {
//                messErrors.Add("Компонент AvCryptMailCOM не установлен");
//            }
//        }
//        else
//        {
//            messErrors.Add("Криптопровайдер Avest не установлен");
//        }

//        return false;
//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    /// <exception cref="Exception"></exception>
//    public bool TryCheckCrl(out IEnumerable<string> errors)
//    {
//        var messErrors = new List<string>();

//        errors = messErrors;

//        foreach (var crl in Crls)
//        {
//            try
//            {
//                if (_cryptSystem == null) return false;
//                if (_connection == null)
//                {
//                    CreateConnection(TypeAuthorization.NoAuthSilent);
//                }

//                var param = new CAvCMParameters();
//                var blob = CreateBlob(BlobType.Text, crl.Name);
//                param.AddParameter((uint) ParamFlags.AVCM_CRL_ISSUER_SUBJECT, blob, 0);

//                var curClr = _connection?.GetCRL(param, 0);

//                if (curClr != null)
//                {
//                    var timeBlob = curClr.GetAttribute((uint) ParamFlags.AVCM_NEXT_UPDATE);
//                    crl.IsValid = timeBlob.GetAsDateTime(0) >= DateTime.Now;
//                }
//            }
//            catch (Exception ex)
//            {
//                if (_cryptSystem != null)
//                {
//                    var errorCode = _cryptSystem.GetLastError();
//                    messErrors.Add(_cryptSystem.GetErrorInformation(errorCode, 0));
//                }
//            }
//        }

//        errors = messErrors;

//        if (Crls.All(crl => crl.IsValid))
//        {
//            return true;
//        }

//        return Crls.Any(crl => !crl.IsValid) && false;
//    }

//    public Dictionary<string, string> ImportCoc(Dictionary<string, string> base64Certs)
//    {
//        var errors = new Dictionary<string, string>();

//        try
//        {
//            if (_cryptSystem == null) return errors;
//            if (_connection == null)
//            {
//                CreateConnection(TypeAuthorization.NoAuthSilent);
//            }

//            foreach (var (key, value) in base64Certs)
//            {
//                var baseCrlBlob = CreateBlob(BlobType.LoadFile, value);

//                if (baseCrlBlob != null)
//                {
//                    var fileInfo = new FileInfo(value);

//                    if (fileInfo.Exists)
//                    {
//                        try
//                        {
//                            if (fileInfo.Extension == ".cer")
//                            {
//                                _connection?.ImportCertificate(baseCrlBlob, 0);
//                            }
//                            else if (fileInfo.Extension == ".crl")
//                            {
//                                _connection?.ImportCRL(baseCrlBlob, "", 0);
//                            }
//                        }
//                        catch
//                        {
//                            var errorCode = _cryptSystem.GetLastError();
//                            var messageError = _cryptSystem.GetErrorInformation(errorCode, 0);

//                            errors.Add(key, messageError);
//                        }
//                    }
//                }
//            }

//            if (_connection == null) return errors;
//            _connection = null;

//            return errors;
//        }
//        catch
//        {
//            if (_cryptSystem != null)
//            {
//                var errorCode = _cryptSystem.GetLastError();
//                var messageError = _cryptSystem.GetErrorInformation(errorCode, 0);

//                throw new Exception(messageError);
//            }
//        }

//        return errors;
//    }

//    public IEnumerable<Certificate> GetCertificates()
//    {
//        CreateConnection(TypeAuthorization.NoAuthSilent);

//        var certificates = new List<Certificate>();

//        if (_connection != null)
//        {
//            var param = new CAvCMParameters();
//            var blob = CreateBlob(BlobType.Integer, 0x10E);
//            param.AddParameter(0x26, blob, 0);

//            var certs = _connection.SelectCertificates(param, 0);

//            for (uint i = 0; i < certs.Count; i++)
//            {
//                var cert = certs.Item[i];

//                var status = cert.Status;
//                var valid = cert.CheckValidity(0);

//                var isTrust = status.IsTrusted;

//                if (isTrust == 1)
//                {

//                    Debug.WriteLine(
//                        $"{cert.ValidityNotAfter} {cert.ValidityNotAfterSec} {cert.ValidityNotBefore} {cert.ValidityNotBeforeSec}");
//                    Debug.WriteLine(cert.Serial.GetAsHex(0));
//                    Debug.WriteLine(cert.PublicKeyId.GetAsHex(0));
//                    Debug.WriteLine(cert.Subject);
//                    Debug.WriteLine(cert.GetSubjectNameAttributeByOid("2.5.4.10", 0).Value);
//                    Debug.WriteLine(cert.GetSubjectNameAttributeByOid("2.5.4.7", 0).Value);
//                    Debug.WriteLine(cert.GetSubjectNameAttributeByOid("2.5.4.9", 0).Value);
//                    Debug.WriteLine(cert.GetSubjectNameAttributeByOid("1.2.840.113549.1.9.1", 0).Value);
//                    Debug.WriteLine(cert.GetSubjectNameAttributeByOid("2.5.4.4", 0).Value);
//                    Debug.WriteLine(cert.GetSubjectNameAttributeByOid("2.5.4.41", 0).Value);

//                    try
//                    {
//                        Debug.WriteLine(cert.GetSubjectNameAttributeByOid("2.5.4.8", 0).Value);
//                    }
//                    catch
//                    {
//                        Debug.WriteLine(GetError());
//                    }

//                    if (cert.ExtensionsCount > 0)
//                    {
//                        Debug.WriteLine(cert.GetExtensionByOid("1.2.112.1.2.1.1.5.1", 0).ValueAsString);
//                        Debug.WriteLine(cert.GetExtensionByOid("1.2.112.1.2.1.1.1.1.2", 0).ValueAsString);
//                    }


//                    var dateOffset = cert.ValidityNotAfter - cert.ValidityNotBefore;
//                    bool isValid;
//                    var seconds = dateOffset.TotalSeconds;

//                    if (seconds > 0.0f)
//                    {
//                        Debug.WriteLine($"Осталось дней {dateOffset.Days} {dateOffset.Hours} {dateOffset.Minutes}");
//                        isValid = true;
//                    }
//                    else
//                    {
//                        seconds = 0.0f;
//                        isValid = false;
//                    }

//                    var certificate = new Certificate
//                    {
//                        SerialNumberCertificate = cert.Serial.GetAsHex(0),
//                        PublicKeyId = cert.PublicKeyId.GetAsHex(0),
//                        Subject = cert.Subject,
//                        OrganizationName = cert.GetSubjectNameAttributeByOid("2.5.4.10", 0).Value,
//                        City = cert.GetSubjectNameAttributeByOid("2.5.4.7", 0).Value,
//                        Address = cert.GetSubjectNameAttributeByOid("2.5.4.9", 0).Value,
//                        Email = cert.GetSubjectNameAttributeByOid("1.2.840.113549.1.9.1", 0).Value,
//                        SurName = cert.GetSubjectNameAttributeByOid("2.5.4.4", 0).Value,
//                        Name = cert.GetSubjectNameAttributeByOid("2.5.4.41", 0).Value,
//                        // Начало действия сертификата
//                        ValidityNotBefore = cert.ValidityNotBefore,
//                        ValidityNotBeforeSec = cert.ValidityNotBeforeSec,
//                        // Время окончания действия сертификата
//                        ValidityNotAfter = cert.ValidityNotAfter,
//                        ValidityNotAfterSec = cert.ValidityNotAfterSec,
//                        IsValid = isValid
//                    };

//                    try
//                    {
//                        certificate.Region = cert.GetSubjectNameAttributeByOid("2.5.4.8", 0).Value;
//                    }
//                    catch
//                    {
//                        certificate.Region = null;
//                    }

//                    if (cert.ExtensionsCount > 0)
//                    {
//                        certificate.Position = cert.GetExtensionByOid("1.2.112.1.2.1.1.5.1", 0).ValueAsString;
//                        certificate.Unp = cert.GetExtensionByOid("1.2.112.1.2.1.1.1.1.2", 0).ValueAsString;
//                    }

//                    certificates.Add(certificate);
//                }
//            }
//        }

//        return certificates;
//    }

//    public void CreateConnection(string publicKeyId, string password)
//    {
//        if (string.IsNullOrEmpty(password)) throw new Exception("Необходимо ввести пароль ключа");

//        var cAvCmParameters = new CAvCMParameters();
//        var blob1 = CreateBlob(BlobType.TextNullByte, publicKeyId);
//        var blob2 = CreateBlob(BlobType.TextNullByte, password);

//        //0x1031 0x100F
//        cAvCmParameters.AddParameter(0x100F, blob1, 0);
//        cAvCmParameters.AddParameter(0x1030, blob2, 0);

//        //File.AppendAllText(AvestLogFileName, "Создание параметров\n");

//        //File.AppendAllText(AvestLogFileName, "Попытка создания соединения Авест\n");
//        // Создание соединения с менеджером сертификатов Avest без авторизации
//        // CAvCMConnection connection = cryptSystem.CreateConnection(Params, (uint)typeAuth);

//        //  CAvCMParameters noAuthParams = new CAvCMParameters();

//        // Создание соединения с менеджером сертификатов Avest

//        try
//        {
//            _connection = _cryptSystem.CreateConnection(cAvCmParameters, (uint) TypeAuthorization.Auth);
//            //File.AppendAllText(AvestLogFileName, "Успешное создание соединения Авест\n");
//        }
//        catch (Exception)
//        {
//            if (_connection != null)
//            {
//                _connection = null;
//            }

//            var lastError = _cryptSystem.GetLastError();

//            // -2125594569 - Действие отменено

//            if (lastError == -2125594569)
//            {
//                //File.AppendAllText(AvestLogFileName, "Действие отменено\n");
//            }
//            else
//            {
//                var error = _cryptSystem.GetErrorInformation(lastError, 0);

//                //File.AppendAllText(AvestLogFileName, $"Ошибка создания соединения Авест - {error}\n");

//                throw new Exception(error);
//            }
//        }
//    }

//    private CAvCMSignedMessage CreateMessage(CAvCMBlob signedMessage, uint flags)
//    {
//        // Создание сообщения для проверки сообщения с отсоединенной подписью
//        var message = _connection.CreateMessage(signedMessage, flags);
//        //File.AppendAllText(AvestLogFileName, "Создание сообщения\n");

//        // Приведение сообщения к подписанному сообщению
//        var signed = message as CAvCMSignedMessage;

//        //File.AppendAllText(AvestLogFileName, "Приведение к Signed\n");

//        return signed;
//    }

//    private bool IsSigned(IAvCMSignedMessage message)
//    {
//        return (message.IsSigned() == 1);
//    }

//    private void CreateConnection(TypeAuthorization typeAuth)
//    {
//        var cAvCmParameters = new CAvCMParameters();

//        //File.AppendAllText(AvestLogFileName, "Создание параметров\n");

//        //File.AppendAllText(AvestLogFileName, "Попытка создания соединения Авест\n");
//        // Создание соединения с менеджером сертификатов Avest без авторизации
//        // CAvCMConnection connection = cryptSystem.CreateConnection(Params, (uint)typeAuth);

//        //  CAvCMParameters noAuthParams = new CAvCMParameters();

//        // Создание соединения с менеджером сертификатов Avest

//        try
//        {
//            _connection = _cryptSystem.CreateConnection(cAvCmParameters, (uint) typeAuth);

//            //File.AppendAllText(AvestLogFileName, "Успешное создание соединения Авест\n");

//            //while (Marshal.ReleaseComObject(cAvCmParameters) > 0) { }
//        }
//        catch (Exception)
//        {
//            if (_connection != null)
//            {
//                //while (Marshal.ReleaseComObject(_connection) > 0)
//                //{
//                //}

//                _connection = null;
//            }

//            var lastError = _cryptSystem.GetLastError();

//            // -2125594569 - Действие отменено

//            if (lastError == -2125594569)
//            {
//                //File.AppendAllText(AvestLogFileName, "Действие отменено\n");
//            }
//            else
//            {
//                var error = _cryptSystem.GetErrorInformation(lastError, 0);

//                //File.AppendAllText(AvestLogFileName, $"Ошибка создания соединения Авест - {error}\n");

//                throw new Exception(error);
//            }
//        }
//    }

//    /// <summary>
//    /// Получение ошибки Avest в строковом виде
//    /// </summary>
//    /// <returns></returns>
//    private string GetError()
//    {
//        var lastError = _cryptSystem.GetLastError();

//        // -2125594569 - Действие отменено

//        if (lastError == -2125594569)
//        {
//            //File.AppendAllText(AvestLogFileName, "Действие отменено\n");
//            return null;
//        }

//        var error = _cryptSystem.GetErrorInformation(lastError, 0);

//        return error;
//    }

//    /// <summary>
//    /// Создание блоба
//    /// </summary>
//    /// <param name="type">Тип блоба</param>
//    /// <param name="data">данные для блоба</param>
//    /// <returns></returns>
//    private CAvCMBlob CreateBlob(BlobType type, object data = null)
//    {
//        // Создание блоба с данными 
//        //var blob = null;
//        var blob = _cryptSystem.CreateBlob(0);

//        try
//        {
//            switch (type)
//            {
//                case BlobType.Integer:
//                {
//                    blob = _cryptSystem.CreateBlob(0);
//                    blob.SetAsInteger(0x10E, 0);
//                    blob.SetAsInteger(Convert.ToUInt32(data), 0);
//                    //File.AppendAllText(AvestLogFileName, "Создание блоба Int\n");
//                    break;
//                }
//                case BlobType.String:
//                {
//                    blob = _cryptSystem.CreateBlob(0);

//                    if (!File.Exists(@"d:\1.pdf")) throw new Exception(@"Файл D:\1.pdf отсутствует");
//                    blob.LoadFromFile(@"d:\1.pdf", 0);
//                    //File.AppendAllText(AvestLogFileName, "Создание блоба String\n");
//                    break;
//                }
//                case BlobType.Text:
//                {
//                    blob = _cryptSystem.CreateBlob(0);
//                    blob.SetAsText(data?.ToString(), 0);
//                    //File.AppendAllText(AvestLogFileName, "Создание блоба Text\n");
//                    break;
//                }
//                case BlobType.TextNullByte:
//                {
//                    blob = _cryptSystem.CreateBlob(0);
//                    blob.SetAsText(data?.ToString(), 1);
//                    //File.AppendAllText(AvestLogFileName, "Создание блоба Text\n");
//                    break;
//                }
//                case BlobType.Hex:
//                {
//                    blob = _cryptSystem.CreateBlob(0);
//                    blob.SetAsHex(data?.ToString(), 0);
//                    //File.AppendAllText(AvestLogFileName, "Создание блоба Hex\n");
//                    break;
//                }
//                case BlobType.Base64:
//                {
//                    blob = _cryptSystem.CreateBlob(0);
//                    blob.SetAsBase64(data?.ToString(), 0);
//                    //File.AppendAllText(AvestLogFileName, "Создание блоба Base64\n");
//                    break;
//                }
//                case BlobType.DateTime:
//                {
//                    //blob = cryptSystem.CreateBlob(0);
//                    //blob.SetAsDateTime((DateTime)data, 0);
//                    //File.AppendAllText(AvestLogFileName, "Создание блоба DateTime\n");
//                    break;
//                }
//                case BlobType.DateTimeSec:
//                {
//                    //blob = cryptSystem.CreateBlob(0);
//                    //blob.SetAsDateTimeSec((uint)data, 0);
//                    //File.AppendAllText(AvestLogFileName, "Создание блоба DateTimeSec\n");
//                    break;
//                }
//                case BlobType.RawData:
//                {
//                    blob = _cryptSystem.CreateBlob(0);
//                    blob.SetAsRawData(data?.ToString(), (uint) (data?.ToString()).Length, 0);
//                    //File.AppendAllText(AvestLogFileName, "Создание блоба RawData\n");
//                    break;
//                }
//                case BlobType.LoadFile:
//                {
//                    blob = _cryptSystem.CreateBlob(0);
//                    blob.LoadFromFile(data?.ToString(), 0);
//                    //File.AppendAllText(AvestLogFileName, "Создание блоба LoadFile\n");
//                    break;
//                }
//                case BlobType.Null:
//                {
//                    blob = _cryptSystem.CreateBlob(0);
//                    break;
//                }
//                default:
//                {
//                    //File.AppendAllText(AvestLogFileName, "Неправильный формат данных для блоба\n");
//                    break;
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            throw new Exception(ex.Message);
//        }

//        return blob;
//    }

//    /// <summary>
//    /// Создание блоба
//    /// </summary>
//    /// <param name="type">Тип блоба</param>
//    /// <param name="data">ссылка на данные для блоба</param>
//    /// <returns></returns>
//    private CAvCMBlob CreateBlob(BlobType type, ref string data)
//    {
//        // Создание блоба с данными 
//        CAvCMBlob blob = null;

//        switch (type)
//        {
//            case BlobType.Integer:
//            {
//                blob = _cryptSystem.CreateBlob(0);
//                blob.SetAsInteger(Convert.ToUInt32(data), 0);
//                //File.AppendAllText(AvestLogFileName, "Создание блоба Int\n");
//                break;
//            }
//            case BlobType.String:
//            {
//                blob = _cryptSystem.CreateBlob(0);
//                //blob.SetAsString(data.ToString(), 0);

//                if (!File.Exists(@"d:\1.pdf")) throw new Exception(@"Файл D:\1.pdf отсутствует");
//                blob.LoadFromFile(@"d:\1.pdf", 0);
//                //File.AppendAllText(AvestLogFileName, "Создание блоба String\n");
//                break;
//            }
//            case BlobType.Text:
//            {
//                blob = _cryptSystem.CreateBlob(0);

//                //blob.SetAsText(data, 0);
//                blob.SetAsText(data, 0);
//                //File.AppendAllText(AvestLogFileName, "Создание блоба Text\n");
//                break;
//            }
//            case BlobType.Hex:
//            {
//                blob = _cryptSystem.CreateBlob(0);
//                blob.SetAsHex(data, 0);
//                //File.AppendAllText(AvestLogFileName, "Создание блоба Hex\n");
//                break;
//            }
//            case BlobType.Base64:
//            {
//                blob = _cryptSystem.CreateBlob(0);
//                blob.SetAsBase64(data, 0);
//                //File.AppendAllText(AvestLogFileName, "Создание блоба Base64\n");
//                break;
//            }
//            case BlobType.DateTime:
//            {
//                //blob = cryptSystem.CreateBlob(0);
//                //blob.SetAsDateTime((DateTime)data, 0);
//                //File.AppendAllText(AvestLogFileName, "Создание блоба DateTime\n");
//                break;
//            }
//            case BlobType.DateTimeSec:
//            {
//                //blob = cryptSystem.CreateBlob(0);
//                //blob.SetAsDateTimeSec((uint)data, 0);
//                //File.AppendAllText(AvestLogFileName, "Создание блоба DateTimeSec\n");
//                break;
//            }
//            case BlobType.RawData:
//            {
//                blob = _cryptSystem.CreateBlob(0);
//                blob.SetAsRawData(data, (uint) data.Length, 0);
//                //File.AppendAllText(AvestLogFileName, "Создание блоба RawData\n");
//                break;
//            }
//            case BlobType.LoadFile:
//                break;
//            case BlobType.Null:
//                break;
//            default:
//            {
//                //File.AppendAllText(AvestLogFileName, "Неправильный формат данных для блоба\n");
//                break;
//            }
//        }

//        return blob;
//    }

//    /// <summary>
//    /// Получение хэша
//    /// </summary>
//    /// <param name="data"></param>
//    /// <param name="algorithm">Алгоритм хеш функции</param>
//    /// <returns></returns>
//    private string GetHash(CAvCMBlob data, string algorithm = "1.2.112.0.2.0.34.101.31.81")
//    {
//        try
//        {
//            // Получить хэш данных
//            var stringHash = _cryptSystem.Hash(data, algorithm, 0).GetAsHex(0);

//            //File.AppendAllText(AvestLogFileName, "Получение хэша - " + stringHash + "\n");

//            return stringHash;
//        }
//        catch (Exception)
//        {
//            var error = GetError();

//            throw new Exception(error);
//        }
//    }

//    /// <summary>
//    /// Инициализация COM объекта Авеста
//    /// </summary>
//    /// <exception cref="Exception"></exception>
//    public void InitAvest()
//    {
//        // File.AppendAllText(AvestLogFileName, "Инициализация Avest\n");

//        try
//        {
//            // Получение объекта типа CryptMailSystem
//            //var cryptMailType = Type.GetTypeFromProgID("AvCryptMailCOMSystem.AvCryptMailSystem");
//            //var cryptMail = Activator.CreateInstance(cryptMailType);

//            // Создание объекта COM для работы Avest
//            var cryptSystem = new CAvCryptMailSystem();

//            //File.AppendAllText(AvestLogFileName, $"Успешно создан объект авест: {cryptSystem.Version}\n");

//            _cryptSystem = cryptSystem;
//        }
//        catch (Exception ex)
//        {
//            // File.AppendAllText(AvestLogFileName,
//            //   $"Ошибка создания объекта авеста {ex.Message}\n");

//            throw new Exception($"Ошибка создания объекта авеста {ex.Message}");
//        }
//    }

//    /// <summary>
//    /// проверка хэша в подписанном сообщении
//    /// </summary>
//    /// <param name="message"></param>
//    /// <param name="blobHash"></param>
//    private IAvCMSignedMessage FinalHashed(IAvCMSignedMessage message, CAvCMBlob blobHash)
//    {
//        try
//        {
//            //File.AppendAllText(AvestLogFileName, "Проверка хэша данных и хэш в подписи\n");

//            // Получение хэша
//            return message.FinalHashed(blobHash);

//        }
//        catch (Exception)
//        {
//            var error = GetError();
//            throw new Exception(error);
//        }
//    }

//    /// <summary>
//    /// Проверка сообщения на корректность
//    /// </summary>
//    /// <param name="message"></param>
//    /// <returns></returns>
//    private bool Verify(IAvCMSignedMessage message)
//    {
//        try
//        {
//            // Проверка подписанного сообщения на валидность
//            message.Verify(0);

//            //File.AppendAllText(AvestLogFileName, "Подпись верна\n");
//        }
//        catch (Exception ex)
//        {
//            if (_cryptSystem.GetLastError() != 0)
//            {
//                //File.AppendAllText(AvestLogFileName, $"{GetError()}\n");
//                return false;
//            }
//            else
//            {
//                //File.AppendAllText(AvestLogFileName, ex.Message + "\n");
//                throw ex.GetBaseException();
//            }
//        }

//        return true;
//    }

//    /// <summary>
//    /// Создание файла с данными
//    /// </summary>
//    /// <param name="data">Данные</param>
//    /// <param name="path">Путь к директории</param>
//    /// <param name="filename">Имя файла</param>
//    public void Save(string data, string path, string filename)
//    {
//        if (!Directory.Exists(path))
//        {
//            Directory.CreateDirectory(path);
//        }

//        // Формирование файла [тип электронного документа]-[дата]-[время]-[УНП].json
//        File.WriteAllText(Path.Combine(path, filename), data);
//    }

//    /// <summary>
//    /// Проверка валидности подписи данных файла
//    /// </summary>
//    /// <param name="filePath">путь к файлу</param>
//    /// <param name="sign">подпись</param>
//    /// <param name="bVerify">признак проверки ЭЦП средствами Avest</param>
//    /// <returns></returns>
//    public bool VerifyFileSign(string filePath, string sign, bool bVerify = true)
//    {
//        //File.AppendAllText(AvestLogFileName, "-------------- Начало обработки сообщения -----------\n");
//        //File.AppendAllText(AvestLogFileName, $"Данные: {filePath}\n");
//        //File.AppendAllText(AvestLogFileName, $"Подпись: {sign}\n");


//        // Создание объекта COM для работы Avest
//        //CAvCryptMailSystem cryptSystem = new CAvCryptMailSystem();

//        if (_cryptSystem != null)
//        {
//            CreateConnection(TypeAuthorization.NoAuth);

//            if (_connection != null)
//            {
//                //string asciiString = Encoding.ASCII.GetString(data);

//                var dataBlob = CreateBlob(BlobType.LoadFile, filePath);

//                if (dataBlob != null)
//                {
//                    // Получить хэш данных
//                    var stringHash = GetHash(dataBlob);
//                    Hash = stringHash;

//                    // Создание блоба с ЭЦП
//                    var signBlob = CreateBlob(BlobType.Base64, sign);

//                    if (signBlob != null)
//                    {
//                        // Создание сообщения для проверки сообщения с отсоединенной подписью
//                        var signedMessage = CreateMessage(signBlob,
//                            (uint) MessageFlags.AVCMF_OPEN_FOR_VERIFYSIGN | (uint) MessageFlags.AVCMF_DETACHED);

//                        // Если успешное приведение
//                        if (signedMessage != null)
//                        {
//                            // Если сообщение является подписанным сообщением
//                            if (IsSigned(signedMessage))
//                            {
//                                //File.AppendAllText(AvestLogFileName, "Сообщение является подписанным\n");

//                                //Console.WriteLine("Сообщение является подписанным");

//                                // Создание сообщения с хэшем
//                                var blobHexMessage = CreateBlob(BlobType.Hex, stringHash);

//                                FinalHashed(signedMessage, blobHexMessage);

//                                if (Verify(signedMessage))
//                                {
//                                    var signature = signedMessage.Signs[0];

//                                    if (signature != null)
//                                    {
//                                        var signerCertificate = signature.SignerCertificate;

//                                        if (signerCertificate != null)
//                                        {
//                                            SerialCertificate = signerCertificate.Serial.GetAsHex(0);
//                                            Subject = signerCertificate.Subject;

//                                            OrganizationName = signerCertificate
//                                                .GetSubjectNameAttributeByOid("2.5.4.10", 0).Value;
//                                            SurName = signerCertificate.GetSubjectNameAttributeByOid("2.5.4.4", 0)
//                                                .Value;
//                                            Name = signerCertificate.GetSubjectNameAttributeByOid("2.5.4.41", 0).Value;
//                                        }
//                                        else
//                                        {
//                                            var error = GetError();
//                                            throw new Exception($"Сертификат подписавшего отсутствует - {error}");
//                                        }

//                                        SignDateTime = signature.SignDateTime;

//                                        //while (Marshal.ReleaseComObject(signerCertificate) > 0) { }
//                                    }
//                                    else
//                                    {
//                                        var error = GetError();
//                                        throw new Exception($"Подпись отсутствует - {error}");
//                                    }

//                                    //while (Marshal.ReleaseComObject(sign) > 0) { }
//                                }
//                                else
//                                {
//                                    var error = GetError();
//                                    throw new Exception($"Подпись не верна - {error}");
//                                }

//                                //while (Marshal.ReleaseComObject(blobHexMessage) > 0) { }

//                            }
//                            else
//                            {
//                                //File.AppendAllText(AvestLogFileName, "Сообщение не подписано\n");
//                                var error = GetError();
//                                throw new Exception($"Сообщение не является подписанным - {error}");
//                            }

//                            //while (Marshal.ReleaseComObject(signedMessage) > 0) { }
//                        }

//                        //while (Marshal.ReleaseComObject(signBlob) > 0) { }
//                    }

//                    //while (Marshal.ReleaseComObject(dataBlob) > 0) { }
//                }
//            }
//            else
//            {
//                //File.AppendAllText(AvestLogFileName, "Ошибка создания подключения\n");
//                throw new Exception("Ошибка создания подключения");
//            }

//            //while (Marshal.ReleaseComObject(_connection) > 0) { }
//        }
//        else
//        {
//            //File.AppendAllText(AvestLogFileName, "Ошибка инициализации Avest\n");
//            throw new Exception("Ошибка инициализации Avest");
//        }

//        //File.AppendAllText(AvestLogFileName, "-------------- Конец обработки сообщения -----------\n");

//        //while (Marshal.ReleaseComObject(_cryptSystem) > 0) { }

//        return true;
//    }

//    /// <summary>
//    /// Проверка валидности подписи данных файла
//    /// </summary>
//    /// <param name="filePath">путь к файлу</param>
//    /// <param name="sign">подпись</param>
//    /// <param name="bVerify">признак проверки ЭЦП средствами Avest</param>
//    /// <returns></returns>
//    public bool VerifyFileSign(string filePath, byte[] sign, bool bVerify = true)
//    {
//        //File.AppendAllText(AvestLogFileName, "-------------- Начало обработки сообщения -----------\n");
//        //File.AppendAllText(AvestLogFileName, $"Данные: {filePath}\n");
//        //File.AppendAllText(AvestLogFileName, $"Подпись: {sign}\n");


//        // Создание объекта COM для работы Avest
//        //CAvCryptMailSystem cryptSystem = new CAvCryptMailSystem();

//        if (_cryptSystem != null)
//        {
//            CreateConnection(TypeAuthorization.NoAuth);

//            if (_connection != null)
//            {
//                //string asciiString = Encoding.ASCII.GetString(data);

//                var dataBlob = CreateBlob(BlobType.LoadFile, filePath);

//                if (dataBlob != null)
//                {
//                    // Получить хэш данных
//                    var stringHash = GetHash(dataBlob);
//                    Hash = stringHash;

//                    var singData = Convert.ToBase64String(sign);

//                    // Создание блоба с ЭЦП
//                    var signBlob = CreateBlob(BlobType.Base64, singData);

//                    if (signBlob != null)
//                    {
//                        // Создание сообщения для проверки сообщения с отсоединенной подписью
//                        var signedMessage = CreateMessage(signBlob,
//                            (uint) MessageFlags.AVCMF_OPEN_FOR_VERIFYSIGN | (uint) MessageFlags.AVCMF_DETACHED);

//                        // Если успешное приведение
//                        if (signedMessage != null)
//                        {
//                            // Если сообщение является подписанным сообщением
//                            if (IsSigned(signedMessage))
//                            {
//                                //File.AppendAllText(AvestLogFileName, "Сообщение является подписанным\n");

//                                //Console.WriteLine("Сообщение является подписанным");

//                                // Создание сообщения с хэшем
//                                var blobHexMessage = CreateBlob(BlobType.Hex, stringHash);

//                                FinalHashed(signedMessage, blobHexMessage);

//                                if (bVerify)
//                                {
//                                    IsVerified = Verify(signedMessage);
//                                    if (!IsVerified) throw new Exception($"Подпись не верна - {GetError()}");
//                                }

//                                var signature = signedMessage.Signs[0];

//                                if (signature != null)
//                                {
//                                    var signerCertificate = signature.SignerCertificate;

//                                    if (signerCertificate != null)
//                                    {
//                                        SerialCertificate = signerCertificate.Serial.GetAsHex(0);
//                                        Subject = signerCertificate.Subject;

//                                        OrganizationName = signerCertificate
//                                            .GetSubjectNameAttributeByOid("2.5.4.10", 0).Value;
//                                        SurName = signerCertificate.GetSubjectNameAttributeByOid("2.5.4.4", 0)
//                                            .Value;
//                                        Name = signerCertificate.GetSubjectNameAttributeByOid("2.5.4.41", 0).Value;
//                                    }
//                                    else
//                                    {
//                                        var error = GetError();
//                                        throw new Exception($"Сертификат подписавшего отсутствует - {error}");
//                                    }

//                                    SignDateTime = signature.SignDateTime;

//                                    //while (Marshal.ReleaseComObject(signerCertificate) > 0) { }
//                                }
//                                else
//                                {
//                                    var error = GetError();
//                                    throw new Exception($"Подпись отсутствует - {error}");
//                                }
//                            }

//                            else
//                            {
//                                //File.AppendAllText(AvestLogFileName, "Сообщение не подписано\n");
//                                var error = GetError();
//                                throw new Exception($"Сообщение не является подписанным - {error}");
//                            }
//                        }
//                    }
//                }
//            }
//            else
//            {
//                //File.AppendAllText(AvestLogFileName, "Ошибка создания подключения\n");
//                throw new Exception("Ошибка создания подключения");
//            }

//            //while (Marshal.ReleaseComObject(_connection) > 0) { }
//        }
//        else
//        {
//            //File.AppendAllText(AvestLogFileName, "Ошибка инициализации Avest\n");
//            throw new Exception("Ошибка инициализации Avest");
//        }

//        //File.AppendAllText(AvestLogFileName, "-------------- Конец обработки сообщения -----------\n");

//        //while (Marshal.ReleaseComObject(_cryptSystem) > 0) { }

//        return true;
//    }

//    /// <summary>
//    /// Проверка валидности подписи бинарных данных
//    /// </summary>
//    /// <param name="data">бинарные данные</param>
//    /// <param name="sign">подпись</param>
//    /// <returns></returns>
//    public bool VerifySign(string data, string sign)
//    {
//        //File.AppendAllText(AvestLogFileName, "-------------- Начало обработки сообщения -----------\n");
//        //File.AppendAllText(AvestLogFileName, $"Данные: {data}\n");
//        //File.AppendAllText(AvestLogFileName, $"Подпись: {sign}\n");


//        // Создание объекта COM для работы Avest
//        //CAvCryptMailSystem cryptSystem = new CAvCryptMailSystem();

//        if (_cryptSystem != null)
//        {
//            if (_connection == null)
//            {
//                CreateConnection(TypeAuthorization.NoAuth);
//            }

//            if (_connection != null)
//            {
//                var dataBlob = CreateBlob(BlobType.Text, data);

//                if (dataBlob != null)
//                {
//                    // Получить хэш данных
//                    var stringHash = GetHash(dataBlob);
//                    Hash = stringHash;

//                    // Создание блоба с ЭЦП
//                    var signBlob = CreateBlob(BlobType.Base64, sign);

//                    if (signBlob != null)
//                    {
//                        // Создание сообщения для проверки сообщения с отсоединенной подписью
//                        var signedMessage = CreateMessage(signBlob,
//                            (uint) MessageFlags.AVCMF_OPEN_FOR_VERIFYSIGN | (uint) MessageFlags.AVCMF_DETACHED);

//                        // Если успешное приведение
//                        if (signedMessage != null)
//                        {
//                            // Если сообщение является подписанным сообщением
//                            if (IsSigned(signedMessage))
//                            {
//                                //File.AppendAllText(AvestLogFileName, "Сообщение является подписанным\n");

//                                //Console.WriteLine("Сообщение является подписанным");

//                                // Создание сообщения с хэшем
//                                var blobHexMessage = CreateBlob(BlobType.Hex, stringHash);

//                                var hashedMessage = FinalHashed(signedMessage, blobHexMessage);

//                                if (Verify(signedMessage))
//                                {
//                                    var signature = signedMessage.Signs[0];

//                                    if (signature != null)
//                                    {
//                                        var signerCertificate = signature.SignerCertificate;

//                                        if (signerCertificate != null)
//                                        {
//                                            SerialCertificate = signerCertificate.Serial.GetAsHex(0);
//                                            Subject = signerCertificate.Subject;

//                                            OrganizationName = signerCertificate
//                                                .GetSubjectNameAttributeByOid("2.5.4.10", 0).Value;
//                                            SurName = signerCertificate.GetSubjectNameAttributeByOid("2.5.4.4", 0)
//                                                .Value;
//                                            Name = signerCertificate.GetSubjectNameAttributeByOid("2.5.4.41", 0).Value;
//                                        }
//                                        else
//                                        {
//                                            var error = GetError();
//                                            throw new Exception($"Сертификат подписавшего отсутствует - {error}");
//                                        }

//                                        SignDateTime = signature.SignDateTime;

//                                        //while (Marshal.ReleaseComObject(signerCertificate) > 0) { }
//                                    }
//                                    else
//                                    {
//                                        var error = GetError();
//                                        throw new Exception($"Подпись отсутствует - {error}");
//                                    }

//                                    // while (Marshal.ReleaseComObject(signature) > 0) { }
//                                }
//                                else
//                                {
//                                    var error = GetError();
//                                    throw new Exception($"Подпись не верна - {error}");
//                                }

//                                //while (Marshal.ReleaseComObject(blobHexMessage) > 0) { }

//                            }
//                            else
//                            {
//                                //File.AppendAllText(AvestLogFileName, "Сообщение не подписано\n");
//                                throw new Exception("Сообщение не является подписанным");
//                            }

//                            //while (Marshal.ReleaseComObject(signedMessage) > 0) { }
//                        }

//                        //while (Marshal.ReleaseComObject(signBlob) > 0) { }
//                    }

//                    //while (Marshal.ReleaseComObject(dataBlob) > 0) { }
//                }
//            }
//            else
//            {
//                //File.AppendAllText(AvestLogFileName, "Ошибка создания подключения\n");
//                throw new Exception("Ошибка создания подключения");
//            }

//            //while (Marshal.ReleaseComObject(_connection) > 0) { }
//        }
//        else
//        {
//            //File.AppendAllText(AvestLogFileName, "Ошибка инициализации Avest\n");
//            throw new Exception("Ошибка инициализации Avest");
//        }

//        //File.AppendAllText(AvestLogFileName, "-------------- Конец обработки сообщения -----------\n");

//        //while (Marshal.ReleaseComObject(_cryptSystem) > 0) { }

//        return true;
//    }

//    /// <summary>
//    /// Создание отделенной подписи хэша данных
//    /// </summary>
//    /// <param name="data">Данные для подписи</param>
//    /// <param name="allCertificates">Передавать все сертификаты</param>
//    public bool Sign(string data, bool allCertificates = false)
//    {
//        //File.AppendAllText(AvestLogFileName, "-------------- Начало обработки сообщения -----------\n");
//        //File.AppendAllText(AvestLogFileName, "-------------- Создание ЭЦП -----------\n");
//        //File.AppendAllText(AvestLogFileName, $"Данные: {data}\n");

//        if (_cryptSystem != null)
//        {
//            // Создание соединения с менеджером сертификатов Avest в авторизацией
//            //CreateConnection(TypeAuthorization.Auth);

//            //if (_connection == null)
//            //{
//            CreateConnection(TypeAuthorization.Auth);
//            //}

//            if (_connection != null)
//            {
//                var dataBlob = CreateBlob(BlobType.Text, data);

//                if (dataBlob != null)
//                {
//                    var hash = GetHash(dataBlob);
//                    Hash = hash;

//                    //File.AppendAllText(AvestLogFileName, $"Хеш сообщения - {hash}");

//                    var nullBlob = CreateBlob(BlobType.Null);

//                    if (nullBlob != null)
//                    {
//                        var signedMessage = CreateMessage(nullBlob,
//                            (uint) MessageFlags.AVCMF_OPEN_FOR_SIGN | (uint) MessageFlags.AVCMF_DETACHED |
//                            ((allCertificates)
//                                ? (uint) MessageFlags.AVCMF_ADD_ALL_CERT
//                                : (uint) MessageFlags.AVCMF_ADD_SIGN_CERT));

//                        if (signedMessage != null)
//                        {
//                            var hashBlob = CreateBlob(BlobType.Hex, hash);

//                            if (hashBlob != null)
//                            {
//                                FinalHashed(signedMessage, hashBlob);

//                                var hashedMessage = FinalHashed(signedMessage, hashBlob);

//                                if (hashedMessage != null)
//                                {
//                                    Signature = hashedMessage.Blob.GetAsBase64(0);
//                                    SignatureBinary = Convert.FromBase64String(Signature);
//                                    //File.AppendAllText(AvestLogFileName, "-------------- ЭЦП успешно создана -----------\n");
//                                }
//                            }
//                        }
//                    }
//                }

//                // Освобождение ресурсов COM
//                //while (Marshal.ReleaseComObject(_connection) > 0) { }
//            }
//            else
//            {
//                //File.AppendAllText(AvestLogFileName, "Ошибка создания подключения\n");
//                return false;
//                //throw new Exception("Ошибка создания подключения");
//            }

//            // Освобождение ресурсов COM
//            //while (Marshal.ReleaseComObject(_cryptSystem) > 0) { }
//        }
//        else
//        {
//            //File.AppendAllText(AvestLogFileName, "Ошибка инициализации Avest\n");
//            throw new Exception("Ошибка инициализации Avest");
//        }

//        //File.AppendAllText(AvestLogFileName, "-------------- Конец обработки сообщения -----------\n");

//        return true;
//    }

//    /// <summary>
//    /// Сохранение данных в файл и создание отделенной подписи хэша данных
//    /// </summary>
//    /// <param name="data">Данные для подписи</param>
//    /// <param name="allCertificates">Передавать все сертификаты</param>
//    public bool SaveFileAndSign(string data, bool allCertificates = false)
//    {
//        try
//        {
//            var fileName = $"{Guid.NewGuid()}.json";
//            FilePath = Path.Combine(Path.GetTempPath(), fileName);

//            using (var writer = new StreamWriter(new FileStream(FilePath, FileMode.Create, FileAccess.Write)))
//            {
//                writer.Write(data);
//                writer.Close();
//            }

//            return SignFile(FilePath, allCertificates);
//        }
//        catch (Exception e)
//        {
//            throw new Exception(e.Message);
//        }
//    }

//    /// <summary>
//    /// Сохранение данных в файл и создание отделенной подписи хэша данных
//    /// </summary>
//    /// <param name="password"></param>
//    /// <param name="data">Данные для подписи</param>
//    /// <param name="allCertificates">Передавать все сертификаты</param>
//    /// <param name="publicKeyId"></param>
//    public bool SaveFileAndSign(string publicKeyId, string password, string data, bool allCertificates = false)
//    {
//        try
//        {
//            var fileName = $"{Guid.NewGuid()}.json";
//            FilePath = Path.Combine(Path.GetTempPath(), fileName);

//            using (var writer = new StreamWriter(new FileStream(FilePath, FileMode.Create, FileAccess.Write)))
//            {
//                writer.Write(data);
//                writer.Close();
//            }

//            return SignFile(publicKeyId, password, FilePath, allCertificates);
//        }
//        catch (Exception e)
//        {
//            throw new Exception(e.Message);
//        }
//    }

//    /// <summary>
//    /// Создание отделенной подписи хэша данных
//    /// </summary>
//    /// <param name="filename">Данные для подписи</param>
//    /// <param name="allCertificates">Передавать все сертификаты</param>
//    public bool SignFile(string filename, bool allCertificates = false)
//    {
//        //File.AppendAllText(AvestLogFileName, "-------------- Начало обработки сообщения -----------\n");
//        //File.AppendAllText(AvestLogFileName, "-------------- Создание ЭЦП -----------\n");
//        //File.AppendAllText(AvestLogFileName, $"файл: {filename}\n");

//        if (_cryptSystem != null)
//        {
//            // Создание соединения с менеджером сертификатов Avest в авторизацией
//            //CreateConnection(TypeAuthorization.Auth);

//            //if (_connection == null)
//            //{
//            CreateConnection(TypeAuthorization.Auth);
//            //}

//            if (_connection != null)
//            {
//                var dataBlob = CreateBlob(BlobType.LoadFile, filename);

//                if (dataBlob != null)
//                {
//                    var hash = GetHash(dataBlob);
//                    Hash = hash;

//                    //File.AppendAllText(AvestLogFileName, $"Хеш сообщения - {hash}");

//                    var nullBlob = CreateBlob(BlobType.Null);

//                    if (nullBlob != null)
//                    {
//                        var signedMessage = CreateMessage(nullBlob,
//                            (uint) MessageFlags.AVCMF_OPEN_FOR_SIGN | (uint) MessageFlags.AVCMF_DETACHED |
//                            ((allCertificates)
//                                ? (uint) MessageFlags.AVCMF_ADD_ALL_CERT
//                                : (uint) MessageFlags.AVCMF_ADD_SIGN_CERT));

//                        if (signedMessage != null)
//                        {
//                            var hashBlob = CreateBlob(BlobType.Hex, hash);

//                            if (hashBlob != null)
//                            {
//                                FinalHashed(signedMessage, hashBlob);

//                                var hashedMessage = FinalHashed(signedMessage, hashBlob);

//                                if (hashedMessage != null)
//                                {
//                                    Signature = hashedMessage.Blob.GetAsBase64(0);
//                                    SignatureBinary = Convert.FromBase64String(Signature);

//                                    //File.AppendAllText(AvestLogFileName, "-------------- ЭЦП успешно создана -----------\n");
//                                }
//                            }
//                        }
//                    }
//                }

//                // Освобождение ресурсов COM
//                //while (Marshal.ReleaseComObject(_connection) > 0) { }
//            }
//            else
//            {
//                //File.AppendAllText(AvestLogFileName, "Ошибка создания подключения\n");
//                return false;
//                //throw new Exception("Ошибка создания подключения");
//            }

//            // Освобождение ресурсов COM
//            //while (Marshal.ReleaseComObject(_cryptSystem) > 0) { }
//        }
//        else
//        {
//            //File.AppendAllText(AvestLogFileName, "Ошибка инициализации Avest\n");
//            throw new Exception("Ошибка инициализации Avest");
//        }

//        //File.AppendAllText(AvestLogFileName, "-------------- Конец обработки сообщения -----------\n");

//        return true;
//    }

//    /// <summary>
//    /// Создание отделенной подписи хэша данных
//    /// </summary>
//    /// <param name="password"></param>
//    /// <param name="filename">Данные для подписи</param>
//    /// <param name="allCertificates">Передавать все сертификаты</param>
//    /// <param name="publicKeyId"></param>
//    public bool SignFile(string publicKeyId, string password, string filename, bool allCertificates = false)
//    {
//        //File.AppendAllText(AvestLogFileName, "-------------- Начало обработки сообщения -----------\n");
//        //File.AppendAllText(AvestLogFileName, "-------------- Создание ЭЦП -----------\n");
//        //File.AppendAllText(AvestLogFileName, $"файл: {filename}\n");

//        if (_cryptSystem != null)
//        {
//            // Создание соединения с менеджером сертификатов Avest в авторизацией
//            //CreateConnection(TypeAuthorization.Auth);

//            //if (_connection == null)
//            //{
//            CreateConnection(publicKeyId, password);
//            //}

//            if (_connection != null)
//            {
//                var dataBlob = CreateBlob(BlobType.LoadFile, filename);

//                if (dataBlob != null)
//                {
//                    var hash = GetHash(dataBlob);
//                    Hash = hash;

//                    //File.AppendAllText(AvestLogFileName, $"Хеш сообщения - {hash}");

//                    var nullBlob = CreateBlob(BlobType.Null);

//                    if (nullBlob != null)
//                    {
//                        var signedMessage = CreateMessage(nullBlob,
//                            (uint) MessageFlags.AVCMF_OPEN_FOR_SIGN | (uint) MessageFlags.AVCMF_DETACHED |
//                            ((allCertificates)
//                                ? (uint) MessageFlags.AVCMF_ADD_ALL_CERT
//                                : (uint) MessageFlags.AVCMF_ADD_SIGN_CERT));

//                        if (signedMessage != null)
//                        {
//                            var hashBlob = CreateBlob(BlobType.Hex, hash);

//                            if (hashBlob != null)
//                            {
//                                FinalHashed(signedMessage, hashBlob);

//                                var hashedMessage = FinalHashed(signedMessage, hashBlob);

//                                if (hashedMessage != null)
//                                {
//                                    Signature = hashedMessage.Blob.GetAsBase64(0);
//                                    SignatureBinary = Convert.FromBase64String(Signature);

//                                    //File.AppendAllText(AvestLogFileName, "-------------- ЭЦП успешно создана -----------\n");
//                                }
//                            }
//                        }
//                    }
//                }

//                // Освобождение ресурсов COM
//                //while (Marshal.ReleaseComObject(_connection) > 0) { }
//            }
//            else
//            {
//                //File.AppendAllText(AvestLogFileName, "Ошибка создания подключения\n");
//                return false;
//                //throw new Exception("Ошибка создания подключения");
//            }

//            // Освобождение ресурсов COM
//            //while (Marshal.ReleaseComObject(_cryptSystem) > 0) { }
//        }
//        else
//        {
//            //File.AppendAllText(AvestLogFileName, "Ошибка инициализации Avest\n");
//            throw new Exception("Ошибка инициализации Avest");
//        }

//        //File.AppendAllText(AvestLogFileName, "-------------- Конец обработки сообщения -----------\n");

//        return true;
//    }
//}

public class Crl
{
    /// <summary>
    /// 
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public bool IsValid { get; set; }
}