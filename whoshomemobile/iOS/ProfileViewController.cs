using Foundation;
using System;
using UIKit;
using System.Collections.Generic;

namespace whoshomemobile.iOS
{
    public partial class ProfileViewController : UIViewController
    {
        private Dictionary<InputType, UITextField> TextBoxes = null;
        private Dictionary<InputType, UIButton> ActionButtons = null;
        private Dictionary<InputType, UIButton> CancelButtons = null;

        private UserPrivate _userPrivate
        {
            get
            {
                return SignInManager.userPrivate;
            }
        }

        private UserPublic _userPublic
        {
            get
            {
                return SignInManager.userPublic;
            }
        }

        private string _informationLabelMessage
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

        private bool _editingUsername
        {
            get
            {
                return UsernameTextBox.Enabled;
            }
        }

        private bool _editingPassword
        {
            get
            {
                return PasswordTextBox.Enabled;
            }
        }

        private bool _editingFullName
        {
            get
            {
                return FullNameTextBox.Enabled;
            }
        }

        private bool _editingMacAddress
        {
            get
            {
                return MacAddressTextBox.Enabled;
            }
        }

        private bool _editingPiID
        {
            get
            {
                return PiIDTextBox.Enabled;
            }
        }

        public ProfileViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            UsernameActionButton.TouchUpInside += UsernameActionClick;
            UsernameCancelButton.TouchUpInside += UsernameCancelClick;
            PasswordActionButton.TouchUpInside += PasswordActionClick;
            PasswordCancelButton.TouchUpInside += PasswordCancelClick;
            FullNameActionButton.TouchUpInside += FullNameActionClick;
            FullNameCancelButton.TouchUpInside += FullNameCancelClick;
            MacAddressActionButton.TouchUpInside += MacAddressActionClick;
            MacAddressCancelButton.TouchUpInside += MacAddressCancelClick;
            PiIDActionButton.TouchUpInside += PiIDActionClick;
            PiIDCancelButton.TouchUpInside += PiIDCancelClick;

            UsernameTextBox.Text = _userPrivate.Id;
            PasswordTextBox.Text = _userPrivate.Password;
            FullNameTextBox.Text = _userPublic.FullName;
            MacAddressTextBox.Text = _userPublic.MacAddress;
            PiIDTextBox.Text = _userPrivate.PiID;

            if (TextBoxes == null)
            {
                TextBoxes = new Dictionary<InputType, UITextField>();
                TextBoxes[InputType.Username] = UsernameTextBox;
                TextBoxes[InputType.Password] = PasswordTextBox;
                TextBoxes[InputType.FullName] = FullNameTextBox;
                TextBoxes[InputType.MacAddress] = MacAddressTextBox;
                TextBoxes[InputType.PiID] = PiIDTextBox;
            }
            if (ActionButtons == null)
            {
                ActionButtons = new Dictionary<InputType, UIButton>();
                ActionButtons[InputType.Username] = UsernameActionButton;
                ActionButtons[InputType.Password] = PasswordActionButton;
                ActionButtons[InputType.FullName] = FullNameActionButton;
                ActionButtons[InputType.MacAddress] = MacAddressActionButton;
                ActionButtons[InputType.PiID] = PiIDActionButton;
            }
            if (CancelButtons == null)
            {
                CancelButtons = new Dictionary<InputType, UIButton>();
                CancelButtons[InputType.Username] = UsernameCancelButton;
                CancelButtons[InputType.Password] = PasswordCancelButton;
                CancelButtons[InputType.FullName] = FullNameCancelButton;
                CancelButtons[InputType.MacAddress] = MacAddressCancelButton;
                CancelButtons[InputType.PiID] = PiIDCancelButton;
            }
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }


        #region Button Click Handlers

        private void UsernameActionClick(object sender, EventArgs e)
        {
            if (!_editingUsername)
            {
                StartEditing(InputType.Username);
            }
            else
            {
                _userPublic.Id = UsernameTextBox.Text;
                //Push to database

                DoneEditing(InputType.Username);
            }
        }

        private void UsernameCancelClick(object sender, EventArgs e)
        {
            UsernameTextBox.Text = _userPublic.Id;
            DoneEditing(InputType.Username);
        }

        private void PasswordActionClick(object sender, EventArgs e)
        {
            if (!_editingPassword)
            {
                StartEditing(InputType.Password);
            }
            else
            {
                string messgae;
                if (InputValidation.ValidatePassword(PasswordTextBox.Text, out messgae))
                {
                    messgae = StringConstants.PasswordChangedMessage;
                    _userPrivate.Password = UsernameTextBox.Text;
                    //Push to database

                    DoneEditing(InputType.Password);
                }
                _informationLabelMessage = messgae;
            }
        }

        private void PasswordCancelClick(object sender, EventArgs e)
        {
            PasswordTextBox.Text = _userPrivate.Password;
            DoneEditing(InputType.Password);
        }

        private void FullNameActionClick(object sender, EventArgs e)
        {
            if (!_editingFullName)
            {
                StartEditing(InputType.FullName);
            }
            else
            {
                _userPublic.FullName = FullNameTextBox.Text;
                //Push to database

                DoneEditing(InputType.FullName);
            }
        }

        private void FullNameCancelClick(object sender, EventArgs e)
        {
            FullNameTextBox.Text = _userPublic.FullName;
            DoneEditing(InputType.FullName);
        }

        private void MacAddressActionClick(object sender, EventArgs e)
        {
            if (!_editingMacAddress)
            {
                StartEditing(InputType.MacAddress);
            }
            else
            {
                _userPublic.MacAddress = MacAddressTextBox.Text;
                //Push to database

                DoneEditing(InputType.MacAddress);
            }
        }

        private void MacAddressCancelClick(object sender, EventArgs e)
        {
            MacAddressTextBox.Text = _userPublic.MacAddress;
            DoneEditing(InputType.MacAddress);
        }

        private void PiIDActionClick(object sender, EventArgs e)
        {
            if (!_editingPiID)
            {
                StartEditing(InputType.PiID);
            }
            else
            {
                _userPrivate.PiID = PiIDTextBox.Text;
                //Push to database

                DoneEditing(InputType.PiID);
            }
        }

        private void PiIDCancelClick(object sender, EventArgs e)
        {
            PiIDTextBox.Text = _userPrivate.PiID;
            DoneEditing(InputType.PiID);
        }

        #endregion

        #region UI Helper methods

        private void StartEditing(InputType inputType)
        {
            TextBoxes[inputType].Enabled = true;
            CancelButtons[inputType].Hidden = false;
            ActionButtons[inputType].SetTitle(StringConstants.ConfirmString, UIControlState.Normal);
        }

        private void DoneEditing(InputType inputType)
        {
            TextBoxes[inputType].Enabled = false;
            CancelButtons[inputType].Hidden = true;
            ActionButtons[inputType].SetTitle(StringConstants.EditString, UIControlState.Normal);
        }
        #endregion
    }
}