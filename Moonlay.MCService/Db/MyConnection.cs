using Microsoft.Data.SqlClient;

namespace Moonlay.MasterData.WebApi.Db
{
    public class MyConnection : IDbConnection
    {
        public MyConnection(SqlConnection connection)
        {
            Connection = connection;
        }

        public SqlConnection Connection { get; }
    }
}
