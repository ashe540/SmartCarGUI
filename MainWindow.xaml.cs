using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Microsoft.Speech.Recognition;
using System.IO;

using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using speakerVerification_v5;
using System.Media;
using System.Speech.Synthesis;




namespace SpeechRecognition
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    
{
        public static Choices choices = new Choices(new string[] { "Radio", "Air_Condition", "Navigation", "Phone", "Car" });

        public static Choices second_choices = new Choices(new string[] { "toggle", "on", "off", "next", "louder", "quieter", "previous", "silent", "out", "random", "shuffle"
                                                                        + "accept", "decline", "call"});

        public static Choices third_choices = new Choices(new string[] { "one", "two", "three", "four", "twenty_one", " " });

        //          public static Choices names = new Choices(new string[] { "Eduardo", "Ivan", "Anti", "Simin", "Evelina", "Kristian", "Timofei", "Leevi", "Christian", "Stephan", "Javier", "Sebastian", "Rico", "Andre", "Carlos", "Nerea", "Francisco", "Lorenzo", "Santiago", "Adria", "Miguel", "Victor", "Guillermo", "Carlotta", "Francesca", "Paolo", "Andi", "Giulia", "Paco" });
        public static Choices names = new Choices(new string[] { "Javier", "Miguel", "Leevi", "Andre", "Paolo" });

        public static Choices numbers = new Choices(new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen", "twenty", "twenty one", "twenty two", "twenty three", "twenty four", "twenty five", "twenty six", "twenty seven", "twenty eight", "twenty nine", "thirty", "thirty one", "thirty two", "thirty three", "thirty four", "thirty five", "thirty six", "thirty seven", "thirty eight", "thirty nine", "forty ", "forty one", "forty two", "forty three", "forty four", "forty five", "forty six", "forty seven", "forty eight", "forty nine", "fifty", "fifty one", "fifty two", "fifty three", "fifty four", "fifty five", "fifty six", "fifty seven", "fifty eight", "fifty nine", "sixty", "sixty one", "sixty two", "sixty three", "sixty four", "sixty five", "sixty six", "sixty seven", "sixty eight", "sixty nine", "seventy", "seventy one", "seventy two", "seventy three", "seventy four", "seventy five", "seventy six", "seventy seven", "seventy eight", "seventy nine", "eighty", "eighty one", "eighty two", "eighty three", "eighty four", "eighty five", "eighty six", "eighty seven", "eighty eight", "eighty nine", "ninety", "ninety one", "ninety two", "ninety three", "ninety four", "ninety five", "ninety six", "ninety seven", "ninety eight", "ninety nine", "one hundred" });

        public static Choices small_numbers = new Choices(new string[] { "one", "two", "three", "four", "five" });

         static SoundPlayer snd;

        static Boolean carOn = false;
        static Boolean radioOn = false;
        static Boolean ACOn = false;
        


        static String DIR = "../../Resources/Sounds/";
        
        static int currentSong = 0;

        static String currentSongPlaying;

        static String[] musicFiles = {"wreckingball.wav","sweethomealabama.wav","gangmanstyle.wav","happy.wav"};
        static String[] musicNames = { "Wrecking Ball - Miley Cyrus", "Sweet Home Alabama - Lynyrd Skynyrd", "Gangman Style - PSY", "Happy - Pharrell Williams" };



        public MainWindow()
        {
            InitializeComponent();
            
        }

  
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            startCommands();
        }

        public void startCommands()
        {

            
            // Create a SpeechRecognitionEngine object for the default recognizer in the en-US locale.
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));

            // Configure the input to the speech recognizer.
            recognizer.SetInputToDefaultAudioDevice();



            Choices choices = new Choices(new string[] { "Radio", "Air_Condition", "Navigation", "Phone", "Car" });
            Choices second_choices = new Choices(new string[] { "warmer", "hotter", "colder", "on", "off", "next", "louder", "quieter", "previous", "silent", "out", "random", "shuffle"
                                                                    + "accept", "decline", "call", "hangup"});
            Choices third_choices = new Choices(new string[] { "one", "two", "three", "four", "twenty_one", " " });

            GrammarBuilder choice = new GrammarBuilder();
            choice.Culture = new System.Globalization.CultureInfo("en-US");
            choice.Append(new SemanticResultKey("Choice", choices));
            choice.Append(new SemanticResultKey("Choice2", second_choices));
            choice.Append(new SemanticResultKey("Choice3", third_choices));
            Grammar speech = new Grammar(choice);
            speech.Name = "Choice";

            speech.SpeechRecognized += new
            EventHandler<SpeechRecognizedEventArgs>(Speech_Handler);



            speech.Name = ("Speech analyser");
            recognizer.LoadGrammarAsync(speech);
            // Add a handler for the speech recognized event.
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);
            recognizer.SpeechHypothesized += new EventHandler<SpeechHypothesizedEventArgs>(recognizer_SpeechHypothesized);

            // Start asynchronous, continuous speech recognition.
            recognizer.RecognizeAsync(RecognizeMode.Multiple);

        }

      
         public void Speech_Handler(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result == null) return;


            RecognitionResult result = e.Result;
            Console.WriteLine(result);
            string[] text = result.Text.Split(new Char[] { ' ' });
            foreach (string i in text) Console.WriteLine(i);
            switch (text[0])
            {
                case "Radio":

                    //Binding myBinding = new Binding("myDataResource");
                    //myBinding.Source = currentSongPlaying;
                    
                    RadioFunctionalities(text);
                    
                    break;
                case "Car":
                    CarFunctionalities(text);
                    break;
                case "Air_Condition":
                    ACFunctionalities(text);
                    break;
                case "Navigation":
                    NavFunctionalities(text);
                    break;
                case "Phone":
                    PhoneFuntionalities(text);
                    break;
            }
        }

        // Handle the SpeechRecognized event.
        public void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Recognized text: " + e.Result.Text);

            terminal.Text = e.Result.Text ;


            if (e.Result.Text.Contains("Radio"))
            {
                music.Content = currentSongPlaying;

            }
            else if (e.Result.Text.Contains("Air_Condition"))
            {
                if (ACOn)
                {
                    ACOn = false;
                    temperature.Content = "";
                    airFlow.Visibility = Visibility.Hidden;
                }
                else
                {
                    temperature.Content = "23 ºC";
                    ACOn = true;
                    airFlow.Visibility = Visibility.Visible;
                }
            }

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

        public void recognizer_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            Console.WriteLine("Speech input was rejected.");
            foreach (RecognizedPhrase phrase in e.Result.Alternates)
            {
                //Console.WriteLine("  Rejected phrase: " + phrase.Text);
                Console.WriteLine("  Confidence score: " + phrase.Confidence);
                //Console.WriteLine("  Grammar name:  " + phrase.Grammar.Name);
            }
        }

        public void recognizer_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            Console.WriteLine("The audio level is now: {0}.", e.AudioLevel);
        }

        public void recognizer_AudioStateChanged(object sender, AudioStateChangedEventArgs e)
        {
            Console.WriteLine("The new audio state is: " + e.AudioState);
        }

        public bool end_of_phrase(object sender, RecognizeCompletedEventArgs e)
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

        public void recognizer_LoadGrammarCompleted(object sender, LoadGrammarCompletedEventArgs e)
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

        public void recognizer_SpeechRecognized2(object sender, SpeechRecognizedEventArgs e)
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


        public void recognizer_userRecognized(object sender, System.Speech.Recognition.SpeechRecognizedEventArgs e)
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
                Console.WriteLine("Access denied.");

            }
        }

    
     public String RadioFunctionalities(string[] input)
        {

           
            if (carOn)
            {

                bool invalid = false;
                CorrectText("Radio functionalities");

                switch (input[1])
                {
                    case "on":

                        if (!radioOn)
                        {
                            snd = new SoundPlayer(DIR + musicFiles[currentSong]);
                            currentSongPlaying = musicNames[currentSong];
                            snd.Play();
                            radioOn = true;
                        }
                        else suggest("Radio is already on");
                        break;
                    case "off":
                        if (snd != null)
                        {
                            currentSongPlaying = "Radio off";
                            snd.Stop();
                            radioOn = false;
                        }
                        else suggest("Radio is already turned off.");

                        break;
                    case "louder":
                        break;
                    case "quieter":
                        break;
                    case "next":
                        if (radioOn)
                        {
                            currentSong = (currentSong + 1) % musicFiles.Length;
                            snd.SoundLocation = DIR + musicFiles[currentSong];
                            currentSongPlaying = musicNames[currentSong];
                            snd.Play();
                        }
                        else suggest("Radio must be on");

                        break;
                    case "silent":
                        break;
                    case "previous":
                        if (radioOn)
                        {
                            if (currentSong > 0) currentSong--;
                            else currentSong = musicFiles.Length - 1;

                            snd.SoundLocation = DIR + musicFiles[currentSong];
                            currentSongPlaying = musicNames[currentSong];
                            snd.Play();
                        }
                        else suggest("Radio must be on");


                        break;
                    case "random":
                    case "shuffle":
                        if (radioOn)
                        {
                            Random rnd = new Random();
                            currentSong = rnd.Next(0, musicFiles.Length - 1);
                            snd.SoundLocation = DIR + musicFiles[currentSong];
                            currentSongPlaying = musicNames[currentSong];
                            snd.Play();
                        }
                        else suggest("Radio must be on");

                        break;
                    default:
                        invalid = true;
                        break;
                }
                if (invalid) Invalid();

               return currentSongPlaying;

            }
            else
            {
                suggest("Car must be turned on");
                return "";
                
            }

        }

        public void CarFunctionalities(string[] input)
        {

            switch (input[1])
            {
                case "on":
                    if (!carOn)
                    {
                        SoundPlayer snd = new SoundPlayer(DIR + "engineon.wav");
                        snd.Play();
                        carOn = true;
                    }
                    else suggest("Car is already on");
                    break;
                case "off":
                    if (carOn)
                    {
                        SoundPlayer snd = new SoundPlayer(DIR + "engineoff.wav");
                        snd.Play();
                        carOn = false;
                    }
                    else suggest("Car is already off");
                    break;
            }
        }

        public void ACFunctionalities(string[] input)
        {
            CorrectText("AC-Functionalities");
            bool invalid = false;
            switch (input[1])
            {
                case "toggle":
                    break;
                default:
                    invalid = true;
                    break;
            }
            if (invalid) Invalid();
        }

        public void NavFunctionalities(string[] input)
        {
            bool invalid = false;
            CorrectText("Nav-Functionalities");
            switch (input[1])
            {
                case "on":
                    break;
                case "off":
                    break;
                default:
                    invalid = true;
                    break;
            }
            if (invalid) Invalid();
        }

        public void PhoneFuntionalities(string[] input)
        {
            bool invalid = false;
            CorrectText("Phone-Functionalities");
            switch (input[1])
            {
                case "accept":
                    break;
                case "decline":
                    break;
                case "call":
                    SpeechSynthesizer synth = new SpeechSynthesizer();
                    synth.Speak("Initiating call. Please wait...");
                    System.Threading.Thread.Sleep(2000);
                    snd = new SoundPlayer(DIR+"phonecall.wav");
                    snd.Play();
                    break;
                case "hangup":
                    snd.Stop();
                    break;
                default:
                    invalid = true;
                    break;
            }
            
            if (invalid) Invalid();
        }

        public void Invalid()
        {
            ErrorText("Invalid Input, try again");
        }

        public void ErrorText(string input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void CorrectText(string input)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void suggest(String suggestion)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SpeakAsync(suggestion);
        }

    }
    
    
    }


