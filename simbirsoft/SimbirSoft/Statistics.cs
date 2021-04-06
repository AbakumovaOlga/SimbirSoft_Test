using System;
using System.Collections.Generic;
using System.Text;

namespace SimbirSoft
{
    public class Statistics
    {
        string Word;
        int Count;

        public Statistics(string word)
        {
            this.Word = word;
            this.Count = 1;
        }

        public void AddCount()
        {
            Count++;
        }

        public int GetCount()
        {
            return Count;
        }

        public string GetWord()
        {
            return Word;
        }

        public string Print()
        {
            return Word + " " + Count;
        }
    }
}
