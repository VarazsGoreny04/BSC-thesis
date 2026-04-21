using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BullseyeCalculator.WPF.View;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();

		Uri iconUri = new("./Icon/icon.ico", UriKind.Relative);
		Icon = BitmapFrame.Create(iconUri);

		int titlebarColor = 0x232323;
		_ = DwmSetWindowAttribute(new System.Windows.Interop.WindowInteropHelper(this).EnsureHandle(), 35, ref titlebarColor, Marshal.SizeOf(titlebarColor));

		SizeChanged += new SizeChangedEventHandler((_, _) => WindowLoaded(Width, Height));
		StateChanged += new EventHandler(
			(_, _) => 
			{ 
				if (WindowState == WindowState.Maximized)
					WindowLoaded(1000, 1000); 
				else
					WindowLoaded(Width, Height); 
			}
		);
	}


	[LibraryImport("dwmapi.dll")]
	private static partial int DwmSetWindowAttribute(IntPtr windowHandle, int attributeID, ref int attributeValue, int attributeSize);

	private void WindowLoaded(double width, double height)
	{
		double controlSize = Math.Clamp(Math.Min(width, height) / 20, 14, 28);
		Application.Current.Resources.Remove("ControlFontSize");
		Application.Current.Resources.Add("ControlFontSize", controlSize);
	}
}