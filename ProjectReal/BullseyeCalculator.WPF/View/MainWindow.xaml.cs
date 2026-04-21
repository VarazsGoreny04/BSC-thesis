using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace BullseyeCalculator.WPF.View;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();

		Uri iconUri = new("./Icon/icon.ico", UriKind.Relative);
		Icon = BitmapFrame.Create(iconUri);

		string titleBarColorString = Application.Current.TryFindResource("BackgroundBrush").ToString() ?? string.Empty;
		int titleBarColor = titleBarColorString.Length > 3 ? Convert.ToInt32(titleBarColorString[3..], 16) : 0;

		DwmSetWindowAttribute(new WindowInteropHelper(this).EnsureHandle(), 35, ref titleBarColor, Marshal.SizeOf(titleBarColor));

		SizeChanged += new SizeChangedEventHandler((_, _) => CalculateFontSize(ActualWidth, ActualHeight));
	}

	[LibraryImport("dwmapi.dll")]
	private static partial int DwmSetWindowAttribute(IntPtr windowHandle, int attributeID, ref int attributeValue, int attributeSize);

	private static void CalculateFontSize(double width, double height)
	{
		Application.Current.Resources["ControlFontSize"] = Math.Clamp(Math.Min(width, height) / 20, 14, 28);
	}
}