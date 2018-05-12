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
            Choices numbers = new Choices(new string[] { "1", "2", "4", "5", "8", "9" });
            //The numbers that on testing where most frequently misheard
            Choices harderNumbers = new Choices(new string[] { "2", "3", "6", "7" });
            _recognizer.LoadGrammar(CreateGrammer(numbers));

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
        public void Listen()
        {
            if(_listening)
            {
                return;
            }
            // Start asynchronous, continuous speech recognition.
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
            _listening = true;
        }
        public void StopListening()
        {
            _listening = false;
            _recognizer.RecognizeAsyncStop();
        }
       
    }
}
