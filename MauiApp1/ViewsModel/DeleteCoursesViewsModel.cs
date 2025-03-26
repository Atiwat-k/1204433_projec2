using System.Diagnostics;
using MauiApp1.Model;
using MauiApp2.Model;
using MauiApp1.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

public class DeleteCoursesViewModel : INotifyPropertyChanged
{
    private readonly StudentService _studentService;
    private readonly CourseService _courseService;
    private Student _student;
    private ObservableCollection<EnrolledCourse> _enrolledCourses;
    
    public event PropertyChangedEventHandler PropertyChanged;

    public string UserId { get; set; }

    protected virtual void OnPropertyChanged(string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public Student Student
    {
        get => _student;
        set
        {
            _student = value;
            OnPropertyChanged(nameof(Student));
            OnPropertyChanged(nameof(StudentProfile));
            OnPropertyChanged(nameof(CurrentTerm));
            Debug.WriteLine($"Student set: {value?.Id}");
        }
    }

    public Profile StudentProfile => Student?.Profile;
    public Term CurrentTerm => Student?.CurrentTerm;

    public ObservableCollection<EnrolledCourse> EnrolledCourses
    {
        get => _enrolledCourses;
        set
        {
            _enrolledCourses = value;
            OnPropertyChanged(nameof(EnrolledCourses));
        }
    }

    public DeleteCoursesViewModel(StudentService studentService, CourseService courseService)
    {
        _studentService = studentService;
        _courseService = courseService;
        EnrolledCourses = new ObservableCollection<EnrolledCourse>();
    }

    public async Task LoadDataAsync(string userId)
    {
        Debug.WriteLine($"Loading data for user: {userId}");
        
        try
        {
            var student = await _studentService.GetStudentByIdAsync(userId);
            if (student != null)
            {
                Student = student;
                await LoadEnrolledCoursesDetails();
            }
            else
            {
                Debug.WriteLine("Student not found");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading data: {ex.Message}");
        }
    }

    private async Task LoadEnrolledCoursesDetails()
    {
        if (CurrentTerm?.EnrolledCourses == null || !CurrentTerm.EnrolledCourses.Any())
        {
            Debug.WriteLine("No enrolled courses");
            EnrolledCourses.Clear();
            return;
        }

        try
        {
            var allCourses = await _courseService.LoadCoursesAsync();
            var enrolledCoursesDetails = new ObservableCollection<EnrolledCourse>();

            foreach (var courseId in CurrentTerm.EnrolledCourses)
            {
                var course = allCourses.FirstOrDefault(c => c.CourseId == courseId);
                enrolledCoursesDetails.Add(new EnrolledCourse
                {
                    CourseId = courseId,
                    CourseName = course?.CourseName ?? "Unknown Course",
                    Credits = (int)(course?.Credits ?? 0)
                });
            }

            EnrolledCourses = enrolledCoursesDetails;
            Debug.WriteLine($"Loaded {EnrolledCourses.Count} enrolled courses");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading course details: {ex.Message}");
        }
    }
}