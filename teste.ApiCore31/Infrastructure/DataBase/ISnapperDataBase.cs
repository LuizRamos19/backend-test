namespace teste.ApiCore31.Infrastructure.DataBase
{
    public interface ISnapperDataBase
    {
        /// <summary>
        /// Create native connection to Oracle data base.
        /// </summary>
        /// <returns>Snapper that contais connection ready for use</returns>
        Snapper.Core.Snapper CreateOracleConnection();
    }
}
