using System;
using Microsoft.Speech.Recognition;

namespace SpeechRecognition
{
    class Grammars
    {
        public Grammars() { }

        public static Grammar Speech()
        {
            Choices choices = new Choices(new string[] { "Radio", "Air_Condition", "Navigation", "Phone", "Car" });
            Choices second_choices = new Choices(new string[] { "warmer", "hotter", "colder", "on", "off", "next", "louder", "quieter", "previous", "silent", "out", "random", "shuffle"
                                                                    + "accept", "decline", "call", "hangup"});
            Choices third_choices = new Choices(new string[] { "one", "two", "three", "four", "twenty_one", " " });

            GrammarBuilder choice = new GrammarBuilder();
            choice.Culture = new System.Globalization.CultureInfo("en-US");
            choice.Append(new SemanticResultKey("Choice", ToChoose.choices));
            choice.Append(new SemanticResultKey("Choice2", ToChoose.second_choices));
            choice.Append(new SemanticResultKey("Choice3", ToChoose.third_choices));
            Grammar speech = new Grammar(choice);
            speech.Name = "Choice";

            //speech.SpeechRecognized += new
            //EventHandler<SpeechRecognizedEventArgs>(Handler.Speech_Handler);
            return speech;
        }

        public static Grammar Confirm()
        {
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = new System.Globalization.CultureInfo("en-US");
            gb.Append(new SemanticResultKey("text", new Choices(new string[] { "Yes", "Yep", "Yeah", "No", "Nope" })));

            Grammar g = new Grammar(gb);
            g.Name = ("Confirmation");

            //g.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Handler.recognizer_SpeechRecognized2);
            return g;
        }

        public static Grammar UserData(Choices choices)
        {
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = new System.Globalization.CultureInfo("en-US");

            gb.Append(new SemanticResultKey("text", choices));

            GrammarBuilder restart = new GrammarBuilder();
            restart.Culture = new System.Globalization.CultureInfo("en-US");
            restart.AppendWildcard();
            restart.Append(new SemanticResultKey("Restart", "Restart configuration"));

            Choices bothg = new Choices(new GrammarBuilder[] { gb, restart });
            GrammarBuilder bf = new GrammarBuilder(bothg);
            bf.Culture = new System.Globalization.CultureInfo("en-US");

            Grammar g = new Grammar(bf);
            g.Name = ("Names");
            return g;
        }
    }
}
