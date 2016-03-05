﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarleyParserLogic
{
    public class EarleyLogic
    {
        public class Chart
        {
            public List<State> chart = new List<State>();


            private static bool AreListItemsTheSame(List<string> list1, List<string> list2)
            {
                if (list1.Count() != list2.Count()) return false;

                for (int i = 0; i < list1.Count(); ++i)
                    if (!list1[i].Equals(list2[i])) return false;

                return true;
            }

            public bool IsChartStateEqual(State state)
            {
                bool contains = false;
                for (int i = 0; i < chart.Count(); i++)
                {
                    if (chart[i].i == state.i &&
                    chart[i].j == state.j &&
                    chart[i].termsLeft == state.termsLeft &&
                    AreListItemsTheSame(chart[i].termsRight.termsRight, state.termsRight.termsRight) &&
                    chart[i].termsRight.dotIndex == state.termsRight.dotIndex)
                    {
                        contains = true;
                        break;
                    }
                }

                return contains;
            }

            public static bool AreChartStatesEqual(State state1, State state2)
            {
                //bool contains = false;
                //if (state1.i == state2.i &&
                //state1.j == state2.j &&
                //state1.termsLeft == state2.termsLeft &&
                //AreListItemsTheSame(state1.termsRight.termsRight, state2.termsRight.termsRight) &&
                //state1.termsRight.dotIndex == state2.termsRight.dotIndex)
                //    contains = true;
                bool contains = state1.i == state2.i &&
                state1.j == state2.j &&
                state1.termsLeft == state2.termsLeft &&
                AreListItemsTheSame(state1.termsRight.termsRight, state2.termsRight.termsRight) &&
                state1.termsRight.dotIndex == state2.termsRight.dotIndex;
                return contains;
            }

            public void AddToChart(State state)
            {
                //if (!chart.Contains(state))
                //if (chart.Count() == 0)

                if (!chart.Any())
                    chart.Add(state);

                if (!IsChartStateEqual(state))
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
                {
                    var rkv = (List<TermsRight>)RulesKeyValues
                    .Where(x => x.Value == left)
                    .SelectMany(y => y.Key).ToList();

                    right = new List<TermsRight>();
                    foreach (var keys in rkv)
                    {
                        List<string> list = new List<string>();
                        list.AddRange(keys.termsRight);
                        right.Add(new TermsRight(list));
                    }
                    return right.ToList();
                }
                return null;
            }

            public static TermsRight ListBuilder(params string[] parameter)
            {
                TermsRight list = new TermsRight(parameter.ToList());
                return list;
            }

            public static List<TermsRight> ListOfListsBuilder(params TermsRight[] parameter)
            {
                List<TermsRight> listOfLists = new List<TermsRight>();
                listOfLists.AddRange(parameter);
                return listOfLists;
            }

            public void InitTest(Dictionary<List<TermsRight>, string> rulesKeyValues, List<string> values)
            {
                RulesKeyValues.Clear();
                Values.Clear();

                RulesKeyValues = rulesKeyValues;
                Values = values;
            }

            public bool IsPartOfGrammar(string text)
            {
                return Values.Contains(text);
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

            public TermsRight AddDotToTheEnd()
            {
                List<string> list = new List<string>();
                list.AddRange(termsRight);
                list.Add(DOT);
                var newKey = new TermsRight(list);

                return newKey;
            }

            public TermsRight AddDotToTheBeginning()
            {
                List<string> list = new List<string>();
                list.AddRange(termsRight);
                list.Insert(0, DOT);
                var newKey = new TermsRight(list);

                return newKey;
            }

            public TermsRight MoveDotRight()
            {
                List<string> list = new List<string>();
                list.AddRange(termsRight);
                int dotIndex = list.FindIndex(x => x.Equals(DOT));
                string bufor = list[dotIndex + 1];
                list[dotIndex + 1] = DOT;
                list[dotIndex] = bufor;

                var newKey = new TermsRight(list);

                return newKey;
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
            public void PrintOutChart()
            {
                for (int i = 0; i < Charts.Count; i++)
                {
                    Console.WriteLine("Chart[" + i + "]");
                    foreach (var chart in Charts[i].chart)
                    {
                        string termsRight = chart.termsRight.termsRight.Aggregate("", (current, term) => current + (" " + term));
                        //foreach (var term in chart.termsRight.termsRight)
                        //{
                        //    termsRight += " " + term;
                        //}

                        Console.WriteLine($"{ chart.termsLeft.PadRight(10, ' ') } \t  { termsRight.PadRight(20, ' ')} \t { chart.i } { chart.j }");
                    }
                }
                Console.WriteLine();
            }

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
                            termsRight = terms.AddDotToTheBeginning(),
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
                //foreach (var terms in Grammar.GetKeys(termsLeft))
                //{
                //    if (state.j < Words.Count())
                //    {
                //        foreach (var term in terms.termsRight)
                //        {
                //            string termLower = term.ToLower();
                //            string sentenceLower = Words[state.j].ToLower();
                //            if (termLower.Equals(sentenceLower))
                //            {
                //                State newState = new State()
                //                {
                //                    termsLeft = termsLeft,
                //                    termsRight = terms.AddDotToTheEnd(),
                //                    i = state.j,
                //                    j = state.j + 1
                //                };
                //                Charts[state.j + 1].AddToChart(newState);
                //            }
                //        }
                //    }
                //}
                var termsLeft = state.termsRight.GetTermAfterDot();
                foreach (State newState in from terms in Grammar.GetKeys(termsLeft)
                                           where state.j < Words.Count()
                                           from term in terms.termsRight
                                           let termLower = term.ToLower() let sentenceLower = Words[state.j].ToLower()
                                           where termLower.Equals(sentenceLower) select new State()
                {
                    termsLeft = termsLeft,
                    termsRight = terms.AddDotToTheEnd(),
                    i = state.j,
                    j = state.j + 1
                })
                {
                    Charts[state.j + 1].AddToChart(newState);
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
                            termsLeft = buforState.termsLeft,
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
                Charts = new List<Chart>(Words.Count() + 1);
                for (int i = 0; i < Words.Count() + 1; i++)
                    Charts.Add(new Chart());

                State dummyStartState =
                    new State()
                    {
                        termsLeft = "$",
                        termsRight = new TermsRight(new List<string>() { DOT, "S" }),
                        i = 0,
                        j = 0
                    };


                Charts[0].AddToChart(dummyStartState);

                for (int i = 0; i < Charts.Count(); ++i)
                {
                    for (int j = 0; j < Charts[i].chart.Count(); j++)
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
                    new State()
                    {
                        termsLeft = "$",
                        termsRight = new TermsRight(new List<string>() { "S", DOT }),
                        i = 0,
                        j = Words.Count()
                    };

                for (int j = 0; j < Charts[Words.Count()].chart.Count(); j++)
                {
                    State state = Charts[Words.Count()].chart[j];

                    if (Chart.AreChartStatesEqual(state, endState))
                        return true;
                    //if (state.Equals(endState))
                    //    return true;
                }

                return false;
            }
        }

        public static string GetStringFromList(List<string> list)
        {
            //foreach (var item in list)
            //    retString += " " + item;
            return list.Aggregate("", (current, item) => current + (" " + item));
        }
    }
}
