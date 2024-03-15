        //// Инициализация библиотеки.

        //using System.Runtime.InteropServices;
        //using DigitalSigning.Av;

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmInit(uint flags);

        //// Функция авторизации пользователя и создания сессии.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        ////internal static extern uint AvCmLogin(int conn_param_count, AvCmConnectionParam[] conn_params, out UIntPtr hc, uint flags);
        //internal static extern uint AvCmLogin(int conn_param_count, IntPtr conn_params, out UIntPtr hc, uint flags);

        //// Функция закрытия сессии.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmLogout(UIntPtr hc, uint flags);

        //// Функция для сброса данных сессии 
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmFlush(UIntPtr hc, uint flags);

        //// Функция генерации подписанного сообщения.
        ////AVCM_FUNCTION AvCmSign(AvCmHc hc, const void* input_message, AvCmSize input_size, void* output_buffer,
        ////     AvCmSize * output_size, AvCmLong flags);
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmSign(UIntPtr hc, byte[] input_message, uint input_size,
        //    out IntPtr output_buffer, out uint output_size, uint flags);

        //// Функция проверки подписи и получения исходного сообщения.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmVerifySign(UIntPtr hc, object input_message, uint input_size,
        //    object[] hsgn_cert_enum, object output_buffer, ref uint output_size, uint flags);

        //// Функция зашифрования сообщения.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmEncrypt(UIntPtr hc, object input_message, uint input_size,
        //    uint cert_count, object[] certificates, object output_buffer, ref uint output_size, uint flags);

        //// Функция расшифрования полученного зашифрованного сообщения.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmDecrypt(UIntPtr hc, object input_message, uint input_size,
        //    object output_buffer, ref uint output_size, uint flags);

        //// Функция генерации подписанного сообщения с последующим его зашифрованием.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmSignAndEncrypt(UIntPtr hc, object input_message, uint input_size,
        //    uint cert_count, object[] certificates, object output_buffer, ref uint output_size, uint flags);

        //// Функция расшифрования сообщения с последующей проверкой подписи, 
        //// и получения исходного сообщения
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmDecryptAndVerifySign(UIntPtr hc, object input_message, uint input_size,
        //    object[] hsgn_cert_enum, object output_buffer, ref uint output_size, uint flags);

        //// Функция получения параметров открытой объекта библиотеки. 
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmGetObjectInfo(UIntPtr handle, uint param_id, object output_buffer,
        //    ref uint output_size, uint flags);

        //// Функция создания копии объекта, 
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmDuplicateHandle(UIntPtr source_handle, UIntPtr dest_hc,
        //    object[] copy_of_handle, uint flags);

        //// Функция закрытия дескриптора объекта библиотеки, 
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmCloseHandle(UIntPtr handle, uint flags);

        //// Функция создания или открытия и разбора подписанного 
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmOpenMsg(UIntPtr hc, byte[] message_data, uint message_size, out UIntPtr hmsg, uint flags);

        //// Функция получения параметров открытого сообщения.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmGetMsgParam(UIntPtr hmsg, uint attr_id, out IntPtr output_buffer,
        //    out uint output_size, uint flags);

        //// Функция выработки подписи под сообщением.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmMsgSign(UIntPtr hmsg, uint flags);

        //// Функция проверки подписи в открытом подписанном сообщении.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmMsgVerifySign(UIntPtr hmsg, UIntPtr hcert, uint flags);

        //// Функция зашифрования открытого сообщения.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmMsgEncrypt(UIntPtr hmsg, uint cert_count, object[] certificates,
        //    uint flags);

        //// Функция расшифрования открытого зашифрованного сообщения.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmMsgDecrypt(UIntPtr hmsg, uint flags);

        //// Функция получения сгенерированного сообщения 
        //// в одном из экспортируемых форматов.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmGetMsg(UIntPtr hmsg, object output_buffer, ref uint output_size,
        //    uint flags);

        //// Функция извлечения содержимого сообщения.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmGetMsgContent(UIntPtr hmsg, object output_buffer, ref uint output_size,
        //    uint flags);

        //// Функция установки содержимого сообщения.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmSetMsgContent(UIntPtr hmsg, object input_buffer, uint input_size,
        //    uint flags);

        //// Функция извлечения одной из подписей данного подписанного сообщения.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmGetMsgSign(UIntPtr hmsg, uint sign_number, out UIntPtr hsign, uint flags);

        //// Функция извлечения атрибутов подписи.
        ////C++ AVCM_FUNCTION AvCmGetSignAttr(AvCmHandle handle, AvCmLong attr_id, const void* attr_param, void* output_buffer, AvCmSize * output_size, AvCmLong flags);
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        ///*
        //internal static extern uint AvCmGetSignAttr(UIntPtr handle, uint attr_id, byte[] attr_param,
        //        out IntPtr output_buffer, out uint output_size, uint flags);
        //*/
        //internal static extern uint AvCmGetSignAttr(UIntPtr handle, uint attr_id, byte[] attr_param,
        //    out IntPtr output_buffer, out uint output_size, uint flags);

        //// Функция поиска сертификата по его подписи.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmFindCertBySign(UIntPtr hsign, out UIntPtr hcert, uint flags);

        //// Функция извлечения атрибутов сертификата.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmGetCertAttr(UIntPtr handle, uint attr_id, byte[] attr_param,
        //    out IntPtr output_buffer, out uint output_size, uint flags);

        //// Функция создания контекста поиска сертификата путем перебора подмножества сертификатов, удовлетворяющих определенным условиям.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmOpenCertEnum(UIntPtr hc, int param_count, AvNative.AvCmEnumGetParam[] @params,
        //    out UIntPtr hcert_enum, uint flags);

        //// Функция перебора объектов в открытом перечислении.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmEnumGet(UIntPtr henum, out UIntPtr handle, uint flags);

        //// Функция возвращает различные параметры возникшей 
        //// в процессе работы одной из функций библиотеки ошибки.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmGetErrorInfo(UIntPtr hc, uint error_code, uint param_id,
        //    string output_buffer, ref uint output_size, uint flags);

        //// Функция добавления сертификата в сообщение
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmMsgAddCert(object hmsg, uint cert_count, object[] certificates,
        //    uint flags);

        //// Функция импорта сертификатов из сообщения
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmMsgImportCerts(object HMsg, uint flags);

        //// Функция генерации СТБ-подписи
        //// deprecated
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmMsgGetSTBSign(object HMsg, object pBuffer, ref uint pBufferSize,
        //    uint flags);

        //// Функция показа информации об объекте
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmShowObjectInfo(object handle, string cpszDlgCaption, string cpszLabel,
        //    string cpszOkButtonCaption, uint flags);

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmShowObjectInfoExt(object handle, uint hwnd, object[] handle2,
        //    uint flags);

        //// Функция добавления сертификатов в контекст
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmEnumAddCerts(object hcert_enum, int param_count,
        //    AvNative.AvCmEnumGetParam[] @params, uint flags);

        //// Функция проверки подписи в открытом подписанном сообщении по номеру
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmMsgVerifySignAtIndex(object hmsg, uint sign_index, uint flags);

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmMsgUpdate(UIntPtr hmsg, byte[] pBuffer, uint iBufferSize,
        //        out UIntPtr pOut, out uint pOutSize, uint flags);

        //// Функция проверки подписи в открытом подписанном сообщении по номеру на заданную дату
        ///*AVCM_FUNCTION AvCmMsgVerifySignAtIndexForDate(
        //    AvCmHmsg hmsg,
        //    SystemTime* verifydate, 
        //    AvCmSize sign_index,
        //    AvCmLong flags);*/

        //// Функция установки дополнительных атрибутов открытого сообщения для подписи
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmMsgSetAttribute(object hmsg, uint attr_id, object attr_param,
        //    object input_buffer, uint input_size, uint flags);

        //// Инициализация библиотеки с указанием рабочего каталога.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmInitEx(string cpszWorkDir, uint flags);

        //// Функция открытия вложенного сообщения
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmOpenInnerMsg(object hmsg, object[] hmsg_inner, uint flags);

        //// Функция диалога создания контекста
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmEnumDlg(UIntPtr hc, string cpszDlgCaption, string cpszLabel,
        //    string cpszOkButtonCaption, object[] hcert_enum, uint flags);

        //// Функция импорта сертификата или списка отозванных сертификатов
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmImport(UIntPtr hc, uint obj_type, byte[] input_data, uint input_size,
        //    int param_count, AvNative.AvCmImportParam[] @params, uint flags);

        //// Функция поиска и открытия списка отозванных сертификатов
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmFindCrl(UIntPtr hc, int param_count, AvNative.AvCmFindCrlParam[] @params,
        //    object[] hcrl, uint flags);

        //// Функция извлечения атрибутов списка отозванных сертификатов. 
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmGetCrlAttr(UIntPtr hcrl, uint attr_id, object output_buffer,
        //    ref uint output_size, uint flags);

        //// Функция предназначена для генерации запроса на сертификат.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmGenerateRequest(UIntPtr hc, int param_count, AvNative.AvCmGenReqParam[] @params,
        //    object[] hreq, uint flags);

        //// Функция предназначена для извлечения атрибутов запроса на сертификат.
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmGetRequestAttr(object hreq, uint attr_id, object output_buffer,
        //    ref uint output_size, uint flags);

        ////Функция предназначена для поиска и открытия запроса на сертификат. 
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmFindRequest(UIntPtr hc, uint param_count, AvNative.AvCmFindReqParam @params,
        //    object[] hreq, uint flags);

        //// Функция предназначена для помещения сертификата, извлеченного из сообщения в справочник сертификатов. 
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmPutCert(object handle, uint flags);

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmSignRawData(UIntPtr hc, string hash_and_sign_oid, object data,
        //    uint data_size, object output_buffer, ref uint output_size, uint flags);

        //// Функция предназначена для проверки статуса сертификата
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmVerifyCertStatus(object handle, uint param_count,
        //    AvNative.AvCmCertStatParam @params, ref uint status_ok, object[] hstatus, uint flags);

        //// Функция предназначена для получения данных статуса сертификата
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmGetCertStatusAttr(object hstatus, uint attr_id, object output_buffer,
        //    ref uint output_size, uint flags);

        ////Функция предназначена для проверки ЭЦП 
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmVerifyRawDataSign(object hcert, string hash_and_sign_oid,
        //    object data_to_be_verified, uint data_size, object sign_value, uint sign_size, uint flags);

        ////Функция установки дескриптора приложения
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmSetActiveWindow(uint hwnd, uint flags);

        ////Получение информации о статусе сертификата
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmMsgOCSPGetResponse(object handle, uint sign_index, object[] hr,
        //    uint flags);

        ////Извлечение атрибутов информации о статусе сертификата
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmMsgOCSPGetResponseAttr(object hr, uint attr_id, object attr_param,
        //    object output_buffer, ref uint output_size, uint flags);

        ////Добавление информации о статусе сертификата в подписанное сообщение
        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmMsgOCSPAddResponse(object handle, uint sign_index, object hr,
        //    uint flags);

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmOpenCert(UIntPtr hc, object input_buffer, uint buffer_size,
        //    object[] hcert, uint flags);

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmOpenCRL(UIntPtr hc, object input_buffer, uint buffer_size,
        //    object[] hcrl, uint flags);

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmCreateScep(UIntPtr hc, string url, object[] hscep, uint flags);

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmScepExecute(object hscep, uint oper_id, uint flags);

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmScepGet(object hscep, uint attr_id, object attr_param,
        //    object output_buffer, ref uint output_size, uint flags);

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmScepSet(object hscep, uint attr_id, object input_buffer,
        //    uint buffer_size, uint flags);

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmOpenEnum(UIntPtr hc, object crypt_sql, object[] henum, uint obj_type,
        //    uint flags);

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmTLSCreateConnect(UIntPtr hc, uint conn_param_count,
        //    AvNative.AvCmConnectionParam conn_params, object[] htls, uint flags);

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmTlsGet(object htsl, string URL, object res_data, ref uint res_size,
        //    uint flags);

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmTlsPost(object htsl, string URL, object data, uint data_size,
        //    object res_data, ref uint res_size, uint flags);

        //[DllImport("AvCryptMail.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //internal static extern uint AvCmDebugLog(string app, string msg, string rem, uint flags);
