using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LinqCourse
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var firstFilePath = "C:\\Users\\mdm\\Documents\\text1.txt";
            var secondFilePath = "C:\\Users\\mdm\\Documents\\text2.txt";
            // var firstFilePath = args[0];
            // var secondFilePath = args[1];
            int n = 3;

            // if (firstFilePath == null || secondFilePath == null || !int.TryParse(args[2], out n))
            // {
            //     Console.WriteLine("Недостаточно данных либо данные не верны");
            //     return;
            // }

            var firstText = File.ReadAllText(firstFilePath ?? string.Empty);
            var secondText = File.ReadAllText(secondFilePath ?? string.Empty);
            var firstTextGrammas = GetTextGrammas(firstText, n).ToList();
            var secondTextGrammas = GetTextGrammas(secondText, n).ToList();
            
            Console.WriteLine(firstTextGrammas.Count);
            Console.WriteLine(secondTextGrammas.Count);
            Console.WriteLine(GetJaccardIndex(firstTextGrammas, firstText, secondTextGrammas,
                    secondText));
            
            Console.ReadKey();
        }


        public static int GetGrammaFrequency(string text, string gramma)
        {
            var result =   Regex.Split(text, @"[[.]|[!]|[?]|[(]|[)]|[[]|[]]|[}]|[{]]")
                .Select(x =>x.Trim())
                .Where(w=>Regex.IsMatch(w, @"^\p{L}+$"))
                .Count(x => x.ToLower() == gramma.ToLower());
            return result;
        }
        
        public static IEnumerable<string> GetTextGrammas(string text, int n)
        {
            //todo: Настроить разбивку с символом }
            var grammas = Regex.Split(text, @"[[.]|[!]|[?]|[(]|[)]|[[]|[]]|[}]|[{]]");
            
            //todo: С проверкой на letters unicode результат грамм = 0, разобраться, почему
            var result = grammas.Select(x => x.Trim())
                .Where(w=>Regex.IsMatch(w, @"^\p{L}+$") && 
                          Regex.Split(w, @"\W+").Length == n)
                .Distinct()
                .ToList();

            return result;

        }

        public static double GetJaccardIndex(IEnumerable<string> firstGrammaSet, string firstText,
            IEnumerable<string> secondGrammaSet, string secondText)
        {
            var common = 0;
            var total = 0;
            //todo: check for uniqueness
            var grammaSet = firstGrammaSet.Union(secondGrammaSet).ToList();
            
            foreach (var gramma in grammaSet)
            {
                var firstTextFrequency = GetGrammaFrequency(firstText, gramma);
                var secondTextFrequency = GetGrammaFrequency(secondText, gramma);
                
                common += firstTextFrequency < secondTextFrequency ? firstTextFrequency : secondTextFrequency;
                total += firstTextFrequency > secondTextFrequency ? firstTextFrequency : secondTextFrequency;

            }
            
            //todo: что возвращать, если total = 0?
            return total != 0 ? (double)common/total : 0;

        }

    }
}