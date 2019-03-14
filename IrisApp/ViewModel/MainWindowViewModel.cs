// https://rachel53461.wordpress.com/2011/12/18/navigation-with-mvvm-2/

namespace IrisApp.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using IrisApp.Utils;

    public class MainWindowViewModel : BaseViewModel
    {
        private IPageViewModel currentPageViewModel;
        private WindowState currentWindowState;
        private Visibility closeMenuButtonVisibility = Visibility.Collapsed;
        private bool isMaximized = false;
        private Visibility openMenuButtonVisibility = Visibility.Visible;
        private List<IPageViewModel> pageViewModels;
        private int selectedPageIndex = 0;

        public MainWindowViewModel()
        {
            // Add available pages and set page
            this.PageViewModels.Add(new HomeViewModel());
            this.PageViewModels.Add(new DatabaseViewModel());
            this.PageViewModels.Add(new SettingsViewModel());
            this.PageViewModels.Add(new AboutViewModel());

            this.CurrentPageViewModel = this.PageViewModels[0];
        }

        public Visibility CloseMenuButtonVisibility
        {
            get => this.closeMenuButtonVisibility;

            set
            {
                this.closeMenuButtonVisibility = value;
                this.OnPropertyChanged(nameof(this.CloseMenuButtonVisibility));
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get => this.currentPageViewModel;

            set
            {
                this.currentPageViewModel = value;
                this.OnPropertyChanged(nameof(this.CurrentPageViewModel));
            }
        }

        public WindowState CurrentWindowState
        {
            get => this.currentWindowState;

            set
            {
                this.currentWindowState = value;
                this.OnPropertyChanged(nameof(this.CurrentWindowState));
            }
        }

        public bool IsMaximized
        {
            get => this.isMaximized;

            set
            {
                this.isMaximized = value;
                this.OnPropertyChanged(nameof(this.IsMaximized));
            }
        }

        public Visibility OpenMenuButtonVisibility
        {
            get => this.openMenuButtonVisibility;

            set
            {
                this.openMenuButtonVisibility = value;
                this.OnPropertyChanged(nameof(this.OpenMenuButtonVisibility));
            }
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

        public int SelectedPageIndex
        {
            get => this.selectedPageIndex;

            set
            {
                this.selectedPageIndex = value;
                this.OnGoToPage(null);
                this.OnPropertyChanged(nameof(this.SelectedPageIndex));
            }
        }

        public ICommand CloseMenu => new RelayCommand<Action>(param =>
        {
            this.CloseMenuButtonVisibility = Visibility.Collapsed;
            this.OpenMenuButtonVisibility = Visibility.Visible;
        });

        public ICommand OpenMenu => new RelayCommand<Action>(param =>
        {
            this.CloseMenuButtonVisibility = Visibility.Visible;
            this.OpenMenuButtonVisibility = Visibility.Collapsed;
        });

        public ICommand RestoreApp => new RelayCommand<Action>(param => { this.CurrentWindowState = WindowState.Normal; });

        public ICommand MaximizeOrRestoreApp => new RelayCommand<Action>(param =>
        {
            this.CurrentWindowState = this.IsMaximized ? WindowState.Normal : WindowState.Maximized;
            this.IsMaximized = !this.IsMaximized;
        });

        public ICommand MinimizeApp => new RelayCommand<Action>(param => { this.CurrentWindowState = WindowState.Minimized; });

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
            this.ChangeViewModel(this.PageViewModels[this.SelectedPageIndex]);
        }
    }
}
