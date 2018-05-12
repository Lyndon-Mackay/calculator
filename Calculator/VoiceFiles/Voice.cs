using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.VoiceFiles
{
    class Voice
    {

        SpeechRecognitionEngine _recognizer =
      new SpeechRecognitionEngine(
        new System.Globalization.CultureInfo("en-NZ"));
        bool _listening = false;

        public Voice(EventHandler<SpeechRecognizedEventArgs> result)
        {
            Choices numbers = new Choices(new string[] { "0","1", "2", "4", "5", "8", "9" });
            _recognizer.LoadGrammar(CreateGrammer(numbers));


            Choices Operators = new Choices(new string[] { "+", "-","*","/" ,"%","=" });
            _recognizer.LoadGrammar(CreateGrammer(Operators));

            //The numbers that on testing where most frequently misheard
            Choices harderNumbers = new Choices(new string[] { "2", "3", "6", "7" });


            Grammar hardGrammer = CreateGrammer(harderNumbers);
            hardGrammer.Priority = 127;//Max
            _recognizer.LoadGrammar(hardGrammer);
            // Configure input to the speech recognizer.
            _recognizer.SetInputToDefaultAudioDevice();

            _recognizer.SpeechRecognized +=result ;
        }
        private Grammar CreateGrammer(Choices numbers)
        {
            GrammarBuilder numbersgb = new GrammarBuilder(numbers);
            Grammar numberGrammer = new Grammar(numbersgb);
            return numberGrammer;
        }

       public bool ToggleListening()
        {
            if(_listening)
            {
                _listening = false;
                _recognizer.RecognizeAsyncStop();
            }
            else
            {
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
                _listening = true;
            }
            return _listening;
        }
    }
}
