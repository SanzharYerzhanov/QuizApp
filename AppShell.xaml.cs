using System.Reflection;

namespace MauiApp5;
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(QuizzesPage), typeof(QuizzesPage));
        Routing.RegisterRoute(nameof(ResultPage), typeof(ResultPage));
    }
}
