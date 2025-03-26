using System.Diagnostics;
using MauiApp1.ViewModel;

namespace MauiApp1.Page
{
    [QueryProperty(nameof(UserId), "userId")]  // รับ parameter ชื่อ "userId"
    public partial class ShowObjectsPage : ContentPage
    {
        private ShowObjectsViewModel ViewModel;

        public ShowObjectsPage(ShowObjectsViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        // Property สำหรับรับ userId
        private string _userId;
        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnUserIdReceived(value);  // เรียกเมธอดเมื่อรับค่า userId
            }
        }

        // เมธอดที่เรียกเมื่อรับค่า userId
        private async void OnUserIdReceived(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Debug.WriteLine($"Received User ID: {userId}");
                ViewModel.UserId = userId;  // ตั้งค่า UserId ใน ViewModel
                await ViewModel.LoadDataAsync();  // โหลดข้อมูลใหม่
            }
            else
            {
                Debug.WriteLine("UserId is null or empty.");
            }
        }


        private async void OnButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UserId))
            {
                await Shell.Current.GoToAsync($"{nameof(AddCoursesPage)}?userId={UserId}");
            }
            else
            {
                Debug.WriteLine("UserId is null or empty. Cannot navigate.");
            }
        }

        private async void deletecoursesButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UserId.ToString()))
            {
                await Shell.Current.GoToAsync($"{nameof(DeleteCoursesPage)}?userId={UserId}");
            }
            else
            {
                Debug.WriteLine("UserId is null or empty. Cannot navigate.");
            }
        }


    }
}