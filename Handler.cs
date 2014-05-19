using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech;
using Microsoft.Speech.Recognition;
using System.IO;
using System.Speech.Synthesis;

using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using speakerVerification_v5;
//using MathWorks.MATLAB.NET.Utility.MWMCRVersion;

namespace SpeechRecognition
{
    class Handler
    {

        private Program main;
        private MainWindow mw;
        public Handler(MainWindow main){
            this.mw = main;
        }



        public void Speech_Handler(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result == null) return;

            
            RecognitionResult result = e.Result;

//            parent.main.musicText.Content = e.Result.Text;
            Grammar g = (Grammar)sender;
            //this.mw.music.Content = e.Result.Text;
            // = e.Result.Text.ToString();

//            MainWindow.updateText(result.Text);

            Console.WriteLine(result);
            string[] text = result.Text.Split(new Char[] { ' ' });
            foreach (string i in text) Console.WriteLine(i);
            switch (text[0])
            {
                case "Radio":
                    Functionalities.RadioFunctionalities(text);
                    break;
                case "Car":
                    Functionalities.CarFunctionalities(text);
                    break;
                case "Air_Condition":
                    Functionalities.ACFunctionalities(text);
                    break;
                case "Navigation":
                    Functionalities.NavFunctionalities(text);
                    break;
                case "Phone":
                    Functionalities.PhoneFuntionalities(text);
                    break;
            }
        }

        // Handle the SpeechRecognized event.
        public static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {


         //   MainWindow.updateText("Recognized text: " + e.Result.Text);

            Console.WriteLine("Recognized text: " + e.Result.Text);
            Console.WriteLine("  Confidence score: " + e.Result.Confidence);
            Console.WriteLine();

            if (e.Result.Semantics.ContainsKey("Passphrase"))
                Console.WriteLine("  The passphrase is: " +
                     e.Result.Semantics["Passphrase"].Value);
        }

        // Handle the SpeechHypothesized event.
        public static void recognizer_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            Console.WriteLine("Speech hypothesized: " + e.Result.Text);
        }

        public static void recognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            Console.WriteLine("Speech input was rejected.");
            foreach (RecognizedPhrase phrase in e.Result.Alternates)
            {
                //Console.WriteLine("  Rejected phrase: " + phrase.Text);
                Console.WriteLine("  Confidence score: " + phrase.Confidence);
                //Console.WriteLine("  Grammar name:  " + phrase.Grammar.Name);
            }
        }

        public static void recognizer_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            Console.WriteLine("The audio level is now: {0}.", e.AudioLevel);
        }

        public static void recognizer_AudioStateChanged(object sender, AudioStateChangedEventArgs e)
        {
            Console.WriteLine("The new audio state is: " + e.AudioState);
        }

        public static bool end_of_phrase(object sender, RecognizeCompletedEventArgs e)
        {
            if (e.InitialSilenceTimeout || e.BabbleTimeout)
            {
                Console.WriteLine(
                  "RecognizeCompleted: BabbleTimeout({0}), InitialSilenceTimeout({1}).",
                  e.BabbleTimeout, e.InitialSilenceTimeout);
                return true;
            }
            return false;
        }

        public static void recognizer_LoadGrammarCompleted(object sender, LoadGrammarCompletedEventArgs e)
        {
            string grammarName = e.Grammar.Name;
            bool grammarLoaded = e.Grammar.Loaded;
            bool grammarEnabled = e.Grammar.Enabled;

            if (e.Error != null)
            {
                Console.WriteLine("LoadGrammar for {0} failed with a {1}.",
                grammarName, e.Error.GetType().Name);
                // Add exception handling code here.
            }
            Console.WriteLine("Grammar {0} {1} loaded and {2} enabled.", grammarName, (grammarLoaded) ? "is" : "is not", (grammarEnabled) ? "is" : "is not");
        }

        public static void recognizer_SpeechRecognized2(object sender, SpeechRecognizedEventArgs e)
        {
            if (Registration.confirmation) Registration.confirmationText = e.Result.Text;
            else Registration.userText = e.Result.Text;

            Registration.textRecognized = true;
            /*            if (e.Result.Text.Contains("Restart configuration"))
                        {
                            initialization();
                            throw SteveScumbagException();
                        }
            */
            Console.WriteLine(e.Result.Text);
        }


        public static void recognizer_userRecognized(object sender, System.Speech.Recognition.SpeechRecognizedEventArgs e)
        {
            if (File.Exists("../../Resources/Data/Test/actual_user_speech.wav"))
            {
                try
                {
                    File.Delete("../../Resources/Data/Test/actual_user_speech.wav");
                }
                catch { }
            }

            FileStream file = new FileStream("../../Resources/Data/Test/actual_user_speech.wav", FileMode.OpenOrCreate);

            e.Result.Audio.WriteToWaveStream(file);
            file.Flush();
            file.Close();
            //MWNumericArray nUser = new MWNumericArray(new int());
            MWNumericArray access = new MWNumericArray(new int());
            speakerVerification sV = new speakerVerification();

            access = (MWNumericArray)sV.mat_speakerVerification_test((MWNumericArray)(Program.registration.currentUser.Id));

            if ((int)access == 1)
            {
                Console.WriteLine("Verification completed. User name:");
                //Console.WriteLine(usersString[(Int32)nUser - 1]);
                Console.WriteLine(Program.registration.currentUser.Name);
                Console.WriteLine();
                Console.WriteLine("Done.");
                Program.dictationCompleted = true;
            }
            else
            {
                SpeechSynthesizer synth = new SpeechSynthesizer();
                synth.Speak("Access denied");
                Console.WriteLine("Access denied.");

            }
        }


    }
}