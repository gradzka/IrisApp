namespace IrisApp.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using IrisApp.Models.Home;
    using IrisApp.Utils;

    public class DialogViewModel : BaseViewModel, IPageViewModel
    {
        private bool isDialogOpen;
        private SubjectModel selectedSubject;
        private ObservableCollection<SubjectModel> subjects;

        public DialogViewModel()
        {
            this.subjects = new ObservableCollection<SubjectModel>();
            this.AddSubject(new SubjectModel() { SubjectID = 0 });
            this.AddSubject(new SubjectModel() { SubjectID = 1 });
            this.AddSubject(new SubjectModel() { SubjectID = 2 });
            this.AddSubject(new SubjectModel() { SubjectID = 3 });
            this.AddSubject(new SubjectModel() { SubjectID = 4 });
            this.AddSubject(new SubjectModel() { SubjectID = 5 });
            this.AddSubject(new SubjectModel() { SubjectID = 6 });
            this.AddSubject(new SubjectModel() { SubjectID = 7 });
            this.AddSubject(new SubjectModel() { SubjectID = 8 });
            this.AddSubject(new SubjectModel() { SubjectID = 9 });
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
                SubjectModel selectedSubject = this.SelectedSubject;

                // existing subject
            }
            else
            {
                // create new subject
            }

            // TODO
            this.IsDialogOpen = false;
        });

        public SubjectModel SelectedSubject
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

        public ObservableCollection<SubjectModel> Subjects
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

        private void AddSubject(SubjectModel subject)
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
