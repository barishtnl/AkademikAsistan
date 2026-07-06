namespace AkademikAsistan.Data
{
    /// <summary>
    /// Uygulama ilk açılışta (veya veritabanı dosyası yoksa) Scripts/schema.sql
    /// içeriğini çalıştırarak tabloları oluşturur. App.xaml.cs içinde, ana pencere
    /// gösterilmeden önce bir kez çağrılır.
    /// </summary>
    public class DatabaseInitializer
    {
        private readonly ISqliteConnectionFactory _connectionFactory;

        public DatabaseInitializer(ISqliteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void EnsureCreated()
        {
            var schemaPath = Path.Combine(AppContext.BaseDirectory, "Scripts", "schema.sql");
            if (!File.Exists(schemaPath))
                throw new FileNotFoundException("schema.sql bulunamadı. csproj'daki CopyToOutputDirectory ayarını kontrol edin.", schemaPath);

            string schemaSql = File.ReadAllText(schemaPath);

            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = schemaSql;
            command.ExecuteNonQuery();
        }
    }
}
