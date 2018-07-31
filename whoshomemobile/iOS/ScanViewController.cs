using System;
using System.Collections.Generic;
using UIKit;

namespace whoshomemobile.iOS
{
    public partial class ScanViewController : UIViewController
    {

        private UserPublic _userPublic
        {
            get
            {
                return SignInManager.userPublic;
            }
        }

        private string _selectedPiID
        {
            get
            {
                return _piIDPickerModel.SelectedPi.PiID;
            }
        }

        private string _selectedPiPerferedName
        {
            get
            {
                return _piIDPickerModel.SelectedPi.PreferedPiNameString;
            }
        }

        private PiIDPickerModel _piIDPickerModel = null;

        private string _message
        {
            get
            {
                return InformationLabel.Text;
            }
            set
            {
                InformationLabel.Text = value;
            }
        }
        
        protected ScanViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            _piIDPickerModel = new PiIDPickerModel(InformationLabel);
            _piIDPickerModel.AuthorizedPis.Add(new AuthorizedPi(_userPublic.Id, _userPublic.FullName, "Home"));
            _piIDPickerModel.AuthorizedPis.AddRange(_userPublic.AuthorizedPiList);
            PiIDDropBox.Model = _piIDPickerModel;

            _message = StringConstants.ScanAndFindOutWhoIsHomeMessage;

            ScanButton.TouchUpInside += ScanButtonClick;
            ClearButton.TouchUpInside += ClearButtonClick;
            RenamePiButton.TouchUpInside += RenameButtonClick;

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public void ScanButtonClick(object sender, EventArgs e)
        {
            List<UserPublic> scannedUsers = null;

            string message;
            if (IoTClientManager.IsDeviceConnected(_selectedPiID, out message))
            {
                scannedUsers = IoTClientManager.ScanMethod(_selectedPiID);
            }
            else
            {
                InformationLabel.TextColor = UIColor.Red;
                _message = message;
                return;
            }

            if (scannedUsers == null)
            {
                InformationLabel.TextColor = UIColor.Red;
                _message = string.Format(StringConstants.SomethingWentWrongWhileScanning, _selectedPiPerferedName);
                return;
            }

            List<string> newScans = new List<string>();

            foreach (UserPublic user in scannedUsers)
            {
                newScans.Add($"{user.FullName} ({user.Id})");
            }

            IsHomeTableView.Source = new IsHomeTableSource(newScans.ToArray());
            IsHomeTableView.ReloadData();

            InformationLabel.TextColor = UIColor.DarkGray;
            _message = string.Format(StringConstants.ScanSuccessfulMessage, _selectedPiPerferedName);
        }

        public void ClearButtonClick(object sender, EventArgs e)
        {
            List<string> empty = new List<string>();

            IsHomeTableView.Source = new IsHomeTableSource(empty.ToArray());
            IsHomeTableView.ReloadData();

            InformationLabel.TextColor = UIColor.DarkGray;
            _message = StringConstants.ScanAndFindOutWhoIsHomeMessage;
        }

        public void RenameButtonClick(object sender, EventArgs e)
        {
            if (_selectedPiID == _userPublic.Id)
            {
                InformationLabel.TextColor = UIColor.Red;
                _message = StringConstants.CannorRenameYourOwnHomeErrorMessage;
                return;
            }
            UIAlertView alert = new UIAlertView();
            alert.Title = "Rename";
            alert.AddButton("OK");
            alert.AddButton("Cancel");
            alert.Message = string.Format(StringConstants.RenamePiMessage, _selectedPiPerferedName);
            alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
            alert.Clicked += (object s, UIButtonEventArgs ev) =>
            {
                if (ev.ButtonIndex == 0)
                {
                    string input = alert.GetTextField(0).Text;

                    InformationLabel.TextColor = UIColor.Red;
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        _message = string.Format(StringConstants.NewPiNameCannotBeEmptyErrorMessage, _selectedPiPerferedName);
                    }
                    else
                    {
                        string message;
                        _userPublic.AuthorizedPiList.Find(ap => ap.PiID == _selectedPiID).PreferedPiNameString = input;

                        if (SignInManager.UpdateUserPublic(InputType.Unspecified, out message))
                        {
                            InformationLabel.TextColor = UIColor.DarkGray;
                            RefreshSpinner();
                        }
                        _message = message;
                    }
                }
            };
            alert.Show();
        }

        public void RefreshSpinner()
        {
            PiIDDropBox.Model = null;
            PiIDDropBox.Model = _piIDPickerModel;
        }
    }
}
