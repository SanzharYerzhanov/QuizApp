using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp5
{
	public partial class QuizzesPage : ContentPage
	{
		public ObservableCollection<Quiz> Quizzes { get; set; }
		public QuizzesPage()
		{
			InitializeComponent();
			QuestionViewModel questionViewModel = new QuestionViewModel(1);
			Quizzes = new ObservableCollection<Quiz>()
			{
				new Quiz()
				{
					Theme = questionViewModel.Theme,
					Complexity = questionViewModel.Level,
					TotalQuestions = questionViewModel._totalQuestions,
					IsClosed = questionViewModel.Level != 1
				}
			};
			BindingContext = this;
		}

        private async void QuizSelected(object sender, EventArgs e)
        {
			Quiz selectedQuiz = (Quiz)((Frame)sender).BindingContext;
			await Shell.Current.Navigation.PushAsync(new MainPage(selectedQuiz.Complexity));
        }
    }
    public class Quiz
	{
		public string Theme { get; set; }
		public int Complexity { get; set; }
		public int TotalQuestions { get; set; }
		public bool IsClosed { get; set; }
	}
}