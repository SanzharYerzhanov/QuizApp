using System.Collections.ObjectModel;

namespace MauiApp5
{
    public partial class MainPage : ContentPage
    {
        public MainPage(int level)
        {
            InitializeComponent();
            QuestionViewModel questionViewModel = new QuestionViewModel(level);
            questionViewModel.Buttons.Add(Button1);
            questionViewModel.Buttons.Add(Button2);
            questionViewModel.Buttons.Add(Button3);
            questionViewModel.Buttons.Add(Button4);
            BindingContext = questionViewModel;
        }
    }
}

