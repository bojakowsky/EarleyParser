using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarleyParser
{
    class Program
    {
        public class Chart
        {
            public List<State> chart = new List<State>();

            public void AddToChart(State state)
            {
                if (!chart.Contains(state))
                    chart.Add(state);
            }
        }
        public class Grammar
        {
            public Dictionary<List<TermsRight>, string> RulesKeyValues
                = new Dictionary<List<TermsRight>, string>();

            public List<string> Values = new List<string>();

            public List<TermsRight> GetKeys(string left)
            {
                List<TermsRight> right = null;
                if (RulesKeyValues.ContainsValue(left))
                    return (List<TermsRight>)RulesKeyValues
                        .Where(x => x.Value == left)
                        .SelectMany(y => y.Key).ToList();
                return right;
            }

            public TermsRight ListBuilder(params string[] parameter)
            {
                TermsRight list = new TermsRight(parameter.ToList());
                return list;
            }

            public List<TermsRight> ListOfListsBuilder(params TermsRight[] parameter)
            {
                List<TermsRight> listOfLists = new List<TermsRight>();
                listOfLists.AddRange(parameter);
                return listOfLists;
            }

            public void InitTest()
            {
                var s1 = ListBuilder("S", "+", "M");
                var s2 = ListBuilder("M");
                var sRKV1 = ListOfListsBuilder(s1, s2);
                RulesKeyValues.Add(sRKV1, "S");
                

                var s3 = ListBuilder("M", "*", "T");
                var s4 = ListBuilder("T");
                var sRKV2 = ListOfListsBuilder(s3, s4);
                RulesKeyValues.Add(sRKV2, "M");

                var s5 = ListBuilder("Number");
                var sRKV3 = ListOfListsBuilder(s5);
                RulesKeyValues.Add(sRKV3, "T");

                var s6 = ListBuilder("1", "2", "3", "4");
                var sRKV4 = ListOfListsBuilder(s6);
                RulesKeyValues.Add(sRKV4, "Number");

                var s7 = ListBuilder("+");
                var sRKV5 = ListOfListsBuilder(s7);
                RulesKeyValues.Add(sRKV5, "+");

                var s8 = ListBuilder("*");
                var sRKV6 = ListOfListsBuilder(s8);
                RulesKeyValues.Add(sRKV6, "*");

                Values.Add("+");
                Values.Add("*");
                Values.Add("Number");
            }

            public bool IsPartOfGrammar(string text)
            {
                return Values.Contains(text);
            }

            public TermsRight AddDotToTheEnd(TermsRight keys)
            {
                keys.termsRight.Add(DOT);
                return new TermsRight(keys.termsRight);
            }

            public TermsRight AddDotToTheBeginning(TermsRight keys)
            {
                keys.termsRight.Insert(0, DOT);
                return new TermsRight(keys.termsRight);
            }
        }

        public const string DOT = "@";

        public class TermsRight
        {
            public int dotIndex = -1;
            public List<string> termsRight;

            public TermsRight(List<string> terms)
            {
                termsRight = terms;
                dotIndex = termsRight.IndexOf(DOT);
            }

            public string GetTermAfterDot()
            {
                if (dotIndex != -1 && dotIndex < termsRight.Count() - 1)
                    return termsRight[dotIndex + 1];
                return String.Empty;
            }

            public bool IsDotIsAtTheEnd()
            {
                if (termsRight[termsRight.Count() - 1] == DOT)
                    return true;
                return false;
            }

            public bool IsTermPartOfSpeech(string term, Grammar grammar)
            {
                if (grammar.Values.Contains(term))
                    return true;
                return false;
            }

            public TermsRight MoveDotRight()
            {
                int dotIndex = termsRight.FindIndex(x => x.Equals(DOT));
                string bufor = termsRight[dotIndex + 1];
                termsRight[dotIndex + 1] = DOT;
                termsRight[dotIndex] = bufor;
                return new TermsRight(termsRight);
            }
        }

        public class State
        {
            public string termsLeft;
            public TermsRight termsRight;
            public int i, j;
        }

        public class EarleyParser
        {
            public void Predictor(State state)
            {
                var termsLeft = state.termsRight.GetTermAfterDot();
                var Keys = Grammar.GetKeys(termsLeft);

                if (Keys != null)
                {
                    foreach (var terms in Keys)
                    {
                        var newState = new State()
                        {
                            termsLeft = termsLeft,
                            termsRight = Grammar.AddDotToTheBeginning(terms),
                            i = state.j,
                            j = state.j
                        };

                        Charts[state.j]
                            .AddToChart(newState);
                    }
                }
            }

            public void Scanner(State state)
            {
                var termsLeft = state.termsRight.GetTermAfterDot();
                foreach (var terms in Grammar.GetKeys(termsLeft))
                {
                    if (state.j < Words.Count())
                    {
                        foreach (var term in terms.termsRight)
                        {
                            string termLower = term.ToLower();
                            string sentenceLower = Words[state.j].ToLower();
                            if (termLower.Equals(sentenceLower))
                            {
                                State newState = new State()
                                {
                                    termsLeft = termsLeft,
                                    termsRight = Grammar.AddDotToTheEnd(terms),
                                    i = state.j,
                                    j = state.j + 1
                                };
                                Charts[state.j + 1].AddToChart(newState);
                            }
                        }
                    }
                }
            }

            public void Completer(State state)
            {
                string left = state.termsLeft;

                for (int i = 0; i < Charts[state.i].chart.Count(); i++)
                {
                    State buforState = Charts[state.i].chart[i];
                    string right = buforState.termsRight.GetTermAfterDot();

                    if (right != string.Empty && left.Equals(right))
                    {
                        State newState = new State()
                        {
                            termsLeft = left,
                            termsRight = buforState.termsRight.MoveDotRight(),
                            i = buforState.i,
                            j = state.j
                        };


                        Charts[state.j].AddToChart(newState);
                    }
                }

            }

            public List<string> Words;
            public Grammar Grammar;
            public List<Chart> Charts;

            public bool Parse(List<string> words, Grammar grammar)
            {
                Grammar = grammar;
                this.Words = words;
                Charts = new List<Chart>(Words.Count());
                for (int i = 0; i < words.Count(); i++)
                    Charts.Add(new Chart());

                State dummyStartState = 
                    new State() {
                        termsLeft = "$",
                        termsRight = new TermsRight(new List<string>() { DOT, "S" }),
                        i = 0, j = 0 };
                

                Charts[0].AddToChart(dummyStartState);

                for (int i = 0; i < Words.Count(); ++i)
                {
                    for (int j = 0 ; j < Charts[i].chart.Count(); j++)
                    {
                        var state = Charts[i].chart[j];
                   
                        var termAfterDot = state.termsRight.GetTermAfterDot();
                        if (!string.IsNullOrEmpty(termAfterDot) && !state.termsRight.IsTermPartOfSpeech(termAfterDot, Grammar))
                            Predictor(state);
                        else if (!string.IsNullOrEmpty(termAfterDot) && state.termsRight.IsTermPartOfSpeech(termAfterDot, Grammar))
                            Scanner(state);
                        else
                            Completer(state);
                    }
                }

                State endState =
                    new State() {
                        termsLeft = "$",
                        termsRight = new TermsRight(new List<string>() { "S", DOT }),
                        i = 0, j = 0 };

                for (int j = 0; j < Charts[Words.Count()].chart.Count(); j++)
                {
                    State state = Charts[Words.Count()].chart[j];

                    if (state.Equals(endState))
                        return true;
                }

                return false;
            }
        }

        static void Main(string[] args)
        {
            List<string> sentence = new List<string>{ "2", "+", "3", "*", "4" };
            Grammar grammar = new Grammar();
            grammar.InitTest();

            EarleyParser parser = new EarleyParser();
            Console.WriteLine(parser.Parse(sentence, grammar) ? "Y" : "N");
        }
    }
}
