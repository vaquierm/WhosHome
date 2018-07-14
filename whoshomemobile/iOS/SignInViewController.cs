using System;
using System.Text.RegularExpressions;
using UIKit;

namespace whoshomemobile.iOS
{
    public partial class SignInViewController : UIViewController
    {
        
        private bool _isSigningIn
        {
            get
            {
                return ActionControl.SelectedSegment == 0;
            }
        }

        private string _username
        {
            get
            {
                return UsernameTextBox.Text;
            }
        }

        private string _password
        {
            get
            {
                return PasswordTextBox.Text;
            }
        }

        private string _fullName
        {
            get
            {
                return FullNameTextBox.Text;
            }
        }

        private string _macAddress
        {
            get
            {
                return MacAddressTextBox.Text;
            }
        }

        private string _piID
        {
            get
            {
                return PiIDTextBox.Text;
            }
        }

        private string _message
        {
            get
            {
                return InformationLabel.Text;
            }
            set {
                InformationLabel.Text = value;
            }
        }

        public SignInViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            OnActionChanged(null, null);
            ActionControl.ValueChanged += OnActionChanged;
            ReadyButton.TouchUpInside += ReadyButtonClick;

            MacAddressTextBox.Text = SignInManager.GetMacAddress();
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void OnActionChanged(object sender, EventArgs e)
        {
            InformationLabel.TextColor = UIColor.DarkGray;
            if (_isSigningIn)
            {
                InformationLabel.Text = StringConstants.SigningInMessage;
                ReadyButton.SetTitle(StringConstants.SignInString, UIControlState.Normal);
                MacAddressTextBox.Hidden = true;
                FullNameTextBox.Hidden = true;
                PiIDTextBox.Hidden = true;
                MacAddressLabel.Hidden = true;
                FullNameLabel.Hidden = true;
                PiIDLabel.Hidden = true;
            }
            else 
            {
                InformationLabel.Text = StringConstants.RegisterMessage;
                ReadyButton.SetTitle(StringConstants.RegisterString, UIControlState.Normal);
                MacAddressTextBox.Hidden = false;
                FullNameTextBox.Hidden = false;
                PiIDTextBox.Hidden = false;
                MacAddressLabel.Hidden = false;
                FullNameLabel.Hidden = false;
                PiIDLabel.Hidden = false;
            }
        }

        /// <summary>
        /// Readies the button click.
        /// Registers or signs in the user depending on what action is selected.
        /// If it is successfull, the main storyboard is loaded.
        /// </summary>
        private void ReadyButtonClick(object sender, EventArgs e)
        {
            string message;
            if (_isSigningIn)
            {
                if (!SignInManager.LogIn(_username, _password, out message)) {
                    InformationLabel.TextColor = UIColor.Red;
                    _message = message;
                    return;
                }
                _message = message;
            }
            else
            {
                if (!SignInManager.Register(_username, _password, _fullName, _macAddress, out message, _piID))
                {
                    InformationLabel.TextColor = UIColor.Red;
                    _message = message;
                    return;
                }
            }
            AppDelegate.LoadStoryboard("Main");
        }


    }
}

