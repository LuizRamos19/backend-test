using teste.ApiCore31.Enumerators;
using teste.ApiCore31.Extensions;

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
        private static readonly string[] origins = { "http://example.com", "http://www.contoso.com" };

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

        /// <summary>
        /// Kafka local host address
        /// </summary>
        public static string KafkaHost => "localhost:9092";

        /// <summary>
        /// Alowed cors origins alias
        /// </summary>
        public static string ExchangeRateApiKey => "351b37bfc54dbe41826820d1";

        /// <summary>
        /// Alowed cors origins alias
        /// </summary>
        public static string RedisInstaceName => "redis";

        /// <summary>
        /// Alowed cors origins alias
        /// </summary>
        public static string RedisConfiguration => "127.0.0.1:6379"; 

    }
}