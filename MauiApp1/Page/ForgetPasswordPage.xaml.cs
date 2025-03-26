using MauiApp1.Page;

namespace MauiApp1.Page;

//รับข้อมูลขารับ
[QueryProperty(nameof(Uname), "uname")]
[QueryProperty(nameof(Pwd), "pwd")]


[QueryProperty(nameof(LoginObject), "data")]
public partial class ForgetPasswordPage : ContentPage
{
	// public String uname = "";
	// public String pwd = "";
	
	public String Uname {get; set;}
	public String Pwd {get; set;}
	public  LoginData LoginObject{ get; set; }  = new LoginData();

	public ForgetPasswordPage()
	{
		InitializeComponent();
		
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("..");
    }

    private void Button_Clicked1(object sender, EventArgs e)
    {
		System.Diagnostics.Debug.WriteLine(Uname);
		System.Diagnostics.Debug.WriteLine(LoginObject.Uname);
    }
}