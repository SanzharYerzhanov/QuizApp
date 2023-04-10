using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp5
{
	public partial class ResultPage : ContentPage
	{
		public string Score { get; set; }
		public string Timer { get; set; }
		public string Accuracy { get; set; }
		public string Theme { get; set; }
		private readonly int score = 0;
		private readonly int totalQuestions = 0;
		private readonly int percentage = 0;
		public ResultPage(QuestionViewModel questionViewModel)
		{
			InitializeComponent();
			score = questionViewModel.Score;
			totalQuestions = questionViewModel._totalQuestions;
            percentage = (int)((score / (double)totalQuestions) * 100);
            Score = score + "/" + totalQuestions;
			Accuracy = percentage + "%";
			Timer = questionViewModel.TimerText;
			Theme = questionViewModel.Theme;
			BindingContext = this;
			if (percentage > 50) label.Text = "You can go to the next level";
			else
			{
                label.Text = "You can't go to the next level";
				button.Text = "Restart";
            }
        }
		private void NextClicked(object sender, EventArgs e)
		{
            
        }
    }
}