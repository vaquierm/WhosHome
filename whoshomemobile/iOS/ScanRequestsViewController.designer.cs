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
    [Register ("ScanRequestsViewController")]
    partial class ScanRequestsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AcceptButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AnswerLaterButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ContinueButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DenyButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView InformationLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView MessageTextView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AcceptButton != null) {
                AcceptButton.Dispose ();
                AcceptButton = null;
            }

            if (AnswerLaterButton != null) {
                AnswerLaterButton.Dispose ();
                AnswerLaterButton = null;
            }

            if (ContinueButton != null) {
                ContinueButton.Dispose ();
                ContinueButton = null;
            }

            if (DenyButton != null) {
                DenyButton.Dispose ();
                DenyButton = null;
            }

            if (InformationLabel != null) {
                InformationLabel.Dispose ();
                InformationLabel = null;
            }

            if (MessageTextView != null) {
                MessageTextView.Dispose ();
                MessageTextView = null;
            }
        }
    }
}