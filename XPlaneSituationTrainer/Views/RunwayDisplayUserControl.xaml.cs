using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XPlaneSituationTrainer.Views {
    /// <summary>
    /// Interaktionslogik für RunwayDisplayUserControl.xaml
    /// </summary>
    public partial class RunwayDisplayUserControl : UserControl {
        private static string RUNWAY_PATTERN = "\b([1-3][0]|[1-2][0-9]|[3][0-7])[LCR]\b";

        public RunwayDisplayUserControl() {
            InitializeComponent();
        }

        public void UpdateRunwayNameVisual(string runwayName) {
            if (IsNameValid(runwayName)) {
                // Check the last character to be an letter.
                var isNumeric = int.TryParse(runwayName.Substring(runwayName.Length - 1, 1), out int n);

                if (runwayName.Length == 1 || (runwayName.Length == 2 && isNumeric)) {
                    // One digit or two digits
                    lblRwyNumber.Text = runwayName;
                    lblRwyOrientation.Text = "";
                } else if (runwayName.Length == 2 && !isNumeric) {
                    // One digit, one letter
                    lblRwyNumber.Text = runwayName.Substring(0,1);
                    lblRwyOrientation.Text = runwayName.Substring(1, 1);
                } else if (runwayName.Length == 3 && !isNumeric) {
                    // Two digits, one letter
                } else {
                    lblRwyNumber.Text = runwayName.Substring(0, 2);
                    lblRwyOrientation.Text = runwayName.Substring(2, 1);
                }
            }
        }

        private bool IsNameValid(string runwayName) {
            return Regex.Matches(runwayName, RUNWAY_PATTERN).Count > 0;
        }
    }
}
