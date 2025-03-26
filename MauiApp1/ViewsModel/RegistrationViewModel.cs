using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp1.Model;
using MauiApp1.Services;
using System.Threading.Tasks;

namespace MauiApp1.ViewModel
{
    public partial class RegistrationViewModel : ObservableObject
    {
        private readonly StudentService _studentService;

        [ObservableProperty]
        string name = "";

        [ObservableProperty]
        string email = "";

        [ObservableProperty]
        string password = "";

        public RegistrationViewModel(StudentService studentService)
        {
            _studentService = studentService;
        }

        // สร้าง id แบบสุ่ม 5 หลัก
        private string GenerateRandomId()
        {
            var random = new Random();
            return random.Next(10000, 99999).ToString();  // สร้างตัวเลขสุ่มระหว่าง 10000 ถึง 99999
        }

        [RelayCommand]
        async Task Register()
        {
            // ตรวจสอบข้อมูล
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            // สร้างนักเรียนใหม่
            var newStudent = new Student
            {
                Id = GenerateRandomId(),  // สร้าง Id แบบสุ่ม 5 หลัก
                Email = Email,
                Password = Password,
                Profile = new Profile
                {
                    Name = Name,  // ใช้ Name จากฟิลด์กรอกข้อมูล
                    Major = "N/A"  // ตั้งค่าเริ่มต้น
                }
            };

            // บันทึกข้อมูลนักเรียนใหม่
            var result = await _studentService.AddStudentAsync(newStudent);

            if (result)
            {
                await Shell.Current.DisplayAlert("Success", "Registration successful!", "OK");
                await Shell.Current.GoToAsync("..");  // กลับไปหน้าเดิม
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Registration failed. Email may already exist.", "OK");
            }
        }
    }
}