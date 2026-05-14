namespace UVSimUI;

public partial class AppShell : Shell
{
    private Tab mainTabBar;
    private int tabCount = 1;

    public AppShell()
    {
        InitializeComponent();

        // Create main TabBar with initial tabs
        mainTabBar = new Tab
        {
            Title = "Tab Select",
            Items =
            {                
                new ShellContent
                {
                    Title = "Color Settings",
                    ContentTemplate = new DataTemplate(typeof(SettingsPage))
                },

                new ShellContent
                {
                    Title = "Tab 1",
                    ContentTemplate = new DataTemplate(typeof(MainPage))
                }
            }
        };

        // Add the TabBar to Shell
        Items.Add(mainTabBar);

        // Set the default tab to Tab 1
        ShellContent defaultTab = mainTabBar.Items[1]; // index 1 = second tab
        CurrentItem = defaultTab;
    }

    public void AddNewTab()
    {
        tabCount++;

        var newTab = new ShellContent
        {
            Title = $"Tab {tabCount}",
            Content = new MainPage(new AppTheme())
        };

        mainTabBar.Items.Add(newTab);
    }
}