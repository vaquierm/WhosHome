using System;
using System.Collections.Generic;
using UIKit;

namespace whoshomemobile.iOS
{
    public class PiIDPickerModel : UIPickerViewModel
    {
        public List<AuthorizedPi> AuthorizedPis = new List<AuthorizedPi>();

        private UILabel InformationLabel;

        public AuthorizedPi SelectedPi = null;

        public PiIDPickerModel(UILabel informationLabel)
        {
            this.InformationLabel = informationLabel;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return AuthorizedPis.Count;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return AuthorizedPis[(int) row].PreferedPiNameString;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            SelectedPi = AuthorizedPis[(int)pickerView.SelectedRowInComponent(0)];
            InformationLabel.TextColor = UIColor.DarkGray;
            InformationLabel.Text = $"Scan {AuthorizedPis[(int) pickerView.SelectedRowInComponent(0)].PreferedPiNameString} and find out who is home!";
        }

        public override nfloat GetComponentWidth(UIPickerView picker, nint component)
        {
             return 240f;
        }

        public override nfloat GetRowHeight(UIPickerView picker, nint component)
        {
            return 40f;
        }
    }
}
