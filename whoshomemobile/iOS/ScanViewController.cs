using System;

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
            if (!string.IsNullOrWhiteSpace(_selectedPiID))
            {
                IoTClientManager.ScanMethod(_selectedPiID);
            }

            IoTClientManager.ScanMethod(_selectedPiID);
        }

        public void ClearButtonClick(object sender, EventArgs e)
        {
            
        }

        public void RenameButtonClick(object sender, EventArgs e)
        {
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
