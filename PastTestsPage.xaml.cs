using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MauiApp5
{
    public partial class PastTestsPage : ContentPage
    {
        public ObservableCollection<PastTest> PastTests { get; set; }
        public PastTestsPage()
        {
            InitializeComponent();
            PastTests = new ObservableCollection<PastTest>();
            MessagingCenter.Subscribe<QuestionViewModel, PastTest>(this, "AddPastTest", (sender, pastTest) =>
            {
                PastTests.Add(pastTest);
            });
            BindingContext = this;
        }
        private void ClearResults_Clicked(object sender, EventArgs e)
        {
            PastTests.Clear();
        }
    }
    public class PastTest
    {
        public string Theme { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }
        public int Percentage { get; set; }
    }
}

