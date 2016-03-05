using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using p = EarleyParserLogic.EarleyLogic;
namespace EarleyParserOnline
{
    /// <summary>
    /// Summary description for EarleyWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class EarleyWebService : System.Web.Services.WebService
    {
        Dictionary<List<p.TermsRight>, string> RulesKeyValues;
        List<string> Values;

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

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InitialTest()
        {
            RulesKeyValues = new Dictionary<List<p.TermsRight>, string>();
            Values = new List<string>();
            var sentence = new List<string> { "I", "feel", "it" };
            p.Grammar grammar = new p.Grammar();
            MaryRunsTest();
            grammar.InitTest(RulesKeyValues, Values);
            p.EarleyParser parser = new p.EarleyParser();
            parser.Parse(sentence, grammar);

            var x = new JavaScriptSerializer().Serialize(parser.Charts);
            this.Context.Response.Write(x);
        }

        public class Response
        {
            public string sentence;
            public List<Rule> rules;
        }

        public class Rule
        {
            public string leftSide;
            public string rightSide;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ParseSentence(string sentenceToParse)
        {
            List<Response> response =
                (List<Response>)new JavaScriptSerializer().Deserialize(sentenceToParse, typeof(List<Response>));

            RulesKeyValues = new Dictionary<List<p.TermsRight>, string>();
            Values = new List<string>();

            var sentence = response[0]
                .sentence?.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)?.ToList();

            if (sentence == null || !sentence.Any())
                CustomErrorParseSentence(out response, out sentence);
            p.Grammar grammar = new p.Grammar();

            Dictionary<string, List<string>> grammarHelper = new Dictionary<string, List<string>>();
            foreach (var rule in response[0].rules)
            {
                if (grammarHelper.Any(r => r.Key == rule.leftSide))
                    grammarHelper[rule.leftSide].Add(rule.rightSide);
                else grammarHelper.Add(rule.leftSide, new List<string>() { rule.rightSide });
            }

            foreach (var entity in grammarHelper)
            {
                List<p.TermsRight> terms = new List<p.TermsRight>();
                foreach (var termRight in entity.Value)
                {
                    List<string> termsRight = termRight.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    terms.Add(new p.TermsRight(termsRight));
                    foreach (var t in termsRight)
                        if (t.All(x => char.IsLower(x)) || t.All(x => char.IsNumber(x)))
                            Values.Add(entity.Key);
                }
                RulesKeyValues.Add(p.Grammar.ListOfListsBuilder(terms.ToArray()), entity.Key);
            }

            grammar.InitTest(RulesKeyValues, Values);
            p.EarleyParser parser = new p.EarleyParser();
            parser.Parse(sentence, grammar);
            var JSONresult = new JavaScriptSerializer().Serialize(parser.Charts);
            return JSONresult;
        }

        private void CustomErrorParseSentence(out List<Response> response, out List<string> sentence)
        {
            sentence = new List<string>() { "wrong", "sentence" };
            response = new List<Response>()
            {
                new Response()
                {
                    rules = new List<Rule>()
                    {
                        new Rule()
                        {
                            leftSide = "S",
                            rightSide = "ADJ Noun"
                        },
                        new Rule()
                        {
                            leftSide = "ADJ",
                            rightSide = "wrong"
                        },
                        new Rule()
                        {
                            leftSide="Noun",
                            rightSide = "sentence"
                        }
                    }
                }
            };
        }
    }
}
