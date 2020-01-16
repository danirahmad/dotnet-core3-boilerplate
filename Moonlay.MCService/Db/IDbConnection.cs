using Microsoft.Data.SqlClient;

namespace Moonlay.MasterData.WebApi.Db
{
    public interface IDbConnection
    {
        SqlConnection Connection { get; }
    }
}