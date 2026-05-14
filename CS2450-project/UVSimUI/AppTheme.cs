namespace UVSimUI;

public class AppTheme : BindableObject
{
    // Define bindable properties for Primary and Secondary colors
    public static readonly BindableProperty PrimaryColorProperty = BindableProperty.Create(nameof(PrimaryColor), typeof(Color), typeof(AppTheme), new Color(76, 114, 29));

    public static readonly BindableProperty SecondaryColorProperty = BindableProperty.Create(nameof(SecondaryColor), typeof(Color), typeof(AppTheme), new Color(255, 255, 255));

    // Properties to get and set colors dynamically
    public Color PrimaryColor
    {
        get => (Color)GetValue(PrimaryColorProperty);
        set => SetValue(PrimaryColorProperty, value);
    }

    public Color SecondaryColor
    {
        get => (Color)GetValue(SecondaryColorProperty);
        set => SetValue(SecondaryColorProperty, value);
    }

    // Method to update both colors dynamically
    public void SetTheme(Color primary, Color secondary)
    {
        PrimaryColor = primary;
        SecondaryColor = secondary;

        if (Application.Current?.Resources != null)
        {
            Application.Current.Resources["PrimaryColor"] = PrimaryColor;
            Application.Current.Resources["SecondaryColor"] = SecondaryColor;
        }
    }
}