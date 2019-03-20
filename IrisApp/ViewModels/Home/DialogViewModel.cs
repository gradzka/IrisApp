namespace IrisApp.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using IrisApp.Utils;
    using IrisApp.ViewModels.Home;

    public class DialogViewModel : BaseViewModel, IPageViewModel
    {
        private bool isDialogOpen;
        private SubjectViewModel selectedSubject;
        private ObservableCollection<SubjectViewModel> subjects;

        public DialogViewModel()
        {
            this.subjects = new ObservableCollection<SubjectViewModel>();
            this.AddSubject(new SubjectViewModel() { SubjectID = 0 });
            this.AddSubject(new SubjectViewModel() { SubjectID = 1 });
            this.AddSubject(new SubjectViewModel() { SubjectID = 2 });
        }

        public bool IsDialogOpen
        {
            get => this.isDialogOpen;

            set
            {
                if (this.isDialogOpen == value)
                {
                    return;
                }

                this.isDialogOpen = value;
                this.OnPropertyChanged(nameof(this.IsDialogOpen));
            }
        }

        public ICommand SaveCommand => new RelayCommand<bool>(subjectsComboboxIsEnabled =>
        {
            if (subjectsComboboxIsEnabled == true)
            {
                SubjectViewModel selectedSubject = this.SelectedSubject;

                // existing subject
            }
            else
            {
                // create new subject
            }

            // TODO
            this.IsDialogOpen = false;
        });

        public SubjectViewModel SelectedSubject
        {
            get => this.selectedSubject;
            set
            {
                if (this.selectedSubject == value)
                {
                    return;
                }

                this.selectedSubject = value;
                this.OnPropertyChanged(nameof(this.SelectedSubject));
            }
        }

        public ObservableCollection<SubjectViewModel> Subjects
        {
            get => this.subjects;
            set
            {
                if (this.subjects == value)
                {
                    return;
                }

                this.subjects = value;
            }
        }

        public ICommand CloseDialogCommand => new RelayCommand<Action>(param =>
        {
            this.IsDialogOpen = false;
        });

        public ICommand ShowDialogCommand => new RelayCommand<Action>(param =>
        {
            this.IsDialogOpen = true;
        });

        private void AddSubject(SubjectViewModel subject)
        {
            try
            {
                this.Subjects.Add(subject);
            }
            catch (Exception)
            {
                // TODO
            }
        }
    }
}
