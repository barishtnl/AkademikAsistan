using AkademikAsistan.Core.Interfaces;
using AkademikAsistan.Core.Models;
using Dapper;

namespace AkademikAsistan.Data.Repositories
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly ISqliteConnectionFactory _connectionFactory;

        public SemesterRepository(ISqliteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IReadOnlyList<SemesterRecord>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT SemesterRecordId, SemesterName, SemesterGpa, CumulativeGpa, TotalCredits
                FROM SemesterRecords
                ORDER BY SemesterRecordId
                """;

            var result = await connection.QueryAsync<SemesterRecord>(sql);
            return result.ToList();
        }

        public async Task<int> AddAsync(SemesterRecord record)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                INSERT INTO SemesterRecords (SemesterName, SemesterGpa, CumulativeGpa, TotalCredits)
                VALUES (@SemesterName, @SemesterGpa, @CumulativeGpa, @TotalCredits);
                SELECT last_insert_rowid();
                """;

            return await connection.ExecuteScalarAsync<int>(sql, record);
        }
    }
}
