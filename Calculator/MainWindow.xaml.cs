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
using Calculator.Network;
using System.Net.Sockets;
using System.Windows.Threading;

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
        AsynchronousClient client;
        AsynchronousSocketListener server;
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
            Evaluate();
        }


        private void HistoryText_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox historyBox = sender as ListBox;
            if (historyBox.SelectedItem != null)
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
            b.Background = voice.ToggleListening() ? Brushes.Green : Brushes.Red;

        }
        void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (!e.Result.Text.Equals("="))
            {
                textManger.WriteToTextBox(e.Result.Text);
            }
            else
            {
                Evaluate();
            }
        }

        private void BtnInterest_Click(object sender, RoutedEventArgs e)
        {
            Interest interest = new Interest();
            this.NavigationService.Navigate(interest);
        }

        private void BtnSync_Click(object sender, RoutedEventArgs e)
        {
            if (ServerChannel.Text.Length == 0 && ClientChannel.Text.Length == 0)
            {
                MessageBox.Show("Note only one of The Client or server should be filled in ");
                return;
                }
            if (int.TryParse(ServerChannel.Text, out int serverPort))
            {
                if(server != null)
                {
                    server.EndSocket();
                }
                server = new AsynchronousSocketListener(serverPort,SyncedData);
                server.StartSocket();
            }

            if (int.TryParse(ClientChannel.Text, out int clientPort))
            {

                client = new AsynchronousClient(clientPort);
                //of type object so I can use LINQ, Then aggregate combines the items into a string with a newline seperating them
                var data = HistoryText.Items.OfType<object>().Aggregate("", (acc, x) => acc += x.ToString() +Environment.NewLine);
                client.Start(data);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void SyncedData(List<string> data)
        {
            Console.WriteLine("hello");
            //allow g
            this.Dispatcher.Invoke(() =>
            {
                Console.WriteLine("lol");
                HistoryText.Items.Clear();
                data.Select(x => HistoryText.Items.Add(x)).ToList();
                //MessageBox.Show(x);
            }, DispatcherPriority.Background);

        }

        private void Evaluate()
        {
            string evaluatedText = textManger.EvaluateResult();
            if (evaluatedText.Length > 0)
            {
                HistoryText.Items.Add(evaluatedText);
            }
        }
    }
}
