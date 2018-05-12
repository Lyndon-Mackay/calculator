using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using org.mariuszgromada.math.mxparser;
namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool OverWriteDisplay = false;

        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Handles the simple case where the buttons are
        /// added to the text dislay for numbers and the basic operations
        /// </summary>
        /// <param name="sender">The button</param>
        /// <param name="e">Boilerplate state information crrently unused</param>
        private void BtnNumberAndOperationClick(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (OverWriteDisplay)
            {
                txtDisplay.Text = b.Content.ToString();
                OverWriteDisplay = false;
            }
            else
            {
                txtDisplay.Text += b.Content.ToString();
                
            }
        }
        /// <summary>
        /// Handles the event when the equals buton is pressed
        /// </summary>
        /// <param name="sender">the equals button</param>
        /// <param name="e">Boilerplate state information crrently unused</param>
        private void BtnEquals_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string evaluatedText = txtDisplay.Text;
            //have to use fullnamespace as it conflicts with windows :(
            org.mariuszgromada.math.mxparser.Expression expression = new org.mariuszgromada.math.mxparser.Expression(evaluatedText);
            double result = expression.calculate();
            if(Double.IsNaN(result))
            {
                txtDisplay.Text = "Error";
            }
            else
            {
                txtDisplay.Text = result.ToString();
                HistoryText.Items.Add(evaluatedText);
                OverWriteDisplay = true;
            }
        }

        private void HistoryText_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox historyBox = sender as ListBox;
            if(historyBox.SelectedItem != null)
            {
                txtDisplay.Text = historyBox.SelectedItem.ToString();
                OverWriteDisplay = false;
            }
        }

        private void BtnBackSpace_Click(object sender, RoutedEventArgs e)
        {
            //subtract the last character
            txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
        }

        private void BtnClearAll_Click(object sender, RoutedEventArgs e)
        {
            HistoryText.Items.Clear();
        }

        private void BtnClearEntry_Click(object sender, RoutedEventArgs e)
        {
            OverWriteDisplay = false;
            txtDisplay.Clear();
        }
    }
}
