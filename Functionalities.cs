using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech;
using Microsoft.Speech.Recognition;
using System.Media;
using System.IO;
using System.Speech.Synthesis;

namespace SpeechRecognition
{
    public class Functionalities
    {
        static SoundPlayer snd;

        static Boolean carOn = false;
        static Boolean radioOn = false;
        static Boolean callUnderway = false;




        static String DIR = "../../Resources/Sounds/";
        
        static int currentSong = 0;
        
        
        static String[] music = {"wreckingball.wav","sweethomealabama.wav","gangmanstyle.wav","happy.wav"};
        static String[] musicNames = { "Wrecking Ball - Miley Cyrus", "Sweet Home Alabama - Lynyrd Skynyrd", "Gangman Style - PSY", "Happy - Pharrell Williams" };



        public Functionalities(){}

        public static String RadioFunctionalities(string[] input)
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
                            snd = new SoundPlayer(DIR + music[currentSong]);
                            snd.Play();
                            radioOn = true;
                        }
                        else suggest("Radio is already on");
                        break;
                    case "off":
                        if (snd != null)
                        {
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
                            currentSong = (currentSong + 1) % music.Length;
                            snd.SoundLocation = DIR + music[currentSong];
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
                            else currentSong = music.Length - 1;

                            snd.SoundLocation = DIR + music[currentSong];
                            snd.Play();
                        }
                        else suggest("Radio must be on");


                        break;
                    case "random":
                    case "shuffle":
                        if (radioOn)
                        {
                            Random rnd = new Random();
                            currentSong = rnd.Next(0, music.Length - 1);
                            snd.SoundLocation = DIR + music[currentSong];
                            snd.Play();
                        }
                        else suggest("Radio must be on");

                        break;
                    default:
                        invalid = true;
                        break;
                }
                if (invalid) Invalid();
                return musicNames[currentSong];

            }
            else
            {
                suggest("Car must be turned on");
                return musicNames[currentSong];
            }

        }

        public static void CarFunctionalities(string[] input)
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

        public static void ACFunctionalities(string[] input)
        {
            CorrectText("AC-Functionalities");
            bool invalid = false;
            switch (input[1])
            {
                case "warmer":
                    break;
                case "colder":
                    break;
                case "hotter":
                    break;
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

        public static void NavFunctionalities(string[] input)
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

        public static void PhoneFuntionalities(string[] input)
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

        public static void Invalid()
        {
            ErrorText("Invalid Input, try again");
        }

        public static void ErrorText(string input)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void CorrectText(string input)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(input);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void suggest(String suggestion)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SpeakAsync(suggestion);
        }

    }
}
