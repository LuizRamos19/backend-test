using System.Data;
using Snapper.Core.DataBase.Engines;

namespace teste.ApiCore31.Helpers
{
    /// <summary>
    /// Class for helpind deal with data base
    /// </summary>
    public static class DataHelper 
    {
        /// <summary>
        /// Check existence for some column in the table
        /// </summary>
        /// <param name="table">Table to check</param>
        /// <param name="columnName">Column name to check</param>
        /// <returns>True our False</returns>
        public static bool ColumnExists(DataTable table, string columnName) =>
            !string.IsNullOrEmpty(columnName) && table.Columns.Contains(columnName.ToLower());

        /// <summary>
        /// Create native connection to Oracle data base.
        /// </summary>
        /// <returns>Snapper that contais connection ready for use</returns>
        public static Snapper.Core.Snapper CreateOracleConnection() => 
            new Snapper.Core.Snapper("XE","localhost","system","Teste123456",new OracleEngine());
    }
}