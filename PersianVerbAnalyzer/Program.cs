using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DependencyBasedSentenceAnalyzer;
using System.IO;

namespace PersianVerbAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            var analyzer = new SentenceAnalyzer("../../../Data/VerbList.txt");
            var sentence =
                "من دارم به شما می‌گویم که این صحبت‌ها به راحتی گفته نخواهد شد و من با شما صحبت زیاد خواهم کرد.";

            var result = SentenceAnalyzer.MakeVerbBasedSentence(sentence);
            var output = new StringBuilder();
            foreach (var dependencyBasedToken in result.SentenceTokens)
            {
                output.AppendLine(dependencyBasedToken.WordForm + "\t" + dependencyBasedToken.Lemma + "\t" +
                                  dependencyBasedToken.CPOSTag
                                  + "\t" + dependencyBasedToken.HeadNumber + "\t" +
                                  dependencyBasedToken.DependencyRelation);
            }
            File.WriteAllText("../../../testOutPut.txt",output.ToString());

            // test speed
            for (int i = 0; i < 10000; i++)
            {
                result = SentenceAnalyzer.MakeVerbBasedSentence(sentence);
                Console.WriteLine(i);
            }
            

            Console.ReadLine();
        }
    }
}
