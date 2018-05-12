using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace Calculator.Managers
{

    class TextBoxManager
    {
        private bool _OverWrite;
        private TextBox _display;
        public TextBoxManager(TextBox textBox)
        {
            _display = textBox;
        }
        /// <summary>
        /// writes text to the display 
        /// handles ases such as overwriting
        /// </summary>
        /// <param name="text">the text to add</param>
        public void WriteToTextBox(string text)
        {
            if(_OverWrite)
            {
                _display.Text = text;
                _OverWrite = false;
                return;
            }
            _display.Text += text;

        }
        /// <summary>
        /// handles backspace command
        /// </summary>
        public void BackSpaceTextBox()
        {
            string text = _display.Text;
            if (text.Length == 0)
            {
                _OverWrite = false;
                return;
            }
            //contains instead of equals as user may have typed other numbers before noticing
            if(text.Contains("Error"))
            {
                _display.Text = "";
                _OverWrite = false;
                return;
            }
            _display.Text = text.Substring(0, text.Length - 1);
        }
        public void ClearText()
        {
            _display.Text = "";
        }
        /// <summary>
        /// Evaluate the Result and display text
        /// </summary>
        /// <returns>The history to display if length is 0 should be discarded</returns>
        public String EvaluateResult()
        {
            string evaluatedText = _display.Text;

            //have to use fullnamespace as it conflicts with another windows defined expression:(
            org.mariuszgromada.math.mxparser.Expression expression = new org.mariuszgromada.math.mxparser.Expression(evaluatedText);
            double result = expression.calculate();

            _OverWrite = true;

            if (Double.IsNaN(result))
            {
                _display.Text = "Error";
            }
            else
            {
                _display.Text = result.ToString();
                return evaluatedText;

            }
            return "";
        }

    }
}
