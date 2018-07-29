using System;
using System.Collections.Generic;
using UIKit;

namespace whoshomemobile.iOS
{
    public partial class ScanRequestsViewController : UIViewController
    {
        private List<ScanRequest> _RequestList;
        private int listIndex = 0;

        private ScanRequest _currentRequest
        {
            get
            {
                if (listIndex < 0 || listIndex >= _RequestList.Count)
                    return null;
                else
                    return _RequestList[listIndex];
            }
        }

        private string _messageText
        {
            get
            {
                return MessageTextView.Text;
            }
            set
            {
                MessageTextView.Text = value;
            }
        }

        private string _informationMessage
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


        public ScanRequestsViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            _RequestList = SignInManager.userPublic.ScanRequestList;

            AcceptButton.TouchUpInside += AcceptButtonClick;
            DenyButton.TouchUpInside += DenyButtonClick;
            AnswerLaterButton.TouchUpInside += LaterButtonClick;
            ContinueButton.TouchUpInside += ContinueButtonClick;

            RefreshView();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void ContinueButtonClick(object sender, EventArgs e) {
            AppDelegate.LoadStoryboard("Main");
        }

        private void AcceptButtonClick(object sender, EventArgs e)
        {
            string message;
            InformationLabel.TextColor = UIColor.DarkGray;
            if (!SignInManager.ProcessPermissionsToRequester(_currentRequest, true, true, out message))
            {
                InformationLabel.TextColor = UIColor.Red;
            }
            listIndex++;
            RefreshView();
        }

        private void DenyButtonClick(object sender, EventArgs e)
        {
            string message;
            InformationLabel.TextColor = UIColor.DarkGray;
            if (!SignInManager.ProcessPermissionsToRequester(_currentRequest, false, true, out message))
            {
                InformationLabel.TextColor = UIColor.Red;
            }
            listIndex++;
            RefreshView();
        }

        private void LaterButtonClick(object sender, EventArgs e)
        {
            listIndex++;
            RefreshView();
        }

        private void RefreshView() {
            if (listIndex < 0 || listIndex >= _RequestList.Count)
            {
                _messageText = string.Empty;
                _informationMessage = StringConstants.ContinueToMainScreenMessage;
                ContinueButton.SetTitle("Continue", UIControlState.Normal);
                AcceptButton.Hidden = true;
                DenyButton.Hidden = true;
                AnswerLaterButton.Hidden = true;
            }
            else
            {
                _messageText = string.Format(StringConstants.WouldLikeToScanYourHomeMessage, _currentRequest?.FullNameRequester, _currentRequest?.UsernameRequester);
            }
        }
    }
}

