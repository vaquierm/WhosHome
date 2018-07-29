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
    [Register ("FeaturesViewController")]
    partial class FeaturesViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AddButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField FullNameTextBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView MacAddressMessageBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField MacAddressTextBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton PermissionButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView PermissionMessageBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField UsernameTextBox { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AddButton != null) {
                AddButton.Dispose ();
                AddButton = null;
            }

            if (FullNameTextBox != null) {
                FullNameTextBox.Dispose ();
                FullNameTextBox = null;
            }

            if (MacAddressMessageBox != null) {
                MacAddressMessageBox.Dispose ();
                MacAddressMessageBox = null;
            }

            if (MacAddressTextBox != null) {
                MacAddressTextBox.Dispose ();
                MacAddressTextBox = null;
            }

            if (PermissionButton != null) {
                PermissionButton.Dispose ();
                PermissionButton = null;
            }

            if (PermissionMessageBox != null) {
                PermissionMessageBox.Dispose ();
                PermissionMessageBox = null;
            }

            if (UsernameTextBox != null) {
                UsernameTextBox.Dispose ();
                UsernameTextBox = null;
            }
        }
    }
}