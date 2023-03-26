using System.Data;

namespace HealthMonitor.Repository
{
    public interface IRepository
    {
        IDbConnection DbConnection { get; }
    }
}
