using Foundation;
using System;
using UIKit;

namespace whoshomemobile.iOS
{
    public partial class ProfileViewController : UIViewController
    {
        private const string _confirmString = "Confirm";
        private const string _editString = "Edit";
        
        private bool _editingUsername
        {
            get
            {
                return UsernameTextBox.Enabled;
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
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void UsernameActionClick(object sender, EventArgs e)
        {
            if (!_editingUsername)
            {
                UsernameTextBox.Enabled = true;
                UsernameCancelButton.Hidden = false;
                UsernameActionButton.SetTitle(_confirmString, UIControlState.Normal);
            }
            else
            {
                UsernameTextBox.Enabled = false;
                UsernameCancelButton.Hidden = true;
                UsernameActionButton.SetTitle(_editString, UIControlState.Normal);
            }
        }

        private void UsernameCancelClick(object sender, EventArgs e)
        {
            UsernameTextBox.Enabled = false;
            UsernameCancelButton.Hidden = true;
            UsernameActionButton.SetTitle(_editString, UIControlState.Normal);
        }
    }
}