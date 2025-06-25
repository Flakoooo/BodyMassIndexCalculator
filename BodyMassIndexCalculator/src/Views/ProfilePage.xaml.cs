using BodyMassIndexCalculator.src.ViewModels;

namespace BodyMassIndexCalculator.src.Views
{
    public partial class ProfilePage : ContentPage
    {
    	public ProfilePage(ProfileViewModel profileViewModel)
    	{
    		InitializeComponent();
    		BindingContext = profileViewModel;
    	}
    }
}