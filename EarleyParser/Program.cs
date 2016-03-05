using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarleyParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            Test test = new Test();
            List<string> sentence = null;
            #region Numeric tests
            //Test that should Pass
            sentence = new List<string> { "2", "+", "3", "*", "4" };
            test.FirstTest(sentence, Test.TestsAvailable.AddAndMultiplyNumbersTest);

            //Test that should Pass
            sentence = new List<string> { "2", "*", "3", "*", "4", "+", "1", "+", "2", "*", "3" };
            test.FirstTest(sentence, Test.TestsAvailable.AddAndMultiplyNumbersTest);

            //Test that shouldn't Pass
            sentence = new List<string> { "2", "3", "*", "4" };
            test.FirstTest(sentence, Test.TestsAvailable.AddAndMultiplyNumbersTest);

            //Test that shouldn't Pass
            sentence = new List<string> { "2", "+", "3", "*", "4", "*", "2", "+", "3", "*", "4", "*", "+" };
            test.FirstTest(sentence, Test.TestsAvailable.AddAndMultiplyNumbersTest);

            //Test that shouldn't Pass
            sentence = new List<string> { "2", "+", "3", "*", "4", "*", "2", "+", "3", "*", "4", "*", "+" };
            test.FirstTest(sentence, Test.TestsAvailable.JohnMaryTest);
            #endregion

            #region Sentence tests
            //Test that should Pass
            sentence = new List<string> { "John", "called", "Mary" };
            test.FirstTest(sentence, Test.TestsAvailable.JohnMaryTest);

            //Test that should Pass
            sentence = new List<string> { "John", "called", "Mary", "from", "Denver" };
            test.FirstTest(sentence, Test.TestsAvailable.JohnMaryTest);

            //Test that shouldn't Pass
            sentence = new List<string> { "John", "Denver", "Mary" };
            test.FirstTest(sentence, Test.TestsAvailable.JohnMaryTest);

            //Test that shouldn't Pass
            sentence = new List<string> { "John", "from", "Mary" };
            test.FirstTest(sentence, Test.TestsAvailable.JohnMaryTest);
            #endregion

            sentence = new List<string> { "Mary", "likes", "a", "dog" };
            test.FirstTest(sentence, Test.TestsAvailable.MaryRunsTest);

            sentence = new List<string> { "Mary", "runs", "fast" };
            test.FirstTest(sentence, Test.TestsAvailable.MaryRunsTest);
            
            sentence = new List<string> { "I", "feel", "it" };
            test.FirstTest(sentence, Test.TestsAvailable.MaryRunsTest);

        }

        public static void WaitForPress()
        {
            Console.WriteLine("Press any key to run next test.");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
