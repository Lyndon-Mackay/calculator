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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for Interest.xaml
    /// </summary>
    public partial class Interest : Page
    {
        const double SMALL_LOAN_ESTABLIHSMENT_PERCENT = 0.2;
        const double MEDIUM_LOAN_ESTABLISHMENT_FEE = 400;
        const double MINIMUM_LOAN = 300.0;
        const double MEDIUM_LOAN = 2001.0;
        const double MAXIMUM_LOAN = 5000.0;
        const int INSERT_ERROR_INDEX = 2;
        public Interest()
        {
            InitializeComponent();
        }
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string numberString = InputTextBox.Text;
            double loanAmount = double.Parse(numberString);

            if (loanAmount < MINIMUM_LOAN)
            {

                SetErrorLabel(String.Format("The minimum loan amount allowed is ${0}", MINIMUM_LOAN));

            }
            else if (loanAmount < MEDIUM_LOAN)
            {
                EstablishmentTextBox.Text = (SMALL_LOAN_ESTABLIHSMENT_PERCENT * loanAmount).ToString();
            }
            else if (loanAmount <= MAXIMUM_LOAN)
            {
                EstablishmentTextBox.Text = MEDIUM_LOAN_ESTABLISHMENT_FEE.ToString();
            }
            else
            {
                SetErrorLabel(String.Format("The maximum loan amount allowed is ${0}", MAXIMUM_LOAN));
            }
        }
        /// <summary>
        /// Puts an error label into the form
        /// </summary>
        /// <param name="error">The erros message to put</param>
        private void SetErrorLabel(string error)
        {
            Label label = null;
            //NO LINQ  :( !!
            label = TryFindPreviousErrorLabel(label);
            if (label == null)
            {
                label = new Label
                {
                    Name = "ErrorLabel",
                    Content = error
                };
                LoanInput.Children.Insert(INSERT_ERROR_INDEX, label);
            }
            else
            {
                label.Content = error;
            }

        }

        /// <summary>
        /// trys to find a previous error label if it exists
        /// </summary>
        /// <param name="label">a label alue will be overwritten by the new error label if found</param>
        /// <returns>The label if found otherwise null</returns>
        private Label TryFindPreviousErrorLabel(Label label)
        {
            foreach (UIElement child in LoanInput.Children)
            {
                if (child is Label)
                {
                    Label labelChild = child as Label;
                    if (labelChild.Name.Equals("ErrorLabel"))
                    {
                        label = child as Label;
                        break;
                    }
                }
            }

            return label;
        }
        /// <summary>
        /// Prevent non-numbers from being entered
        /// </summary>
        /// <param name="sender">The textbox</param>
        /// <param name="e">unused boilerplate</param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
