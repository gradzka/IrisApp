﻿namespace IrisApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;
    using IrisApp.Models;
    using IrisApp.Models.Home;
    using IrisApp.Models.IrisProcessor;
    using IrisApp.Utils;

    public class SaveDialogViewModel : BaseViewModel, IPageViewModel
    {
        private bool isDialogOpen;
        private SubjectModel selectedSubject;
        private ObservableCollection<SubjectModel> subjects;
        private bool rBCreateIsChecked;

        public SaveDialogViewModel(IrisProcessorModel processor, ObservableCollection<LogModel> logs)
            : base(processor, logs)
        {
            this.subjects = new ObservableCollection<SubjectModel>();
            this.rBCreateIsChecked = true;
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
                if (value == true)
                {
                    SubjectModel selected = this.SelectedSubject;
                    this.Subjects.Clear();
                    List<int> subjectIDs = this.Processor.GetAllSubjectIDs();
                    if (subjectIDs != null)
                    {
                        if (!subjectIDs.Contains(0))
                        {
                            this.Subjects.Add(new SubjectModel() { SubjectID = 0 });
                        }

                        foreach (int subjectID in subjectIDs)
                        {
                            this.Subjects.Add(new SubjectModel() { SubjectID = subjectID });
                        }

                        if (selected != null)
                        {
                            this.SelectedSubject = this.Subjects.Where(x => x.SubjectID == selected.SubjectID).FirstOrDefault();
                        }
                    }
                    else
                    {
                        this.isDialogOpen = false;
                    }
                }

                this.GetLogsFromProcessor();
                this.OnPropertyChanged(nameof(this.IsDialogOpen));
            }
        }

        public bool RBCreateIsChecked
        {
            get => this.rBCreateIsChecked;

            set
            {
                if (this.rBCreateIsChecked == value)
                {
                    return;
                }

                this.rBCreateIsChecked = value;
                this.OnPropertyChanged(nameof(this.RBCreateIsChecked));
            }
        }

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

        public ICommand SaveCommand => new RelayCommand<bool>(async subjectsComboboxIsEnabled =>
        {
            this.IsDialogOpen = false;

            SampleModel sample = await this.Processor.SaveToDBAsync((subjectsComboboxIsEnabled == true && this.SelectedSubject != null) == true ? this.SelectedSubject.SubjectID : -1);
            if (sample != null)
            {
                if (!Directory.Exists(this.Processor.PathToImages))
                {
                    Directory.CreateDirectory(this.Processor.PathToImages);
                }
                if (!Directory.Exists(sample.Path))
                {
                    Directory.CreateDirectory(sample.Path);
                }
                this.Processor.SaveImage(Path.Combine(sample.Path, $"{sample.SubjectID.ToString()}_{sample.TemplateID.ToString()}_{sample.ChosenEye.ToString()}.png"));
            }

            this.GetLogsFromProcessor();
        });

        public ICommand ShowDialogCommand => new RelayCommand<Action>(param =>
        {
            this.IsDialogOpen = true;
        });
    }
}
