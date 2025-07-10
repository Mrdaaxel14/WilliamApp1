namespace WilliamApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes explicitly
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

            // Set LoginPage as the initial route
            GoToAsync("//LoginPage");
        }
    }
}
