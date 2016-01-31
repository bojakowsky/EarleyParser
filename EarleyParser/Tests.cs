using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EarleyParser;
using p = EarleyParser.Program.Grammar;
namespace EarleyParser
{
    public class Test
    {
        //Rules of the Grammar
        Dictionary<List<Program.TermsRight>, string> RulesKeyValues;
        List<string> Values;

        public enum TestsAvailable
        {
            AddAndMultiplyNumbersTest,
            JohnMaryTest
        }

        private void AddAndMultiplyNumberTest()
        {
            #region Rules section
            var s1 = p.ListBuilder("S", "+", "M");
            var s2 = p.ListBuilder("M");
            var sRKV1 = p.ListOfListsBuilder(s1, s2);
            RulesKeyValues.Add(sRKV1, "S");


            var s3 = p.ListBuilder("M", "*", "T");
            var s4 = p.ListBuilder("T");
            var sRKV2 = p.ListOfListsBuilder(s3, s4);
            RulesKeyValues.Add(sRKV2, "M");

            var s5 = p.ListBuilder("Number");
            var sRKV3 = p.ListOfListsBuilder(s5);
            RulesKeyValues.Add(sRKV3, "T");

            var s6 = p.ListBuilder("1", "2", "3", "4");
            var sRKV4 = p.ListOfListsBuilder(s6);
            RulesKeyValues.Add(sRKV4, "Number");

            var s7 = p.ListBuilder("+");
            var sRKV5 = p.ListOfListsBuilder(s7);
            RulesKeyValues.Add(sRKV5, "+");

            var s8 = p.ListBuilder("*");
            var sRKV6 = p.ListOfListsBuilder(s8);
            RulesKeyValues.Add(sRKV6, "*");

            Values.Add("+");
            Values.Add("*");
            Values.Add("Number");
            #endregion
        }
        private void JohnMaryTest()
        {
            #region Rules section
            //S → NP VP
            var s1 = p.ListBuilder("NP", "VP");
            var sRKV1 = p.ListOfListsBuilder(s1);
            RulesKeyValues.Add(sRKV1, "S");

            //NP → NP PP
            //NP → Noun
            var s2 = p.ListBuilder("NP", "PP");
            var s3 = p.ListBuilder("Noun");
            var sRKV2 = p.ListOfListsBuilder(s2, s3);
            RulesKeyValues.Add(sRKV2, "NP");

            //VP → Verb NP
            //VP → VP PP
            var s4 = p.ListBuilder("Verb", "NP");
            var s5 = p.ListBuilder("VP", "PP");
            var sRKV3 = p.ListOfListsBuilder(s4, s5);
            RulesKeyValues.Add(sRKV3, "VP");

            //PP → Prep NP
            var s6 = p.ListBuilder("Prep", "NP");
            var sRKV4 = p.ListOfListsBuilder(s6);
            RulesKeyValues.Add(sRKV4, "PP");

            //Noun → “john”
            //Noun → “mary”
            //Noun → “denver”
            var s7 = p.ListBuilder("John");
            var s8 = p.ListBuilder("Mary");
            var s9 = p.ListBuilder("Denver");
            var sRKV5 = p.ListOfListsBuilder(s7, s8, s9);
            RulesKeyValues.Add(sRKV5, "Noun");

            //Verb → “called”
            var s10 = p.ListBuilder("called");
            var sRKV6 = p.ListOfListsBuilder(s10);
            RulesKeyValues.Add(sRKV6, "Verb");

            //Prep → “from”
            var s11 = p.ListBuilder("from");
            var sRKV7 = p.ListOfListsBuilder(s11);
            RulesKeyValues.Add(sRKV7, "Prep");

            Values.Add("Prep");
            Values.Add("Verb");
            Values.Add("Noun");
            #endregion
        }
        public void FirstTest(List<string> sentence, TestsAvailable test)
        {
            RulesKeyValues = new Dictionary<List<Program.TermsRight>, string>();
            Values = new List<string>();
            switch (test)
            {
                case TestsAvailable.AddAndMultiplyNumbersTest:
                    AddAndMultiplyNumberTest();
                    break;
                case TestsAvailable.JohnMaryTest:
                    JohnMaryTest();
                    break;
                default:
                    break;
            }

            //Gramar initalizing with Rules created above
            Program.Grammar grammar = new Program.Grammar();
            grammar.InitTest(RulesKeyValues, Values);

            //Runing test for given grammar and sentence
            RunTest(grammar, sentence);
        }


        public static void RunTest(Program.Grammar grammar, List<string> sentence)
        {
            //Printing out what the sentence is
            Console.WriteLine(Program.GetStringFromList(sentence));

            //Initializing parser
            Program.EarleyParser parser = new Program.EarleyParser();

            //Printing information if parsing was successfull (sentence was parsed for given grammar or not)
            Console.WriteLine(parser.Parse(sentence, grammar) ? "Parsed successfully +" : "Parsing was unsuccessfull -");

            //Printing chart
            parser.PrintOutChart();
            Program.WaitForPress();
        }
    }
}

