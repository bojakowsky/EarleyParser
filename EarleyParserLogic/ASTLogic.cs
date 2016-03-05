using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ExtensionMethods.MyExtensions;
using earley = EarleyParserLogic.EarleyLogic;
namespace EarleyParserLogic
{
    public class ASTLogic
    {
        public static void BuildAbstractSyntaxTree(earley.EarleyParser parser)
        {
            List<string> child = new List<string>();
            List<List<string>> tree = new List<List<string>>();


            parser.Charts.Reverse();
            foreach (var chart in parser.Charts)
                chart.chart.Reverse();

            parser.Words.Reverse();
            foreach (var word in parser.Words)
            {
                hasSChanged = false;
                collapse = false;
                BuildSyntaxNode(parser, word, child);
                tree.Add(child);
                child = new List<string>();

            }

            foreach (var ch in tree)
            {
                string line = "";
                foreach (var node in ch.AsEnumerable().Reverse())
                {
                    line += "\t" + node;
                }
                Console.WriteLine(line);
            }
        }

        public static bool hasSChanged;
        public static bool collapse;
        public static void BuildSyntaxNode(earley.EarleyParser parser, string s, List<string> tree)
        {
            foreach (var charts in parser.Charts)
            {

                MatchEnum? match = null;
                foreach (var chart in charts.chart)
                {
                    if (hasSChanged)
                    {
                        match = charts.chart.MatchLevelInRightTermsOnStateList(s);
                        if (match == MatchEnum.None) break;

                        if (match == MatchEnum.Perfect)
                        {
                            if (chart.termsRight.termsRight.ContainsPerfectMatchAfterDot(s))
                                BuildChartTree(chart, ref tree, ref s, match);
                        }
                        else if (match == MatchEnum.Good)
                        {
                            if (chart.termsRight.termsRight.ContainsAfterDot(s))
                                BuildChartTree(chart, ref tree, ref s, match);
                        }

                        if (collapse) return;
                    }
                    else
                    {
                        if (chart.termsRight.termsRight.Contains(s))
                        {
                            BuildChartTree(chart, ref tree, ref s);
                            hasSChanged = true;
                        }
                    }

                }
            }
        }

        private static void BuildChartTree(earley.State chart, ref List<string> tree, ref string s, MatchEnum? match = null)
        {
            if (hasSChanged)
            {
                foreach (var term in chart.termsRight.termsRight.StringListAfterDot())
                    if (term != earley.DOT && term == s)
                        tree.Add(term);
            }
            else
            {
                foreach (var term in chart.termsRight.termsRight)
                    if (term != earley.DOT && term == s)
                        tree.Add(term);
            }

            if (chart.termsLeft == "S")
            {
                tree.Add(chart.termsLeft);
                collapse = true;
            }
            else
                s = chart.termsLeft;
        }
    }
}

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static bool ContainsAfterDot(this List<string> strList, string match)
        {
            int dotIndex = strList.FindIndex(x => x.Equals(earley.DOT));
            dotIndex++;

            for (int i = dotIndex; i < strList.Count(); i++)
                if (strList[i].Equals(match)) return true;

            return false;
        }

        public static bool ContainsPerfectMatchAfterDot(this List<string> strList, string match)
        {
            int dotIndex = strList.FindIndex(x => x.Equals(earley.DOT));
            dotIndex++;

            if ((strList.Count()) - dotIndex == 1)
                for (int i = dotIndex; i < strList.Count(); i++)
                    if (strList[i].Equals(match)) return true;

            return false;
        }

        public static List<string> StringListAfterDot(this List<string> strList)
        {
            int dotIndex = strList.FindIndex(x => x.Equals(earley.DOT));
            int numberOfElementsAfterDot = (strList.Count() - 1) - dotIndex;
            return strList.GetRange(dotIndex + 1, numberOfElementsAfterDot);
        }

        public enum MatchEnum
        {
            Perfect,
            Good,
            None

        }

        public static MatchEnum MatchLevelInRightTermsOnStateList(this List<earley.State> stateList, string match)
        {
            bool goodMatch = false;
            bool perfectMatch = false;

            for (int i = 0; i < stateList.Count(); i++)
            {
                if (!goodMatch)
                    goodMatch = stateList[i].termsRight.termsRight.ContainsAfterDot(match);

                perfectMatch = stateList[i].termsRight.termsRight.ContainsPerfectMatchAfterDot(match);
                if (perfectMatch) return MatchEnum.Perfect;
            }
            if (goodMatch) return MatchEnum.Good;
            else return MatchEnum.None;
        }
    }
}
