using Microsoft.Data.Sqlite;

namespace AkademikAsistan.Data
{
    /// <summary>
    /// SQLite bağlantılarını tek bir yerden üretir. Repository'ler bu factory'i
    /// constructor injection ile alır; bağlantı string'i App katmanından (appsettings
    /// veya sabit bir yoldan) DI container'a kaydedilir.
    /// </summary>
    public interface ISqliteConnectionFactory
    {
        SqliteConnection CreateConnection();
    }

    public class SqliteConnectionFactory : ISqliteConnectionFactory
    {
        private readonly string _connectionString;

        public SqliteConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqliteConnection CreateConnection() => new(_connectionString);
    }
}
