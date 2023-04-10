using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp5
{
    public class Question
    {
        public string Text { get; set; }
        public string Image { get; set; }
        public string Theme { get; set; }
        public List<string> Answers { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public int Level { get; set; }
    }
}
