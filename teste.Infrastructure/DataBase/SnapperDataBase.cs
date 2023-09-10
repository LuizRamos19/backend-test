using Snapper.Core.DataBase.Engines;

namespace teste.ApiCore31.Infrastructure.DataBase
{
    public class SnapperDataBase : ISnapperDataBase
    {
        /// <summary>
        /// Create native connection to Oracle database.
        /// </summary>
        /// <returns>Snapper that contais connection ready for use</returns>
        Snapper.Core.Snapper ISnapperDataBase.CreateOracleConnection() =>
            new Snapper.Core.Snapper("XE", "localhost", "system", "Teste123456", new OracleEngine());
    }
}
