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
        /***
         * Constanst from nimble website
         */ 
        const decimal SMALL_LOAN_ESTABLIHSMENT_PERCENT = 0.2m;
        const decimal MEDIUM_LOAN_ESTABLISHMENT_FEE = 400m;
        const decimal SMALL_LOAN_MONTHLY_FEE = .04m;
        const decimal MINIMUM_LOAN = 300.0m;
        const decimal MEDIUM_LOAN = 2001.0m;
        const decimal MEDIUM_ANNUAL_INTEREST_RATE = .476158m;
        const decimal MAXIMUM_LOAN = 5000.0m;
        const int INSERT_ERROR_INDEX = 2;
        const string ERROR_LABEL_NAME = "ErrorLabel";
        const string REPAYMENT_LABEL_NAME = "RepaymentLabel";
        public Interest()
        {
            InitializeComponent();
        }
        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string numberString = InputTextBox.Text;
            decimal loanAmount = decimal.Parse(numberString);

            if (loanAmount < MINIMUM_LOAN)
            {
                string errorString = String.Format("The minimum loan amount allowed is ${0}", MINIMUM_LOAN);

                SetDynamicLabel(ERROR_LABEL_NAME,errorString);

            }
            else if (loanAmount < MEDIUM_LOAN)
            {
                EstablishmentTextBox.Text = (SMALL_LOAN_ESTABLIHSMENT_PERCENT * loanAmount).ToString();

                decimal monthlyLoanAmount = loanAmount * SMALL_LOAN_MONTHLY_FEE;

                string repaymentString = String.Format("Your monthly loan fee is ${0}", monthlyLoanAmount);
                SetDynamicLabel(REPAYMENT_LABEL_NAME, repaymentString);
            }
            else if (loanAmount <= MAXIMUM_LOAN)
            {
                EstablishmentTextBox.Text = MEDIUM_LOAN_ESTABLISHMENT_FEE.ToString();

                decimal yearlyLoanAmount = loanAmount * MEDIUM_ANNUAL_INTEREST_RATE;

                string repaymentString = String.Format("Your yearly Interest charge is ${0}", yearlyLoanAmount);
                SetDynamicLabel(REPAYMENT_LABEL_NAME, repaymentString);
            }
            else
            {
                string errorString = String.Format("The maximum loan amount allowed is ${0}", MAXIMUM_LOAN);
                SetDynamicLabel(ERROR_LABEL_NAME, errorString);
            }
        }
        /// <summary>
        /// Overwrites a previous label or creates a new one
        /// if it does not exists
        /// </summary>
        /// <param name="labelName">The name of the label to overwrite or create</param>
        /// <param name="TextContent">The text the new label should display</param>
        private void SetDynamicLabel(string labelName,string TextContent)
        {
            Label repaymentRate = null;
            repaymentRate = TryFindPreviousLabel(labelName);
            if(repaymentRate == null)
            {
                repaymentRate = new Label
                {
                    Name = labelName,
                    Content = TextContent
                };
                LoanInput.Children.Add(repaymentRate);
            }
            else
            {
                repaymentRate.Content = TextContent;
            }

        }
        /// <summary>
        /// trys to find a previous label if it exists
        /// </summary>
        /// <param name="name">the name of the label to find</param>
        /// <returns>The label if found otherwise null</returns>
        private Label TryFindPreviousLabel(string name)
        {
            Label label = null;
            foreach (UIElement child in LoanInput.Children)
            {
                if (child is Label)
                {
                    Label labelChild = child as Label;
                    if (labelChild.Name.Equals(name))
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
