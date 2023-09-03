using System.ComponentModel.DataAnnotations;

namespace teste.Consumer.Enumerators
{
    /// <summary>
    /// Enumerator for all package bodies existent or in use.
    /// </summary>
    public enum OracleProcedure
    {
        /// <summary>
        /// PROCESS_SALE, that is the name in the Oracle Database.
        /// </summary>
        [Display(Name = "SAVE_SALE")]
        SaveSale
    }
}
