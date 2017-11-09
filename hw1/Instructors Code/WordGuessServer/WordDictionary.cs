using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

namespace WordGuessServer
{
    public static class WordDictionary
    {
        private static readonly Dictionary<string, string> WordSet = new Dictionary<string, string>(15000);
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        public static void Load()
        {
            WordSet.Clear();
            var thisExe = Assembly.GetExecutingAssembly();
            var stream = thisExe.GetManifestResourceStream("WordGuessServer.WordList.txt");

            if (stream == null) return;

            var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                string record = reader.ReadLine();
                var fields = record?.Split(',');

                if (fields == null ||
                    fields.Length!=2 ||
                    string.IsNullOrEmpty(fields[0]) ||
                    string.IsNullOrEmpty(fields[1])) continue;

                try
                {
                    if (!string.IsNullOrWhiteSpace(fields[0]) &&
                        !string.IsNullOrWhiteSpace(fields[1]) &&
                        !WordSet.ContainsKey(fields[0]))
                    {
                        WordSet.Add(fields[0], fields[1]);
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        public static string GetRandomWord()
        {
            var index = Random.Next(0, WordSet.Count);
            var result = WordSet.ElementAt(index).Key;
            return result;
        }

        public static string GetDefinition(string word)
        {
            string result = "";
            try { result = WordSet[word]; }
            catch
            {
                // ignored
            }
            return result;
        }
    }
}
