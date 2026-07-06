using System.Collections.ObjectModel;
using System.Windows.Input;
using AkademikAsistan.App.Common;
using AkademikAsistan.Core.Interfaces;
using AkademikAsistan.Core.Models;

namespace AkademikAsistan.App.ViewModels.DersPlani
{
    /// <summary>
    /// Haftalık ders programı ekranının ViewModel'i. Tüm veritabanı işlemleri
    /// ICourseRepository'ye devredilir; bu sınıf sadece UI durumunu yönetir.
    /// </summary>
    public class DersPlaniViewModel : ViewModelBase
    {
        private readonly ICourseRepository _courseRepository;
        private const string DefaultSemester = "2025-2026 Güz";

        public DersPlaniViewModel(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;

            Courses = new ObservableCollection<Course>();
            DayGroups = new ObservableCollection<DayGroup>();

            LoadCommand = new AsyncRelayCommand(_ => LoadAsync(), onException: HandleError);
            AddCourseCommand = new AsyncRelayCommand(_ => AddCourseAsync(), _ => CanAddCourse(), HandleError);
            DeleteCourseCommand = new AsyncRelayCommand(param => DeleteCourseAsync(param), onException: HandleError);

            // Tasarım zamanında / ilk açılışta otomatik yükle.
            LoadCommand.Execute(null);
        }

        public ObservableCollection<Course> Courses { get; }

        /// <summary>Haftalık görünüm için güne göre gruplanmış dersler (Pazartesi..Cuma).</summary>
        public ObservableCollection<DayGroup> DayGroups { get; }

        // ----- Yeni ders formu -----

        private string _newCourseName = string.Empty;
        public string NewCourseName { get => _newCourseName; set => SetProperty(ref _newCourseName, value); }

        private string _newCourseCode = string.Empty;
        public string NewCourseCode { get => _newCourseCode; set => SetProperty(ref _newCourseCode, value); }

        private string _newInstructorName = string.Empty;
        public string NewInstructorName { get => _newInstructorName; set => SetProperty(ref _newInstructorName, value); }

        private int _newDayOfWeek = 1;
        public int NewDayOfWeek { get => _newDayOfWeek; set => SetProperty(ref _newDayOfWeek, value); }

        private string _newStartTime = "09:00";
        public string NewStartTime { get => _newStartTime; set => SetProperty(ref _newStartTime, value); }

        private string _newEndTime = "10:50";
        public string NewEndTime { get => _newEndTime; set => SetProperty(ref _newEndTime, value); }

        private string _newClassroom = string.Empty;
        public string NewClassroom { get => _newClassroom; set => SetProperty(ref _newClassroom, value); }

        private int _newCredit = 3;
        public int NewCredit { get => _newCredit; set => SetProperty(ref _newCredit, value); }

        private string? _errorMessage;
        public string? ErrorMessage { get => _errorMessage; private set => SetProperty(ref _errorMessage, value); }

        public ICommand LoadCommand { get; }
        public ICommand AddCourseCommand { get; }
        public ICommand DeleteCourseCommand { get; }

        private bool CanAddCourse() => !string.IsNullOrWhiteSpace(NewCourseName);

        private async Task LoadAsync()
        {
            ErrorMessage = null;
            var courses = await _courseRepository.GetBySemesterAsync(DefaultSemester);

            Courses.Clear();
            foreach (var course in courses)
                Courses.Add(course);

            RebuildDayGroups();
        }

        private async Task AddCourseAsync()
        {
            ErrorMessage = null;

            var course = new Course
            {
                CourseName = NewCourseName,
                CourseCode = NewCourseCode,
                InstructorName = NewInstructorName,
                DayOfWeek = NewDayOfWeek,
                StartTime = NewStartTime,
                EndTime = NewEndTime,
                Classroom = NewClassroom,
                Credit = NewCredit,
                Semester = DefaultSemester
            };

            course.CourseId = await _courseRepository.AddAsync(course);
            Courses.Add(course);
            RebuildDayGroups();

            // Formu temizle, ders eklemeye devam edebilsin diye gün/saat korunur.
            NewCourseName = string.Empty;
            NewCourseCode = string.Empty;
            NewInstructorName = string.Empty;
            NewClassroom = string.Empty;
        }

        private async Task DeleteCourseAsync(object? parameter)
        {
            if (parameter is not Course course) return;

            ErrorMessage = null;
            await _courseRepository.DeleteAsync(course.CourseId);
            Courses.Remove(course);
            RebuildDayGroups();
        }

        private void RebuildDayGroups()
        {
            DayGroups.Clear();
            var dayNames = new[] { "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar" };

            for (int day = 1; day <= 7; day++)
            {
                var coursesForDay = Courses
                    .Where(c => c.DayOfWeek == day)
                    .OrderBy(c => c.StartTime)
                    .ToList();

                // Hafta sonu boşsa görünümü kalabalıklaştırmamak için ekleme yapılmaz.
                if (day >= 6 && coursesForDay.Count == 0) continue;

                DayGroups.Add(new DayGroup(dayNames[day - 1], coursesForDay));
            }
        }

        private void HandleError(Exception ex) => ErrorMessage = ex.Message;
    }

    /// <summary>Bir günün adı ve o güne ait derslerin listesi (haftalık görünüm için).</summary>
    public class DayGroup
    {
        public DayGroup(string dayName, IReadOnlyList<Course> courses)
        {
            DayName = dayName;
            Courses = courses;
        }

        public string DayName { get; }
        public IReadOnlyList<Course> Courses { get; }
    }
}
