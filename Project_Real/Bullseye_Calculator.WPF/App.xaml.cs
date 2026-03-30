using Bullseye_Calculator.WPF.View;
using Bullseye_Calculator.WPF.ViewModel;
using System.Windows;

namespace Bullseye_Calculator.WPF;

public partial class App : Application
{
	private MainWindow menu = null!;

	//private Calculator model;
	private CalculatorViewModel viewModel = null!;
	//private DispatcherTimer _timer = null!;

	public App() => Startup += new StartupEventHandler(AppStartUp);

	public void AppStartUp(object? sender, StartupEventArgs e)
	{
		/*_model.Moving += new EventHandler<SnakeFieldEventArgs>(Moving);
		_model.EndGame += new EventHandler<SnakeEventArgs>(ScoreAdvanced);*/

		viewModel = new CalculatorViewModel();
		/*_viewModel.NewGame += new EventHandler(NewGame);
		_viewModel.Resume += new EventHandler(Resume);
		_viewModel.Pause += new EventHandler(Pause);*/

		menu = new MainWindow { DataContext = viewModel };
		//_menu.Closing += new CancelEventHandler(Closing);
		menu.Show();

		/*_game = new GameWindow { DataContext = _viewModel };
		_game.Closing += new CancelEventHandler(Closing);
		_game.KeyDown += InputConverter;


		_timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.4d) };
		_timer.Tick += new EventHandler(OneStep);*/
	}
}