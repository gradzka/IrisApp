namespace IrisApp.ViewModels.Home
{
    public class HomeWithDialogViewModel : BaseViewModel, IPageViewModel
    {
        public HomeWithDialogViewModel()
        {
            this.HomeViewModel = new HomeViewModel();
            this.DialogViewModel = new DialogViewModel();
        }

        public HomeViewModel HomeViewModel { get; set; }

        public DialogViewModel DialogViewModel { get; set; }
    }
}
