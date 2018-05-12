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

        SpeechRecognitionEngine recognizer =
      new SpeechRecognitionEngine(
        new System.Globalization.CultureInfo("en-NZ"));


        public Voice(EventHandler<SpeechRecognizedEventArgs> result)
        {
            Choices numbers = new Choices(new string[] { "1", "2", "4", "5", "8", "9" });
            //The numbers that on testing where most frequently misheard
            Choices harderNumbers = new Choices(new string[] { "2", "3", "6", "7" });
            recognizer.LoadGrammar(CreateGrammer(numbers));

            Grammar hardGrammer = CreateGrammer(harderNumbers);
            hardGrammer.Priority = 127;//Max
            recognizer.LoadGrammar(hardGrammer);
            // Configure input to the speech recognizer.
            recognizer.SetInputToDefaultAudioDevice();

            recognizer.SpeechRecognized +=result ;
        }
        private Grammar CreateGrammer(Choices numbers)
        {
            GrammarBuilder numbersgb = new GrammarBuilder(numbers);
            Grammar numberGrammer = new Grammar(numbersgb);
            return numberGrammer;
        }
        public void Listen()
        {

            // Start asynchronous, continuous speech recognition.
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }
       
    }
}
