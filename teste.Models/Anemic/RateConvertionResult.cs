
using System;

namespace teste.Models.Anemic
{
    /// <summary>
    /// Class of Anemic object to store data from result convertion
    /// </summary>
    public class RateConvertionResult
    {
        public RateConvertionResult()
        {
            TransactionAmount = 0;
            TimeLastUpdateUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Time where transaction must expire
        /// </summary>
        public DateTime TimeLastUpdateUtc { get; set; }

        /// <summary>
        /// Tthe Transaction Amount converted value
        /// </summary>
        public double TransactionAmount { get; set; }
    }
}
