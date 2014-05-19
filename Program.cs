using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;
//using System.Windows.Threading;
using System.ComponentModel;
using System.Threading;

namespace SpeechRecognition
{
    public class Program
    {

        public Program() {
        }

        public static Registration registration;
        public static int id;
        public static SpeechRecognitionEngine sre;

        public static Boolean dictationCompleted = false;


//        public static MainWindow mainWindow = new MainWindow();


      
        public void Start(MainWindow mainWindow)
        {


//            mainWindow.updateText("LOCAAAA");

//            mainWindow.ShowDialog();

//            mainWindow.updateText("");

            
            
            
            registration = new Registration();
            Dict dict = new Dict();
            Appearance appear = new Appearance();

            //Initialisation of ressources
            appear.init();
            dict.init();
            
            //Registration or Login
            registration.initialization();

            //Capture Audio Data different class for all users (Initialization) -> Store audio file or raw

            //Ask user to say his name for voice recognition

            //Check for name in "database"
            if (registration.getCurrentUser() != null)
            {



                Grammar dictation = new DictationGrammar();
                dictation.Name = "Dictation Grammar";
                sre = new System.Speech.Recognition.SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
                SpeechSynthesizer synth = new SpeechSynthesizer();
                synth.SetOutputToDefaultAudioDevice();

                synth.Speak("Starting user verification.");

                sre.SetInputToDefaultAudioDevice();

                sre.LoadGrammar(dictation);
                sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Handler.recognizer_userRecognized);

                sre.RecognizeAsync(RecognizeMode.Multiple);

                while (!dictationCompleted) ;

                //Check for correct password as well as for correct user voice (DTW or Gaussian) in handler
                sre.SpeechRecognized -= new EventHandler<SpeechRecognizedEventArgs>(Handler.recognizer_userRecognized);

                //User is in database
                Console.WriteLine("Successfully authenticated!");
                synth.SpeakAsync("You are now authenticated. Welcome "+registration.currentUser.Name);

                //If login successful give access to SpeechRecognitionInterface
                //Interaction.start();

                Console.ReadLine();
            }
        } // END OF MAIN

           }
}