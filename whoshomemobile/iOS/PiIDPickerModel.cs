using System;
using System.Collections.Generic;
using UIKit;

namespace whoshomemobile.iOS
{
    public class PiIDPickerModel : UIPickerViewModel
    {
        public List<string> PiIDs = new List<string>();

        private UILabel InformationLabel;

        public string SelectedPiID = null;

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
            return PiIDs.Count;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return PiIDs[(int) row];
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            SelectedPiID = PiIDs[(int)pickerView.SelectedRowInComponent(0)];
            InformationLabel.TextColor = UIColor.DarkGray;
            InformationLabel.Text = $"Scan the Pi '{PiIDs[(int) pickerView.SelectedRowInComponent(0)]}' and find out who is home!";
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
