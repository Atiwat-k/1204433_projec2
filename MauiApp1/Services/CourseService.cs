using MauiApp2.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class CourseService
    {
        private readonly string _fileName = "courses.json"; // ชื่อไฟล์ใน Resources/Raw

        public CourseService() { }

        public async Task<List<Courses>> LoadCoursesAsync()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("courses.json");
                using var reader = new StreamReader(stream);
                var jsonData = await reader.ReadToEndAsync();
                
                if (string.IsNullOrEmpty(jsonData))
                {
                    Debug.WriteLine("courses.json is empty");
                    return new List<Courses>();
                }

                var courses = JsonConvert.DeserializeObject<List<Courses>>(jsonData);
                Debug.WriteLine($"Loaded {courses.Count} courses");

                return courses;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading courses.json: {ex.Message}");
                return new List<Courses>();
            }
        }
    }
}
