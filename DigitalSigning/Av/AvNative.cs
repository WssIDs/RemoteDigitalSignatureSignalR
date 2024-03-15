using DigitalSigning.Av.Base64;
using DigitalSigning.Logging;
using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace DigitalSigning.Av;

/// <summary>
/// Класс нативных вызовов AvCryptMail
/// </summary>
public static partial class AvNative
{
    /// <summary>
    /// Вспомогательный класс для возможности инициализировать и удалять элементы массива
    /// </summary>
    private static class Arrays
    {
        public static T[] InitializeWithDefaultInstances<T>(int length) where T : new()
        {
            var array = new T[length];
            for (var i = 0; i < length; i++)
            {
                array[i] = new T();
            }

            return array;
        }
        public static string[] InitializeStringArrayWithDefaultInstances(int length)
        {
            var array = new string[length];
            for (var i = 0; i < length; i++)
            {
                array[i] = string.Empty;
            }

            return array;
        }
        public static void DeleteArray<T>(IEnumerable<T> array) where T : System.IDisposable
        {
            foreach (var element in array)
            {
                element?.Dispose();
            }
        }
    }
    /// <summary>
    /// Класс контейнера информации об ошибке
    /// </summary>
    public class Error
    {
        /// <summary>
        /// 
        /// </summary>
        public Error() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public Error(Error obj)
        {
            State = obj.State;
            Code = obj.Code;
            Result = obj.Result;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code = "0x0";

        /// <summary>
        /// 
        /// </summary>
        public string Result { get; set; } = string.Empty;
    }
    /// <summary>
    /// Класс контейнера сертификатного атрибута
    /// </summary>
    public class CertAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Oid { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public readonly Error Error = new();

        /// <summary>
        /// 
        /// </summary>
        public CertAttribute(){}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public CertAttribute(CertAttribute obj)
        {
            State = obj.State;
            Oid = obj.Oid;
            Title = obj.Title;
            Value = obj.Value;
            Error = obj.Error;
        }
    }
    /// <summary>
    /// Класс контейнера сертификата
    /// </summary>
    public class Cert
    {
        /// <summary>
        /// 
        /// </summary>
        public Cert() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public Cert(Cert obj)
        {
            State = obj.State;
            Attributes = obj.Attributes;
            Name = obj.Name;
            SerialNumber = obj.SerialNumber;
            ThumbPrint = obj.ThumbPrint;
            PublicKeyId = obj.PublicKeyId;
            NotBefore = obj.NotBefore;
            NotAfter = obj.NotAfter;
            PublicKey = obj.PublicKey;
            Error = obj.Error;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool State;

        /// <summary>
        /// 
        /// </summary>
        public readonly List<CertAttribute> Attributes = new();

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// серийный номер сертификата
        /// </summary>
        public string SerialNumber { get; set; } = string.Empty;

        /// <summary>
        /// контрольная характеристика
        /// </summary>
        public string ThumbPrint { get; set; } = string.Empty; 

        /// <summary>
        /// идентификатор ключа субъекта (CertId)
        /// </summary>
        public string PublicKeyId { get; set; } = string.Empty; 

        /// <summary>
        /// дата начала срока действия сертификата
        /// </summary>
        public DateTime NotBefore { get; set; }

        /// <summary>
        /// дата окончания срока действия сертификата
        /// </summary>
        public DateTime NotAfter { get; set; }

        /// <summary>
        /// открытый ключ сертификата
        /// </summary>
        public string PublicKey { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public readonly Error Error = new();


        public CertAttribute GetAttributeByOid(string oid)
        {
            return Attributes.FirstOrDefault(a => a.Oid == oid);
        }
    }
    /// <summary>
    /// Класс контейнера списка сертификатов
    /// </summary>
    public class CertList
    {
        /// <summary>
        /// 
        /// </summary>
        public CertList() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public CertList(CertList obj)
        {
            State = obj.State;
            Certs = obj.Certs;
            Error = obj.Error;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Cert> Certs { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public Error Error = new();
    }
    /// <summary>
    /// Класс контейнера электронной подписи
    /// </summary>
    public class Sign
    {
        /// <summary>
        /// 
        /// </summary>
        public Sign() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public Sign(Sign obj)
        {
            State = obj.State;
            Error = obj.Error;
            Date = obj.Date;
            Cert = obj.Cert;
            Valid = obj.Valid;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Error Error { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public string Date { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public Cert Cert { get; set; } = new();

        /// <summary>
        /// 
        /// </summary>
        public bool Valid { get; set; }
    }

    /// <summary>
    /// Класс контейнера результата выполнения функции/метода
    /// </summary>
    public class Common
    {
        /// <summary>
        /// 
        /// </summary>
        public Common()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public Common(Common obj)
        {
            Data = obj.Data;
            State = obj.State;
            Value = obj.Value;
            ExtraValue = obj.ExtraValue;
            ExtraValueInBytes = obj.ExtraValueInBytes;
            Error = obj.Error;
        }

        public byte[] Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExtraValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] ExtraValueInBytes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Error Error { get; set; } = new();
    }

    public class XaDesSignData
    {
        public XaDesSignData()
        {
            State = false;
            Error = new Error();
        }

        public string SignValue { get; set; }
        
        public string Certificate { get; set; } 
        
        public bool State { get; set; }

        public Error Error { get; set; }
    }

#pragma warning disable 0649

    /// <summary>
    /// Данные необходимые при подключении для идентификации пользователя
    /// и подключения к справочникам сертификатов/СОС.
    /// В настоящее время планируется хранение
    /// справочников сертификатов в базе данных
    /// [StructLayout(LayoutKind.Sequential, Pack=1)]
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AvCmConnectionParam
    {
        /// <summary>
        /// идентификатор параметра
        /// </summary>
        public uint ParamId;

        /// <summary>
        /// значение параметра (string)
        /// </summary>
        public nint Param;
    }

    /// <summary>
    /// Атрибуты поиска объектов
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct AvCmEnumGetParam
    {
        /// <summary>
        /// идентификатор параметра
        /// </summary>
        public uint ParamId;

        /// <summary>
        /// уточнение идентификатора
        /// </summary>
        public uint ParamSpec; 

        /// <summary>
        /// значение параметра
        /// </summary>
        public uint Param;
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IBaseParam
    {
        /// <summary>
        /// 
        /// </summary>
        public uint ParamId { get; set; }

        /// <summary>
        /// значение параметра
        /// </summary>
        public object Param { get; set; }
    }

    /// <summary>
    /// Атрибуты импорта объектов
    /// </summary>
    public struct AvCmImportParam
    {
        /// <summary>
        /// 
        /// </summary>
        public uint ParamId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Param { get; set; }
    }

    /// <summary>
    /// Данные используемые при поиске списков отозванных сертификатов
    /// </summary>
    public struct AvCmFindCrlParam
    {
        /// <summary>
        /// 
        /// </summary>
        public uint ParamId { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public object Param { get; set; }
    }

    /// <summary>
    /// Данные используемые при генерации запроса на сертификат
    /// </summary>
    public struct AvCmGenReqParam : IBaseParam
    {
        /// <summary>
        /// 
        /// </summary>
        public uint ParamId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Param { get; set; }
    }

    /// <summary>
    /// Данные используемые при поиске запроса на сертификат
    /// </summary>
    public struct AvCmFindReqParam : IBaseParam
    {
        /// <summary>
        /// 
        /// </summary>
        public uint ParamId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Param { get; set; }
    }

    /// <summary>
    /// Данные используемые при запросе статуса сертификата
    /// </summary>
    public struct AvCmCertStatParam : IBaseParam
    {
        /// <summary>
        /// 
        /// </summary>
        public uint ParamId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Param { get; set; }
    }

#pragma warning restore 0649


    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]

    [return: MarshalAs(UnmanagedType.Bool)]

    public static extern bool SetDllDirectory(string lpPathName);

    // C# doesn't support varargs so all arguments must be explicitly defined.
    // CallingConvention.Cdecl must be used since the stack is cleaned up by the caller.

    // int printf( const char *format [, argument]... )

    [DllImport("msvcrt.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int printf(string format, int i, double d);

    [DllImport("msvcrt.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int printf(string format, int i, string s);

    //calling: NativeMethods.printf("\nPrint params: %i %f", 99, 99.99);

    // Инициализация библиотеки.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    public static extern uint AvCmInit(uint flags);

    // Функция авторизации пользователя и создания сессии.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    //internal static extern uint AvCmLogin(int conn_param_count, ref object conn_params, out UIntPtr hc, uint flags);
    /* original code
    internal static extern uint AvCmLogin(int conn_param_count, AvCmConnectionParam[] conn_params, 
        out nuint hc, uint flags);
    */
    internal static extern uint AvCmLogin(int connParamCount, IntPtr connParams,
        out UIntPtr hc, uint flags);

    // Функция закрытия сессии.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmLogout(UIntPtr hc, uint flags);

    // Функция для сброса данных сессии 
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmFlush(UIntPtr hc, uint flags);

    // Функция генерации подписанного сообщения.
    //AVCM_FUNCTION AvCmSign(AvCmHc hc, const void* input_message, AvCmSize input_size, void* output_buffer,
    //     AvCmSize * output_size, AvCmLong flags);
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmSign(UIntPtr hc, byte[] inputMessage, uint inputSize,
        out IntPtr outputBuffer, out uint outputSize, uint flags);

    // Функция проверки подписи и получения исходного сообщения.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmVerifySign(nuint hc, object inputMessage, uint inputSize,
        object[] hsgnCertEnum, object outputBuffer, ref uint outputSize, uint flags);

    // Функция зашифрования сообщения.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmEncrypt(nuint hc, object inputMessage, uint inputSize,
        uint certCount, object[] certificates, object outputBuffer, ref uint outputSize, uint flags);

    // Функция расшифрования полученного зашифрованного сообщения.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmDecrypt(nuint hc, object inputMessage, uint inputSize,
        object outputBuffer, ref uint outputSize, uint flags);

    // Функция генерации подписанного сообщения с последующим его зашифрованием.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmSignAndEncrypt(nuint hc, object inputMessage, uint inputSize,
        uint certCount, object[] certificates, object outputBuffer, ref uint outputSize, uint flags);

    // Функция расшифрования сообщения с последующей проверкой подписи, 
    // и получения исходного сообщения
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmDecryptAndVerifySign(nuint hc, object inputMessage, uint inputSize,
        object[] hsgnCertEnum, object outputBuffer, ref uint outputSize, uint flags);

    // Функция получения параметров открытой объекта библиотеки. 
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmGetObjectInfo(nuint handle, uint paramId, object outputBuffer,
        ref uint outputSize, uint flags);

    // Функция создания копии объекта, 
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmDuplicateHandle(nuint sourceHandle, nuint destHc,
        object[] copyOfHandle, uint flags);

    // Функция закрытия дескриптора объекта библиотеки, 
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmCloseHandle(UIntPtr handle, uint flags);

    // Функция создания или открытия и разбора подписанного 
    [DllImport("AvCryptMail", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmOpenMsg(UIntPtr hc, byte[] messageData, uint messageSize,
        out UIntPtr hMsg, uint flags);

    // Функция получения параметров открытого сообщения.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmGetMsgParam(UIntPtr hMsg, uint attrId, out IntPtr outputBuffer,
        out uint outputSize, uint flags);

    // Функция выработки подписи под сообщением.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmMsgSign(UIntPtr hMsg, uint flags);

    // Функция проверки подписи в открытом подписанном сообщении.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmMsgVerifySign(UIntPtr hMsg, UIntPtr hCert, uint flags);

    // Функция зашифрования открытого сообщения.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmMsgEncrypt(nuint hMsg, uint certCount, object[] certificates,
        uint flags);

    // Функция расшифрования открытого зашифрованного сообщения.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmMsgDecrypt(UIntPtr hMsg, uint flags);

    // Функция получения сгенерированного сообщения 
    // в одном из экспортируемых форматов.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmGetMsg(nuint hMsg, object outputBuffer, ref uint outputSize,
        uint flags);

    // Функция извлечения содержимого сообщения.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmGetMsgContent(nuint hMsg, object outputBuffer, ref uint outputSize,
        uint flags);

    // Функция установки содержимого сообщения.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmSetMsgContent(nuint hMsg, object inputBuffer, uint inputSize,
        uint flags);

    // Функция извлечения одной из подписей данного подписанного сообщения.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmGetMsgSign(UIntPtr hMsg, uint signNumber, out UIntPtr hSign, uint flags);

    // Функция извлечения атрибутов подписи.
    //C++ AVCM_FUNCTION AvCmGetSignAttr(AvCmHandle handle, AvCmLong attr_id, const void* attr_param, void* output_buffer, AvCmSize * output_size, AvCmLong flags);
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    /*
internal static extern uint AvCmGetSignAttr(nuint handle, uint attr_id, byte[] attr_param,
out nint output_buffer, out uint output_size, uint flags);
*/
    internal static extern uint AvCmGetSignAttr(UIntPtr handle, uint attrId, byte[] attrParam,
        out IntPtr outputBuffer, out uint outputSize, uint flags);

    // Функция поиска сертификата по его подписи.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmFindCertBySign(UIntPtr hSign, out UIntPtr hCert, uint flags);

    // Функция извлечения атрибутов сертификата.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmGetCertAttr(UIntPtr handle, uint attrId, byte[] attrParam,
        out IntPtr outputBuffer, out uint outputSize, uint flags);

    // Функция создания контекста поиска сертификата путем 
    // перебора подмножества сертификатов, удовлетворяющих определенным условиям.
    // TODO исправить
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmOpenCertEnum(UIntPtr hc, int paramCount, AvCmEnumGetParam[] @params,
        out UIntPtr hCertEnum, uint flags);

    // Функция перебора объектов в открытом перечислении.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmEnumGet(UIntPtr hEnum, out UIntPtr handle, uint flags);

    // Функция возвращает различные параметры возникшей 
    // в процессе работы одной из функций библиотеки ошибки.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmGetErrorInfo(nuint hc, uint errorCode, uint paramId,
        string outputBuffer, ref uint outputSize, uint flags);

    // Функция добавления сертификата в сообщение
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmMsgAddCert(object hMsg, uint certCount, object[] certificates,
        uint flags);

    // Функция импорта сертификатов из сообщения
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmMsgImportCerts(object hMsg, uint flags);

    // Функция генерации СТБ-подписи
    // deprecated
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmMsgGetSTBSign(UIntPtr hMsg, out IntPtr pBuffer, out uint pBufferSize, uint flags);

    // Функция показа информации об объекте
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmShowObjectInfo(object handle, string cpszDlgCaption, string cpszLabel,
        string cpszOkButtonCaption, uint flags);

    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmShowObjectInfoExt(object handle, uint hWnd, object[] handle2,
        uint flags);

    // Функция добавления сертификатов в контекст
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmEnumAddCerts(object hCertEnum, uint paramCount,
        AvCmEnumGetParam @params, uint flags);

    // Функция проверки подписи в открытом подписанном сообщении по номеру
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmMsgVerifySignAtIndex(object hMsg, uint signIndex, uint flags);

    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmMsgUpdate(UIntPtr hMsg, byte[] pBuffer, uint iBufferSize,
        out UIntPtr pOut, out uint pOutSize, uint flags);

    // Функция проверки подписи в открытом подписанном сообщении по номеру на заданную дату
    /*AVCM_FUNCTION AvCmMsgVerifySignAtIndexForDate(
        AvCmHmsg hmsg,
        SystemTime* verifydate, 
        AvCmSize sign_index,
        AvCmLong flags);*/

    // Функция установки дополнительных атрибутов открытого сообщения для подписи
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmMsgSetAttribute(object hMsg, uint attrId, object attrParam,
        object inputBuffer, uint inputSize, uint flags);

    // Инициализация библиотеки с указанием рабочего каталога.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmInitEx(string cpszWorkDir, uint flags);

    // Функция открытия вложенного сообщения
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmOpenInnerMsg(object hMsg, object[] hmsgInner, uint flags);

    // Функция диалога создания контекста
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmEnumDlg(nuint hc, string cpszDlgCaption, string cpszLabel,
        string cpszOkButtonCaption, object[] hcertEnum, uint flags);

    // Функция импорта сертификата или списка отозванных сертификатов
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmImport(UIntPtr hc, uint objType, byte[] inputData, uint inputSize,
        int paramCount, AvCmImportParam[] @params, uint flags);

    // Функция поиска и открытия списка отозванных сертификатов
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmFindCrl(nuint hc, int paramCount, AvCmFindCrlParam[] @params,
        out object[] hCrl, uint flags);

    // Функция извлечения атрибутов списка отозванных сертификатов. 
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmGetCrlAttr(nuint hCrl, uint attrId, object outputBuffer,
        ref uint outputSize, uint flags);

    // Функция предназначена для генерации запроса на сертификат.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmGenerateRequest(nuint hc, uint paramCount, AvCmGenReqParam @params,
        object[] hReq, uint flags);

    // Функция предназначена для извлечения атрибутов запроса на сертификат.
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmGetRequestAttr(object hReq, uint attrId, object outputBuffer,
        ref uint outputSize, uint flags);

    //Функция предназначена для поиска и открытия запроса на сертификат. 
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmFindRequest(nuint hc, uint paramCount, AvCmFindReqParam @params,
        object[] hReq, uint flags);

    // Функция предназначена для помещения сертификата, извлеченного из сообщения в справочник сертификатов. 
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmPutCert(object handle, uint flags);
    
    // Функция предназначена для проверки статуса сертификата
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmVerifyCertStatus(object handle, uint paramCount,
        AvCmCertStatParam @params, ref uint statusOk, object[] hStatus, uint flags);

    // Функция предназначена для получения данных статуса сертификата
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmGetCertStatusAttr(object hStatus, uint attrId, object outputBuffer,
        ref uint outputSize, uint flags);

    //Функция генерации Raw ЭЦП 
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmSignRawData(UIntPtr hc, IntPtr hashAndSignOid, byte[] data,
        uint dataSize, out byte[] outputBuffer, out uint outputSize, uint flags);
    
    //Функция предназначена для проверки Raw ЭЦП 
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmVerifyRawDataSign(UIntPtr hCert, IntPtr hashAndSignOid,
        byte[] dataToBeVerified, uint dataSize, byte[] signValue, uint signSize, uint flags);

    /*
    //Функция открытия сертификата
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmOpenCert(UIntPtr hc, byte[] certificate, uint bufferSize, out UIntPtr hCert, uint flags);
    */

    //Функция установки дескриптора приложения
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmSetActiveWindow(uint hWnd, uint flags);

    //Получение информации о статусе сертификата
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmMsgOCSPGetResponse(object handle, uint signIndex, object[] hr,
        uint flags);

    //Извлечение атрибутов информации о статусе сертификата
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmMsgOCSPGetResponseAttr(object hr, uint attrId, object attrParam,
        object outputBuffer, ref uint outputSize, uint flags);

    //Добавление информации о статусе сертификата в подписанное сообщение
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmMsgOCSPAddResponse(object handle, uint signIndex, object hr,
        uint flags);

    //Функция открытия сертификата
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmOpenCert(UIntPtr hc, byte[] certificate, uint bufferSize, out UIntPtr hCert, uint flags);
    
    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmOpenCRL(nuint hc, object inputBuffer, uint bufferSize,
        object[] hCrl, uint flags);

    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmCreateScep(nuint hc, string url, object[] hScep, uint flags);

    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmScepExecute(object hScep, uint operationId, uint flags);

    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmScepGet(object hScep, uint attrId, object attrParam,
        object outputBuffer, ref uint outputSize, uint flags);

    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmScepSet(object hScep, uint attrId, object inputBuffer,
        uint bufferSize, uint flags);

    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmOpenEnum(nuint hc, object cryptSql, object[] hEnum, uint objType,
        uint flags);

    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmTLSCreateConnect(nuint hc, uint connParamCount,
        AvCmConnectionParam connParams, object[] hTls, uint flags);

    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmTlsGet(object hTsl, string url, object resData, ref uint resSize,
        uint flags);

    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmTlsPost(object hTsl, string url, object data, uint dataSize,
        object resData, ref uint resSize, uint flags);

    [DllImport("AvCryptMail", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern uint AvCmDebugLog(string app, string msg, string rem, uint flags);

    [DllImport("Kernel32.dll")]
    public static extern nint GetProcessHeap();

    [DllImport("Kernel32")]
    public static extern bool HeapFree(nint hHeap, uint flags, nint lpMem);

    [DllImport("kernel32", SetLastError=true)]
    private static extern bool GetSystemTime(out SystemTime systemTime);
    [DllImport("kernel32", SetLastError=true)]
    private static extern bool SetSystemTime(ref SystemTime systemTime);

    [StructLayout(LayoutKind.Sequential)]
    private readonly struct SystemTime
    {
        private readonly short wYear;
        private readonly short wMonth;
        private readonly short wDayOfWeek;
        private readonly short wDay;
        private readonly short wHour;
        private readonly short wMinute;
        private readonly short wSecond;
        private readonly short wMilliseconds;

        public override string ToString()
        {
            //return wYear.ToString() + "-" + wMonth.ToString() + "-" + wDay.ToString() + " "
            //        + wHour.ToString() + ":" + wMinute.ToString() + ":" + wSecond.ToString();

            //return string.Format("{0:D}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D3}",
            //        wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);

            return $"{wYear:D}-{wMonth:D2}-{wDay:D2} {wHour:D2}:{wMinute:D2}:{wSecond:D2}";
        }
    }

    /// <summary>
    /// Удаление пробельных символов в строке
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static string RemoveWhitespace(this string input)
    {
        return new string(input.ToCharArray()
            .Where(c => !char.IsWhiteSpace(c))
            .ToArray());
    }
    /// <summary>
    /// Извлечение из OU строки элементов по шаблону (C=,CN=...)
    /// </summary>
    /// <param name="s"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    private static string ExtractByPattern(string s, string pattern = "CN=")
    {
        const string stop = ",";
        const string eq = "=";

        var startIndex = s.IndexOf(pattern, 0, StringComparison.Ordinal);
        var stopIndex = s.IndexOf(stop, startIndex, StringComparison.Ordinal);

        // вариант, когда pattern последний в строке (корректируем длину по начальной строке)
        // либо содержит запятую в тексте (проверяем, встречается ли дальше знак = )
        if (stopIndex == -1 || s.IndexOf(eq, startIndex + pattern.Length, StringComparison.Ordinal) == -1)
        {
            stopIndex = s.Length;
        }

        return s.Substring(startIndex + pattern.Length, stopIndex - startIndex - pattern.Length);
    }

    ///
    /// работа с base64
    ///

        
    /// <summary>
    /// Строка -> байт-массив (с раскодировкой из base64, при необходимости)
    /// ВАЖНО: Encoding.Default обязательно для всяких "кириллиц"
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private static byte[] StringToByteArray(string s)
    {
        static bool IsBase64(string s)
        {
            // WARN20220211: как показала практика - как пришло, так пусть и обрабатывается
            return false;
            //return ((new Regex(@"^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$", RegexOptions.IgnoreCase)).IsMatch(s));
        }
        return IsBase64(s)
            ? Convert.FromBase64String(s)
            : Encoding.Default.GetBytes(s);
        /*
        return IsBase64Encoded(s)
            ? Encoding.Default.GetBytes(base64_decode(s))
            : Encoding.Default.GetBytes(s);
        */
    }
    /// <summary>
    /// Строка -> байт-массив (с раскодировкой из base64, при необходимости)
    /// ВАЖНО: Encoding.Default обязательно для всяких "кириллиц"
    /// Версия для DoxLogic
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    internal static byte[] StringOrBase64ToByteArrayDoxL(string s)
    {
        static bool IsBase64Str(string s)
        {
            return new Regex(@"^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$", RegexOptions.IgnoreCase).IsMatch(s);
        }
        return IsBase64Str(s)
            ? Convert.FromBase64String(s)
            : Encoding.Default.GetBytes(s);
    }

    /// <summary>
    /// Строка -> байт-массив (с раскодировкой из base64, при необходимости)
    /// ВАЖНО: Encoding.Default обязательно для всяких "кириллиц"
    /// Signature ver
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private static byte[] StringOrBase64ToByteArray(string s)
    {
        static bool IsBase64Str(string s)
        {
            return new Regex(@"^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)?$", RegexOptions.IgnoreCase).IsMatch(s);
        }
        return IsBase64Str(s)
            ? Convert.FromBase64String(s)
            : Encoding.Default.GetBytes(s);
    }

    ///
    ///--------------------- key processing -------------------------
    ///
    private static readonly Func<Type, uint> SizeOfType = (Func<Type, uint>)Delegate.CreateDelegate(typeof(Func<Type, uint>), typeof(Marshal).GetMethod("SizeOfType", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new InvalidOperationException());

    /// <summary>
    /// 
    /// </summary>
    public static void Shutdown()
    {
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);
    }

    /// <summary>
    /// Универсальный метод установления связи с библиотекой AvCryptMail
    /// по-дефолту авторизация включена, для работы в пассивном режиме
    /// (проверка хэша, подписи, т.д.) isAuthNeed=false
    /// </summary>
    public static Error AvCryptMailLogin(out nuint hc, string key, string password, bool isAuthNeed = true)
    {

        try
        {
            // в любом случае вначале завершаем активность AvCryptMail
            _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);

            if (!CheckAvCmResult(AvCmInit(AvConst.AVCMF_STARTUP)))
            {
                hc = (UIntPtr)null;
                return GetError("Ошибка вызова AvCmInit(AVCMF_STARTUP)", "0x0");
            }

            uint aRc;
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(password))
            {
                // массив параметров авторизации
                //AvCmConnectionParam[] @params = Arrays.InitializeWithDefaultInstances<AvCmConnectionParam>(2);
                uint AVCM_DBT_MS_REGISTRY = 0x100;
                //uint AVCM_DBT_E_MEMORY = 0x107;
                //uint ERROR = 0x777;
                var @params = new AvCmConnectionParam[3];
                unsafe
                {
                    var pKey = Marshal.StringToCoTaskMemAnsi(key);
                    var pPwd = Marshal.StringToCoTaskMemAnsi(password);

                    @params[0].Param = pKey;
                    @params[0].ParamId = AvConst.AVCM_PUB_KEY_ID;

                    @params[1].Param = pPwd;
                    @params[1].ParamId = AvConst.AVCM_PASSWORD;

                    @params[2].Param = (nint)(&AVCM_DBT_MS_REGISTRY);
                    @params[2].ParamId = AvConst.AVCM_DB_TYPE;

                    //int size = Marshal.SizeOf (typeof (AvCmConnectionParam));
                    //nint ptr = Marshal.AllocHGlobal(size*2);

                    fixed (AvCmConnectionParam* pBuffer = @params)
                    {
                        // подключаемся к библиотеке
                        aRc = AvCmLogin(2, (nint)pBuffer, out hc, AvConst.AVCMF_SILENT);
                    }
                    Marshal.FreeCoTaskMem(pKey);
                    Marshal.FreeCoTaskMem(pPwd);
                }
            }
            else
            {
                // если не нужна авторизация (проверка хэша, проверка подписи, и т.д. -
                // parameters = (AvConst.AVCMF_SILENT | AvConst.AVCMF_NO_AUTH)
                // в противном случае parameters = 0
                var parameters = isAuthNeed ? 0 : (AvConst.AVCMF_SILENT | AvConst.AVCMF_NO_AUTH);
                // массив параметров авторизации
                // так как теперь реализовано чз nint, передаем nint.Zero
                //TODO раскомментировать
                //AvCmConnectionParam[] @params = null;
                var @params = IntPtr.Zero;

                aRc = AvCmLogin(0, @params, out hc, parameters);
            }

            // возвращаем расширенное описание ошибки
            if (!CheckAvCmResult(aRc))
            {
                return GetError(aRc);
            }


            // вcё ок
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        var rs = new Error { State = true };
        return new Error(rs);
    }

    /// <summary>
    /// Функция обработки кода возврата и вывода сообщения об ошибке
    /// </summary>
    /// <param name="aRc"></param>
    /// <returns></returns>
    private static bool CheckAvCmResult(uint aRc) //, out string outBuf
    {
        var errBuff = new string(new char[4096]);
        uint outputSize = sizeof(char);

        //outBuf = "Ok";

        if (aRc == AvConst.AVCMR_SUCCESS) return true;
        var rc = AvCmGetErrorInfo((nuint)0, aRc, AvConst.AVCM_DESCRIPTION,
            errBuff, ref outputSize, AvConst.AVCMF_THREAD_ERROR);

        //var error = GetError(rc);
        //outBuf = rc != Constants.AVCMR_SUCCESS ? "Функция выполнена успешно" : err_buff;
        return false;

    }
    /// <summary>
    /// Возвращает статус ошибки
    /// </summary>
    /// <param name="message"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    private static Error GetError(string message, string code = null)
    {
        var rs = new Error();
        if (!string.IsNullOrEmpty(code))
        {
            rs.Code = code;
        }

        rs.Result = message;
        if (!CheckAvCmResult(AvCmInit(AvConst.AVCMF_SHUTDOWN))) {
            rs.Result = "Ошибка вызова AvCmInit(AVCMF_SHUTDOWN)";
        }
        //AvCmInit(AvConst.AVCMF_SHUTDOWN);

        return new Error(rs);
    }
    /// <summary>
    /// Возвращает статус ошибки
    /// </summary>
    /// <param name="res"></param>
    /// <returns></returns>
    private static Error GetError(uint res)
    {
        var errBuff = new string(new char[4096]);
        uint outputSize = sizeof(char);

        var rc = AvCmGetErrorInfo((nuint)0, res, AvConst.AVCM_DESCRIPTION,
            errBuff, ref outputSize, AvConst.AVCMF_THREAD_ERROR);
        if (rc != AvConst.AVCMR_SUCCESS)
        {
            var ss = new StringBuilder("Ошибка AvCmGetErrorInfo ");
            ss.Append(rc);
            ss.Append('.');
            //var ss << "Ошибка AvCmGetErrorInfo " << std::hex << rc << ".";
            return GetError(ss.ToString(), "0x0");
        }

        var sb = new StringBuilder();
        //sb.Append(err_buff); 

        sb.Append(AvErrorCodes.MapErrToString(res));
        //string hex_code = $" (0x{res:X})";
        //sb.Append(hex_code);
        var text = sb.ToString();
        return GetError(text, $"0x{res:X}");
    }

    //public static bool TryCheckCrl(IEnumerable<Crl> crls, out IEnumerable<string> errors)
    //{
    //    try
    //    {
    //        AvCryptMailLogin(out var hc, "", "", false);

    //        foreach (var crl in crls)
    //        {
    //            var param = new AvCmFindCrlParam
    //            {
    //                param = Marshal.StringToCoTaskMemAnsi(crl.Name+ "\0"),
    //                param_id = 0x1
    //            };

    //            var result = AvCmFindCrl(hc,0, param, out var test,0);

    //            var error = GetError(result);
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine(e);
    //        throw;
    //    }

    //    _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);

    //    errors = null;
    //    return true;
    //}

    //---

    /// <summary>
    /// Подпись detached (без вложения исходного сообщения)
    /// Данные в input передаются на AvCryptMail как byte[]
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Common SignDetachedAsByteArrayUn(byte[] input, string key = "", string password = "")
    {
        var result = new Common();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new Common(result);
        }

        // данные для подписи

        // обязательно передавать флаг AVCMF_ADD_SIGN_CERT, чтобы в подпись включался сертификат подписанта
        // или AVCMF_ADD_ONLY_CERT
        var aRc = AvCmSign(hc, input, (uint)input.Length, out var buffer, out var bufferSize,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_DETACHED | AvConst.AVCMF_ADD_ONLY_CERT |
            AvConst.AVCMF_ALLOC); // | AvConst.AVCMF_ADD_SIGN_CERT
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Common(result);
        }

        var buffManaged = new byte[bufferSize]; // managed массив под данные результата
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // обязательно освободить память, тк юзали AVCMF_ALLOC
        HeapFree(GetProcessHeap(), 0, buffer);

        result.State = true;
        // конвертим результат в base64 строку (с переносами строк Base64FormattingOptions.InsertLineBreaks)
        result.Value = Convert.ToBase64String(buffManaged, 0, buffManaged.Length, Base64FormattingOptions.None); // Base64FormattingOptions.InsertLineBreaks
        AvCmInit(AvConst.AVCMF_SHUTDOWN); // завершаем сеанс работы с hw-ключом

        return new Common(result);
    }
    /// <summary>
    /// Подпись detached (без вложения исходного сообщения)
    /// Данные в input передаются на AvCryptMail как есть
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Common SignDetachedUn(string input, string key = "", string password = "")
    {
        var result = new Common();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new Common(result);
        }

        // данные для подписи
        var decoded = StringToByteArray(input);

        // обязательно передавать флаг AVCMF_ADD_SIGN_CERT, чтобы в подпись включался сертификат подписанта
        // или AVCMF_ADD_ONLY_CERT
        var aRc = AvCmSign(hc, decoded, (uint)decoded.Length, out var buffer, out var bufferSize,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_DETACHED | AvConst.AVCMF_ADD_ONLY_CERT |
            AvConst.AVCMF_ALLOC); // | AvConst.AVCMF_ADD_SIGN_CERT


        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Common(result);
        }

        var buffManaged = new byte[bufferSize]; // managed массив под данные результата
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // обязательно освободить память, тк юзали AVCMF_ALLOC
        HeapFree(GetProcessHeap(), 0, buffer);

        result.State = true;
        // конвертим результат в base64 строку (с переносами строк Base64FormattingOptions.InsertLineBreaks)
        result.Value = Convert.ToBase64String(buffManaged, 0, buffManaged.Length, Base64FormattingOptions.None); // Base64FormattingOptions.InsertLineBreaks
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN); // завершаем сеанс работы с hw-ключом

        return new Common(result);
    }
    /// <summary>
    /// Подпись detached (без вложения исходного сообщения)
    /// Версия для СЭД DoxL
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Common SignDetachedDoxlUn(string input, string key = "", string password = "")
    {
        var result = new Common();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new Common(result);
        }

        // если передана base64-строка, для DoxLogic необходимо разобрать в обычную строку
        //byte[] decoded = StringOrBase64ToByteArrayDoxL(input); // закомментировал, сделал обработку base64
        var b64Encoder = Base64Encoder.Default;
        var decoded = b64Encoder.FromBase(input);

        // обязательно передавать флаг AVCMF_ADD_SIGN_CERT, чтобы в подпись включался сертификат подписанта
        var aRc = AvCmSign(hc, decoded, (uint)decoded.Length, out var buffer, out var bufferSize,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_DETACHED | AvConst.AVCMF_ADD_SIGN_CERT |
            AvConst.AVCMF_ALLOC); // | AvConst.AVCMF_ADD_SIGN_CERT
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Common(result);
        }

        var buffManaged = new byte[bufferSize]; // managed массив под данные результата
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // обязательно освободить память, тк использовали AVCMF_ALLOC
        HeapFree(GetProcessHeap(), 0, buffer);

        result.State = true;
        // конвертируем результат в base64 строку (с переносами строк Base64FormattingOptions.InsertLineBreaks)
        result.Value = Convert.ToBase64String(buffManaged, 0, buffManaged.Length, Base64FormattingOptions.None); // Base64FormattingOptions.InsertLineBreaks
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN); // завершаем сеанс работы с hw-ключом

        return new Common(result);
    }
    /// <summary>
    /// Подпись detached (без вложения исходного сообщения)
    /// Версия для Национального архива
    /// На вход подаётся без преобразования, предварительно используется "контрольная характеристика"
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Common SignDetachedNationalArchiveUn(string input, string key = "", string password = "")
    {
        var result = new Common();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new Common(result);
        }

        // для нац.архива данные приходят в ASCII (win-1251)
        // var adContentName = new String(input.getBytes("ISO8859-1"), "utf-8");
        var decoded = Encoding.Default.GetBytes(input);

        // обязательно передавать флаг AVCMF_ADD_SIGN_CERT, чтобы в подпись включался сертификат подписанта
        // или флаг AVCMF_ADD_ONLY_CERT
        var aRc = AvCmSign(hc, decoded, (uint)decoded.Length, out var buffer, out var bufferSize,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_DETACHED | AvConst.AVCMF_ADD_SIGN_CERT |
            AvConst.AVCMF_ALLOC); // | AvConst.AVCMF_ADD_SIGN_CERT
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Common(result);
        }

        var buffManaged = new byte[bufferSize];                // managed массив под данные результата
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length);  // копируем из unmanaged в managed

        // обязательно освободить память, тк использовали AVCMF_ALLOC
        HeapFree(GetProcessHeap(), 0, buffer);

        result.State = true;
        // конвертируем результат в base64 строку (с переносами строк Base64FormattingOptions.InsertLineBreaks)
        result.Value = Convert.ToBase64String(buffManaged, 0, buffManaged.Length, Base64FormattingOptions.None); // Base64FormattingOptions.InsertLineBreaks
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN); // завершаем сеанс работы с hw-ключом

        return new Common(result);
    }
    /// <summary>
    /// Подпись с вложением исходного сообщения
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Common SignAttachedUn(string input, string key = "", string password = "")
    {
        var result = new Common();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new Common(result);
        }

        // данные для хеширования и подписания из строки в bytearray
        var decoded = StringToByteArray(input);
        // обязательно передавать флаг AVCMF_ADD_SIGN_CERT, чтобы в подпись включался сертификат подписанта
        var aRc = AvCmSign(hc, decoded, (uint)decoded.Length, out var buffer, out var bufferSize,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_ADD_SIGN_CERT | AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Common(result);
        }

        var buffManaged = new byte[bufferSize]; // managed массив под данные результата
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // обязательно освободить память, т.к. AVCMF_ALLOC
        HeapFree(GetProcessHeap(), 0, buffer);

        result.State = true;
        // конвертируем результат в base64 строку (для переносов строк - Base64FormattingOptions.InsertLineBreaks)
        result.Value = Convert.ToBase64String(buffManaged, 0, buffManaged.Length, Base64FormattingOptions.None);
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN); // завершаем сеанс работы с hw-ключом

        return new Common(result);
    }

    /// <summary>
    /// Построение BELTHASH для блока данных
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Common GetHashUn(byte[] input, string key = "", string password = "")
    {
        var result = new Common();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password, false);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new Common(result);
        }

        // данные для хеширования из строки в bytearray
        //string input = "SWxpdmVvbjEyM3N0cmVldFVuaXRlZFN0YXRlc09mQW1lcmljYQ==";
        //string decoded = base64_decode(input);
        /*byte[] decoded = IsBase64Encoded(input)
            ? Encoding.Default.GetBytes(base64_decode(input))
            : Encoding.Default.GetBytes(input);*/
        //var decoded = StringToByteArray(input);

        var aRc = AvCmOpenMsg(hc, input, (uint)input.Length, out var hMsg,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_OPEN_FOR_CALC_BELTHASH);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new Common(result);
        }

        aRc = AvCmGetMsgParam(hMsg, AvConst.AVCM_HASH_VALUE, out var buffer, out var bufferSize, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new Common(result);
        }

        var buffManaged = new byte[bufferSize];
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // преобразовываем из byte[] в string.UTF8 HEX
        var hexHash = string.Join("", buffManaged.Select(c => ((int)c).ToString("X2")));

        HeapFree(GetProcessHeap(), 0, buffer);

        result.State = true;
        result.Value = hexHash;
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);
        return new Common(result);
    }

    /// <summary>
    /// Построение BELTHASH для блока данных
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Common GetHashUn(string input, string key = "", string password = "")
    {
        var result = new Common();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password, false);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new Common(result);
        }

        // данные для хеширования из строки в bytearray
        //string input = "SWxpdmVvbjEyM3N0cmVldFVuaXRlZFN0YXRlc09mQW1lcmljYQ==";
        //string decoded = base64_decode(input);
        /*byte[] decoded = IsBase64Encoded(input)
            ? Encoding.Default.GetBytes(base64_decode(input))
            : Encoding.Default.GetBytes(input);*/
        var decoded = StringToByteArray(input);

        var aRc = AvCmOpenMsg(hc, decoded, (uint)decoded.Length, out var hMsg,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_OPEN_FOR_CALC_BELTHASH);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new Common(result);
        }

        aRc = AvCmGetMsgParam(hMsg, AvConst.AVCM_HASH_VALUE, out var buffer, out var bufferSize, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new Common(result);
        }

        var buffManaged = new byte[bufferSize];
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // преобразовываем из byte[] в string.UTF8 HEX
        var hexHash = string.Join("", buffManaged.Select(c => ((int)c).ToString("X2")));

        HeapFree(GetProcessHeap(), 0, buffer);

        result.State = true;
        result.Value = hexHash;
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);
        return new Common(result);
    }

    public static Common GetHashBytes(byte[] input, string key = "", string password = "")
    {
        var result = new Common();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password, false);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new Common(result);
        }

        // данные для хеширования из строки в bytearray
        //string input = "SWxpdmVvbjEyM3N0cmVldFVuaXRlZFN0YXRlc09mQW1lcmljYQ==";
        //string decoded = base64_decode(input);
        /*byte[] decoded = IsBase64Encoded(input)
            ? Encoding.Default.GetBytes(base64_decode(input))
            : Encoding.Default.GetBytes(input);*/
        //var decoded = StringToByteArray(input);

        var aRc = AvCmOpenMsg(hc, input, (uint)input.Length, out var hMsg,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_OPEN_FOR_CALC_BELTHASH);
        
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new Common(result);
        }

        aRc = AvCmGetMsgParam(hMsg, AvConst.AVCM_HASH_VALUE, out var buffer, out var bufferSize, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new Common(result);
        }

        var buffManaged = new byte[bufferSize];
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed
        
        HeapFree(GetProcessHeap(), 0, buffer);

        result.State = true;
        result.Data = buffManaged;
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);
        return new Common(result);
    }

    public static Common GetHashSignInfoXml(byte[] input, string key = "", string password = "")
    {
        var result = new Common();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, string.Empty, string.Empty, false);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new Common(result);
        }

       
        var aRc = AvCmOpenMsg(hc, input, (uint)input.Length, out var hMsg, AvConst.AVCMF_OPEN_FOR_CALC_BELTHASH);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new Common(result);
        }

        aRc = AvCmGetMsgParam(hMsg, AvConst.AVCM_HASH_VALUE, out var buffer, out var bufferSize, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new Common(result);
        }

        var buffManaged = new byte[bufferSize];
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        HeapFree(GetProcessHeap(), 0, buffer);

        result.State = true;
        result.Data = buffManaged;
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);
        return new Common(result);
    }


    /// <summary>
    /// Построение BELTHASH для файла
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Common GetFileHashUn(string input, string key = "", string password = "")
    {
        var result = new Common();

        // в input имя файла
        try
        {
            if (!File.Exists(input))
            {
                result.Error = new Error
                {
                    State = false,
                    Code = "0x0",
                    Result = $"Файл {input} не найден"
                };

                return new Common(result);
            }

            // устанавливаем соединение с AvCryptMail
            var res = AvCryptMailLogin(out var hc, key, password, false);
            // ошибка
            if (!res.State)
            {
                result.Error = res;
                return new Common(result);
            }

            byte[] decoded;
            try
            {
                // try/catch так как может упасть при IOException
                decoded = File.ReadAllBytes(input);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception Message: " + e.Message);
                result.Error = new Error 
                { 
                    State = false,
                    Code = "0x0",
                    Result = e.Message
                };

                return new Common(result);
            }

            var aRc = AvCmOpenMsg(hc, decoded, (uint)decoded.Length, out var hMsg,
                AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_OPEN_FOR_CALC_BELTHASH);
            if (!CheckAvCmResult(aRc))
            {
                result.Error = res;
                return new Common(result);
            }

            aRc = AvCmGetMsgParam(hMsg, AvConst.AVCM_HASH_VALUE, out var buffer, out var bufferSize, AvConst.AVCMF_ALLOC);
            if (!CheckAvCmResult(aRc))
            {
                result.Error = res;
                return new Common(result);
            }

            var buffManaged = new byte[bufferSize];
            Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

            // преобразовываем из byte[] в string.UTF8 HEX
            var hexHash = string.Join("", buffManaged.Select(c => ((int)c).ToString("X2")));

            HeapFree(GetProcessHeap(), 0, buffer);

            result.State = true;
            result.Value = hexHash;
            _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);
            return new Common(result);
        }
        catch (Exception e)
        {
            Debug.WriteLine("Exception Message: " + e.Message);
            result.Error = new Error { State=false, Code="file read error", Result=e.Message };
            return new Common(result);
        }
        //return getHashAndSignUn(input, key, password);
    }
    /// <summary>
    /// Построение BELTHASH для блока данных и подписание
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Common GetHashAndSignUn(string input, string key = "", string password = "")
    {
        var result = new Common();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new Common(result);
        }

        // данные для хеширования и подписания из строки в bytearray
        var decoded = StringToByteArray(input);

        var aRc = AvCmOpenMsg(hc, decoded, (uint)decoded.Length, out var hMsg,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_OPEN_FOR_CALC_BELTHASH);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new Common(result);
        }

        aRc = AvCmGetMsgParam(hMsg, AvConst.AVCM_HASH_VALUE, out var buffer, out var bufferSize, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new Common(result);
        }

        var buffManaged = new byte[bufferSize];
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // преобразовываем из byte[] в string.UTF8 HEX
        var hexHash = string.Join("", buffManaged.Select(c => ((int)c).ToString("X2")));

        HeapFree(GetProcessHeap(), 0, buffer); // освобождаем память

        // теперь выработка подписи
        //---
        // вначале сюда передавалась hex_hash, но, определились с Максом, что это неправильно
        // и следует подписывать исходное сообщение
        decoded = StringToByteArray(input);

        // обязательно передавать флаг AVCMF_ADD_SIGN_CERT, чтобы в подпись включался сертификат подписанта
        aRc = AvCmSign(hc, decoded, (uint)decoded.Length, out buffer, out bufferSize,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_DETACHED | AvConst.AVCMF_ADD_SIGN_CERT |
            AvConst.AVCMF_ALLOC); // | AvConst.AVCMF_ADD_SIGN_CERT
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Common(result);
        }

        buffManaged = new byte[bufferSize];                      // managed массив под данные результата
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // обязательно освободить память, т.к. AVCMF_ALLOC
        HeapFree(GetProcessHeap(), 0, buffer);

        // конвертируем результат в base64 строку (с переносами строк Base64FormattingOptions.InsertLineBreaks)
        var hexHashSigned = Convert.ToBase64String(buffManaged, 0, buffManaged.Length, Base64FormattingOptions.None);
        //---
        result.Data = decoded;
        result.State = true;
        result.Value = hexHash; // hash входного блока данных
        result.ExtraValue = hexHashSigned; // подпись входного блока данных
        result.ExtraValueInBytes = buffManaged; // Подпись в bytes
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);
        return new Common(result);
    }
    
    /// <summary>
    /// Построение BELTHASH для файла и подписание
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Common GetHashFileAndSignUn(string input, string key = "", string password = "")
    {
        var result = new Common();

        if (!File.Exists(input))
        {
            result.Error = new Error 
            { 
                State = false,
                Code = "0x0",
                Result = $"Файл {input} не найден"
            };

            return new Common(result);
        }

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new Common(result);
        }

        // данные
        byte[] decoded;
        try {
            // try/catch так как может упасть при IOException
            decoded = File.ReadAllBytes(input);
        }
        catch (Exception e)
        {
            Debug.WriteLine("Exception Message: " + e.Message);

            result.Error = new Error
            {
                State = false,
                Code = "0x0",
                Result = e.Message
            };

            return new Common(result);
        }

        var aRc = AvCmOpenMsg(hc, decoded, (uint)decoded.Length, out var hMsg,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_OPEN_FOR_CALC_BELTHASH);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new Common(result);
        }

        aRc = AvCmGetMsgParam(hMsg, AvConst.AVCM_HASH_VALUE, out var buffer, out var bufferSize, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new Common(result);
        }

        var buffManaged = new byte[bufferSize];
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // преобразовываем из byte[] в string.UTF8 HEX
        var hexHash = string.Join("", buffManaged.Select(c => ((int)c).ToString("X2")));

        HeapFree(GetProcessHeap(), 0, buffer); // освобождаем память

        // теперь выработка подписи
        //---
        // вначале сюда передавалась hex_hash, но, определились с Максом, что это неправильно
        // и следует подписывать исходное сообщение
        //decoded = StringToByteArray(input);

        // обязательно передавать флаг AVCMF_ADD_SIGN_CERT, чтобы в подпись включался сертификат подписанта

        aRc = AvCmSign(hc, decoded, (uint)decoded.Length, out buffer, out bufferSize,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_DETACHED | AvConst.AVCMF_ADD_SIGN_CERT |
        AvConst.AVCMF_ALLOC); // | AvConst.AVCMF_ADD_SIGN_CERT
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Common(result);
        }

        buffManaged = new byte[bufferSize];                      // managed массив под данные результата
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // обязательно освободить память, т.к. AVCMF_ALLOC
        HeapFree(GetProcessHeap(), 0, buffer);

        // конвертируем результат в base64 строку (с переносами строк Base64FormattingOptions.InsertLineBreaks)
        var hexHashSigned = Convert.ToBase64String(buffManaged, 0, buffManaged.Length);
        //---
        result.Data = decoded;
        result.State = true;
        result.Value = hexHash; // hash входного блока данных
        result.ExtraValue = hexHashSigned; // подпись входного блока данных
        result.ExtraValueInBytes = buffManaged; // Подпись в bytes
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);
        return new Common(result);
    }


    /// <summary>
    /// Подписание XML Raw подписью без создания ASN1
    /// </summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static XaDesSignData SignXmlFile(byte[] data, string key = "", string password = "")
    {
        var result = new XaDesSignData();
        
        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new XaDesSignData { Error = result.Error };
        }

        // данные
        var decoded = data;

        // алгоритм подписи 1.2.112.0.2.0.34.101.45.12  = bign-with-hbelt согласно СТБ 34.101.45

        
        
        var aRc = AvCmOpenMsg(hc, decoded, (uint)decoded.Length, out var hMsg,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_OPEN_FOR_CALC_BELTHASH);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new XaDesSignData { Error = result.Error };
        }

        /*
        var algorithm = "1.2.112.0.2.0.34.101.45.12";
        var pAlg = Marshal.StringToCoTaskMemAnsi(algorithm);
        
        aRc = AvCmSignRawData(hc, pAlg, decoded, (uint)decoded.Length, out var sign, out var signSize,
            AvConst.AVCMF_RAW_SIGN | (uint)AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new XaDesSignData { Error = result.Error };
        }
        */

        aRc = AvCmMsgGetSTBSign(hMsg, out var Psgn, out var pSgnSize, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new XaDesSignData { Error = result.Error };
        }

        var signInByte = new byte[pSgnSize];                      
        Marshal.Copy(Psgn, signInByte, 0, (int)pSgnSize); 
        var sgn = Convert.ToBase64String(signInByte);  // Значение ЭЦП
        Marshal.FreeHGlobal(Psgn);

        // Поиск сертификата
        var myCertsStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        myCertsStore.Open(OpenFlags.ReadOnly);
        X509Certificate2 x509Certificate2 = null;

        foreach (X509Certificate2 cert in myCertsStore.Certificates)
        {
            var publicKey = string.Empty;
            // Ищем значение PublicKey
            foreach (var extension in cert.Extensions)
            {
                var asnData = new AsnEncodedData(extension.Oid, extension.RawData);
                switch (extension.Oid?.Value)
                {
                    case "2.5.29.14":
                        publicKey = new X509SubjectKeyIdentifierExtension(asnData, extension.Critical).SubjectKeyIdentifier ?? string.Empty;
                        break;
                }
            }
            
            if (publicKey.Equals(key))
            {
                x509Certificate2 = cert;
                break;
            }
        }

        if (x509Certificate2 == null)
        {
            return new XaDesSignData { 
                Error = new Error
                {
                    Code = "Не найден сертификат", 
                    Result = "Не найден сертификат в локальном хранилище сертификатов",
                    State = true
                }
            };
        }

        var certificate = x509Certificate2!.GetRawCertData();
        var certStr = Convert.ToBase64String(certificate);

        result.SignValue = sgn;
        result.Certificate = certStr;

        /*
        aRc = AvCmGetMsgParam(hMsg, AvConst.AVCM_HASH_VALUE, out var buffer, out var bufferSize, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new XaDesSignData { Error = result.Error };
        }

        var buffManaged = new byte[bufferSize];
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // преобразовываем из byte[] в string.UTF8 HEX
        var hexHash = string.Join("", buffManaged.Select(c => ((int)c).ToString("X2")));

        HeapFree(GetProcessHeap(), 0, buffer); // освобождаем память

        // теперь выработка подписи
        //---
        // вначале сюда передавалась hex_hash, но, определились с Максом, что это неправильно
        // и следует подписывать исходное сообщение
        //decoded = StringToByteArray(input);

        // обязательно передавать флаг AVCMF_ADD_SIGN_CERT, чтобы в подпись включался сертификат подписанта

        aRc = AvCmSign(hc, decoded, (uint)decoded.Length, out buffer, out bufferSize,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_ADD_SIGN_CERT | AvConst.AVCMF_DETACHED | AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new XaDesSignData { Error = result.Error };
        }

        buffManaged = new byte[bufferSize];                      // managed массив под данные результата
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // обязательно освободить память, т.к. AVCMF_ALLOC
        HeapFree(GetProcessHeap(), 0, buffer);

        // конвертируем результат в base64 строку (с переносами строк Base64FormattingOptions.InsertLineBreaks)
        var hexHashSigned = Convert.ToBase64String(buffManaged, 0, buffManaged.Length);
        
        
        // Открываем подписанное сообщение в формате PKCS для вытаскивания оттуда необходимых аттрибутов
        var resultForOpenSignMessage = AvCmOpenMsg(hc, buffManaged, (uint)buffManaged.Length, out var hOpenMessage,AvConst.AVCMF_IN_PKCS7);
        if (!CheckAvCmResult(resultForOpenSignMessage))
        {
            result.Error = GetError(resultForOpenSignMessage);
            return new XaDesSignData { Error = result.Error };
        }
        // Получаем из подписанного сообщения параметры ЭЦП
        var resultForSign = AvCmGetMsgSign(hOpenMessage, 0, out var hSign, 0);
        if (!CheckAvCmResult(resultForSign))
        {
            result.Error = GetError(resultForSign);
            return new XaDesSignData { Error = result.Error };
        }

        // Получаем значение ЭЦП
        var signResult = AvCmGetSignAttr(hSign, (uint)AvConst.AVCM_SIGN, null, out var signBuff, out var signSize, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(signResult))
        {
            result.Error = GetError(signResult);
            return new XaDesSignData { Error = result.Error };
        }
        var byteArray = new byte[signSize];
        Marshal.Copy(signBuff, byteArray, 0, (int)signSize);
        result.SignValue = Convert.ToBase64String(byteArray);

        // Получаем дату и время подписи
        var dateSignResult = AvCmGetSignAttr(hSign, (uint)AvConst.AVCM_SIGN_DATE_TIME, null, out var dateSignBuf, out var dateSignSize, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(dateSignResult))
        {
            result.Error = GetError(dateSignResult);
            return new XaDesSignData { Error = result.Error };
        }
        
        var st = (SystemTime)Marshal.PtrToStructure(dateSignBuf, typeof(SystemTime))!;
        Marshal.FreeHGlobal(dateSignBuf); // освобождаем память, так как было AVCMF_ALLOC
        result.SignDate = st.ToString();

        // Получаем серийный номер сертификата подписанта
        var serialNumberCertResult = AvCmGetSignAttr(hSign, (uint)AvConst.AVCM_SERIAL_AS_STRING, null, out var snBuf, out var snSize, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(serialNumberCertResult))
        {
            result.Error = GetError(serialNumberCertResult);
            return new XaDesSignData { Error = result.Error };
        }

        
        result.SignSerialNumberCertificate = Marshal.PtrToStringAnsi(snBuf);
        Marshal.FreeHGlobal(snBuf);
        */
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);

        result.State = true;
        
        return result;
    }


    /// <summary>
    /// Построение BELTHASH для файла и подписание
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Common GetHashDataAndSignUn(byte[] input, string key = "", string password = "")
    {
        var result = new Common();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new Common(result);
        }

        //var base64 = Convert.ToBase64String(input);
        //var utf8bytes = Encoding.Convert(Encoding.ASCII, Encoding.UTF8, input);
        //var utf8 = Encoding.UTF8.GetString(utf8bytes);
        //var base64Utf8 = Convert.ToBase64String(utf8bytes);
        //input = utf8bytes;

        var aRc = AvCmOpenMsg(hc, input, (uint)input.Length, out var hMsg,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_OPEN_FOR_CALC_BELTHASH);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new Common(result);
        }

        aRc = AvCmGetMsgParam(hMsg, AvConst.AVCM_HASH_VALUE, out var buffer, out var bufferSize, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            return new Common(result);
        }

        var buffManaged = new byte[bufferSize];
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // преобразовываем из byte[] в string.UTF8 HEX
        var hexHash = string.Join("", buffManaged.Select(c => ((int)c).ToString("X2")));

        HeapFree(GetProcessHeap(), 0, buffer); // освобождаем память

        // теперь выработка подписи
        //---
        // вначале сюда передавалась hex_hash, но, определились с Максом, что это неправильно
        // и следует подписывать исходное сообщение
        //decoded = StringToByteArray(input);

        // обязательно передавать флаг AVCMF_ADD_SIGN_CERT, чтобы в подпись включался сертификат подписанта

        aRc = AvCmSign(hc, input, (uint)input.Length, out buffer, out bufferSize,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_DETACHED | AvConst.AVCMF_ADD_SIGN_CERT |
        AvConst.AVCMF_ALLOC); // | AvConst.AVCMF_ADD_SIGN_CERT
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Common(result);
        }

        buffManaged = new byte[bufferSize];                      // managed массив под данные результата
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // обязательно освободить память, т.к. AVCMF_ALLOC
        HeapFree(GetProcessHeap(), 0, buffer);

        // конвертируем результат в base64 строку (с переносами строк Base64FormattingOptions.InsertLineBreaks)
        var hexHashSigned = Convert.ToBase64String(buffManaged, 0, buffManaged.Length);
        //---
        result.Data = input;
        result.State = true;
        result.Value = hexHash; // hash входного блока данных
        result.ExtraValue = hexHashSigned; // подпись входного блока данных
        result.ExtraValueInBytes = buffManaged; // Подпись в bytes
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);
        return new Common(result);
    }

    /// <summary>
    /// Построение BELTHASH для файла и подписание
    /// </summary>
    /// <param name="input"></param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Common GetHashDataAndSignUn(string input, string key = "", string password = "")
    {
        var result = new Common();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            Logger.Log($"{result.Error.Result} {result.Error.Code}");
            return new Common(result);
        }

        Logger.Log("Успешное соединение");

        var decoded = Encoding.Default.GetBytes(input);

        var aRc = AvCmOpenMsg(hc, decoded, (uint)decoded.Length, out var hMsg,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_OPEN_FOR_CALC_BELTHASH);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            Logger.Log($"Открытие сообщения для подписи - {result.Error.Result} {result.Error.Code}");
            return new Common(result);
        }

        Logger.Log("Открытие сообщения для подписи - успешно");

        aRc = AvCmGetMsgParam(hMsg, AvConst.AVCM_HASH_VALUE, out var buffer, out var bufferSize, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = res;
            Logger.Log($"Получение хэша - {result.Error.Result} {result.Error.Code}");
            return new Common(result);
        }

        Logger.Log("Получение хэша - успешно");

        var buffManaged = new byte[bufferSize];
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // преобразовываем из byte[] в string.UTF8 HEX
        var hexHash = string.Join("", buffManaged.Select(c => ((int)c).ToString("X2")));

        Logger.Log($"Хэш - {hexHash}");

        HeapFree(GetProcessHeap(), 0, buffer); // освобождаем память

        // теперь выработка подписи
        //---
        // вначале сюда передавалась hex_hash, но, определились с Максом, что это неправильно
        // и следует подписыв
        // ать исходное сообщение
        //decoded = StringToByteArray(input);

        // обязательно передавать флаг AVCMF_ADD_SIGN_CERT, чтобы в подпись включался сертификат подписанта

        aRc = AvCmSign(hc, decoded, (uint)decoded.Length, out buffer, out bufferSize,
            AvConst.AVCMF_IN_RAW_DATA | AvConst.AVCMF_DETACHED | AvConst.AVCMF_ADD_SIGN_CERT |
        AvConst.AVCMF_ALLOC); // | AvConst.AVCMF_ADD_SIGN_CERT
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            Logger.Log($"Подпись сообщения - {result.Error.Result} {result.Error.Code}");
            return new Common(result);
        }

        Logger.Log($"Подпись сообщения - успешно");

        buffManaged = new byte[bufferSize];                      // managed массив под данные результата
        Marshal.Copy(buffer, buffManaged, 0, buffManaged.Length); // копируем из unmanaged в managed

        // обязательно освободить память, т.к. AVCMF_ALLOC
        HeapFree(GetProcessHeap(), 0, buffer);

        // конвертируем результат в base64 строку (с переносами строк Base64FormattingOptions.InsertLineBreaks)
        var hexHashSigned = Convert.ToBase64String(buffManaged, 0, buffManaged.Length);

        Logger.Log($"Подпись - {hexHashSigned}");

        //---
        result.Data = decoded;
        result.State = true;
        result.Value = hexHash; // hash входного блока данных
        result.ExtraValue = hexHashSigned; // подпись входного блока данных
        result.ExtraValueInBytes = buffManaged; // Подпись в bytes
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);
        return new Common(result);
    }

    /// <summary>
    /// Верификация Detached подписи
    /// Вариант с передачей параметра data в необработанном виде (что пришло, то и передается на
    /// функцию криптопровайдера Авест)
    /// !!! Если падает на функции AvCmOpenMsg, значит, скорее всего, StringToByteArray не раскодирует
    /// исходные данные по причине сбоя в IsBase64
    /// </summary>
    /// <param name="data">Данные для проверки подписью</param>
    /// <param name="sign">Подпись</param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Sign VerifySignUn(string data, string sign, string key = "", string password = "")
    {
        var result = new Sign();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password, false);
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new Sign(result);
        }

        // извлекаем подпись из строки base64 в byte[]
        // важно! обязательно преобразовать и подпись
        var sDecoded = StringOrBase64ToByteArray(sign);

        // добавление в пакет подписи
        var aRc = AvCmOpenMsg(hc, sDecoded, (uint)sDecoded.Length, out var hMsg, AvConst.AVCMF_OPEN_FOR_VERIFYSIGN | AvConst.AVCMF_DETACHED);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Sign(result);
        }

        // конвертируем данные из строки в byte[]
        var dDecoded = StringToByteArray(data);

        // добавление в пакет сообщения
        aRc = AvCmMsgUpdate(hMsg, dDecoded, (uint)dDecoded.Length, out _, out _, AvConst.AVCMF_UPDATE_FINAL);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Sign(result);
        }

        // привязка подписи к сообщению
        aRc = AvCmGetMsgSign(hMsg, 0, out var hSign, 0);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Sign(result);
        }

        // получение даты формирования электронной подписи
        string timeStamp;

        // !!! обязательно AvConst.AVCMF_ALLOC (если подключаться собственноручно, то библиотека AvMailCrypt не понимает)
        aRc = AvCmGetSignAttr(hSign, AvConst.AVCM_SIGN_DATE_TIME, null, out var signT, out _, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc)) // если в подписи не присутствует дата
        {
            //result.error = GetError(aRc);
            //return new Sign(result);

            timeStamp = $"{0:D}-{0:D2}-{0:D2} {0:D2}:{0:D2}:{0:D2}.{0:D3}";
        }
        else
        {
            // получаем структуру SystemTime из unmanaged области
            var st = (SystemTime)Marshal.PtrToStructure(signT, typeof(SystemTime))!;      // managed struct

            Marshal.FreeHGlobal(signT); // освобождаем память, так как было AVCMF_ALLOC
            //HeapFree(GetProcessHeap(), 0, sign_t); // или вот это?

            //GetSystemTime(st); // использовалась для тестов
            //string time_stamp = new string(new char[256]);

            //time_stamp = string.Format("{0:D}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D3}", st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond, st.wMilliseconds);
            timeStamp = st.ToString();
        }

        aRc = AvCmFindCertBySign(hSign, out var hCert, 0);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Sign(result);
        }

        // верификация подписи
        var isValid = CheckAvCmResult(AvCmMsgVerifySign(hMsg, hCert,
            AvConst.AVCMF_VERIFY_ON_SIGN_DATE | AvConst.AVCMF_NO_CRL_VERIFY));

        //Cert cert = getCert(h_cert);
        var cert = new Cert();
        GetCertAllAttributes(hCert, cert); // получаем информацию сертификата
        if (!cert.State)
        {
            cert.Name = "Электронная подпись не содержит сертификат подписавшего";
            cert.Attributes.Add(new CertAttribute { Title = "certError", Value = cert.Name });
        }

        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);

        result.Date = timeStamp;
        result.Cert = cert;
        result.Valid = isValid;
        result.State = true;
        return new Sign(result);
    }

    /// <summary>
    /// Верификация Detached подписи
    /// Вариант с преобразованием параметра data из base64 перед передачей на криптопровайдера Авест
    /// !!! Если падает на функции AvCmOpenMsg, значит, скорее всего, StringToByteArray не раскодирует 
    /// исходные данные по причине сбоя в IsBase64
    /// </summary>
    /// <param name="data">Данные для проверки подписью</param>
    /// <param name="sign">Подпись</param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Sign VerifySignDataInBase64Un(string data, string sign, string key = "", string password = "")
    {
        var result = new Sign();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password, false);
        if (!res.State)
        {
            result.Error = res;
            return new Sign(result);
        }

        // извлекаем подпись из строки base64 в byte[]
        //byte[] s_decoded = Convert.FromBase64String(sign); // StringOrBase64ToByteArray(sign);
        var b64Encoder = Base64Encoder.Default;
        var sDecoded = b64Encoder.FromBase(sign);

        // добавление в пакет подписи
        var aRc = AvCmOpenMsg(hc, sDecoded, (uint)sDecoded.Length, out var hMsg, AvConst.AVCMF_OPEN_FOR_VERIFYSIGN | AvConst.AVCMF_DETACHED);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Sign(result);
        }

        // извлекаем данные из строки base64 в byte[]
        // byte[] d_decoded = StringToByteArray(data); // старая версия, когда было просто string->bytearray
        //byte[] d_decoded = Convert.FromBase64String(data); // падает на нестандартных base64 строках
        //var b64Encoder = Base64Encoder.Default;
        var dDecoded = b64Encoder.FromBase(data);

        // добавление в пакет сообщения
        aRc = AvCmMsgUpdate(hMsg, dDecoded, (uint)dDecoded.Length, out _, out _, AvConst.AVCMF_UPDATE_FINAL);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Sign(result);
        }

        // привязка подписи к сообщению
        aRc = AvCmGetMsgSign(hMsg, 0, out var hSign, 0);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Sign(result);
        }

        // получение даты формирования электронной подписи
        string timeStamp;  // = new string(new char[256]);

        // !!! обязательно AvConst.AVCMF_ALLOC (если подключаться собственноручно, то библиотека AvMailCrypt не понимает)
        aRc = AvCmGetSignAttr(hSign, AvConst.AVCM_SIGN_DATE_TIME, null, out var signT, out _, AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc)) // если в подписи не присутствует дата
        {
            //result.error = GetError(aRc);
            //return new Sign(result);

            timeStamp = $"{0:D}-{0:D2}-{0:D2} {0:D2}:{0:D2}:{0:D2}.{0:D3}";
        } else { 
            // получаем структуру SystemTime из unmanaged области
            var st = (SystemTime)Marshal.PtrToStructure(signT, typeof(SystemTime))!;      // managed struct

            Marshal.FreeHGlobal(signT); // освобождаем память, так как было AVCMF_ALLOC
            //HeapFree(GetProcessHeap(), 0, sign_t); // или вот это?

            //GetSystemTime(st); // использовалась для тестов
            //string time_stamp = new string(new char[256]);

            //time_stamp = string.Format("{0:D}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D3}", st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond, st.wMilliseconds);
            timeStamp = st.ToString();
        }

        aRc = AvCmFindCertBySign(hSign, out var hCert, 0);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Sign(result);
        }

        // верификация подписи
        var isValid = CheckAvCmResult(AvCmMsgVerifySign(hMsg, hCert, 
            AvConst.AVCMF_VERIFY_ON_SIGN_DATE | AvConst.AVCMF_NO_CRL_VERIFY));

        var cert = new Cert();
        GetCertAllAttributes(hCert, cert); // получаем информацию сертификата
        if (!cert.State)
        {
            cert.Name = "Электронная подпись не содержит сертификат подписавшего";
            cert.Attributes.Add(new CertAttribute { Title = "certError", Value = cert.Name });
        }

        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);

        result.Date = timeStamp;
        result.Cert = cert;
        result.Valid = isValid;
        result.State = true;
        return new Sign(result);
    }


    /// <summary>
    /// Верификация Detached подписи, когда данные в byte[]
    /// </summary>
    /// <param name="data">Данные для проверки подписью</param>
    /// <param name="signValue">Подпись</param>
    /// <param name="certificate"></param>
    /// <returns></returns>
    public static Sign VerifySmallSignForXmlDsig(byte[] data, string signValue, string certificate)
    {
        var result = new Sign();
        var b64Encoder = Base64Encoder.Default;
        var hash = Convert.ToBase64String(data);

        var cert = Convert.FromBase64String(certificate);
        var sign2 = Convert.FromBase64String(signValue);
        var sign = b64Encoder.FromBase(signValue);

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, "", "", false);
        if (!res.State)
        {
            result.Error = res;
            return new Sign(result);
        }
        
        // Открываем сертиф
        var aRc = AvCmOpenCert(hc, cert, (uint)cert.Length, out var hCrt, 0);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Sign(result);
        }

        // 1.2.112.0.2.0.34.101.45.12  = bign-with-hbelt согласно СТБ 34.101.45
        
        var algorithm = "1.2.112.0.2.0.34.101.45.12";
        var pAlg = Marshal.StringToCoTaskMemAnsi(algorithm);
        
        aRc = AvCmVerifyRawDataSign(hCrt, pAlg, data, (uint)data.Length, sign, (uint)sign.Length, AvConst.AVCMF_RAW_SIGN | AvConst.AVCMF_NO_CERT_VERIFY | AvConst.AVCMF_NO_CRL_VERIFY);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new Sign(result);
        }

        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);
        
        result.Valid = true;
        return new Sign(result);
    }




    /// <summary>
    /// Получить данные сертификата
    /// </summary>
    /// <param name="cert"></param>
    /// <param name="certificate"></param>
    /// <returns></returns>
    private static List<CertAttribute> GetCertAllAttributes(nuint cert, Cert certificate)
    {
        uint aRc;
        nint buffer;
        uint bufferSize;

        // буфер в int
        int Buf2Int(nint buf, uint bufSize)
        {
            var value = new byte[bufSize];
            Marshal.Copy(buf, value, 0, (int)bufSize);
            HeapFree(GetProcessHeap(), 0, buf); // освобождаем память
            return BitConverter.ToInt32(value, 0);
        }

        // буфер в byte[]
        byte[] Buf2Bytes(nint buf, uint bufSize)
        {
            var value = new byte[bufSize];
            Marshal.Copy(buf, value, 0, (int)bufSize);
            HeapFree(GetProcessHeap(), 0, buf); // освобождаем память
            return value;
        }

        // получить строку сертификата по параметру
        byte[] GetCertNfoByParam(uint param)
        {
            var bytesAttr = Array.Empty<byte>();
            aRc = AvCmGetCertAttr(cert, param, bytesAttr, out buffer, out bufferSize, AvConst.AVCMF_ALLOC);
            if (!CheckAvCmResult(aRc))
            {
                return Encoding.Default.GetBytes("*** ошибка разбора");
            }

            var data = Buf2Bytes(buffer, bufferSize);

            return data;
        }

        // заполнение структуры массивом байт 
        T BytesToStructure<T>(byte[] bytes)
        {
            var size = Marshal.SizeOf(typeof(T));
            if (bytes.Length < size)
                throw new Exception("Invalid parameter");

            var ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, ptr, size);
                return (T)Marshal.PtrToStructure(ptr, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        // заполняем общую информацию о сертификате

        uint AVCM_SERIAL_AS_STRING = 0x100E;
        uint AVCM_SUBJECT_AS_STRING = 0x101C;
        uint AVCM_PUB_KEY_ID = 0x100F;
        uint AVCM_CERT_SHA1 = 0x1039;           // thumbprint
        uint AVCM_NOT_BEFORE = 0x1A;            // дата начала действия сертификата
        uint AVCM_NOT_AFTER = 0x1B;             // дата окончания действия сертификата
        uint AVCM_PUB_KEY = 0x1F;               // открытый ключ сертификата

        //var certParams = new { AVCM_SERIAL_AS_STRING, AVCM_SUBJECT_AS_STRING, AVCM_PUB_KEY_ID, AVCM_CERT_SHA1,
        //                        AVCM_NOT_BEFORE, AVCM_NOT_AFTER};

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        var sbTmp = GetCertNfoByParam(AvConst.AVCM_ISSUER_AS_STRING);
        string issuer = sbTmp.ToUtf8("windows-1251");

        sbTmp = GetCertNfoByParam(AVCM_SUBJECT_AS_STRING);
        certificate.Name = sbTmp.ToUtf8("windows-1251");

        sbTmp = GetCertNfoByParam(AVCM_SERIAL_AS_STRING);
        certificate.SerialNumber = sbTmp.ToUtf8("windows-1251");

        sbTmp = GetCertNfoByParam(AVCM_PUB_KEY_ID);
        certificate.PublicKeyId = sbTmp.ToUtf8("windows-1251");

        sbTmp = GetCertNfoByParam(AVCM_CERT_SHA1);
        certificate.ThumbPrint = sbTmp.ToUtf8("windows-1251");

        sbTmp = GetCertNfoByParam(AVCM_NOT_BEFORE);
        var st = BytesToStructure<SystemTime>(sbTmp);


        certificate.NotBefore = DateTime.Parse(st.ToString());

        sbTmp = GetCertNfoByParam(AVCM_NOT_AFTER);
        st = BytesToStructure<SystemTime>(sbTmp);
        certificate.NotAfter = DateTime.Parse(st.ToString());

        sbTmp = GetCertNfoByParam(AVCM_PUB_KEY);
        certificate.PublicKey = AvUtil.ByteArrayToString(sbTmp); //Encoding.Default.GetString(sb_tmp, 0, sb_tmp.Length - 1);

        // возвращаемый список атрибутов сертификата
        var attrList = new List<CertAttribute>();

        // получаем количество дополнений сертификата

        uint AVCM_EXT_COUNT = 0x21;   // количество дополнительных атрибутов
        var bytesAttribute = Array.Empty<byte>();   // заглушка
        aRc = AvCmGetCertAttr(cert,
            AVCM_EXT_COUNT,
            bytesAttribute,
            out buffer,
            out bufferSize,
            AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            var err = GetError(aRc);
            return null;
        }
        var extCount = Buf2Int(buffer, bufferSize);

        // получаем последовательно все дополнения сертификата

        uint AVCM_EXT_OID = 0x1022; // идентификатор объекта (OID) дополнения
        for (var i=0; i < extCount; i++) // цикл по number
        {
            // получаем oid
            aRc = AvCmGetCertAttr(cert,
                AVCM_EXT_OID,
                BitConverter.GetBytes(i),
                out buffer,
                out bufferSize,
                AvConst.AVCMF_ATTR_BY_NUM | AvConst.AVCMF_ALLOC);
            if (!CheckAvCmResult(aRc))
            {
                continue;
            }

            // байтовое представление oid
            var oidBuf = Buf2Bytes(buffer, bufferSize);
            // иногда бывает, что буфер пуст
            if (oidBuf.Length <= 0) continue;

            uint AVCM_EXT_AS_STRING = 0x1024;
            aRc = AvCmGetCertAttr(cert,
                AVCM_EXT_AS_STRING,
                oidBuf, //
                out buffer,
                out bufferSize,
                AvConst.AVCMF_ATTR_BY_OID | AvConst.AVCMF_ALLOC);
            if (!CheckAvCmResult(aRc))
            {
                continue;
            }
            // байтовое представление value
            var valBuf = Buf2Bytes(buffer, bufferSize);
            // если буфер пуст, подставляем сообщение об ошибке в value
            if (!(valBuf.Length > 0))
            {
                valBuf = Encoding.Default.GetBytes("*** ошибка разбора");
            }

            attrList.Add(new CertAttribute() {
                State = true,
                Oid = oidBuf.ToUtf8("windows-1251"),
                Value = valBuf.ToUtf8("windows-1251")
            });
        }

        // SUBJECT

        // получаем количество атрибутов владельца сертификата (subject)

        uint AVCM_SUBJECT_ATTR_COUNT = 0x1;
        bytesAttribute = Array.Empty<byte>();
        aRc = AvCmGetCertAttr(cert,
            AVCM_SUBJECT_ATTR_COUNT,
            bytesAttribute,
            out buffer,
            out bufferSize,
            AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            var err = GetError(aRc);
            return null;
        }
        var subjectCount = Buf2Int(buffer, bufferSize);

        // получаем последовательно все атрибуты subject

        uint AVCM_SUBJECT_ATTR_OID = 0x1033;   // идентификатор объекта (OID) атрибута
        // имени владельца сертификата в виде строки
        for (var i = 0; i < subjectCount; i++) // цикл по number
        {
            aRc = AvCmGetCertAttr(cert,
                AVCM_SUBJECT_ATTR_OID,
                BitConverter.GetBytes(i),
                out buffer,
                out bufferSize,
                AvConst.AVCMF_ATTR_BY_NUM | AvConst.AVCMF_ALLOC);
            if (!CheckAvCmResult(aRc))
            {
                continue;
                // или Add(ошибка получения)
            }

            var oidBuf = Buf2Bytes(buffer, bufferSize);
            if (oidBuf.Length <= 0) continue;

            uint AVCM_SUBJECT_ATTR = 0x101D;
            aRc = AvCmGetCertAttr(cert,
                AVCM_SUBJECT_ATTR,
                oidBuf,
                out buffer,
                out bufferSize,
                AvConst.AVCMF_ATTR_BY_OID | AvConst.AVCMF_ALLOC);
            if (!CheckAvCmResult(aRc))
            {
                continue;
            }
            // байтовое представление value
            var valBuf = Buf2Bytes(buffer, bufferSize);
            // если буфер пуст, подставляем сообщение об ошибке в value
            if (!(valBuf.Length > 0))
            {
                valBuf = Encoding.Default.GetBytes("Ошибка разбора");
            }

            attrList.Add(new CertAttribute()
            {
                State = true,
                Oid = oidBuf.ToUtf8("windows-1251"),
                Value = valBuf.ToUtf8("windows-1251")
            });
        }


        // атрибутники !!!


        // получаем количество атрибутов сертификата
        /*
        uint AVCM_ATTR_COUNT = 0x324;   // количество атрибутов
        byte[] bytesAttribute2 = { };    // заглушка
        aRc = AvCmGetCertAttr(cert,
                             AVCM_ATTR_COUNT,
                             bytesAttribute2,
                             out buffer,
                             out buffer_size,
                             AvConst.AVCMF_ALLOC);
        if (!CheckAvCmResult(aRc))
        {
            var err = GetError(aRc);
            return null;
        }

        var attrCount = buf2int(buffer, buffer_size);
        */

        // присоединяем к сертификату информацию и атрибуты
        certificate.Attributes.AddRange(attrList);
        certificate.State = true;
        return new List<CertAttribute>(attrList);
    }

    /// <summary>
    /// Получить список сертификатов установленных в системе средствами AvCryptMail
    /// </summary>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static CertList GetCertificatesListAvCryptMailUn(string key = "", string password = "")
    {
        var result = new CertList();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password, false); // есть сомнения, что достаточно noauth
        // ошибка
        if (!res.State)
        {
            result.Error = res;
            return new CertList(result);
        }

        // string issuerName = "CN=ПУЦ, OU=ПУЦ, O=ПУЦ, STREET=ПУЦ, L=ПУЦ, ST=ПУЦ, C=BY";
        var @params = Arrays.InitializeWithDefaultInstances<AvCmEnumGetParam>(1);

        //2.5.29.29

        //const string oid = "2.5.29.29";
        //var pOid = Marshal.StringToCoTaskMemAnsi(oid);

        // в param требуется передать указатель на параметр (Авест-извращенцы), поэтому
        // заруливаем чз unsafe
        uint AVCM_TYPE_MY = 0x10E; // сертификаты, установленные в хранилище My
        unsafe { 
            @params[0].ParamId = AvConst.AVCM_TYPE; //AVCM_CERT_ISSUERS_CHAIN; //AVCM_ATTRIBUTE_CERTS
            @params[0].ParamSpec = 0;
            @params[0].Param = (uint)&AVCM_TYPE_MY;
        }

        var aRc = AvCmOpenCertEnum(hc, 1, @params, out var hCertEnum, 0); // AvConst.AVCMF_ALL_CERT);
        if (!CheckAvCmResult(aRc))
        {
            result.Error = GetError(aRc);
            return new CertList(result);
        }

        var enumFlag = AvConst.AVCMF_START;

        do
        {
            aRc = AvCmEnumGet(hCertEnum, out var hAc, enumFlag);
            if (aRc == AvConst.AVCMR_NOT_FOUND)
            {
                break;
            }
            if (!CheckAvCmResult(aRc))
            {
                result.Error = GetError(aRc);
                return new CertList(result);
            }

            var cert = new Cert();
            GetCertAllAttributes(hAc, cert); // получаем информацию сертификата

            //Cert a_cert = getCert(new nuint(BitConverter.ToUInt64(BitConverter.GetBytes(h_ac.ToInt64()), 0))); //h_ac.ToInt64()
            if (cert.State) result.Certs.Add(cert); // всё ОК, добавляем данные сертификата
            enumFlag = AvConst.AVCMF_NEXT;

        } while (true);

        result.State = true;
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);

        return new CertList(result);
    }

    /// <summary>
    /// Получить список сертификатов установленных в системе средствами AvCryptMail
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="key"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public static Error ImportCrlAvCryptMailUn(string filePath, string key = "", string password = "")
    {
        var result = new CertList();

        // устанавливаем соединение с AvCryptMail
        var res = AvCryptMailLogin(out var hc, key, password, false); // есть сомнения, что достаточно noauth
        // ошибка
        if (!res.State)
        {
            return res;
            //return new CertList(result);
        }

        var inputData = File.ReadAllBytes(filePath);

        var @params = Arrays.InitializeWithDefaultInstances<AvCmImportParam>(2);

        //const string path = "log.txt";
        var pPath = Marshal.StringToCoTaskMemAnsi(AvConst.Path);
        nuint hObj;

        unsafe
        {
            @params[0].ParamId = (uint) AvConst.AVCM_RESULT_HANDLE;
            @params[0].Param = (uint) &hObj;
            @params[1].ParamId = (uint)AvConst.AVCM_OPERATION_REPORT;
            @params[1].Param = (uint) &pPath; //AVCM_CERT_ISSUERS_CHAIN; //AVCM_ATTRIBUTE_CERTS
        }

        var aRc = AvCmImport(hc, (uint)AvConst.AVCM_CRL, inputData, (uint)inputData.Length, @params.Length,  @params, 0);
        if (!CheckAvCmResult(aRc))
        {
            var error = GetError(aRc);
            return error;
        }

        Marshal.FreeCoTaskMem(pPath);

        AvCmCloseHandle(hObj, 0);

        var enumFlag = AvConst.AVCMF_START;

        //do
        //{
        //    aRc = AvCmEnumGet(hCertEnum, out var hAc, enumFlag);
        //    if (aRc == AvConst.AVCMR_NOT_FOUND)
        //    {
        //        break;
        //    }
        //    if (!CheckAvCmResult(aRc))
        //    {
        //        result.Error = GetError(aRc);
        //        return new CertList(result);
        //    }

        //    var cert = new Cert();
        //    GetCertAllAttributes(hAc, cert); // получаем информацию сертификата

        //    //Cert a_cert = getCert(new nuint(BitConverter.ToUInt64(BitConverter.GetBytes(h_ac.ToInt64()), 0))); //h_ac.ToInt64()
        //    if (cert.State) result.Certs.Add(cert); // всё ОК, добавляем данные сертификата
        //    enumFlag = AvConst.AVCMF_NEXT;

        //} while (true);

        //result.State = true;
        _ = AvCmInit(AvConst.AVCMF_SHUTDOWN);

        //return new CertList(result);

        return res;
    }

    /// <summary>
    /// Получить список сертификатов, установленных в системе используя X509Store
    /// </summary>
    /// <returns></returns>
    public static CertList GetCertificatesListX509ApiUn()
    {
        var result = new CertList();

        var myStore = new X509Store(StoreLocation.CurrentUser);
        myStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
        try
        {
            foreach (var myCert in myStore.Certificates)
            {
                if (myCert is not {SerialNumber: { }, Thumbprint: { }}) continue;

                var cert = new Cert
                {
                    Name = ExtractByPattern(myCert.SubjectName.Name).Replace("\"", "'"),
                    SerialNumber = myCert.SerialNumber.Replace("\"", "'"),
                    ThumbPrint = myCert.Thumbprint.Replace("\"", "'")
                };

                foreach (var ext in myCert.Extensions)
                {
                    var attr = new CertAttribute();
                    var asnData = new AsnEncodedData(ext.Oid, ext.RawData);

                    if (asnData.Oid != null)
                    {
                        attr.Oid = asnData.Oid.Value;
                        if (ext.Oid != null)
                        {
                            attr.Title = ext.Oid.FriendlyName;
                            // OID value: 2.5.29.14 OID description: id-ce-subjectKeyIdentifier
                            if (ext.Oid.Value is "2.5.29.14")
                            {
                                attr.Value = RemoveWhitespace(asnData.Format(false));
                                cert.PublicKeyId = attr.Value;
                            }
                            else
                            {
                                attr.Value = asnData.Format(true);
                            }
                        }
                    }

                    //attr.value = ext.Oid.Value.ToString().Equals("2.5.29.14") ? 
                    //            RemoveWhitespace(asnData.Format(false)) : asnData.Format(true);

                    attr.State = true;
                    cert.Attributes.Add(new CertAttribute(attr));
                }
                cert.State = true;
                result.Certs.Add(new Cert(cert));
            }
            result.State = true;
        }
        catch
        {
            result.State = false;
        }
        finally
        {
            myStore.Close();
        }

        return new CertList(result); //certificates.Add(x509.IssuerName.Name);
    }
    /// <summary>
    /// Получить CertID по "контрольной характеристике" thumbPrint
    /// </summary>
    /// <returns></returns>
    public static string GetCertIdByThumb(string thumbprint)
    {
        var myStore = new X509Store(StoreLocation.CurrentUser);
        myStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

        foreach (var asnData in from myCert in myStore.Certificates let cert = new Cert
                 {
                     //cert.name = ExtractByPattern(MyCert.SubjectName.Name).Replace("\"", "'");
                     //cert.serialNumber = MyCert.SerialNumber.Replace("\"", "'");
                     ThumbPrint = myCert.Thumbprint.Replace("\"", "'")
                 } where cert.ThumbPrint.Contains(thumbprint) from asnData in from ext in myCert.Extensions
                     let asnData = new AsnEncodedData(ext.Oid, ext.RawData)
                     where ext.Oid is {Value: "2.5.29.14"}
                     select asnData select asnData)
        {
            return RemoveWhitespace(asnData.Format(false));
        }
        return string.Empty;
    }

    /// <summary>
    /// Найти объект Certificate по "контрольной характеристике" thumbPrint
    /// Вариант 2, с Find
    /// </summary>
    /// <param name="thumbprint"></param>
    /// <returns></returns>
    public static X509Certificate2 FindX509Certificate(string thumbprint)
    {
        X509Store certificateStore = null;
        X509Certificate2 certificate = null;

        try
        {
            certificateStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certificateStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            var certificates = certificateStore.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            if (certificates.Count > 0)
            {
                certificate = certificates[0];
            }
        }
        finally
        {
            certificateStore?.Close();
        }

        return certificate;
    }

    /// <summary>
    /// Получить из объекта Certificate строку со значением CertId
    /// </summary>
    /// <param name="certificate"></param>
    /// <returns></returns>
    public static string GetX509CertificateId(X509Certificate2 certificate)
    {
        if (certificate == null) return string.Empty;

        foreach (var asnData in from ext in certificate.Extensions
                 let asnData = new AsnEncodedData(ext.Oid, ext.RawData)
                 where ext.Oid is {Value: "2.5.29.14"}
                 select asnData)
        {
            return RemoveWhitespace(asnData.Format(false));
        }

        // ничего не нашли
        return string.Empty;
    }


    public static List<Crl> GetCrls()
    {
        return new List<Crl>
        {
            new Crl
            {
                Title = "Корневой удостоверяющий центр ГосСУОК",
                Name =
                    "CN=Корневой удостоверяющий центр ГосСУОК, O=\"Республиканское унитарное предприятие \"\"Национальный центр электронных услуг\"\"\", C=BY"
            },
            new Crl
            {
                Title = "Республиканский удостоверяющий центр ГосСУОК",
                Name =
                    "CN=Республиканский удостоверяющий центр ГосСУОК, EMAIL=rca@pki.gov.by, O=\"Республиканское унитарное предприятие \"\"Национальный центр электронных услуг\"\"\", STREET=\"пр-т Машерова, 25\", L=г. Минск, ST=Минская, C=BY"
            }
        };
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="errors"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool TryCheckAvest(out IEnumerable<string> errors, out int type)
    {
        var messErrors = new List<string>();

        errors = messErrors;

        var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE");

        var avKeyBelPro = key?.OpenSubKey(@"Microsoft\Cryptography\Defaults\Provider\Avest CSP Bel Pro");

        // Проверка на Bign
        var avKey = key?.OpenSubKey(@"Microsoft\Cryptography\Defaults\Provider\Avest CSP Bign");
       

        if (avKeyBelPro != null && avKey != null)
        {
            type = 3;
            return true;
        }
        else if (avKey != null)
        {
            type = 1;
            return true;
        }
        else if (avKeyBelPro != null)
        {
            type = 2;
            return true;
        }

        messErrors.Add("Криптопровайдер Avest не установлен");

        type = 0;
        return false;
    }
}

public static class ByteEncodingExtensions
{
    public static string ToUtf8(this byte[] bytes, string encoding)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Array.Resize(ref bytes, bytes.Length - 1);

        return Encoding.UTF8.GetString(Encoding.Convert(Encoding.GetEncoding(encoding), Encoding.UTF8, bytes));
    }
}