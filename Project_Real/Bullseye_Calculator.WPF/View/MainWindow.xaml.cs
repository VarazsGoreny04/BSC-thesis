using System.Windows;
using System.Windows.Media.Imaging;

namespace Bullseye_Calculator.WPF.View;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();

		Uri iconUri = new("./Icon/icon.ico", UriKind.Relative);
		Icon = BitmapFrame.Create(iconUri);
	}
}