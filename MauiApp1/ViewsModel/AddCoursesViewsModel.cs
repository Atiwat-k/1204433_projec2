using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Page;
using MauiApp1.Model;
using MauiApp1.Services;
using MauiApp2.Model;
using System.Linq;

namespace MauiApp1.ViewModel;

public partial class AddCourseViewModel : ObservableObject
{
    private readonly CourseService _courseService;
    
    [ObservableProperty]
    private ObservableCollection<Courses> _courseList = new();
    
    [ObservableProperty]
    private ObservableCollection<Courses> _filteredCourseList = new();
    
    [ObservableProperty]
    private string _searchText;
    
    public ICommand AddCourseCommand { get; private set; }
    public ICommand ShowAllCoursesCommand { get; private set; }
    public int UserId { get; set; }

    public AddCourseViewModel(CourseService courseService)
    {
        _courseService = courseService;
        AddCourseCommand = new Command(async () => await ExecuteAddCourseCommand());
        ShowAllCoursesCommand = new RelayCommand(ExecuteShowAllCoursesCommand);
        
        _ = LoadCoursesAsync();
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterCourses(value);
    }

    private void FilterCourses(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            FilteredCourseList = new ObservableCollection<Courses>(CourseList);
            return;
        }

        var filtered = CourseList.Where(c => 
            c.CourseName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            c.CourseId.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();

        FilteredCourseList = new ObservableCollection<Courses>(filtered);
    }

    private void ExecuteShowAllCoursesCommand()
    {
        FilteredCourseList = new ObservableCollection<Courses>(CourseList);
        SearchText = string.Empty;
    }

    private async Task ExecuteAddCourseCommand()
    {
        if (UserId > 0)
        {
            await Shell.Current.GoToAsync($"{nameof(AddCoursesPage)}?userId={UserId}");
        }
        else
        {
            Console.WriteLine("Invalid UserId");
        }
    }

    private async Task LoadCoursesAsync()
    {
        var courses = await _courseService.LoadCoursesAsync();
        if (courses != null)
        {
            CourseList.Clear();
            foreach (var course in courses)
            {
                CourseList.Add(course);
            }
            
            // Initialize filtered list with all courses
            FilteredCourseList = new ObservableCollection<Courses>(CourseList);
        }
    }
}