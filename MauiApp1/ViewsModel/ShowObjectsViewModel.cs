using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private readonly CourseService _courseService;

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

        [ObservableProperty]
        private bool isLoading;

        public ShowObjectsViewModel(StudentService studentService, CourseService courseService)
        {
            _studentService = studentService;
            _courseService = courseService;
        }

        [RelayCommand]
        public async Task LoadDataAsync()
        {
            IsLoading = true;
            try
            {
                // Clear existing data
                AllTerms.Clear();
                DisplayedCourses.Clear();
                
                var student = await _studentService.GetStudentByIdAsync(UserId);
                if (student != null)
                {
                    StudentProfile = student.Profile;

                    // Collect all terms (current and previous)
                    AllTerms.Add(student.CurrentTerm);
                    foreach (var term in student.PreviousTerms)
                    {
                        AllTerms.Add(term);
                    }

                    // Set default to current term
                    SelectedTerm = student.CurrentTerm;
                    await UpdateDisplayedCourses();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        partial void OnSelectedTermChanged(Term value)
        {
            _ = UpdateDisplayedCourses();
        }

        private async Task UpdateDisplayedCourses()
        {
            if (SelectedTerm != null)
            {
                try
                {
                    IsLoading = true;
                    var courses = new ObservableCollection<EnrolledCourse>();
                    var courseList = await _courseService.LoadCoursesAsync();

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
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error updating courses: {ex.Message}");
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        [RelayCommand]
        private void SelectTerm(Term term)
        {
            SelectedTerm = term;
        }

        [RelayCommand]
        public async Task RefreshData()
        {
            await LoadDataAsync();
        }
    }
}