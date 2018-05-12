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
using System.Speech.Recognition;
using Calculator.VoiceFiles;
using Calculator.Managers;
namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Page
    {
        /*When displaying results the next input should overwrite 
         * This keeps the state
         */ 
        Voice voice;
        TextBoxManager textManger;

        public MainWindow()
        {
            InitializeComponent();
            voice = new Voice(new EventHandler<SpeechRecognizedEventArgs>(Recognizer_SpeechRecognized));
            textManger = new TextBoxManager(txtDisplay);
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
            textManger.WriteToTextBox(b.Content.ToString());
        }
        /// <summary>
        /// Handles the event when the equals buton is pressed
        /// </summary>
        /// <param name="sender">the equals button</param>
        /// <param name="e">Boilerplate state information crrently unused</param>
        private void BtnEquals_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string evaluatedText = textManger.EvaluateResult();
            if (evaluatedText.Length > 0)
            {
                HistoryText.Items.Add(evaluatedText);
            }
        }

        private void HistoryText_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox historyBox = sender as ListBox;
            if(historyBox.SelectedItem != null)
            {
                textManger.WriteToTextBox(historyBox.SelectedItem.ToString());

            }
        }

        private void BtnBackSpace_Click(object sender, RoutedEventArgs e)
        {
            //subtract the last character
            textManger.BackSpaceTextBox();
        }

        private void BtnClearAll_Click(object sender, RoutedEventArgs e)
        {
            HistoryText.Items.Clear();
        }

        private void BtnClearEntry_Click(object sender, RoutedEventArgs e)
        {
            textManger.ClearText();
        }

        private void BtnVoice_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            b.Background = voice.ToggleListening()? Brushes.Green :Brushes.Red;

        }
        void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            textManger.WriteToTextBox( e.Result.Text);
        }
    }
}
