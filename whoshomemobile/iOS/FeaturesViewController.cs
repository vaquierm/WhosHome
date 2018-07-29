using System;

using UIKit;

namespace whoshomemobile.iOS
{
    public partial class FeaturesViewController : UIViewController
    {

        private string _fullName
        {
            get
            {
                return FullNameTextBox.Text;
            }
            set
            {
                FullNameTextBox.Text = value;
            }
        }

        private string _macAddress
        {
            get
            {
                return MacAddressTextBox.Text;
            }
            set
            {
                MacAddressTextBox.Text = value;
            }
        }

        private string _usernameToRequest
        {
            get
            {
                return UsernameTextBox.Text;
            }
            set
            {
                UsernameTextBox.Text = value;
            }
        }

        private string _macAddressMessage
        {
            get
            {
                return MacAddressMessageBox.Text;
            }
            set
            {
                MacAddressMessageBox.Text = value;
            }
        }

        private string _permissionMessage
        {
            get
            {
                return PermissionMessageBox.Text;
            }
            set
            {
                PermissionMessageBox.Text = value;
            }
        }

        public FeaturesViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            AddButton.TouchUpInside += AddButtonClick;
            PermissionButton.TouchUpInside += PermissionButtonClick;

            _macAddressMessage = StringConstants.AddYourFriendsMessage;
            _permissionMessage = StringConstants.PermissionMessage;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void AddButtonClick(object sender, EventArgs e)
        {
            string message;
            MacAddressMessageBox.TextColor = UIColor.DarkGray;
            if (!SignInManager.AddMacAddress(_fullName, _macAddress, out message))
            {
                MacAddressMessageBox.TextColor = UIColor.Red;
            }
            else
            {
                _fullName = string.Empty;
                _macAddress = string.Empty;
            }
            _macAddressMessage = message;
        }

        private void PermissionButtonClick(object sender, EventArgs e)
        {
            string message;
            PermissionMessageBox.TextColor = UIColor.DarkGray;
            if (!SignInManager.RequestPermission(_usernameToRequest, out message))
            {
                PermissionMessageBox.TextColor = UIColor.Red;
            }
            else
            {
                _usernameToRequest = string.Empty;
            }
            _permissionMessage = message;
        }
    }
}

