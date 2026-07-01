namespace AkademikAsistan.Core.Models
{
    /// <summary>
    /// Bir döneme ait GNO/YNO (DNO) kaydı. SemesterRecords tablosuna birebir karşılık gelir.
    /// </summary>
    public class SemesterRecord
    {
        public int SemesterRecordId { get; set; }
        public string SemesterName { get; set; } = string.Empty;

        /// <summary>O dönemin ağırlıklı ortalaması (DNO).</summary>
        public double SemesterGpa { get; set; }

        /// <summary>O döneme kadarki genel ağırlıklı ortalama (GNO).</summary>
        public double CumulativeGpa { get; set; }

        public int TotalCredits { get; set; }
    }
}
