using BullseyeCalculator.WPF.View;
using BullseyeCalculator.WPF.ViewModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BullseyeCalculator.WPF;

public partial class App : Application
{
	#region Enums

	private enum MapType : uint
	{
		MAPVK_VK_TO_VSC = 0x0,
		MAPVK_VSC_TO_VK = 0x1,
		MAPVK_VK_TO_CHAR = 0x2,
		MAPVK_VSC_TO_VK_EX = 0x3,
	}

	#endregion

	#region Fields

	private MainWindow mainWindow = null!;
	private CalculatorViewModel viewModel = null!;

	#endregion

	#region Constructors

	public App() => Startup += new StartupEventHandler(AppStartUp);

	#endregion

	#region Private methods

	private void AppStartUp(object? sender, StartupEventArgs e)
	{
		viewModel = new CalculatorViewModel();

		mainWindow = new MainWindow { DataContext = viewModel };
		mainWindow.KeyDown += new KeyEventHandler((_, e) =>
			{
				char input = GetCharFromKey(e.Key);

				if (input is >= '\u0020' and < '\u007F')
				{
					viewModel.PushInputCommand.Execute(input);
					return;
				}

				if (e.Key == Key.D3)
				{
					viewModel.PushInputCommand.Execute('\u005E');
					return;
				}

				(e.Key switch
				{
					Key.Back => viewModel.PopInputCommand,
					Key.Enter => viewModel.EvaluateCommand,
					Key.Delete => viewModel.ClearInputCommand,
					Key.F1 => viewModel.StandardModeCommand,
					Key.F2 => viewModel.EuclideanModeCommand,
					Key.F3 => viewModel.InterpolationModeCommand,
					Key.Tab => viewModel.ShowStepsCommand,
					Key.Escape => viewModel.ShowOptionsCommand,
					_ => null
				})?.Execute(null);
			}
		);
		mainWindow.Show();
	}

	[DllImport("user32.dll")]
	private static extern int ToUnicode(
		uint wVirtKey,
		uint wScanCode,
		byte[] lpKeyState,
		[Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
		StringBuilder pwszBuff,
		int cchBuff,
		uint wFlags);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static partial bool GetKeyboardState(byte[] lpKeyState);

	[DllImport("user32.dll")]
	private static extern uint MapVirtualKey(uint uCode, MapType uMapType);

	private static char GetCharFromKey(Key key)
	{
		int virtualKey = KeyInterop.VirtualKeyFromKey(key);
		byte[] keyboardState = new byte[256];
		GetKeyboardState(keyboardState);

		uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
		StringBuilder stringBuilder = new(2);

		int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);

		return result > 0 ? stringBuilder[0] : '\u0000';
	}

	#endregion
}