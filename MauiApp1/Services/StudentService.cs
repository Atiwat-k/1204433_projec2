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
        private readonly string _studentsFileName = "students.json";
        private List<Student> _students;

        public async Task<List<Student>> LoadStudentsAsync()
        {
            if (_students != null)
            {
                return _students;
            }

            try
            {
                // ตรวจสอบว่ามีไฟล์ใน AppData หรือไม่
                var appDataPath = Path.Combine(FileSystem.AppDataDirectory, _studentsFileName);
                
                if (!File.Exists(appDataPath))
                {
                    // ถ้าไม่มี ให้คัดลอกจากไฟล์ใน Resources
                    using var stream = await FileSystem.OpenAppPackageFileAsync(_studentsFileName);
                    using var reader = new StreamReader(stream);
                    var jsonData = await reader.ReadToEndAsync();
                    
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        await File.WriteAllTextAsync(appDataPath, jsonData);
                    }
                }

                // อ่านข้อมูลจาก AppData
                var json = await File.ReadAllTextAsync(appDataPath);
                _students = JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
                return _students;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading students: {ex.Message}");
                return new List<Student>();
            }
        }

        public async Task<Student> GetStudentByIdAsync(string id)
        {
            var students = await LoadStudentsAsync();
            return students.FirstOrDefault(s => s.Id == id);
        }

        public async Task<bool> AddStudentAsync(Student newStudent)
        {
            var students = await LoadStudentsAsync();

            if (students.Any(s => s.Email == newStudent.Email))
            {
                return false;
            }

            students.Add(newStudent);
            return await SaveStudentsAsync(students);
        }

        public async Task<bool> UpdateStudentAsync(Student updatedStudent)
        {
            var students = await LoadStudentsAsync();
            var existingStudent = students.FirstOrDefault(s => s.Id == updatedStudent.Id);
            
            if (existingStudent == null)
            {
                return false;
            }

            // อัปเดตข้อมูลนักเรียน
            existingStudent.Email = updatedStudent.Email;
            existingStudent.Password = updatedStudent.Password;
            existingStudent.Profile = updatedStudent.Profile;
            existingStudent.CurrentTerm = updatedStudent.CurrentTerm;
            existingStudent.PreviousTerms = updatedStudent.PreviousTerms;

            return await SaveStudentsAsync(students);
        }

        public async Task<bool> EnrollCourseAsync(string studentId, string courseId)
        {
            var students = await LoadStudentsAsync();
            var student = students.FirstOrDefault(s => s.Id == studentId);
            
            if (student == null)
            {
                return false;
            }

            // ตรวจสอบว่าวิชานี้เคยลงทะเบียนแล้วหรือไม่
            if (student.CurrentTerm.EnrolledCourses.Contains(courseId))
            {
                return false;
            }

            // เพิ่มวิชาลงใน enrolled_courses
            student.CurrentTerm.EnrolledCourses.Add(courseId);
            return await SaveStudentsAsync(students);
        }

        public async Task<bool> DropCourseAsync(string studentId, string courseId)
        {
            var students = await LoadStudentsAsync();
            var student = students.FirstOrDefault(s => s.Id == studentId);
            
            if (student == null)
            {
                return false;
            }

            // ลบวิชาออกจาก enrolled_courses
            return student.CurrentTerm.EnrolledCourses.Remove(courseId) && 
                   await SaveStudentsAsync(students);
        }

        private async Task<bool> SaveStudentsAsync(List<Student> students)
        {
            try
            {
                var json = JsonConvert.SerializeObject(students, Formatting.Indented);
                var filePath = Path.Combine(FileSystem.AppDataDirectory, _studentsFileName);
                await File.WriteAllTextAsync(filePath, json);
                
                // อัปเดต cache
                _students = students;
                
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving students: {ex.Message}");
                return false;
            }
        }
    }
}