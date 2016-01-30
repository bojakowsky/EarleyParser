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
            public Dictionary<List<string>, string> RulesKeyValues
                = new Dictionary<List<string>, string>();

            List<string> Values = new List<string>();

            public List<string> GetKeys(string left)
            {
                List<string> right = null;
                if (RulesKeyValues.ContainsValue(left))
                    return RulesKeyValues
                        .Where(x => x.Value == left)
                        .Select(x => x.Key).First();
                return right;
            }

            public bool IsPartOfGrammar(string text)
            {
                return Values.Contains(text);
            }
        }

        public class State
        {
            public string left;
            public List<string> right;
            public int i, j;
        }

        public class EarleyParser
        {
            public void Parse(List<string> words, Grammar grammar)
            {
                List<Chart> charts = new List<Chart>(words.Count());
                State dummyStartState = 
                    new State() { left = "$", right = new List<string>() { "@", "S" }, i = 0, j = 0 };
                charts[0].AddToChart(dummyStartState);

                for (int i = 0; i < words.Count(); ++i)
                {
                    foreach (var state in charts[i].chart)
                    {

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
