using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MauiApp5
{
    public class QuestionViewModel : INotifyPropertyChanged
    {
        public string Theme { get; set; }
        public string Date { get => DateTime.Now.ToString("g", new CultureInfo("en-US")); }
        public int Level { get; set; }
        public int _currentQuestionIndex = 0;
        public int _totalQuestions = 0;
        public ObservableCollection<Question> _questions;
        public ObservableCollection<Question> Questions;
        public ObservableCollection<Button> Buttons;
        public IDispatcherTimer _timer;
        private int _secondsElapsed;
        private bool _timerRunning;

        public event PropertyChangedEventHandler PropertyChanged;

        public QuestionViewModel(int level)
        {
            _questions = new ObservableCollection<Question>
            {
                new Question()
                {
                     Text = "Найдите асимптоту функции:",
                     Image = "question1.png",
                     Theme = "Асимптоты, лимит функции",
                     Answers = new List<string> { "Paris", "London", "Rome", "Berlin" },
                     CorrectAnswerIndex = 0,
                     Level = 1
                },
                new Question()
                {
                     Text = "What is the currency of Japan?",
                     Theme = "Асимптоты, лимит функции",
                     Answers = new List<string> { "Yen", "Dollar", "Euro", "Pound" },
                     CorrectAnswerIndex = 0,
                     Level = 1
                }
            };
            Questions = new ObservableCollection<Question>(_questions.Where(x => x.Level == level));
            _totalQuestions = Questions.Count;
            Question = Questions[_currentQuestionIndex];
            SubmitCommand = new Command(OnSubmission);
            AnswerSelected = new Command(OnSelection);
            GoToQuestionsCommand = new Command<string>(GoToQuestions);
            Theme = Question.Theme;
            Level = level;
            _timer = Application.Current.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timerRunning = true;
            _timer.Start();
            IsCompleted = false;
            Buttons = new ObservableCollection<Button>();
        }
        private string _timerText;
        public string TimerText
        {
            get { return _timerText; }
            set
            {
                if (_timerText != value)
                {
                    _timerText = value;
                    OnPropertyChanged(nameof(TimerText));
                }
            }
        }
        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
            }
        }
        private string selectedAnswer;
        public string SelectedAnswer
        {
            get { return selectedAnswer; }
            set
            {
                if (selectedAnswer != value)
                {
                    selectedAnswer = value;
                    OnPropertyChanged(nameof(SelectedAnswer));
                }
            }
        }
        private int _score = 0;
        public int Score
        {
            get => _score;
            set
            {
                if (_score != value)
                {
                    _score = value;
                    OnPropertyChanged(nameof(Score));
                }
            }
        }
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        private Question _question;
        public Question Question
        {
            get => _question;
            set
            {
                _question = value;
                OnPropertyChanged();
                Title = "Question " + (_currentQuestionIndex + 1) + "/" + _totalQuestions;
            }
        }
        public ICommand SubmitCommand { get; set; }
        public ICommand AnswerSelected { get; set; }
        public ICommand CompleteCommand { get; set; }
        public ICommand GoToQuestionsCommand { get; set; }
        private Button _previouslySelectedButton;
        private void OnSelection(object sender)
        {
            Button button = sender as Button;
            if (_previouslySelectedButton != null)
            {
                    _previouslySelectedButton.BackgroundColor = Colors.White;
                    _previouslySelectedButton.TextColor = Colors.Black;
            }
            button.BackgroundColor = Color.FromArgb("#512BD4");
            button.TextColor = Colors.White;
            _previouslySelectedButton = button;
            SelectedAnswer = button.Text;
        }
        private int firstClicked = 1;
        private async void OnSubmission(object parameter)
        {
            Button button = parameter as Button;
            if (SelectedAnswer != null && firstClicked % 2 != 0)
            {
                button.Text = "Next";
                if (SelectedAnswer == Question.Answers[Question.CorrectAnswerIndex])
                {
                    Score++;
                    _previouslySelectedButton.BackgroundColor = Colors.Green;
                    foreach(Button answer in Buttons)
                    {
                        if (answer != _previouslySelectedButton)
                        {
                            answer.IsEnabled = false;
                            answer.BackgroundColor = Colors.White;
                        }
                    }
                }
                else
                {
                    _previouslySelectedButton.BackgroundColor = Colors.Red;
                    _previouslySelectedButton.TextColor = Colors.White;
                    foreach(Button answer in Buttons)
                    {
                        if(answer.Text == Question.Answers[Question.CorrectAnswerIndex])
                        {
                            answer.BackgroundColor = Colors.Green;
                            answer.TextColor = Colors.White;
                        }
                        else if (answer != _previouslySelectedButton)
                        {
                            answer.BackgroundColor = Colors.White;
                            answer.TextColor = Colors.Black;
                        }
                        answer.IsEnabled = false;
                    }
                }
                firstClicked++;
            }
            else if(SelectedAnswer != null && firstClicked % 2 == 0)
            {
                button.Text = "Submit";
                _currentQuestionIndex++;
                if (_currentQuestionIndex < _totalQuestions)
                {
                    Question = Questions[_currentQuestionIndex];
                    foreach (Button answer in Buttons)
                    {
                        SelectedAnswer = null;
                        answer.IsEnabled = true;
                        answer.BackgroundColor = Colors.White;
                        answer.TextColor = Colors.Black;
                    }
                }
                else if (_currentQuestionIndex == _totalQuestions)
                {
                    _timerRunning = false;
                    MessagingCenter.Send(this, "AddPastTest", new PastTest
                    {
                        Theme = Theme,
                        Level = Level,
                        Time = TimerText,
                        Score = Score,
                        Date = Date,
                        Percentage = (int)((Score / (double)_totalQuestions) * 100)
                    });
                    await Shell.Current.Navigation.PushAsync(new ResultPage(this));
                    IsCompleted = true;
                }

                firstClicked++;
            }
        }
        private void GoToQuestions(object sender)
        {
            Button button = sender as Button;
            if (button.Text == "next") _currentQuestionIndex++;
            else _currentQuestionIndex--;
            Question = Questions[_currentQuestionIndex];
            OnPropertyChanged(nameof(Question));
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_timerRunning)
            {
                _secondsElapsed++;
                TimeSpan timeElapsed = TimeSpan.FromSeconds(_secondsElapsed);

                // Format timer text to show hours, minutes, and seconds
                if (timeElapsed.TotalHours >= 1)
                {
                    TimerText = $"{(int)timeElapsed.TotalHours}:{timeElapsed:mm\\:ss}";
                }
                else
                {
                    TimerText = timeElapsed.ToString(@"mm\:ss");
                }
            }
            else
            {
                _timer.Stop();
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
