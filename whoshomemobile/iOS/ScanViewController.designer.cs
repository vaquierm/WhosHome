// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace whoshomemobile.iOS
{
    [Register ("ScanViewController")]
    partial class ScanViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ClearButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel InformationLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView IsHomeStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView PiIDDropBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RenamePiButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ScanButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ClearButton != null) {
                ClearButton.Dispose ();
                ClearButton = null;
            }

            if (InformationLabel != null) {
                InformationLabel.Dispose ();
                InformationLabel = null;
            }

            if (IsHomeStackView != null) {
                IsHomeStackView.Dispose ();
                IsHomeStackView = null;
            }

            if (PiIDDropBox != null) {
                PiIDDropBox.Dispose ();
                PiIDDropBox = null;
            }

            if (RenamePiButton != null) {
                RenamePiButton.Dispose ();
                RenamePiButton = null;
            }

            if (ScanButton != null) {
                ScanButton.Dispose ();
                ScanButton = null;
            }
        }
    }
}