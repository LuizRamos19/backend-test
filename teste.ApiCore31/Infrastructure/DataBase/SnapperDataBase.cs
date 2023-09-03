using Snapper.Core.DataBase.Engines;

namespace teste.ApiCore31.Infrastructure.DataBase
{
    public class SnapperDataBase : ISnapperDataBase
    {
        public Snapper.Core.Snapper CreateOracleConnection() =>
            new Snapper.Core.Snapper("XE", "localhost", "system", "Teste123456", new OracleEngine());
    }
}
