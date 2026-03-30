using System;
using System.Runtime.InteropServices;
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

		int titlebarColor = 0x232323;
		_ = DwmSetWindowAttribute(new System.Windows.Interop.WindowInteropHelper(this).EnsureHandle(), 35, ref titlebarColor, Marshal.SizeOf(titlebarColor));
	}


	[LibraryImport("dwmapi.dll")]
	private static partial int DwmSetWindowAttribute(IntPtr windowHandle, int attributeID, ref int attributeValue, int attributeSize);
}