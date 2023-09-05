
using System;

namespace teste.Models
{
    /// <summary>
    /// Class for store data from result convertion
    /// </summary>
    public class RateConvertionResult
    {
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
