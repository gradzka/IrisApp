namespace IrisApp.Models.IrisProcessor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms.Integration;
    using IrisApp.Models.Home;
    using IrisApp.Utils;
    using IrisApp.ViewModels.Settings;
    using Neurotec.Biometrics;
    using Neurotec.Biometrics.Client;
    using Neurotec.Biometrics.Gui;
    using Neurotec.Devices;
    using Neurotec.Licensing;

    public class VeriEyeProcessorModel : IrisProcessorModel
    {
        private NBiometricClient biometricClient;
        private NIris iris;
        private NSubject subject;

        public VeriEyeProcessorModel()
        {
            if (this.ObtainLicenses())
            {
                this.biometricClient = new NBiometricClient
                {
                    UseDeviceManager = true,
                    CustomDataSchema = NBiographicDataSchema.Parse("(Path string)")
                };
                this.IsProcessorReady = this.ConnectToDB();
            }
        }

        public override async Task<List<SubjectModel>> GetAllSubjectsAsync()
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.LicensesUnavailable);
                    return null;
                }

                List<SubjectModel> subjects = new List<SubjectModel>();
                foreach (int subjectID in this.GetAllSubjectIDs())
                {
                    NSubject subject = (NSubject)await this.GetSubjectAsync(subjectID);
                    subjects.Add(new SubjectModel()
                    {
                        Path = subject.GetProperty("Path").ToString(),
                        SamplesCount = subject.Irises.Count,
                        SubjectID = subjectID
                    });
                }

                return subjects;
            }
            catch (Exception)
            {
                this.ResultLogs.Add(LogSingleton.Instance.SubjectsUnavailable);
                return null;
            }
        }

        public override List<int> GetAllSubjectIDs()
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.LicensesUnavailable);
                    return null;
                }

                string[] status = this.biometricClient.ListIds();

                return Array.ConvertAll(status, int.Parse).ToList();
            }
            catch (Exception)
            {
                this.ResultLogs.Add(LogSingleton.Instance.SubjectIDsUnavailable);
                return null;
            }
        }

        public override List<SourceModel> GetDevices()
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.LicensesUnavailable);
                    return null;
                }

                this.biometricClient.DeviceManager.DeviceTypes = NDeviceType.IrisScanner;
                this.biometricClient.DeviceManager.Initialize();

                List<SourceModel> devices = new List<SourceModel>();
                foreach (NDevice device in this.biometricClient.DeviceManager.Devices)
                {
                    devices.Add(new SourceModel() { Name = device.DisplayName, Device = device });
                }

                LogModel foundDevices = devices.Count == 1 ? LogSingleton.Instance.FoundDevice : LogSingleton.Instance.FoundDevices;
                foundDevices.Description = $"{devices.Count} {foundDevices.Description}";
                this.ResultLogs.Add(foundDevices);
                return devices;
            }
            catch (Exception)
            {
                this.ResultLogs.Add(LogSingleton.Instance.DevicesUnavailable);
                return null;
            }
        }

        public override object GetPreviewControl(bool beforeCapturingFromDevice = false)
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.LicensesUnavailable);
                    return false;
                }

                if (beforeCapturingFromDevice)
                {
                    this.iris = new NIris();
                }

                return new WindowsFormsHost() { Child = new NIrisView() { Iris = this.iris } };
            }
            catch (Exception)
            {
                this.ResultLogs.Add(LogSingleton.Instance.PreviewUnavailable);
                return null;
            }
        }

        public override Tuple<EnrollmentViewModel, MatchingViewModel> GetSettings()
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.LicensesUnavailable);
                    return null;
                }

                EnrollmentViewModel enrollmentViewModel = new EnrollmentViewModel
                {
                    InnerBoundaryRadiusFrom = Convert.ToByte(this.biometricClient.GetProperty("Irises.InnerBoundaryFrom")),
                    InnerBoundaryRadiusTo = Convert.ToByte(this.biometricClient.GetProperty("Irises.InnerBoundaryTo")),
                    OuterBoundaryRadiusFrom = Convert.ToByte(this.biometricClient.GetProperty("Irises.OuterBoundaryFrom")),
                    OuterBoundaryRadiusTo = Convert.ToByte(this.biometricClient.GetProperty("Irises.OuterBoundaryTo")),
                    QualityThreshold = this.biometricClient.IrisesQualityThreshold
                };

                MatchingViewModel matchingViewModel = new MatchingViewModel();
                matchingViewModel.SelectedFAR = matchingViewModel.FAR.FirstOrDefault(x => x.Key == this.MatchingThresholdToFAR(this.biometricClient.MatchingThreshold) * 100);
                matchingViewModel.MaximalRotation = (int)this.biometricClient.IrisesMaximalRotation;
                matchingViewModel.SelectedMatchingSpeed = matchingViewModel.MatchingSpeed.FirstOrDefault(x => x.Equals(this.biometricClient.IrisesMatchingSpeed.ToString(), StringComparison.Ordinal));
                matchingViewModel.MaximalResultCount = this.biometricClient.MatchingMaximalResultCount;
                matchingViewModel.IsFirstReadOnlyChecked = this.biometricClient.MatchingFirstResultOnly;

                return new Tuple<EnrollmentViewModel, MatchingViewModel>(enrollmentViewModel, matchingViewModel);
            }
            catch (Exception)
            {
                this.ResultLogs.Add(LogSingleton.Instance.SettingsRestored);
                return null;
            }
        }

        public override async Task IdentifyAsync()
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.LicensesUnavailable);
                    return;
                }

                if (this.subject == null)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.UseSelectedSource);
                    return;
                }

                NBiometricTask task = this.biometricClient.CreateTask(NBiometricOperations.Identify, this.subject);
                NBiometricTask performedTask = await this.biometricClient.PerformTaskAsync(task);
                if (performedTask.Status != NBiometricStatus.Ok)
                {
                    LogModel identificationFailed = LogSingleton.Instance.IdentificationFailed;
                    identificationFailed.Description = $"{identificationFailed.Description} (status = {performedTask.Status})";
                    this.ResultLogs.Add(identificationFailed);
                    return;
                }

                LogModel identificationResult = LogSingleton.Instance.IdentificationResult;
                identificationResult.Description = $"{identificationResult.Description}\n{this.subject.MatchingResults[0].Id} {this.subject.MatchingResults[0].Score.ToString()}";
                this.ResultLogs.Add(identificationResult);
                return;
            }
            catch (Exception)
            {
                this.ResultLogs.Add(LogSingleton.Instance.IdentificationFailed);
                return;
            }
        }

        public override async Task<bool> LoadFromImageAsync(string pathToImageFile, char eye)
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.LicensesUnavailable);
                    return false;
                }

                if (!File.Exists("Irises.ndf"))
                {
                    this.ResultLogs.Add(LogSingleton.Instance.IrisesNDF);
                    return false;
                }

                this.subject = new NSubject();

                this.iris = new NIris()
                {
                    FileName = pathToImageFile
                };

                if (eye == 'L')
                {
                    this.iris.Position = NEPosition.Left;
                }
                else if (eye == 'R')
                {
                    this.iris.Position = NEPosition.Right;
                }
                else if (eye == 'U')
                {
                    this.iris.Position = NEPosition.Unknown;
                }
                else
                {
                    return false;
                }

                this.subject.Irises.Add(this.iris);
                this.biometricClient.IrisesTemplateSize = NTemplateSize.Large;

                NBiometricTask task = this.biometricClient.CreateTask(NBiometricOperations.CreateTemplate, this.subject);
                NBiometricTask performedTask = await this.biometricClient.PerformTaskAsync(task);
                if (performedTask.Status != NBiometricStatus.Ok)
                {
                    LogModel extractionFailed = LogSingleton.Instance.ExtractionFailed;
                    extractionFailed.Description = $"{extractionFailed.Description} (status = {performedTask.Status})";
                    this.ResultLogs.Add(extractionFailed);
                    return false;
                }

                this.ResultLogs.Add(LogSingleton.Instance.ExtractionResult);
                var a = this.subject.GetTemplate();
                return true;
            }
            catch (Exception)
            {
                this.ResultLogs.Add(LogSingleton.Instance.ExtractionFailed);
                return false;
            }
        }

        public override async Task<bool> LoadFromScannerAsync(object device, char eye)
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.LicensesUnavailable);
                    return false;
                }

                if (!File.Exists("Irises.ndf"))
                {
                    this.ResultLogs.Add(LogSingleton.Instance.IrisesNDF);
                    return false;
                }

                this.subject = new NSubject();
                if (this.iris == null)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.PreviewUnavailable);
                    return false;
                }

                this.biometricClient.IrisScanner = (NIrisScanner)device;

                if (eye == 'L')
                {
                    this.iris.Position = NEPosition.Left;
                }
                else if (eye == 'R')
                {
                    this.iris.Position = NEPosition.Right;
                }
                else if (eye == 'U')
                {
                    this.iris.Position = NEPosition.Unknown;
                }
                else
                {
                    return false;
                }

                this.subject.Irises.Add(this.iris);

                this.biometricClient.IrisesTemplateSize = NTemplateSize.Large;
                NBiometricTask task = this.biometricClient.CreateTask(NBiometricOperations.Capture | NBiometricOperations.CreateTemplate, this.subject);
                NBiometricTask performedTask = await this.biometricClient.PerformTaskAsync(task);
                if (performedTask.Status != NBiometricStatus.Ok)
                {
                    LogModel captureExtractionFailed = LogSingleton.Instance.CaptureExtractionFailed;
                    captureExtractionFailed.Description = $"{captureExtractionFailed.Description} (status = {performedTask.Status})";
                    this.ResultLogs.Add(captureExtractionFailed);
                    return false;
                }

                this.ResultLogs.Add(LogSingleton.Instance.CaptureExtractionResult);
                return true;
            }
            catch (Exception)
            {
                this.ResultLogs.Add(LogSingleton.Instance.CaptureExtractionFailed);
                return false;
            }
        }

        public override bool RemoveSubject(int subjectID)
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.LicensesUnavailable);
                    return false;
                }

                var status = this.biometricClient.Delete(subjectID.ToString());
                if (status != NBiometricStatus.Ok)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.DeleteSubjectError);
                    return false;
                }

                this.ResultLogs.Add(LogSingleton.Instance.DeleteSubjectDone);
                return true;
            }
            catch (Exception)
            {
                this.ResultLogs.Add(LogSingleton.Instance.DeleteSubjectError);
                return false;
            }
        }

        public override void SaveImage(string pathToImageFile)
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.LicensesUnavailable);
                    return;
                }

                if (this.subject == null || this.subject.Irises.Count == 0 || (this.subject.Irises.Count > 0 && this.subject.Irises[this.subject.Irises.Count - 1].Image == null))
                {
                    this.ResultLogs.Add(LogSingleton.Instance.SaveImageError);
                    return;
                }

                this.subject.Irises[this.subject.Irises.Count - 1].Image.Save(pathToImageFile);
                this.ResultLogs.Add(LogSingleton.Instance.SaveImageDone);
            }
            catch (Exception)
            {
                this.ResultLogs.Add(LogSingleton.Instance.SaveImageError);
                return;
            }
        }

        public override async Task<SampleModel> SaveToDBAsync(int subjectID)
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.LicensesUnavailable);
                    return null;
                }

                // new subject
                if (subjectID == 0)
                {
                    subjectID = this.CreateNextSubjectID();
                    if (subjectID == 0)
                    {
                        LogModel saveTemplateError = LogSingleton.Instance.SaveTemplateError;
                        saveTemplateError.Description = $"{saveTemplateError.Description} (cannot create subjectID)";
                        this.ResultLogs.Add(saveTemplateError);
                        return null;
                    }

                    this.subject.Id = subjectID.ToString();
                    this.subject.SetProperty("Path", "IrisDB\\" + subjectID.ToString());
                    NBiometricTask task = this.biometricClient.CreateTask(NBiometricOperations.Enroll, this.subject);
                    NBiometricTask performedTask = await this.biometricClient.PerformTaskAsync(task);
                    if (performedTask.Status != NBiometricStatus.Ok)
                    {
                        LogModel saveTemplateErrorTask = LogSingleton.Instance.SaveTemplateError;
                        saveTemplateErrorTask.Description = $"{saveTemplateErrorTask.Description} (status = {performedTask.Status})";
                        this.ResultLogs.Add(saveTemplateErrorTask);
                        return null;
                    }


                    LogModel saveTemplateDone = LogSingleton.Instance.SaveTemplateDone;
                    saveTemplateDone.Description = $"{saveTemplateDone.Description} {subjectID.ToString()}";
                    this.ResultLogs.Add(saveTemplateDone);
                    return new SampleModel() { SubjectID = subjectID, TemplateID = 1, Path = Path.Combine("IrisDB", subjectID.ToString()), ChosenEye = this.subject.Irises.Last().Position.ToString()[0] };
                }

                // existing
                else
                {
                    NSubject subj = (NSubject)await this.GetSubjectAsync(subjectID);

                    NIris newIris = this.subject.Irises[0];
                    this.subject.Irises.Remove(newIris);
                    subj.Irises.Add(newIris);
                    this.subject = subj;

                    NBiometricTask task = this.biometricClient.CreateTask(NBiometricOperations.Update, subj);
                    NBiometricTask performedTask = await this.biometricClient.PerformTaskAsync(task);
                    if (performedTask.Status != NBiometricStatus.Ok)
                    {
                        LogModel saveTemplateErrorTask = LogSingleton.Instance.SaveTemplateError;
                        saveTemplateErrorTask.Description = $"{saveTemplateErrorTask.Description} (status = {performedTask.Status})";
                        this.ResultLogs.Add(saveTemplateErrorTask);
                        return null;
                    }

                    LogModel saveTemplateDone = LogSingleton.Instance.SaveTemplateDone;
                    saveTemplateDone.Description = $"{saveTemplateDone.Description} {subjectID.ToString()}";
                    this.ResultLogs.Add(saveTemplateDone);
                    return new SampleModel() { SubjectID = subjectID, TemplateID = subj.Irises.Count, Path = Path.Combine("IrisDB", subjectID.ToString()), ChosenEye = subj.Irises.Last().Position.ToString()[0] };
                }
            }
            catch (Exception)
            {
                this.ResultLogs.Add(LogSingleton.Instance.SaveTemplateError);
                return null;
            }
        }

        public override void SetSettings(Tuple<EnrollmentViewModel, MatchingViewModel> settings)
        {
            try
            {
                if (!this.IsProcessorReady)
                {
                    this.ResultLogs.Add(LogSingleton.Instance.LicensesUnavailable);
                    return;
                }

                this.biometricClient.SetProperty("Irises.InnerBoundaryFrom", settings.Item1.InnerBoundaryRadiusFrom);
                this.biometricClient.SetProperty("Irises.InnerBoundaryTo", settings.Item1.InnerBoundaryRadiusTo);
                this.biometricClient.SetProperty("Irises.OuterBoundaryFrom", settings.Item1.OuterBoundaryRadiusFrom);
                this.biometricClient.SetProperty("Irises.OuterBoundaryTo", settings.Item1.OuterBoundaryRadiusTo);
                this.biometricClient.IrisesQualityThreshold = settings.Item1.QualityThreshold;
                this.biometricClient.MatchingThreshold = this.FARToMatchingThreshold(settings.Item2.SelectedFAR.Key / 100);
                this.biometricClient.IrisesMaximalRotation = (float)settings.Item2.MaximalRotation;
                Enum.TryParse(settings.Item2.SelectedMatchingSpeed, out NMatchingSpeed myStatus);
                this.biometricClient.IrisesMatchingSpeed = myStatus;
                this.biometricClient.MatchingMaximalResultCount = settings.Item2.MaximalResultCount;
                this.biometricClient.MatchingFirstResultOnly = settings.Item2.IsFirstReadOnlyChecked;

                this.ResultLogs.Add(LogSingleton.Instance.SettingsSaveDone);
            }
            catch (Exception)
            {
                this.ResultLogs.Add(LogSingleton.Instance.SettingsSaveError);
                return;
            }
        }

        protected override bool ConnectToDB()
        {
            try
            {
                this.biometricClient.DatabaseConnection = null;
                this.biometricClient.RemoteConnections.Clear();
                this.biometricClient.SetDatabaseConnectionToSQLite("./IrisDB.db");

                this.ResultLogs.Add(LogSingleton.Instance.DatabaseConnectionDone);
                return true;
            }
            catch (Exception)
            {
                this.ResultLogs.Add(LogSingleton.Instance.DatabaseConnectionError);
                return false;
            }
        }

        protected override async Task<object> GetSubjectAsync(int subjectID)
        {
            try
            {
                NSubject subj = new NSubject { Id = subjectID.ToString() };
                NBiometricTask task = this.biometricClient.CreateTask(NBiometricOperations.Get, subj);
                NBiometricTask performedTask = await this.biometricClient.PerformTaskAsync(task);
                if (performedTask.Status != NBiometricStatus.Ok)
                {
                    return null;
                }

                return performedTask.Subjects.Count > 0 ? performedTask.Subjects[0] : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private int CreateNextSubjectID()
        {
            try
            {
                return this.GetAllSubjectIDs().LastOrDefault() + 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int FARToMatchingThreshold(double far)
        {
            if (far > 1)
            {
                far = 1;
            }
            else if (far <= 0.0)
            {
                far = 1E-100;
            }

            return (int)Math.Round(-Math.Log10(far) * 12.0);
        }

        private double MatchingThresholdToFAR(int threshold)
        {
            if (threshold < 0)
            {
                threshold = 0;
            }

            return Math.Pow(10.0, -threshold / 12.0);
        }

        private bool ObtainLicenses()
        {
            string[] components = new string[] { "Biometrics.IrisExtraction", "Biometrics.IrisMatching", "Devices.IrisScanners" };

            try
            {
                foreach (string component in components)
                {
                    if (!NLicense.ObtainComponents("/local", 5000, component))
                    {
                        LogModel licensesUnavailable = LogSingleton.Instance.SaveTemplateDone;
                        licensesUnavailable.Description = $"{licensesUnavailable.Description} (\" {component}\"";
                        this.ResultLogs.Add(licensesUnavailable);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                this.ResultLogs.Add(LogSingleton.Instance.LicensesUnavailable);
                return false;
            }
        }
    }
}
