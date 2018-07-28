// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace whoshomemobile.iOS
{
    [Register ("SignInViewController")]
    partial class SignInViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl ActionControl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel FullNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField FullNameTextBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel InformationLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MacAddressLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField MacAddressTextBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PasswordTextBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ReadyButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView SignInView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField UsernameTextBox { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ActionControl != null) {
                ActionControl.Dispose ();
                ActionControl = null;
            }

            if (FullNameLabel != null) {
                FullNameLabel.Dispose ();
                FullNameLabel = null;
            }

            if (FullNameTextBox != null) {
                FullNameTextBox.Dispose ();
                FullNameTextBox = null;
            }

            if (InformationLabel != null) {
                InformationLabel.Dispose ();
                InformationLabel = null;
            }

            if (MacAddressLabel != null) {
                MacAddressLabel.Dispose ();
                MacAddressLabel = null;
            }

            if (MacAddressTextBox != null) {
                MacAddressTextBox.Dispose ();
                MacAddressTextBox = null;
            }

            if (PasswordTextBox != null) {
                PasswordTextBox.Dispose ();
                PasswordTextBox = null;
            }

            if (ReadyButton != null) {
                ReadyButton.Dispose ();
                ReadyButton = null;
            }

            if (SignInView != null) {
                SignInView.Dispose ();
                SignInView = null;
            }

            if (UsernameTextBox != null) {
                UsernameTextBox.Dispose ();
                UsernameTextBox = null;
            }
        }
    }
}