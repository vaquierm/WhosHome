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
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public void ScanButtonClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_selectedPiID))
            {
                IoTClientManager.ScanMethod(_selectedPiID);
            }
        }
    }
}
