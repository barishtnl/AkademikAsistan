namespace AkademikAsistan.Core.Models
{
    /// <summary>
    /// Haftalık ders programındaki tek bir ders kaydı. Courses tablosuna birebir karşılık gelir.
    /// </summary>
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string? CourseCode { get; set; }
        public string? InstructorName { get; set; }

        /// <summary>1=Pazartesi ... 7=Pazar (Core.Enums.WeekDay ile eşleşir).</summary>
        public int DayOfWeek { get; set; }

        /// <summary>"HH:mm" formatında, örn. "09:00".</summary>
        public string StartTime { get; set; } = "09:00";

        /// <summary>"HH:mm" formatında, örn. "10:50".</summary>
        public string EndTime { get; set; } = "10:50";

        public string? Classroom { get; set; }
        public int Credit { get; set; }
        public string Semester { get; set; } = string.Empty;
    }
}
