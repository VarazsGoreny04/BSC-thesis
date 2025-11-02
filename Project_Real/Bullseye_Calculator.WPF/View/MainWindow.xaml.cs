using System.Windows;
using System.Windows.Media.Imaging;

namespace Bullseye_Calculator.WPF.View;

public partial class MainWindow : Window
{
	public static readonly DependencyProperty GreetingProperty = DependencyProperty.Register(nameof(Greeting), typeof(string), typeof(MainWindow));

	public string Greeting
	{
		get => (string)GetValue(GreetingProperty);
		set => SetValue(GreetingProperty, value);
	}

	public MainWindow()
	{
		InitializeComponent();

		Uri iconUri = new("./Icon/icon.ico", UriKind.Relative);
		Icon = BitmapFrame.Create(iconUri);
	}
}