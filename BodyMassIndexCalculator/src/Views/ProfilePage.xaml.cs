using BodyMassIndexCalculator.src.ViewModels;

namespace BodyMassIndexCalculator.src.Views
{
    public partial class ProfilePage : ContentPage
    {
        private readonly ProfileViewModel _viewModel;
        public ProfilePage(ProfileViewModel profileViewModel)
    	{
    		InitializeComponent();
    		BindingContext = _viewModel = profileViewModel;
    	}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.RefreshDataCommand.Execute(null);
        }
    }
}