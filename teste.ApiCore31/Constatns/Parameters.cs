namespace teste.ApiCore31.Constatns
{
    /// <summary>
    /// Class of parameters for application use
    /// </summary>
    public static class Parameters
    {
        /// <summary>
        /// List of cors allowed origins
        /// </summary>
        private static readonly string[] origins = {"http://example.com", "http://www.contoso.com"};

        /// <summary>
        /// Api description parameter
        /// </summary>
        public static string ApiDescription => "Teste Avanade para construção de API para processamento de dados";


        /// <summary>
        /// API Version
        /// </summary>
        public static string ApiCurrentVersion => "v1";

        /// <summary>
        /// API Name
        /// </summary>
        public static string ApiName => "Teste API";

        /// <summary>
        /// Alowed cors origins alias
        /// </summary>
        public static string AllowedSpecificOrigins => "_allowSpecificOrigins";

        /// <summary>
        /// List of allowed cors origins
        /// </summary>
        public static string[] Origins => origins;
    }
}