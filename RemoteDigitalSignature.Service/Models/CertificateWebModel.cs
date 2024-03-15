namespace RemoteDigitalSignature.Service.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CertificateResultWebModel : BaseWithErrorResultWebModel
    {
        /// <summary>
        /// 
        /// </summary>
        public CertificateResultWebModel()
        {
            Attributes = new List<CertificateAttributeResultWebModel>();
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CertificateAttributeResultWebModel> Attributes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// серийный номер сертификата
        /// </summary>
        public string SerialNumber { get; set; } = null!;

        /// <summary>
        /// контрольная характеристика
        /// </summary>
        public string ThumbPrint { get; set; } = null!;

        /// <summary>
        /// идентификатор ключа субъекта (CertId)
        /// </summary>
        public string PublicKeyId { get; set; } = null!;

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
        public string PublicKey { get; set; } = null!;
    }

    /// <summary>
    /// 
    /// </summary>
    public class CertificateAttributeResultWebModel : BaseWithErrorResultWebModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Oid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Value { get; set; }
    }
}