using System;
using Microsoft.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace SpeechRecognition
{
    public class Registration
    {
        public static string userText;
        public static string confirmationText;
        public static Boolean textRecognized = false;
        public static Boolean confirmation = false;

        public User currentUser;


        public static Dictionary<String, int> userDict = new Dictionary<String, int>() { { "Javier", 1 }, { "Miguel", 2 }, { "Andre", 3 }, { "Leevi", 4 }, { "Paolo", 5 }, };


        public Registration() { }

        public User getCurrentUser()
        {
            return this.currentUser;
        }

        /**
         * Use synthesizer to ask user his name or age
         */
        public static string askUserForData(string question, Choices choices)
        {
            try
            {
                SpeechSynthesizer synth = new SpeechSynthesizer();
                synth.Speak(question);

                // Create a SpeechRecognitionEngine object for the default recognizer in the en-US locale.
                using (
                SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US")))
                {
                    // Configure the input to the speech recognizer.
                    recognizer.SetInputToDefaultAudioDevice();

                    Grammar userdata = Grammars.UserData(choices);
                    recognizer.LoadGrammarAsync(userdata);
                    recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Handler.recognizer_SpeechRecognized2);

                    resetControlVariables();
                    // Start asynchronous, continuous speech recognition.
                    recognizer.RecognizeAsync(RecognizeMode.Multiple);

                    // Keep the console window open.
                    while (!textRecognized) ;

                    recognizer.SpeechRecognized -= Handler.recognizer_SpeechRecognized2;

                    Boolean restartConfig = false;

                    if (userText.Contains("restart")) restartConfig = true;
                    else synth.Speak("Is" + userText + " correct?");

                    if (restartConfig) ;
                    else if (confirmSelection()) return userText;
                    else return askUserForData(question, choices);
                } //END OF USING SPEECH RECOGNIZER
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.StackTrace.ToString());
            }
            return userText;
        }

        public void initialization()
        {
            if (!File.Exists("userInfo.txt"))
            {
                String users = askUserForData("How many users do you want to add to the system?", ToChoose.small_numbers);

                int numUsers;
                Dict.numDictionary.TryGetValue(users, out numUsers);

                System.IO.StreamWriter file = new System.IO.StreamWriter("userInfo.txt", false);

                try
                {
                    for (int i = 1; i <= numUsers; i++)
                    {
                        //Ask user name return string + confirm
                        string name = "";

                        if (i == 1) askUserForData("What's your name?", ToChoose.names);
                        else askUserForData("What is the name of user" + i + "?", ToChoose.names);

                        name = userText;

                        //ask user age + confirm
                        if (i == 1) askUserForData("How old are you?", ToChoose.numbers);
                        else askUserForData("How old is " + name + "?", ToChoose.numbers);

                        string ageStr = userText;
                        int age;
                        Dict.numDictionary.TryGetValue(ageStr, out age);

                        int id;
                        userDict.TryGetValue(name, out id);


                        if (i == 1)
                        {
                            currentUser = new User(name, age, id);
                            currentUser.DateOfBirth();
                        }

                        file.WriteLine(name + "," + age + "," + id + "\n");
                    }
                }
                catch (Exception superbogus) { Console.WriteLine("Superbogus Exception detected: " + superbogus.StackTrace); }
                finally { file.Close(); }
            }
            else
            {
                //If file exists then ask for log in name and assign to currentUser
                askUserForData("What is your name?", ToChoose.names);
                User user = getUserFromDB(userText);

                if (user != null) currentUser = user;
            }
        }

        public static User getUserFromDB(String name)
        {
            String line;
            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader("userInfo.txt");
            while ((line = file.ReadLine()) != null)
            {
                String[] array = line.Split(',');
                if (array[0].Equals(name))
                {
                    int age, id;
                    int.TryParse(array[1], out age);
                    int.TryParse(array[2], out id);
                    return new User(name, age, id);
                }
            }
            return null;
        }

        static Boolean confirmSelection()
        {
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));

            recognizer.SetInputToDefaultAudioDevice();

            // Add a handler for the speech recognized event.
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Handler.recognizer_SpeechRecognized2);
            Grammar confirm = Grammars.Confirm();
            recognizer.LoadGrammarAsync(confirm);

            confirmation = true;
            textRecognized = false;
            // Start synchronous speech recognition.
            recognizer.RecognizeAsync();

            while (!textRecognized) ;

            recognizer.SpeechRecognized -= Handler.recognizer_SpeechRecognized2;

            switch (confirmationText)
            {
                case "Yes":
                case "Yeah":
                case "Yep":
                    return true;
                case "No":
                case "Nope":
                    return false;
                default: return false;
            }
        }
        //END OF WORD RECOGNITION



        static void resetControlVariables()
        {
            textRecognized = false;
            confirmation = false;
        }
    }
}