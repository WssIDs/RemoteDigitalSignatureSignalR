namespace RemoteDigitalSignature.Service.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class RevocationListCertificateResultWebModel : BaseWithErrorResultWebModel
    {
        /// <summary>
        /// 
        /// </summary>
        public RevocationListCertificateResultWebModel()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public string SimpleName { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public DateTime ThisUpdate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime NextUpdate { get; set; }
    }
}