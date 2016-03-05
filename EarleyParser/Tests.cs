using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EarleyParser;
using p = EarleyParserLogic.EarleyLogic;
namespace EarleyParser
{
    public class Test
    {

        //Rules of the Grammar
        Dictionary<List<p.TermsRight>, string> RulesKeyValues;
        List<string> Values;

        public enum TestsAvailable
        {
            AddAndMultiplyNumbersTest,
            JohnMaryTest,
            MaryRunsTest
        }

        private void AddAndMultiplyNumberTest()
        {
            #region Rules section
            var s1 = p.Grammar.ListBuilder("S", "+", "M");
            var s2 = p.Grammar.ListBuilder("M");
            var sRKV1 = p.Grammar.ListOfListsBuilder(s1, s2);
            RulesKeyValues.Add(sRKV1, "S");


            var s3 = p.Grammar.ListBuilder("M", "*", "T");
            var s4 = p.Grammar.ListBuilder("T");
            var sRKV2 = p.Grammar.ListOfListsBuilder(s3, s4);
            RulesKeyValues.Add(sRKV2, "M");

            var s5 = p.Grammar.ListBuilder("Number");
            var sRKV3 = p.Grammar.ListOfListsBuilder(s5);
            RulesKeyValues.Add(sRKV3, "T");

            var s6 = p.Grammar.ListBuilder("1", "2", "3", "4");
            var sRKV4 = p.Grammar.ListOfListsBuilder(s6);
            RulesKeyValues.Add(sRKV4, "Number");

            var s7 = p.Grammar.ListBuilder("+");
            var sRKV5 = p.Grammar.ListOfListsBuilder(s7);
            RulesKeyValues.Add(sRKV5, "+");

            var s8 = p.Grammar.ListBuilder("*");
            var sRKV6 = p.Grammar.ListOfListsBuilder(s8);
            RulesKeyValues.Add(sRKV6, "*");

            Values.Add("+");
            Values.Add("*");
            Values.Add("Number");
            #endregion
        }

        private void MaryRunsTest()
        {
            var s1 = p.Grammar.ListBuilder("NP", "VP");
            var sRKV1 = p.Grammar.ListOfListsBuilder(s1);
            RulesKeyValues.Add(sRKV1, "S");

            var s3 = p.Grammar.ListBuilder("Pronoun");
            var sRKV2 = p.Grammar.ListOfListsBuilder(s3);
            RulesKeyValues.Add(sRKV2, "NP");


            var s4 = p.Grammar.ListBuilder("I");
            var s5 = p.Grammar.ListBuilder("it");
            var sRKV3 = p.Grammar.ListOfListsBuilder(s4, s5);
            RulesKeyValues.Add(sRKV3, "Pronoun");

            var s7 = p.Grammar.ListBuilder("VP", "NP");
            var s120 = p.Grammar.ListBuilder("Verb");
            var sRKV5 = p.Grammar.ListOfListsBuilder(s7, s120);
            RulesKeyValues.Add(sRKV5, "VP");

            var s8 = p.Grammar.ListBuilder("feel");
            var sRKV6 = p.Grammar.ListOfListsBuilder(s8);
            RulesKeyValues.Add(sRKV6, "Verb");

            Values.Add("Verb");
            Values.Add("Pronoun");
            Values.Add("Noun");
        }

        //private void MaryRunsTest()
        //{
        //    var s1 = p.Grammar.ListBuilder("NP", "VP");
        //    var sRKV1 = p.Grammar.ListOfListsBuilder(s1);
        //    RulesKeyValues.Add(sRKV1, "S");

        //    var s3 = p.Grammar.ListBuilder("Noun");
        //    var sRKV2 = p.Grammar.ListOfListsBuilder(s3);
        //    RulesKeyValues.Add(sRKV2, "NP");


        //    var s4 = p.Grammar.ListBuilder("Mary");
        //    var sRKV3 = p.Grammar.ListOfListsBuilder(s4);
        //    RulesKeyValues.Add(sRKV3, "Noun");

        //    var s7 = p.Grammar.ListBuilder("Verb", "Adv");
        //    var sRKV5 = p.Grammar.ListOfListsBuilder(s7);
        //    RulesKeyValues.Add(sRKV5, "VP");

        //    var s8 = p.Grammar.ListBuilder("runs");
        //    var sRKV6 = p.Grammar.ListOfListsBuilder(s8);
        //    RulesKeyValues.Add(sRKV6, "Verb");


        //    var s9 = p.Grammar.ListBuilder("fast");
        //    var sRKV7 = p.Grammar.ListOfListsBuilder(s9);
        //    RulesKeyValues.Add(sRKV7, "Adv");

        //    Values.Add("Verb");
        //    Values.Add("Adv");
        //    Values.Add("Noun");
        //}
        //private void MaryRunsTest()
        //{
        //    var s1 = p.Grammar.ListBuilder("NP", "VP");
        //    var sRKV1 = p.Grammar.ListOfListsBuilder(s1);
        //    RulesKeyValues.Add(sRKV1, "S");

        //    var s2 = p.Grammar.ListBuilder("Det", "Noun");
        //    var s3 = p.Grammar.ListBuilder("Noun");
        //    var sRKV2 = p.Grammar.ListOfListsBuilder(s2, s3);
        //    RulesKeyValues.Add(sRKV2, "NP");


        //    var s4 = p.Grammar.ListBuilder("Mary");
        //    var s5 = p.Grammar.ListBuilder("dog");
        //    var sRKV3 = p.Grammar.ListOfListsBuilder(s4, s5);
        //    RulesKeyValues.Add(sRKV3, "Noun");

        //    var s6 = p.Grammar.ListBuilder("a");
        //    var sRKV4 = p.Grammar.ListOfListsBuilder(s6);
        //    RulesKeyValues.Add(sRKV4, "Det");

        //    var s7 = p.Grammar.ListBuilder("Verb", "NP");
        //    var sRKV5 = p.Grammar.ListOfListsBuilder(s7);
        //    RulesKeyValues.Add(sRKV5, "VP");

        //    var s8 = p.Grammar.ListBuilder("likes");
        //    var sRKV6 = p.Grammar.ListOfListsBuilder(s8);
        //    RulesKeyValues.Add(sRKV6, "Verb");

        //    Values.Add("Verb");
        //    Values.Add("Noun");
        //    Values.Add("Det");
        //}

        private void JohnMaryTest()
        {
            #region Rules section
            //S → NP VP
            var s1 = p.Grammar.ListBuilder("NP", "VP");
            var sRKV1 = p.Grammar.ListOfListsBuilder(s1);
            RulesKeyValues.Add(sRKV1, "S");

            //NP → NP PP
            //NP → Noun
            var s2 = p.Grammar.ListBuilder("NP", "PP");
            var s3 = p.Grammar.ListBuilder("Noun");
            var sRKV2 = p.Grammar.ListOfListsBuilder(s2, s3);
            RulesKeyValues.Add(sRKV2, "NP");

            //VP → Verb NP
            //VP → VP PP
            var s4 = p.Grammar.ListBuilder("Verb", "NP");
            var s5 = p.Grammar.ListBuilder("VP", "PP");
            var sRKV3 = p.Grammar.ListOfListsBuilder(s4, s5);
            RulesKeyValues.Add(sRKV3, "VP");

            //PP → Prep NP
            var s6 = p.Grammar.ListBuilder("Prep", "NP");
            var sRKV4 = p.Grammar.ListOfListsBuilder(s6);
            RulesKeyValues.Add(sRKV4, "PP");

            //Noun → “john”
            //Noun → “mary”
            //Noun → “denver”
            var s7 = p.Grammar.ListBuilder("John");
            var s8 = p.Grammar.ListBuilder("Mary");
            var s9 = p.Grammar.ListBuilder("Denver");
            var sRKV5 = p.Grammar.ListOfListsBuilder(s7, s8, s9);
            RulesKeyValues.Add(sRKV5, "Noun");

            //Verb → “called”
            var s10 = p.Grammar.ListBuilder("called");
            var sRKV6 = p.Grammar.ListOfListsBuilder(s10);
            RulesKeyValues.Add(sRKV6, "Verb");

            //Prep → “from”
            var s11 = p.Grammar.ListBuilder("from");
            var sRKV7 = p.Grammar.ListOfListsBuilder(s11);
            RulesKeyValues.Add(sRKV7, "Prep");

            Values.Add("Prep");
            Values.Add("Verb");
            Values.Add("Noun");
            #endregion
        }
        public void FirstTest(List<string> sentence, TestsAvailable test)
        {
            RulesKeyValues = new Dictionary<List<p.TermsRight>, string>();
            Values = new List<string>();
            switch (test)
            {
                case TestsAvailable.AddAndMultiplyNumbersTest:
                    AddAndMultiplyNumberTest();
                    break;
                case TestsAvailable.JohnMaryTest:
                    JohnMaryTest();
                    break;
                case TestsAvailable.MaryRunsTest:
                    MaryRunsTest();
                    break;
                default:
                    break;
            }

            //Gramar initalizing with Rules created above
            p.Grammar grammar = new p.Grammar();
            grammar.InitTest(RulesKeyValues, Values);

            //Runing test for given grammar and sentence
            RunTest(grammar, sentence);
        }


        public static void RunTest(p.Grammar grammar, List<string> sentence)
        {
            //Printing out what the sentence is
            Console.WriteLine(p.GetStringFromList(sentence));

            //Initializing parser
            p.EarleyParser parser = new p.EarleyParser();

            //Printing information if parsing was successfull (sentence was parsed for given grammar or not)
            Console.WriteLine(parser.Parse(sentence, grammar) ? "Parsed successfully +" : "Parsing was unsuccessfull -");

            //Printing chart
            parser.PrintOutChart();
            EarleyParserLogic.ASTLogic.BuildAbstractSyntaxTree(parser);
            Program.WaitForPress();
        }
    }
}

