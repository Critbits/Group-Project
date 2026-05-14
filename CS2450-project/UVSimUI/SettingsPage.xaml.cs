namespace UVSimUI;

public partial class SettingsPage : ContentPage
{
	private readonly AppTheme _appTheme;
	public SettingsPage(AppTheme appTheme)
	{
		InitializeComponent();
		_appTheme = appTheme;
	}

	// Event handler for primary color input
	private void OnPrimaryColorTextChanged(object sender, TextChangedEventArgs e)
	{
		var colorText = e.NewTextValue;

		// Try to parse the primary color input
		if (Color.TryParse(colorText, out var newColor))
		{
			_appTheme.PrimaryColor = newColor;
			PrimaryColorBox.BackgroundColor = newColor;
		}
		else
		{
			PrimaryColorBox.BackgroundColor = Colors.Transparent;
		}
	}

	// Event handler for secondary color input
	private void OnSecondaryColorTextChanged(object sender, TextChangedEventArgs e)
	{
		var colorText = e.NewTextValue;

		// Try to parse the secondary color input
		if (Color.TryParse(colorText, out var newColor))
		{
			_appTheme.SecondaryColor = newColor;
			SecondaryColorBox.BackgroundColor = newColor;
		}
		else
		{
			SecondaryColorBox.BackgroundColor = Colors.Transparent;
		}
	}

	// Event handler for submit button click
	private void OnSubmitClicked(object sender, EventArgs e)
	{
		// When the user submits, the colors are already updated
		if (Application.Current?.Resources != null)
		{
			Application.Current.Resources["PrimaryColor"] = _appTheme.PrimaryColor;
			Application.Current.Resources["SecondaryColor"] = _appTheme.SecondaryColor;
		}
		DisplayAlert("Success", "Colors have been updated!", "OK");
	}
}
