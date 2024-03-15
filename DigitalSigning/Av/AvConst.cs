using System.Runtime.InteropServices;

namespace DigitalSigning.Av;

/// <summary>
/// 
/// </summary>wssids2010    Ws291290
internal static class AvConst
{

    public static readonly string Path = "C:\\Program Files (x86)\\Avest\\AvPCM_nces";

    // AvCmOpenCertEnum 
    //
    public static readonly uint AVCMF_ALL_CERT = 0x8000;

    // AvCmSign AvCmSignAndEncrypt AvCmMsgSign
    //                                                                      
    public static readonly uint AVCMF_ADD_ALL_CERT = 0x80000;

    // AvCmSign AvCmSignAndEncrypt AvCmMsgSign
    //                                                                  
    public static readonly uint AVCMF_ADD_SIGN_CERT = 0x100000; 

    public static readonly uint AVCMF_ADD_ONLY_CERT = 0x40000;

    //                                                              
    public static readonly uint AVCMF_DETACHED = 0x2000000;

    // AvCmInit
    //                                                                                
    public static readonly uint AVCMF_STARTUP = 0x1;

    public static readonly uint AVCMF_SHUTDOWN = 0x2;

    // AvCmLogin
    // Подключение без атентификации пользователя
    public static readonly uint AVCMF_NO_AUTH = 0x4;

    // Подключение без окна "Загрузка сертификатов"
    public static readonly uint AVCMF_SILENT = 0x40;

    // AvCmLogin:
    public static readonly uint AVCMF_FORCE_TOKEN_CONTROL = 0x10000;

    // AvCmLogin:
    public static readonly uint AVCMF_DENY_TOKEN_CONTROL = 0x20000;

    // AvCmLogin:
    public static readonly uint AVCMF_IGNORE_CRL_ABSENCE = 0x1;

    // AvCmLogin:
    public static readonly uint AVCMF_IGNORE_CRL_EXPIRE = 0x8;

    // AvCmOpenCertEnum:
    public static readonly uint AVCMF_REQUEST_RESULT = 0x1;

    // AvCmEnumDlg:
    public static readonly uint AVCMF_ONLY_ENCR_CERTS = 0x400;

    // AvCmSign, AvCmSignAndEncrypt, AvCmMsgSign:
    public static readonly uint AVCMF_REPEAT_AUTHENTICATION = 0x800;

    // AvCmMsgImportCerts:
    public static readonly uint AVCMF_IMPORT_ALL_CERTS = 0x80000;

    // AvCmMsgImportCerts, AvCmVerify   AvCmVerifySign:                                 
    public static readonly uint AVCMF_IMPORT_CRL = 0x40000;

    // AvCmVerifySign:
    public static readonly uint AVCMF_NO_CERT_VERIFY = 0x8000000;

    // AvCmVerifySign   AvCmDecryptAndVerifySign
    public static readonly uint AVCMF_VERIFY_ON_SIGN_DATE = 0x1;

    // AvCmEncrypt:
    public static readonly uint AVCMF_IGNORE_BAD_CERTS = 0x20;

    // AvCmGenerateRequest:
    public static readonly uint AVCMF_ALLOW_TO_SELECT_FILE = 0x8;

    // AvCmSignRawData, AvCmVerifyRawDataSign: ASN.1
    public static readonly uint AVCMF_RAW_SIGN = 0x40000;
    public static readonly uint AVCMF_UPDATE_HASHVALUE = 0x40000000;

    // AvCmMsgUpdate
    public static readonly uint AVCMF_UPDATE_FINAL = 0x80000000;

    // AvCmOpenMsg
    public static readonly uint AVCMF_OPEN_FOR_SIGN = 0x1000;
    public static readonly uint AVCMF_OPEN_FOR_VERIFYSIGN = 0x2000;
    public static readonly uint AVCMF_OPEN_FOR_ENCRYPT = 0x4000;
    public static readonly uint AVCMF_OPEN_FOR_DECRYPT = 0x8000;
    public static readonly uint AVCMF_UNICODE = 0x10000000;

    // AvCmCreateScep
    public static readonly uint AVCMF_SCEP_OFFLINE = 0x1;

    public static readonly uint AVCMF_RELOGIN = 0x0100;
    public static readonly uint AVCMF_OPEN_FOR_CALC_BELTHASH = 0x010000;
    public static readonly uint AVCMF_OPEN_FOR_CALC_BHFHASH = 0x020000;

    // Для функции AvCmOpenMsg
    // Входные данные будут вставлены в выходное сообщение как PKCS#7 Data
    public static readonly uint AVCMF_IN_RAW_DATA = 0x100;

    // Для функции AvCmSign AvCmEncrypt AvCmDecrypt AvCmSignAndEncrypt AvCmOpenMsg  
    // Входное сообщение в формате PKCS#7
    public static readonly uint AVCMF_IN_PKCS7 = 0x200;

    // Функция должна самостоятельно распределить память
    // для выходного сообщения функцией Win32 API HeapAlloc,
    // после использования выходного сообщения, вызвавшая программа
    // должна самостоятельно очистить выделенную память функцией HeapFree.
    public static readonly uint AVCMF_ALLOC = 0x1000000;

    // AvCmSetMsgContent: Добавить данные к содержимому сообщения 
    public static readonly uint AVCMF_APPEND = 0x4000000;

    // ----- AvCmLogin -----
    //
    public static readonly uint AVCM_DB_TYPE = 0x1;
    // Microsoft Windows
    public static readonly uint AVCM_DBT_MS_REGISTRY = 0x100;
    // Oracle
    public static readonly uint AVCM_DBT_ORACLE = 0x101;
    // Sybase Anywhere,                                 
    // Sybase OpenClient
    public static readonly uint AVCM_DBT_SYBASE = 0x102;
    //                                 
    public static readonly uint AVCM_DBT_FILE = 0x103;
    //                                          
    public static readonly uint AVCM_DBT_ARCHIVE_FILE = 0x104;
    //                                          
    public static readonly uint AVCM_DBT_ARCHIVE_MEMORY = 0x105;
    //                                                                                     
    public static readonly uint AVCM_DBT_E_MEMORY = 0x107;
    // #PKCS11       
    public static readonly uint AVCM_SLOTID = 0x10A;
    //                                               
    public static readonly uint AVCM_DB_HANDLE = 0x2;
    // DSN -                
    public static readonly uint AVCM_DB_DSN = 0x3;
    //                             
    public static readonly uint AVCM_DB_UID = 0x4;
    //                                
    public static readonly uint AVCM_DB_PWD = 0x5;
    // Microsoft Windows
    public static readonly uint AVCM_DB_MS_NAME = 0x6;
    // Microsoft Windows
    public static readonly uint AVCM_DB_MS_ROOT_NAME = 0x7;
    // Oracle
    public static readonly uint AVCM_DB_CONNECTSTR = 0x4;
    //                        
    public static readonly uint AVCM_DB_FILE_PATH = 0x3;
    //                                 
    public static readonly uint AVCM_DB_ARCHIVE_FILE_PATH = 0x3;
    //                                
    public static readonly uint AVCM_DB_ARCHIVE_PTR = 0x6;
    //                                    
    public static readonly uint AVCM_DB_ARCHIVE_SIZE = 0x7;
    //                                              
    public static readonly uint AVCM_PASSWORD = 0x1030;
    // A CommonName                     
    public static readonly uint AVCM_COMMON_NAME = 0x1031;

    // 4.0.0
    public static readonly uint AVCM_HASH_VALUE = 0x402;
    public static readonly uint AVCM_UNP = 0x1065;

    // ----- AvCmOpenCertEnum -----
    //                                       
    public static readonly uint AVCM_AUTHORITY_KEY_IDENTIFIER = 0x100C;

    // (X.509 Name)
    public static readonly uint AVCM_ISSUER_AS_STRING = 0x100D;
    //
    public static readonly uint AVCM_SERIAL_AS_STRING = 0x100E;
    //
    public static readonly uint AVCM_PUB_KEY_ID = 0x100F;
    //
    public static readonly uint AVCM_SERIAL_AS_INTEGER = 0x18;
    //     /
    public static readonly uint AVCM_NOT_BEFORE = 0x1A;
    //     /
    public static readonly uint AVCM_NOT_AFTER = 0x1B;
    //     /
    public static readonly uint AVCM_KEY_NOT_BEFORE = 0x2A;
    //     /
    public static readonly uint AVCM_KEY_NOT_AFTER = 0x2B;
    //
    public static readonly uint AVCM_D_GREATER = 0x110;
    //
    public static readonly uint AVCM_D_LESS = 0x111;
    // (X.509 Name)
    public static readonly uint AVCM_SUBJECT_AS_STRING = 0x101C;
    // (X.509 Name)
    public static readonly uint AVCM_SUBJECT_ATTR = 0x101D;
    //
    public static readonly uint AVCM_PUB_KEY = 0x1F;
    // (X.509 AltName)
    public static readonly uint AVCM_SUBJ_ALT_NAME_ATTR = 0x1020;
    //
    public static readonly uint AVCM_EXT_AS_STRING = 0x1024;
    //
    public static readonly uint AVCM_ATTR_AS_STRING = 0x1043;
    //
    public static readonly uint AVCM_PURPOSE = 0x25;
    //
    public static readonly uint AVCM_P_SIGN = 0x01;
    //
    public static readonly uint AVCM_P_CRYPT = 0x02;
    //
    public static readonly uint AVCM_P_NON_REPUDIABLE = 0x04;
    //
    public static readonly uint AVCM_TYPE = 0x26;
    //
    public static readonly uint AVCM_TYPE_MY = 0x10E;
    //
    public static readonly uint AVCM_TYPE_ROOT = 0x10F;
    // (X.509 Name)
    public static readonly uint AVCM_ISSUER_ATTR = 0x1032;
    // (OID)
    // (attr_param)
    public static readonly uint AVCM_EXT_KEY_USAGE_OID = 0x1027;
    //
    public static readonly uint AVCM_CERT_ISSUERS_CHAIN = 0x1029;
    //
    public static readonly uint AVCM_PUB_KEY_ALG_PARAMS = 0x31;

    // ----- AvCmFindCrl -----

    // Subject   AvCmImport   AvCmFindCrl
    public static readonly uint AVCM_CRL_ISSUER_SUBJECT = 0x1;
    //                                                                                     
    public static readonly uint AVCM_CRL_ISSUER_CERT = 0x2;

    // ----- AvCmGenerateRequest -----
    // AvCmGenerateRequest
    public static readonly uint AVCM_TEMPLATE = 0x2F;

    // Успешное выполнение функции
    public static readonly uint AVCMR_SUCCESS = 0;

    // ----- Атрибуты функции AvCmGetMsgParam -----

    // вид открытого сообщения.
    public static readonly int AVCM_FORMAT = 0xA;

    // данные не имеют структуры PKCS#7.
    public static readonly int AVCM_MF_RAW_DATA = 0x103;

    // данные не имеют структуры PKCS#7.
    public static readonly int AVCM_MF_NONE = 0x104;

    // подписанное сообщение в формате PKCS#7 SignedData.
    public static readonly int AVCM_MF_SIGNED_DATA = 0x105;

    // зашифрованное сообщение в формате PKCS#7 EnvelopedData.
    public static readonly int AVCM_MF_ENVELOPED_DATA = 0x106;

    // количество подписей в подписанном сообщении.
    public static readonly int AVCM_SIGN_COUNT = 0x107;

    // данные о формате вложенного сообщения
    public static readonly int AVCM_INNER_FORMAT = 0x108;

    // ----- Атрибуты функций AvCmGetSignAttr AvCmGetCertAttr AvCmOpenCertEnum AvCmGetRequestAttr -----

    // количество атрибутов имени (X.509 Name) владельца сертификата
    public static readonly int AVCM_SUBJECT_ATTR_COUNT = 0x1;

    // количество атрибутов имени (X.509 Name) издателя сертификата
    public static readonly int AVCM_ISSUER_ATTR_COUNT = 0x2;

    // идентификатор объекта (OID) атрибута имени владельца сертификата в виде строки
    public static readonly int AVCM_SUBJECT_ATTR_OID = 0x1033;

    // идентификатор объекта (OID) атрибута имени владельца сертификата в виде строки
    public static readonly int AVCM_ISSUER_ATTR_OID = 0x1034;

    // значение дополнения сертификата в виде BLOB
    public static readonly int AVCM_EXT_BLOB = 0x5;

    // значение атрибута сертификата в виде BLOB
    public static readonly int AVCM_ATTR_BLOB = 0x325;

    // идентификатор подписанного атрибута в виде строки ASCIIZ
    public static readonly int AVCM_AUTH_OID = 0x1040; //0x6;

    // идентификатор неподписанного атрибута в виде строки ASCIIZ
    public static readonly int AVCM_UNAUTH_OID = 0x1041; //0x7;

    // значение подписанного атрибута в виде BLOB
    public static readonly int AVCM_AUTH_BLOB = 0x8;

    // значение неподписанного атрибута в виде BLOB
    public static readonly int AVCM_UNAUTH_BLOB = 0x9;

    // версия подписи
    public static readonly int AVCM_VERSION = 0xB;

    // идентификатор алгоритма хэширования
    public static readonly int AVCM_HASH_ALG_OID = 0x1010;

    // идентификатор алгоритма подписи
    public static readonly int AVCM_SIGN_ALG_OID = 0x1011;

    // подпись
    public static readonly int AVCM_SIGN = 0x12;

    // дата и время выработки подписи, которые находятся в 
    // списке подписанных атрибутов сообщения.
    public static readonly uint AVCM_SIGN_DATE_TIME = 0x13;

    // количество подписанных атрибутов
    public static readonly int AVCM_AUTH_COUNT = 0x14;

    // количество количество неподписанных атрибутов
    public static readonly int AVCM_UNAUTH_COUNT = 0x15;

    // подписанный атрибут в виде строки ASCIIZ
    public static readonly int AVCM_AUTH_AS_STRING = 0x1016;

    // неподписанный атрибут в виде строки ASCIIZ
    public static readonly int AVCM_UNAUTH_AS_STRING = 0x1017;

    // Результат полной проверки корректности сертификата
    public static readonly int AVCM_VALID = 0x2C;

    // DER-представление сертификата
    public static readonly int AVCM_BLOB = 0x2D;

    // Получение режима сгенерированного запроса (совместимости с MS CA).
    public static readonly int AVCM_MSCA_COMPATIBLE = 0x30;

    // идентификатор алгоритма открытого ключа сертификата
    public static readonly int AVCM_PUB_KEY_ALG_OID = 0x101E;

    // количество дополнительных атрибутов
    public static readonly int AVCM_EXT_COUNT = 0x21;

    // идентификатор объекта (OID) дополнения
    public static readonly int AVCM_EXT_OID = 0x1022;

    // наименование идентификатора объекта (OID) дополнения
    public static readonly int AVCM_EXT_OID_NAME = 0x1026;

    // признак критичности дополнения
    public static readonly int AVCM_EXT_CRITICAL = 0x23;

    // количество элементов в списке ограничений применения ключа сертификата
    public static readonly int AVCM_EXT_KEY_USAGE_COUNT = 0x28;

    // наименование идентификатора объекта (OID) из списка ограничений 
    // применения ключа сертификата, в том случае, 
    // если данный OID зарегестрирован в операционной системе.  
    // При этом необходима передача номера либо идентификатора в списке (параметр attr_param).  
    public static readonly int AVCM_EXT_KEY_USAGE_NAME = 0x1028;

    // количество дополнительных атрибутов
    public static readonly int AVCM_ATTR_COUNT = 0x324;

    // идентификатор объекта (OID) дополнения
    public static readonly int AVCM_ATTR_OID = 0x1042;

    // наименование идентификатора объекта (OID) атрибута
    public static readonly int AVCM_ATTR_OID_NAME = 0x1044;

    // Параметры функции AvCmGetErrorInfo 
    // краткое описание ошибки
    public static readonly int AVCM_SHORT_STRING = 0x1;

    // подробное описание ошибки
    public static readonly uint AVCM_DESCRIPTION = 0x2;

    // код ошибки библиотеки. 
    // В этом случае в качестве указателя на выходной  буфер 
    // должен быть передан указатель на int.
    public static readonly int AVCM_ERROR_CODE = 0x3;

    // вернуть дескриптор импортированного и открытого объекта
    public static readonly int AVCM_RESULT_HANDLE = 0x2;

    // DER-представление сертификата
    public static readonly int AVCM_CERTIFICATE = 0x1;

    // DER-представление списка отозванных сертификатов
    public static readonly int AVCM_CRL = 0x2;

    // DER-представление запроса на сертификат в формате PKCS#10
    public static readonly int AVCM_PKCS10_REQUEST = 0x4;

    // DER-представление подписанной заявки на сертификат в формате PKCS#7 SignedData
    public static readonly int AVCM_PKCS7_REQUEST = 0x5;

    // DER-представление цепочки сертификатов (p7b)
    public static readonly int AVCM_PKCS7_CHAIN = 0x8;

    // DER-представление атрибутного сертификата
    public static readonly int AVCM_ATTRIBUTE_CERTIFICATE = 0x9;

    // Используется совместно с AvCmLogin:AVCM_DBT_E_MEMORY
    public static readonly int AVCM_STORES = 0xC9;

    // дата/время издания списка отозванных  сертификатов
    public static readonly int AVCM_THIS_UPDATE = 0x1A;

    // дата/время окончания действия списка отозванных  сертификатов
    public static readonly int AVCM_NEXT_UPDATE = 0x1B;

    // SHA-1 хэш от DER-представление списка отозванных сертификатов
    public static readonly int AVCM_SHA1_HASH = 0x2E;

    // дата/время отзыва сертификата
    public static readonly int AVCM_CS_REVOCATION_TIME = 0x301;

    // причина отзыва сертификата
    public static readonly int AVCM_CS_REVOCATION_REASON = 0x302;

    // причина недоверия сертификату
    public static readonly int AVCM_CS_UNTRUST_REASON = 0x303;

    // режим проверки статуса
    public static readonly int AVCM_CHECK_MODE = 0x401;

    // проверка статуса с использованием локального СОС
    public static readonly int AVCM_CM_OFFLINE = 0x402;

    // проверка статуса сертификата с обращением
    public static readonly int AVCM_CM_ONLINE = 0x403;

    // адрес OCSP сервера
    public static readonly int AVCM_RESPONDER_URL = 0x404;

    // Список точек распостранения  СОС из сертификата
    public static readonly int AVCM_CRL_DISTRIBUTION_POINTS = 0x1035;

    // Каталог по умолчанию для выбора импортируемого файла
    public static readonly int AVCM_IMPORT_PATH = 0x1036;

    // Каталог по умолчанию для сохранения файла созданного запроса
    public static readonly int AVCM_EXPORT_PATH = 0x1037;

    // Указывает на необходимость создания информационного файла с описанием выполненных операций. 
    // Полный путь и имя файла описания выполненной операции
    public static readonly int AVCM_OPERATION_REPORT = 0x1038;

    // AvCmGenerateRequest – сгенерировать запрос в продолжение переданного сертификата
    public static readonly int AVCM_CERT_PROLONGATION = 0x405;

    // AvCmImport - импорт сертификатов и/или СОС из файла
    public static readonly int AVCM_ANY_FILE = 0x406;

    // AvCmImport: вернуть дескриптор объекта, если он присутствует в справочниках
    public static readonly int AVCMF_RETURN_HANDLE_IF_EXISTS = 0x1;

    // Для функции AvCmVerifySign AvCmDecryptAndVerifySign 
    // Не заполнять  выходное сообщение
    public static readonly uint AVCMF_NO_OUTPUT = 0x200000;

    // Для функции AvCmVerifySign и AvCmDecryptAndVerifySign
    // Импорт сертификаты и списки отозванных сертификатов 
    // из сообщения в хранилище сертификатов. 
    public static readonly uint AVCMF_IMPORT = 0x10;

    // Для функции AvCmImport
    // после импорта сертификатов обязательно вывести диалог выбора контейнера с личным ключом
    public static readonly uint AVCMF_SELECT_MY_CERT = 0x2;

    // Для функции AvCmGetErrorInfo
    // Получить ошибку сессии по идентификатору вызывающего потока.
    public static readonly uint AVCMF_THREAD_ERROR = 0x08;

    // AvCmMsgVerifySign, AvCmMsgVerifySignAtIndex, AvCmVerifyRawDataSign 
    // Игнорировать отсутствие подходящего СОС
    public static readonly uint AVCMF_NO_CRL_VERIFY = 0x20;

    // ----- Атрибуты функции AvCmGetObjectInfo -----

    // Получение дескриптора личного сертификата. 
    public static readonly uint AVCM_MY_CERT = 0x8;

    // Получение количество дочерних объектов.
    public static readonly uint AVCM_CHILDREN_COUNT = 0x9;

    // Для функции AvCmGetSignAttr AvCmGetCertAttr
    // Атрибут по номеру.
    public static readonly uint AVCMF_ATTR_BY_NUM = 0x400000;

    // Для функции AvCmGetSignAttr AvCmGetCertAttr 
    // Атрибут по его идентификатору объекта (OID).
    public static readonly uint AVCMF_ATTR_BY_OID = 0x800000;

    // Для функции AvCmEnumGet
    // Получить следующий по порядку элемент перечисления.
    public static readonly uint AVCMF_NEXT = 0x40;

    // Для функции AvCmEnumGet
    // Начать перебор сертификатов с начала.
    public static readonly uint AVCMF_START = 0x80;

    // Для функции AvCmOpenCertEnum
    // Передача дескриптора сертификата для выборки атрибутных сертификатов
    public static readonly uint AVCM_ATTRIBUTE_CERTS = 0x323;

    // Объект не найден
    public static readonly uint AVCMR_NOT_FOUND = 0x25 + 0xE82A0100;

    // Функция не реализована
    public static readonly uint AVCMR_NOT_IMPLEMENTED = 0x26 + 0xE82A0100;

    //AvCmMsgSetAttribute
    public static readonly uint AVCM_PKCS7_CERTS = 0x340;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    struct ImageInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255 /* this must be synchronized with the C++ code! */)]
        public string barcodeType;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct da_i2k_input_file_info
    {
        [MarshalAs(UnmanagedType.LPTStr)]
        public string image_path;

        [MarshalAs(UnmanagedType.LPArray)]
        public string[] image_files;

        public int num_images;
    }
    /*
    void SomeUnsafeMethod(IntPtr ptr)
    {
        unsafe
        {
            var vptr = (void*)ptr;
            // do something with the void pointer
        }
    }
    */
}