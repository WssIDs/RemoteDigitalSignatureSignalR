namespace Common.Exceptions
{
    /// <summary>
    /// Исключение CommonException
    /// </summary>
    public class CommonException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public CommonException(string message) : base(message) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CommonException(string message, Exception innerException) : base(message, innerException) { }
    }
}
