using System;
using System.Text.RegularExpressions;
using UIKit;

namespace whoshomemobile.iOS
{
    public partial class SignInViewController : UIViewController
    {
        private const string _signingInMessage = "Sign in to Who's Home";
        private const string _registerMessage = "Register to Who's Home";

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
                InformationLabel.Text = _signingInMessage;
                ReadyButton.SetTitle("Sign In", UIControlState.Normal);
                MacAddressTextBox.Hidden = true;
                FullNameTextBox.Hidden = true;
                PiIDTextBox.Hidden = true;
                MacAddressLabel.Hidden = true;
                FullNameLabel.Hidden = true;
                PiIDLabel.Hidden = true;
            }
            else 
            {
                InformationLabel.Text = _registerMessage;
                ReadyButton.SetTitle("Register", UIControlState.Normal);
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
                if (!ValidatePassword(_password, out message))
                {
                    InformationLabel.TextColor = UIColor.Red;
                    _message = message;
                    return;
                }
                if (!SignInManager.Register(_username, _password, _fullName, _macAddress, out message, _piID))
                {
                    InformationLabel.TextColor = UIColor.Red;
                    _message = message;
                    return;
                }
            }
            AppDelegate.LoadStoryboard("Main");
        }

        /// <summary>
        /// Validates the password.
        /// </summary>
        /// <returns><c>true</c>, if password was validated, <c>false</c> otherwise.</returns>
        /// <param name="password">Password.</param>
        /// <param name="ErrorMessage">Error message.</param>
        private bool ValidatePassword(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                ErrorMessage = "Password should not be empty";
                return false;
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one lower case letter";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one upper case letter";
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "Password should not be less than or greater than 12 characters";
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one numeric value";
                return false;
            }

            else if (!hasSymbols.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one special case characters";
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

