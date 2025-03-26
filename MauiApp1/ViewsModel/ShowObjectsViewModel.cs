using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Model;
using MauiApp1.Services;

namespace MauiApp1.ViewModel
{
    public partial class ShowObjectsViewModel : ObservableObject
    {
        private readonly StudentService _studentService;
        private readonly CourseService _courseService; // New service to load course details

        [ObservableProperty]
        private ObservableCollection<Term> allTerms = new();

        [ObservableProperty]
        private Term selectedTerm;

        [ObservableProperty]
        private ObservableCollection<EnrolledCourse> displayedCourses = new();

        [ObservableProperty]
        private string userId;

        [ObservableProperty]
        private Profile studentProfile;

        public ShowObjectsViewModel(StudentService studentService, CourseService courseService)
        {
            _studentService = studentService;
            _courseService = courseService; // Initialize CourseService
        }

        public async Task LoadDataAsync()
        {
            var student = await _studentService.GetStudentByIdAsync(UserId);
            if (student != null)
            {
                StudentProfile = student.Profile;

                // Collect all terms (current and previous)
                AllTerms.Clear();
                AllTerms.Add(student.CurrentTerm);
                foreach (var term in student.PreviousTerms)
                {
                    AllTerms.Add(term);
                }

                // Set default to current term
                SelectedTerm = student.CurrentTerm;
                UpdateDisplayedCourses();
            }
        }

        partial void OnSelectedTermChanged(Term value)
        {
            UpdateDisplayedCourses();
        }

        private async void UpdateDisplayedCourses()
        {
            if (SelectedTerm != null)
            {
                var courses = new ObservableCollection<EnrolledCourse>();
                var courseList = await _courseService.LoadCoursesAsync(); // Get all courses

                foreach (var courseId in SelectedTerm.EnrolledCourses)
                {
                    var course = courseList.FirstOrDefault(c => c.CourseId == courseId);
                    if (course != null)
                    {
                        courses.Add(new EnrolledCourse
                        {
                            CourseId = course.CourseId,
                            CourseName = course.CourseName,
                            Credits = (int)course.Credits
                        });
                    }
                }
                DisplayedCourses = courses;
            }
        }

        [RelayCommand]
        private void SelectTerm(Term term)
        {
            SelectedTerm = term;
        }
    }
    
}
