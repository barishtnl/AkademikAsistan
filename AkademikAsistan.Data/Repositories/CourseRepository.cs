using AkademikAsistan.Core.Interfaces;
using AkademikAsistan.Core.Models;
using Dapper;

namespace AkademikAsistan.Data.Repositories
{
    /// <summary>
    /// ICourseRepository'nin Dapper + SQLite implementasyonu. ViewModel'ler bu sınıfı
    /// değil, ICourseRepository'yi bilir (DI container App katmanında bağlar).
    /// </summary>
    public class CourseRepository : ICourseRepository
    {
        private readonly ISqliteConnectionFactory _connectionFactory;

        public CourseRepository(ISqliteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IReadOnlyList<Course>> GetBySemesterAsync(string semester)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                SELECT CourseId, CourseName, CourseCode, InstructorName, DayOfWeek,
                       StartTime, EndTime, Classroom, Credit, Semester
                FROM Courses
                WHERE Semester = @Semester
                ORDER BY DayOfWeek, StartTime
                """;

            var result = await connection.QueryAsync<Course>(sql, new { Semester = semester });
            return result.ToList();
        }

        public async Task<int> AddAsync(Course course)
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = """
                INSERT INTO Courses
                    (CourseName, CourseCode, InstructorName, DayOfWeek, StartTime, EndTime, Classroom, Credit, Semester)
                VALUES
                    (@CourseName, @CourseCode, @InstructorName, @DayOfWeek, @StartTime, @EndTime, @Classroom, @Credit, @Semester);
                SELECT last_insert_rowid();
                """;

            return await connection.ExecuteScalarAsync<int>(sql, course);
        }

        public async Task DeleteAsync(int courseId)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM Courses WHERE CourseId = @CourseId", new { CourseId = courseId });
        }
    }
}
