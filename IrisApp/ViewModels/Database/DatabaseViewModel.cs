namespace IrisApp.ViewModels.Database
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;
    using IrisApp.Models.Home;
    using IrisApp.Models.IrisProcessor;
    using IrisApp.Utils;

    public class DatabaseViewModel : BaseViewModel, IPageViewModel
    {
        private ObservableCollection<SubjectModel> subjects;

        public DatabaseViewModel(IrisProcessorModel processor, ObservableCollection<LogModel> logs)
            : base(processor, logs)
        {
            this.Subjects = new ObservableCollection<SubjectModel>();
            if (processor.IsProcessorReady)
            {
                this.GetSubjectsFromDatabaseAsync();
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

        public ICommand DeleteAllCommand => new RelayCommand<bool>(deleteSelectedIsChecked =>
        {
            try
            {
                foreach (int subjectID in this.Processor.GetAllSubjectIDs())
                {
                    if (this.Processor.RemoveSubject(subjectID))
                    {
                        SubjectModel subject = this.Subjects.FirstOrDefault(x => x.SubjectID == subjectID);
                        if ((deleteSelectedIsChecked && subject.IsSelected) || !deleteSelectedIsChecked)
                        {
                            this.Subjects.Remove(subject);
                            Directory.Delete(subject.Path, true);
                        }
                    }
                }
            }
            catch (Exception)
            {
                this.GetSubjectsFromDatabaseAsync();
            }
            finally
            {
                this.GetLogsFromProcessor();
            }
        });

        public ICommand DeleteTupleCommand => new RelayCommand<SubjectModel>(param =>
        {
            try
            {
                if (this.Processor.RemoveSubject(param.SubjectID))
                {
                    this.Subjects.Remove(param);
                    Directory.Delete(param.Path, true);
                }
            }
            catch (Exception)
            {
               // TODO
            }
            finally
            {
                this.GetLogsFromProcessor();
            }
        });

        public ICommand GoToFolderCommand => new RelayCommand<string>(param =>
        {
            if (Directory.Exists(param))
            {
                Process.Start(param);
            }
            else
            {
                this.Logs.Insert(0, LogSingleton.Instance.DirectoryNotFound);
            }
        });

        public ICommand RefreshCommand => new RelayCommand<Action>(param =>
        {
            this.GetSubjectsFromDatabaseAsync();
        });

        public async void GetSubjectsFromDatabaseAsync()
        {
            try
            {
                this.Subjects.Clear();
                foreach (SubjectModel subject in await this.Processor.GetAllSubjectsAsync())
                {
                    this.Subjects.Add(subject);
                }
            }
            catch (Exception)
            {
                this.Subjects.Clear();
            }
            finally
            {
                this.GetLogsFromProcessor();
            }
        }
    }
}
