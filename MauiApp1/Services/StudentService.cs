using MauiApp1.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    public class StudentService
    {
        private List<Student> _students;

        // โหลดข้อมูลนักเรียนจากไฟล์ JSON
        public async Task<List<Student>> LoadStudentsAsync()
        {
            if (_students != null)
            {
                return _students;  // ถ้ามีข้อมูลอยู่แล้ว ไม่ต้องโหลดซ้ำ
            }

            try
            {
                // อ่านไฟล์ students.json จากโฟลเดอร์ Raw
                using var stream = await FileSystem.OpenAppPackageFileAsync("students.json");
                using var reader = new StreamReader(stream);
                var json = await reader.ReadToEndAsync();
                _students = JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
                return _students;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading JSON: {ex.Message}");
                return new List<Student>();
            }
        }

        // ค้นหานักเรียนด้วย Id
        public async Task<Student> GetStudentByIdAsync(string id)
        {
            var students = await LoadStudentsAsync();
            return students.FirstOrDefault(s => s.Id == id);
        }

        // เพิ่มนักเรียนใหม่
        public async Task<bool> AddStudentAsync(Student newStudent)
        {
            var students = await LoadStudentsAsync();

            // ตรวจสอบว่าอีเมลซ้ำหรือไม่
            if (students.Any(s => s.Email == newStudent.Email))
            {
                return false;  // อีเมลซ้ำ
            }

            // เพิ่มนักเรียนใหม่
            students.Add(newStudent);

            // บันทึกข้อมูลกลับลงไฟล์ JSON
            try
            {
                var json = JsonConvert.SerializeObject(students, Formatting.Indented);
                var filePath = Path.Combine(FileSystem.AppDataDirectory, "students.json");
                await File.WriteAllTextAsync(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving JSON: {ex.Message}");
                return false;
            }
        }
    }
}