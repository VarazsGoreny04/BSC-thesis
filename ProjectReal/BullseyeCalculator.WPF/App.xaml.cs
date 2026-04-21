using BullseyeCalculator.WPF.View;
using BullseyeCalculator.WPF.ViewModel;
using System.Windows;

namespace BullseyeCalculator.WPF;

public partial class App : Application
{
	#region Fields

	private MainWindow menu = null!;
	private CalculatorViewModel viewModel = null!;

	#endregion

	#region Constructors

	public App() => Startup += new StartupEventHandler(AppStartUp);

	#endregion

	#region Public methods

	public void AppStartUp(object? sender, StartupEventArgs e)
	{
		viewModel = new CalculatorViewModel();

		menu = new MainWindow { DataContext = viewModel };
		menu.Show();
	}

	#endregion
}