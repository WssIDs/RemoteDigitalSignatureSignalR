using System.Runtime.InteropServices;
using System.Text;
using static DigitalSigning.Av.AvNative;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using static DigitalSigning.Crypt32.Crypt32Native;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace DigitalSigning.Crypt32;

/// <summary>
/// 
/// </summary>
public static class Crypt32Native
{
    #region Structs

    [StructLayout(LayoutKind.Sequential)]
    public struct CERT_CONTEXT

    {
        public int dwCertEncodingType;
        public nint pbCertEncoded;
        public int cbCertEncoded;
        public nint pCertInfo;
        public nint hCertStore;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CTL_CONTEXT
    {
        public int dwMsgAndCertEncodingType;
        public byte[] pbCtlEncoded;
        public int cbCtlEncoded;
        public CTL_INFO pCtlInfo;
        public nint hCertStore;
        public nint hCryptMsg;
        public byte[] pbCtlContent;
        public int cbCtlContent;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CTL_INFO
    {
        public int dwVersion;
        public CTL_USAGE SubjectUsage;
        public CRYPTOAPI_BLOB ListIdentifier;
        public CRYPTOAPI_BLOB SequenceNumber;
        public FILETIME ThisUpdate;
        public FILETIME NextUpdate;
        public CRYPT_ALGORITHM_IDENTIFIER SubjectAlgorithm;
        public int cCTLEntry;
        public CTL_ENTRY rgCTLEntry;
        public int cExtension;
        public nint rgExtension;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CTL_ENTRY
    {
        public CRYPTOAPI_BLOB SubjectIdentifier;

        public int cAttribute;

        public CRYPT_ATTRIBUTE rgAttribute;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CRYPT_ATTRIBUTE
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string pszObjId;

        public int cValue;

        public CRYPTOAPI_BLOB rgValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CTL_USAGE
    {
        public int cUsageIdentifier;

        [MarshalAs(UnmanagedType.LPStr)]
        public string rgpszUsageIdentifier;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CERT_INFO
    {
        public int dwVersion;
        public CRYPTOAPI_BLOB SerialNumber;
        public CRYPT_ALGORITHM_IDENTIFIER SignatureAlgorithm;
        public CRYPTOAPI_BLOB Issuer;
        public FILETIME NotBefore;
        public FILETIME NotAfter;
        public CRYPTOAPI_BLOB Subject;
        public CERT_PUBLIC_KEY_INFO SubjectPublicKeyInfo;
        public CRYPT_BIT_BLOB IssuerUniqueId;
        public CRYPT_BIT_BLOB SubjectUniqueId;
        public int cExtension;
        public nint rgExtension;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CERT_PUBLIC_KEY_INFO
    {
        CRYPT_ALGORITHM_IDENTIFIER Algorithm;
        CRYPT_BIT_BLOB PublicKey;
    }

[StructLayout(LayoutKind.Sequential)]

    public struct CRYPT_BIT_BLOB
    {
        public int cbData;

        public nint pbData;

        public int cUnusedBits;

    }

    [StructLayout(LayoutKind.Sequential)]

    public struct CRL_CONTEXT

    {

        public Int32 dwCertEncodingType;

        public IntPtr pbCrlEncoded;

        public Int32 cbCrlEncoded;

        public IntPtr pCrlInfo;

        public IntPtr hCertStore;

    }


    [StructLayout(LayoutKind.Sequential)]

    public struct CRL_INFO

    {

        public Int32 dwVersion;

        public CRYPT_ALGORITHM_IDENTIFIER SignatureAlgorithm;

        public CRYPTOAPI_BLOB Issuer;

        public FILETIME ThisUpdate;

        public FILETIME NextUpdate;

        public int cCRLEntry;

        public nint rgCRLEntry;

        public nint cExtension;

        public nint rgExtension;

    }


    [StructLayout(LayoutKind.Sequential)]

    public struct CRYPT_ALGORITHM_IDENTIFIER
    {

        [MarshalAs(UnmanagedType.LPStr)] public String pszObjId;

        public CRYPTOAPI_BLOB Parameters;

    }


    [StructLayout(LayoutKind.Sequential)]

    public struct CRYPTOAPI_BLOB
    {

        public int cbData;

        public nint pbData;

    }


    [StructLayout(LayoutKind.Sequential)]

    public struct CRL_ENTRY

    {

        public CRYPTOAPI_BLOB SerialNumber;

        public FILETIME RevocationDate;

        public int cExtension;

        public nint rgExtension;

    }


    [StructLayout(LayoutKind.Sequential)]

    public struct CERT_EXTENSION
    {
        [MarshalAs(UnmanagedType.LPStr)] public string pszObjId;

        public bool fCritical;

        public CRYPTOAPI_BLOB Value;

    }

    #endregion

    [DllImport("CRYPT32.DLL", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern nint CertEnumCTLsInStore(nint hCertStore, nint pPrevCtlContext);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeLibrary(nint hModule);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="storeProvider"></param>
    /// <param name="encodingType"></param>
    /// <param name="hcryptProv"></param>
    /// <param name="flags"></param>
    /// <param name="pvPara"></param>
    /// <returns></returns>
    [DllImport("CRYPT32.DLL", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern nint CertOpenStore(
        uint storeProvider,
        uint encodingType,
        nint hcryptProv,
        uint flags,
        string pvPara);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hCertStore"></param>
    /// <param name="pPrevCertContext"></param>
    /// <returns></returns>
    [DllImport("CRYPT32.DLL", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern nint CertEnumCertificatesInStore(nint hCertStore, nint pPrevCertContext);

    [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern nint CertCreateContext(
        uint dwContextType,
        uint dwEncodingType,
        byte[] pbEncoded,
        int cbEncoded,
        uint dwFlags,
        nint pCreatePara);

    [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool CertFreeCertificateContext(
    nint pCrlContext);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="dwCertEncodingType"></param>
    /// <param name="pbCrlEncoded"></param>
    /// <param name="cbCrlEncoded"></param>
    /// <returns></returns>
    [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern nint CertCreateCRLContext(
        uint dwCertEncodingType,
        byte[] pbCrlEncoded,
        int cbCrlEncoded);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dwCertEncodingType"></param>
    /// <param name="pbCrlEncoded"></param>
    /// <param name="cbCrlEncoded"></param>
    /// <returns></returns>
    [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern nint CertCreateCTLContext(
        uint dwMsgAndCertEncodingType,
        byte[] pbCtlEncoded,
        int cbCtlEncoded);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hCertStore"></param>
    /// <param name="pCertContext"></param>
    /// <param name="dwAddDisposition"></param>
    /// <param name="ppStoreContext"></param>
    /// <returns></returns>
    [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool CertAddCRLContextToStore(
        nint hCertStore,
        nint pCertContext,
        uint dwAddDisposition,
        nint ppStoreContext);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hCertStore"></param>
    /// <param name="pCertContext"></param>
    /// <param name="dwAddDisposition"></param>
    /// <param name="ppStoreContext"></param>
    /// <returns></returns>
    [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool CertAddCertificateContextToStore(
        nint hCertStore,
        nint pCertContext,
        uint dwAddDisposition,
        out nint ppStoreContext);

    [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool CertDeleteCertificateFromStore(nint pCertContext);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hCertStore"></param>
    /// <param name="pCtlContext"></param>
    /// <param name="dwAddDisposition"></param>
    /// <param name="ppStoreContext"></param>
    /// <returns></returns>
    [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool CertAddCTLContextToStore(
        nint hCertStore,
        nint pCtlContext,
        uint dwAddDisposition,
        out nint ppStoreContext);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pCrlContext"></param>
    /// <returns></returns>
    [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool CertFreeCRLContext(
        nint pCrlContext);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hCertStore"></param>
    /// <param name="dwCertEncodingType"></param>
    /// <param name="dwFindFlags"></param>
    /// <param name="dwFindType"></param>
    /// <param name="pvFindPara"></param>
    /// <param name="pPrevCrlContext"></param>
    /// <returns></returns>
    [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern nint CertFindCRLInStore(
        nint hCertStore,
        uint dwCertEncodingType,
        uint dwFindFlags,
        uint dwFindType,
        nint pvFindPara,
        nint pPrevCrlContext);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pCertContext"></param>
    /// <param name="dwType"></param>
    /// <param name="dwFlags"></param>
    /// <param name="pvTypePara"></param>
    /// <param name="pszNameString"></param>
    /// <param name="cchNameString"></param>
    /// <returns></returns>
    [DllImport("crypt32.dll", EntryPoint = "CertGetNameStringW", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    private static extern uint CertGetNameString(
        CERT_CONTEXT pCertContext,
        uint dwType,
        uint dwFlags,
        [In, MarshalAs(UnmanagedType.LPWStr)] string pvTypePara,
        StringBuilder pszNameString,
        uint cchNameString);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hCertStore"></param>
    /// <param name="pIssuerContext"></param>
    /// <param name="pPrevCrlContext"></param>
    /// <param name="pdwFlags"></param>
    /// <returns></returns>
    [DllImport("crypt32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    private static extern nint CertGetCRLFromStore(
        nint hCertStore,
        nint pIssuerContext,
        nint pPrevCrlContext,
        nint pdwFlags);

    [DllImport("crypt32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
    private static extern bool CertGetCertificateContextProperty(
        nint pCertContext,
        uint dwPropId,
        out byte[] pvData,
        out int pcbData);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pCrlContext"></param>
    /// <returns></returns>
    [DllImport("crypt32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
    private static extern bool CertDeleteCRLFromStore(
        nint pCrlContext);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hCertStore"></param>
    /// <param name="pPrevCrlContext"></param>
    /// <returns></returns>
    [DllImport("crypt32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
    private static extern IntPtr CertEnumCRLsInStore(IntPtr hCertStore, IntPtr pPrevCrlContext);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pCrlContext"></param>
    /// <param name="dwPropId"></param>
    /// <param name="pvData"></param>
    /// <param name="pcbData"></param>
    /// <returns></returns>
    [DllImport("crypt32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
    private static extern bool CertGetCRLContextProperty(IntPtr pCrlContext, uint dwPropId, out byte[] pvData, out int pcbData);


    [DllImport("crypt32.dll", EntryPoint = "CertNameToStrW", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
    public static extern int CertNameToStr(uint dwCertEncodingType, ref CRYPTOAPI_BLOB pName, uint dwStrType, StringBuilder psz, int csz);

    [DllImport("crypt32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
    public static extern bool CertCloseStore(nint hCertStore, uint dwFlags);


    public static bool AddOrUpdateCertificateToStore(string filename, bool bAddToRoot = true)
    {
        byte[] rawData = File.ReadAllBytes(filename);

        var result = false;
        var cert = new X509Certificate2(rawData);

        X509Store StoreCA = new(StoreName.CertificateAuthority, StoreLocation.CurrentUser);
        StoreCA.Open(OpenFlags.ReadWrite);
        X509Certificate2Collection CertCollCA = StoreCA.Certificates.Find(X509FindType.FindBySerialNumber, cert.SerialNumber, false);

        if (CertCollCA.Count == 0)
        {
            StoreCA.Add(cert);
            Debug.WriteLine("Add New Cert: " + cert.IssuerName.Name);
        }
        else
        {
            foreach (X509Certificate2 Cert in CertCollCA)
            {
                if (cert.SerialNumber == Cert.SerialNumber)
                {
                    if (!Cert.Verify())
                    {
                        StoreCA.Add(cert);
                        Debug.WriteLine("Change Cert: " + Cert.IssuerName.Name);
                    }
                }
            }
        }

        StoreCA.Close();

        if (bAddToRoot)
        {

            X509Store StoreRoot = new(StoreName.Root, StoreLocation.CurrentUser);
            StoreRoot.Open(OpenFlags.ReadWrite);
            X509Certificate2Collection CertCollRoot = StoreRoot.Certificates.Find(X509FindType.FindBySerialNumber, cert.SerialNumber, false);

            if (CertCollRoot.Count == 0)
            {
                StoreRoot.Add(cert);
                Debug.WriteLine("Add New Cert: " + cert.IssuerName.Name);
            }
            else
            {
                foreach (X509Certificate2 Cert in CertCollRoot)
                {
                    if (cert.SerialNumber == Cert.SerialNumber)
                    {
                        if (!Cert.Verify())
                        {
                            StoreRoot.Add(cert);
                            Debug.WriteLine("Change Cert: " + Cert.IssuerName.Name);
                        }
                    }
                }
            }

            var newCertAdded = StoreRoot.Certificates.Find(X509FindType.FindBySerialNumber, cert.SerialNumber, true).FirstOrDefault();

            StoreRoot.Close();

            result = newCertAdded != null;
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <exception cref="Exception"></exception>
    public static void AddOrUpdateCRLToStore(string filename)
    {
        byte[] rawData = File.ReadAllBytes(filename);

        IntPtr hLocalCertStore = CertOpenStore(
              CryptConst.CERT_STORE_PROV_SYSTEM,
              0,
              IntPtr.Zero,
              CryptConst.CERT_SYSTEM_STORE_CURRENT_USER,
              CryptConst.SystemStore);

        var crlContext = CertCreateCRLContext(
            CryptConst.X509_ASN_ENCODING | CryptConst.PKCS_7_ASN_ENCODING,
            rawData,
            rawData.Length);

        var contextLocal = (CRL_CONTEXT)Marshal.PtrToStructure(crlContext, typeof(CRL_CONTEXT));

        var revokedCertificatelocal = GetRevokedCertificate(contextLocal);

        var foundCrl = FindRevokedCertificateInStore(hLocalCertStore, contextLocal);

        bool needImport;
        if (foundCrl != null)
        {
            needImport = !foundCrl.State && revokedCertificatelocal.State;
        }
        else
        {
            needImport = true;
        }

        if (needImport)
        {
            if (crlContext == IntPtr.Zero)
            {
                string error = "Ошибка импорта СОС #" + Marshal.GetLastWin32Error();
                throw new Exception(error);
            }

            bool crlAddResult = CertAddCRLContextToStore(
                hLocalCertStore, crlContext, CryptConst.CERT_STORE_ADD_REPLACE_EXISTING, IntPtr.Zero);

            if (!crlAddResult)

            {
                string error = "Ошибка импорта СОС #" + Marshal.GetLastWin32Error();
                throw new Exception(error);
            }
        }

        CertFreeCRLContext(crlContext);
        CertCloseStore(hLocalCertStore, CryptConst.CERT_CLOSE_STORE_FORCE_FLAG);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <exception cref="Exception"></exception>
    public static void AddOrUpdateCRLsToStore(IEnumerable<string> filenames)
    {
        IntPtr hLocalCertStore = CertOpenStore(
              CryptConst.CERT_STORE_PROV_SYSTEM,
              0,
              IntPtr.Zero,
              CryptConst.CERT_SYSTEM_STORE_CURRENT_USER,
              CryptConst.SystemStore);

        foreach (var filename in filenames)
        {
            byte[] rawData = File.ReadAllBytes(filename);

            var crlContext = CertCreateCRLContext(
            CryptConst.X509_ASN_ENCODING | CryptConst.PKCS_7_ASN_ENCODING,
            rawData,
            rawData.Length);

            var contextLocal = (CRL_CONTEXT)Marshal.PtrToStructure(crlContext, typeof(CRL_CONTEXT));

            var revokedCertificatelocal = GetRevokedCertificate(contextLocal);

            var foundCrl = FindRevokedCertificateInStore(hLocalCertStore, contextLocal);

            bool needImport;
            if (foundCrl != null)
            {
                needImport = !foundCrl.State && revokedCertificatelocal.State;
            }
            else
            {
                needImport = true;
            }

            if (needImport)
            {
                if (crlContext == IntPtr.Zero)
                {
                    string error = "Ошибка импорта СОС #" + Marshal.GetLastWin32Error();
                    CertCloseStore(hLocalCertStore, CryptConst.CERT_CLOSE_STORE_FORCE_FLAG);
                    throw new Exception(error);
                }

                bool crlAddResult = CertAddCRLContextToStore(
                    hLocalCertStore, crlContext, CryptConst.CERT_STORE_ADD_REPLACE_EXISTING, IntPtr.Zero);

                if (!crlAddResult)

                {
                    string error = "Ошибка импорта СОС #" + Marshal.GetLastWin32Error();
                    CertCloseStore(hLocalCertStore, CryptConst.CERT_CLOSE_STORE_FORCE_FLAG);
                    throw new Exception(error);
                }
            }

            CertFreeCRLContext(crlContext);
        }
       
        CertCloseStore(hLocalCertStore, CryptConst.CERT_CLOSE_STORE_FORCE_FLAG);
    }

    public static RevocationListCertificate FindRevokedCertificate(string name)
    {
        IntPtr hLocalCertStore = CertOpenStore(CryptConst.CERT_STORE_PROV_SYSTEM, 0, IntPtr.Zero, CryptConst.CERT_SYSTEM_STORE_CURRENT_USER, CryptConst.SystemStore);

        var res = FindRevokedCertificateInStore(hLocalCertStore, name);

        CertCloseStore(hLocalCertStore, CryptConst.CERT_CLOSE_STORE_FORCE_FLAG);

        return res;
    }

    public static List<RevocationListCertificate> FindRevokedCertificates(IEnumerable<string> names)
    {
        IntPtr hLocalCertStore = CertOpenStore(CryptConst.CERT_STORE_PROV_SYSTEM, 0, IntPtr.Zero, CryptConst.CERT_SYSTEM_STORE_CURRENT_USER, CryptConst.SystemStore);

        var res = FindRevokedCertificatesInStore(hLocalCertStore, names);

        CertCloseStore(hLocalCertStore, CryptConst.CERT_CLOSE_STORE_FORCE_FLAG);

        return res;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="store"></param>
    /// <returns></returns>
    private static List<CRL_CONTEXT> GetAllCRLContexts(nint store)
    {
        var currentCRLContexts = new List<CRL_CONTEXT>();

        var newCrlContext = IntPtr.Zero;

        if (newCrlContext == IntPtr.Zero)
        {
            newCrlContext = CertEnumCRLsInStore(store, IntPtr.Zero);

            if (newCrlContext != IntPtr.Zero)
            {
                var crlContext = (CRL_CONTEXT)Marshal.PtrToStructure(newCrlContext, typeof(CRL_CONTEXT));
                //Debug.WriteLine(GetRevokedCertificate(crlContext).Name);
                currentCRLContexts.Add(crlContext);
            }
        }

        while (newCrlContext != IntPtr.Zero)
        {
            newCrlContext = CertEnumCRLsInStore(store, newCrlContext);

            if (newCrlContext != IntPtr.Zero)
            {
                var crlContext = (CRL_CONTEXT)Marshal.PtrToStructure(newCrlContext, typeof(CRL_CONTEXT));
                //Debug.WriteLine(GetRevokedCertificate(crlContext).Name);
                currentCRLContexts.Add(crlContext);
            }
        }

        CertFreeCRLContext(newCrlContext);

        return currentCRLContexts;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="store"></param>
    /// <returns></returns>
    private static Dictionary<nint,CERT_CONTEXT> GetAllCertContexts(nint store)
    {
        var currentCertContexts = new Dictionary<nint, CERT_CONTEXT>();

        var newCertContext = IntPtr.Zero;
        while ((newCertContext = CertEnumCertificatesInStore(store, newCertContext)) != IntPtr.Zero)
        {
            if (newCertContext != IntPtr.Zero)
            {
                if (!newCertContext.Equals(IntPtr.Zero))
                {
                    if (!currentCertContexts.ContainsKey(newCertContext))
                    {
                        var certContext = (CERT_CONTEXT)Marshal.PtrToStructure(newCertContext, typeof(CERT_CONTEXT));
                        currentCertContexts.Add(newCertContext, certContext);
                    }
                }
            }
        }

        CertFreeCertificateContext(newCertContext);

        return currentCertContexts;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="store"></param>
    /// <param name="certContext"></param>
    /// <returns></returns>
    private static Certificate FindCertificateInStore(nint store, nint pCertContext, CERT_CONTEXT certContext)
    {
        var constexts = GetAllCertContexts(store);
        
        var localCertificate = GetCertificate(pCertContext, certContext);

        if (constexts.Count == 0)
        {
            return null;
        }

        Certificate returnCert = null;

        foreach (var context in constexts)
        {
            var certificate = GetCertificate(context.Key, context.Value);

            if (certificate != null)
            {
                if (localCertificate.Subject == certificate.Subject)
                {
                    returnCert = certificate;
                }
            }
        }

        return returnCert;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="store"></param>
    /// <param name="certificate"></param>
    /// <returns></returns>
    private static Certificate FindCertificateInStore(nint store, Certificate certificate)
    {
        var constexts = GetAllCertContexts(store);

        //var localCertificate = GetCertificate(certContext);

        if (constexts.Count == 0) return null;

        Certificate returnCert = null;

        foreach (var context in constexts)
        {
            var currentCertificate = GetCertificate(context.Key, context.Value);

            if (certificate != null)
            {
                if (certificate.Subject == currentCertificate.Subject)
                {
                    returnCert = certificate;
                }
            }
        }

        return returnCert;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="store"></param>
    /// <param name="clrContext"></param>
    /// <returns></returns>
    private static RevocationListCertificate FindRevokedCertificateInStore(nint store, CRL_CONTEXT clrContext)
    {
        var constexts = GetAllCRLContexts(store);

        var localRevokedCertificate = GetRevokedCertificate(clrContext);

        if (constexts.Count == 0) return null;

        foreach (var context in constexts)
        {
            var revokedCertificate = GetRevokedCertificate(context);

            if (revokedCertificate != null)
            {
                if (localRevokedCertificate.Name == revokedCertificate.Name)
                {
                    return revokedCertificate;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="store"></param>
    /// <param name="clrContext"></param>
    /// <returns></returns>
    private static List<RevocationListCertificate> FindRevokedCertificatesInStore(nint store, IEnumerable<CRL_CONTEXT> clrContexts)
    {
        var res = new List<RevocationListCertificate>();

        var constexts = GetAllCRLContexts(store);

        var localRevokedCertificates = GetRevokedCertificates(clrContexts);

        if (constexts.Count == 0) return null;

        foreach (var context in constexts)
        {
            var revokedCertificate = GetRevokedCertificate(context);

            if (revokedCertificate != null)
            {
                var local = localRevokedCertificates.FirstOrDefault(x => x.Name == revokedCertificate.Name);

                if(local != null)
                {
                    res.Add(local);
                }
            }
        }

        return res;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="store"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private static RevocationListCertificate FindRevokedCertificateInStore(nint store, string name)
    {

        var constexts = GetAllCRLContexts(store);

        if (constexts.Count == 0) return null;

        foreach (var context in constexts)
        {
            var revokedCertificate = GetRevokedCertificate(context);
            if (revokedCertificate != null)
            {
                if (revokedCertificate.Name == name)
                {
                    return revokedCertificate;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="store"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private static List<RevocationListCertificate> FindRevokedCertificatesInStore(nint store, IEnumerable<string> names)
    {
        var res = new List<RevocationListCertificate>();

        var constexts = GetAllCRLContexts(store);

        if (constexts.Count == 0) return null;

        foreach (var context in constexts)
        {
            var revokedCertificate = GetRevokedCertificate(context);
            if (revokedCertificate != null)
            {
                if (names.Contains(revokedCertificate.Name))
                {
                    res.Add(revokedCertificate);
                }
            }
        }

        return res;
    }

    private static RevocationListCertificate GetRevokedCertificate(CRL_CONTEXT clrContext)
    {
        var CRLInfo = (CRL_INFO)Marshal.PtrToStructure(clrContext.pCrlInfo, typeof(CRL_INFO));

        var csz = CertNameToStr(CryptConst.X509_ASN_ENCODING | CryptConst.PKCS_7_ASN_ENCODING,
           ref CRLInfo.Issuer,
           CryptConst.CERT_X500_NAME_STR,
           null,
           0
       );

        var psz = new StringBuilder(csz);

        ///

        csz = CertNameToStr(CryptConst.X509_ASN_ENCODING | CryptConst.PKCS_7_ASN_ENCODING, ref CRLInfo.Issuer, CryptConst.CERT_X500_NAME_STR, psz, csz);

        if (csz <= 0)
        {
            throw new Exception("Ошибка получения имени сертификата #" + Marshal.GetLastWin32Error());
        }

        if (CRLInfo.rgCRLEntry != IntPtr.Zero)
        {
            var crlEntry = (CRL_ENTRY)Marshal.PtrToStructure(CRLInfo.rgCRLEntry, typeof(CRL_ENTRY));

            var revoc = crlEntry.RevocationDate.ToDateTime();

            var value = new byte[crlEntry.SerialNumber.cbData];
            Marshal.Copy(crlEntry.SerialNumber.pbData, value, 0, crlEntry.SerialNumber.cbData);

            var serial = BitConverter.ToString(value).Replace("-", string.Empty);
        }

        var thisUpdate = CRLInfo.ThisUpdate.ToDateTime();
        var nextUpdate = CRLInfo.NextUpdate.ToDateTime();

        var currentDate = DateTime.Now;

        var test = Regex.Match(psz.ToString(), @"([A-Z]*\s?)=((.*?)),\s");

        Dictionary<string,string> kvs = new Dictionary<string,string>();

        while (test.Success)
        {
            kvs.Add(test.Groups[1].ToString(), test.Groups[2].ToString());
            test = test.NextMatch();
        }

        var simpleName = kvs.FirstOrDefault(x => x.Key == "CN");

        var rc = new RevocationListCertificate
        {
            Name = psz.ToString(),
            SimpleName = simpleName.Value,
            ThisUpdate = thisUpdate,
            NextUpdate = nextUpdate,
            State = true,
        };


        if (currentDate < thisUpdate)
        {
            rc.Error.Result = $"Срок действия СОС {rc.SimpleName} не наступил";
            rc.Error.Code = "0x0";
            rc.Error.State = true;
            rc.State = false;
        }
        else if (currentDate > nextUpdate)
        {
            rc.Error.Result = $"Срок действия СОС {rc.SimpleName} истек";
            rc.Error.Code = "0x0";
            rc.Error.State = true;
            rc.State = false;
        }

        return rc;
    }

    private static Certificate GetCertificate(string filename)
    {
        byte[] rawData = File.ReadAllBytes(filename);

        var pFileCertContext = CertCreateContext(
                CryptConst.CERT_STORE_CERTIFICATE_CONTEXT,
               CryptConst.X509_ASN_ENCODING | CryptConst.PKCS_7_ASN_ENCODING,
               rawData,
               rawData.Length,
               CryptConst.CERT_CREATE_CONTEXT_NOCOPY_FLAG, IntPtr.Zero);

        var fileCertContext = (CERT_CONTEXT)Marshal.PtrToStructure(pFileCertContext, typeof(CERT_CONTEXT));
        var localCertificate = GetCertificate(pFileCertContext, fileCertContext);

        CertFreeCertificateContext(pFileCertContext);

        return localCertificate;
    }

    private static Certificate GetCertificate(nint pCertContext, CERT_CONTEXT certContext)
    {
        var certificate = new Certificate
        {
            PCertContex = pCertContext
        };
        certificate.SetContext(certContext);

        if (certContext.pbCertEncoded == IntPtr.Zero) return null;

        if (certContext.pCertInfo != IntPtr.Zero)
        {
            var certInfo = (CERT_INFO)Marshal.PtrToStructure(certContext.pCertInfo, typeof(CERT_INFO));
            certificate.Version = certInfo.dwVersion;

            var csz = CertNameToStr(CryptConst.X509_ASN_ENCODING | CryptConst.PKCS_7_ASN_ENCODING,
               ref certInfo.Issuer,
               CryptConst.CERT_X500_NAME_STR,
               null,
               0
            );

            var psz = new StringBuilder(csz);

            ///

            _ = CertNameToStr(CryptConst.X509_ASN_ENCODING | CryptConst.PKCS_7_ASN_ENCODING, ref certInfo.Issuer, CryptConst.CERT_X500_NAME_STR, psz, csz);

            certificate.Issuer = psz.ToString();

            // Subject

            csz = CertNameToStr(CryptConst.X509_ASN_ENCODING | CryptConst.PKCS_7_ASN_ENCODING,
              ref certInfo.Subject,
              CryptConst.CERT_X500_NAME_STR,
              null,
              0
           );

            psz = new StringBuilder(csz);

            ///

            _ = CertNameToStr(CryptConst.X509_ASN_ENCODING | CryptConst.PKCS_7_ASN_ENCODING, ref certInfo.Subject, CryptConst.CERT_X500_NAME_STR, psz, csz);

            certificate.Subject = psz.ToString();

            var value = new byte[certInfo.SerialNumber.cbData];
            Marshal.Copy(certInfo.SerialNumber.pbData, value, 0, (int)certInfo.SerialNumber.cbData);
            //AvNative.HeapFree(AvNative.GetProcessHeap(), 0, crlEntry.SerialNumber.pbData); // освобождаем память

            var serial = BitConverter.ToString(value).Replace("-", string.Empty);


            certificate.Serial = serial;


            certificate.NotBefore = certInfo.NotBefore.ToDateTime();
            certificate.NotAfter = certInfo.NotAfter.ToDateTime();

            var currentDate = DateTime.Now;

            if (currentDate < certificate.NotBefore)
            {
                certificate.Error.Result = $"Срок действия сертификата {certificate.Subject} не наступил";
                certificate.Error.Code = "0x0";
                certificate.Error.State = true;
                certificate.State = false;
            }
            else if (currentDate > certificate.NotAfter)
            {
                certificate.Error.Result = $"Срок действия сертификата {certificate.Subject} истек";
                certificate.Error.Code = "0x0";
                certificate.Error.State = true;
                certificate.State = false;
            }
        }

        if (!certificate.Error.State)
        {
            certificate.State = true;
        }

        return certificate;
    }

    private static List<RevocationListCertificate> GetRevokedCertificates(IEnumerable<CRL_CONTEXT> clrContexts)
    {
        var crls = new List<RevocationListCertificate>();

        foreach (var item in clrContexts)
        {
            var crl = GetRevokedCertificate(item);
            crls.Add(crl);
        }

        return crls;
    }
}

public static class FILETIMEExtensions
{
    public static DateTime ToDateTime(this FILETIME time)
    {
        ulong high = (ulong)time.dwHighDateTime;
        uint low = (uint)time.dwLowDateTime;
        long fileTime = (long)((high << 32) + low);
        try
        {
            return DateTime.FromFileTime(fileTime);
        }
        catch
        {
            return DateTime.FromFileTime(0xFFFFFFFF);
        }
    }
}


public class Certificate
{
    /// <summary>
    /// 
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime NotAfter { get; set; }

    /// <summary>
    /// Дата начала действия сертификата
    /// </summary>
    public DateTime NotBefore { get; set; }

    /// <summary>
    /// Дата окончания действия сертификата
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool State { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Serial { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Error Error { get; set; } = new Error();

    /// <summary>
    /// 
    /// </summary>
    private CERT_CONTEXT _context;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public CERT_CONTEXT GetContext() => _context;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void SetContext(CERT_CONTEXT context) => _context = context;

    public override bool Equals(object obj)
    {
        // If the passed object is null
        if (obj == null)
        {
            return false;
        }
        return obj is Certificate certificate
            && (Subject == certificate.Subject)
            && (Issuer == certificate.Issuer);
    }
    //Overriding the GetHashCode method
    //GetHashCode method generates hashcode for the current object
    public override int GetHashCode()
    {
        //Performing BIT wise OR Operation on the generated hashcode values
        //If the corresponding bits are different, it gives 1.
        //If the corresponding bits are the same, it gives 0.
        return Subject.GetHashCode() ^ Issuer.GetHashCode();
    }

    public nint PCertContex { get; set; }
}

public class RevocationListCertificate
{
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string SimpleName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime ThisUpdate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime NextUpdate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool State { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Error Error { get; set; } = new Error();
}