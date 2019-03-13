// https://rachel53461.wordpress.com/2011/12/18/navigation-with-mvvm-2/

namespace IrisApp.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    public class MainWindowViewModel : BaseViewModel
    {
        private int selectedItem = 0;
        private IPageViewModel currentPageViewModel;
        private List<IPageViewModel> pageViewModels;

        public MainWindowViewModel()
        {
            // Add available pages and set page
            this.PageViewModels.Add(new HomeViewModel());
            this.PageViewModels.Add(new DatabaseViewModel());
            this.PageViewModels.Add(new SettingsViewModel());
            this.PageViewModels.Add(new AboutViewModel());

            this.CurrentPageViewModel = this.PageViewModels[0];
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (this.pageViewModels == null)
                {
                    this.pageViewModels = new List<IPageViewModel>();
                }

                return this.pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return this.currentPageViewModel;
            }

            set
            {
                this.currentPageViewModel = value;
                this.OnPropertyChanged(nameof(this.CurrentPageViewModel));
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.selectedItem;
            }

            set
            {
                this.selectedItem = value;
                this.OnGoToPage(null);
                this.OnPropertyChanged(nameof(this.SelectedIndex));
            }
        }

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!this.PageViewModels.Contains(viewModel))
            {
                this.PageViewModels.Add(viewModel);
            }

            this.CurrentPageViewModel = this.PageViewModels.FirstOrDefault(vm => vm == viewModel);
        }

        private void OnGoToPage(object obj)
        {
            this.ChangeViewModel(this.PageViewModels[this.SelectedIndex]);
        }
    }
}
