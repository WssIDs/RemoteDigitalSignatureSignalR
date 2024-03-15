namespace DigitalSigning.Crypt32;

/// <summary>
/// 
/// </summary>
public static class CryptConst
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_STORE_PROV_SYSTEM = 10;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_SYSTEM_STORE_LOCAL_MACHINE = (2 << 16);

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_SYSTEM_STORE_CURRENT_USER = (1 << 16);

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint X509_ASN_ENCODING = 0x00000001;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_CLOSE_STORE_FORCE_FLAG = 0x00000001;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_CLOSE_STORE_CHECK_FLAG = 0x00000002;


    /// <summary>
    /// 
    /// </summary>
    public static readonly uint PKCS_7_ASN_ENCODING = 0x00010000;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_STORE_ADD_REPLACE_EXISTING = 3;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_STORE_ADD_REPLACE_EXISTING_INHERIT_PROPERTIES = 5;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_STORE_ADD_NEWER = 6;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CRL_FIND_ANY = 0;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CRL_FIND_ISSUED_BY = 1;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CRL_FIND_EXISTING = 2;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CRL_FIND_ISSUED_FOR = 3;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_FRIENDLY_NAME_PROP_ID = 11;    // string

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_ARCHIVED_PROP_ID = 19;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_ISSUER_PUBLIC_KEY_MD5_HASH_PROP_ID = 24;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_SUBJECT_PUBLIC_KEY_MD5_HASH_PROP_ID = 25;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CRL_FIND_ISSUED_BY_AKI_FLAG = 0x1;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CRL_FIND_ISSUED_BY_SIGNATURE_FLAG = 0x2;

    /// <summary>
    /// 
    /// </summary>
    public static readonly string MyStore = "My";

    /// <summary>
    /// 
    /// </summary>
    public static readonly string SystemStore = "CA";

    public static readonly string TrustedPublisher = "TrustedPublisher";

    public static readonly string AuthRoot = "AuthRoot";

    public static readonly string Root = "Root";

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_SIMPLE_NAME_STR = 1;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_OID_NAME_STR = 2;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_X500_NAME_STR = 3;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_XML_NAME_STR = 4;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_NAME_EMAIL_TYPE = 1;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_NAME_RDN_TYPE = 2;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_NAME_ATTR_TYPE = 3;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_NAME_SIMPLE_DISPLAY_TYPE = 4;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_NAME_FRIENDLY_DISPLAY_TYPE = 5;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_NAME_DNS_TYPE = 6;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_NAME_URL_TYPE = 7;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_NAME_UPN_TYPE = 8;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_NAME_ISSUER_FLAG = 0x1;
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_NAME_DISABLE_IE4_UTF8_FLAG = 0x00010000;
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_NAME_SEARCH_ALL_NAMES_FLAG = 0x2;
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_NAME_STR_ENABLE_PUNYCODE_FLAG = 0x00200000;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_STORE_CERTIFICATE_CONTEXT = 1;
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_STORE_CRL_CONTEXT = 2;
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_STORE_CTL_CONTEXT = 3;

    /// <summary>
    /// 
    /// </summary>
    public static readonly uint CERT_CREATE_CONTEXT_NOCOPY_FLAG = 0x1;
}