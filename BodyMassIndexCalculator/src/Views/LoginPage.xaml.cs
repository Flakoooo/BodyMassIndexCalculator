using BodyMassIndexCalculator.src.ViewModels;

namespace BodyMassIndexCalculator.src.Views
{
    public partial class LoginPage : ContentPage
    {
    	public LoginPage(LoginViewModel loginViewModel)
    	{
    		InitializeComponent();
            BindingContext = loginViewModel;
    	}
    }
}