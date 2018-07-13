using System;

using UIKit;

namespace whoshomemobile.iOS
{
    public partial class ScanViewController : UIViewController
    {

        private UserPrivate _userPrivate
        {
            get
            {
                return SignInManager.userPrivate;
            }
        }

        private string _selectedPiID
        {
            get
            {
                return _piIDPickerModel.SelectedPiID;
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
            _piIDPickerModel.PiIDs.Add(_userPrivate.PiID);
            _piIDPickerModel.PiIDs.AddRange(_userPrivate.AuthorizedPiList);
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
