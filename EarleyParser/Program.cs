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
            public Dictionary<List<List<string>>, string> RulesKeyValues
                = new Dictionary<List<List<string>>, string>();

            public List<string> Values = new List<string>();

            public List<List<string>> GetKeys(string left)
            {
                List<List<string>> right = null;
                if (RulesKeyValues.ContainsValue(left))
                    return (List<List<string>>)RulesKeyValues
                        .Where(x => x.Value == left)
                        .Select(y=>y.Key.FirstOrDefault());
                return right;
            }

            public bool IsPartOfGrammar(string text)
            {
                return Values.Contains(text);
            }
        }

        public const string DOT = "@";
        public class State
        {
            int dotIndex = -1;

            public string termsLeft;
            public List<string> termsRight;
            public int i, j;

            public string GetTermAfterDot()
            {
                if (dotIndex != -1 && dotIndex < termsRight.Count() - 1)
                    return termsRight[dotIndex];
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
        }

        public class EarleyParser
        {
            public void Predictor(State state)
            {
                var termsLeft = state.GetTermAfterDot();
                foreach (var term in Grammar.GetKeys(termsLeft))
                {
                    Charts[state.j]
                        .AddToChart(new State()
                        {
                            termsLeft = termsLeft,
                            termsRight = term,
                            i = state.j,
                            j = state.j
                        });
                }
            }

            public List<string> Words;
            public Grammar Grammar;
            public List<Chart> Charts;

            public void Parse(List<string> words, Grammar grammar)
            {
                Grammar = grammar;
                this.Words = words;
                Charts = new List<Chart>(Words.Count());
                State dummyStartState = 
                    new State() { termsLeft = "$", termsRight = new List<string>() { DOT, "S" }, i = 0, j = 0 };
                Charts[0].AddToChart(dummyStartState);

                for (int i = 0; i < Words.Count(); ++i)
                {
                    foreach (var state in Charts[i].chart)
                    {
                        var termAfterDot = state.GetTermAfterDot();
                        if (!string.IsNullOrEmpty(termAfterDot) && !state.IsTermPartOfSpeech(termAfterDot, Grammar))
                            Predictor(state);
                        else if (!string.IsNullOrEmpty(termAfterDot) && state.IsTermPartOfSpeech(termAfterDot, Grammar))
                            throw new NotImplementedException();
                        //scanner(state)
                        else
                            throw new NotImplementedException();
                        //completer(state)
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            var input = new List<string> { "(", "(", ")", ")" };


        }
    }
}
