using Bullseye_Calculator.View;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace Bullseye_Calculator;

public partial class App : Application
{
	private MainWindow _menu = null!;
	private DispatcherTimer _timer = null!;

	public App()
	{
		Startup += new StartupEventHandler(AppStartup);
	}

	public void AppStartup(object? sender, StartupEventArgs e)
	{
		/*_model = new SnakeGameModel(new SnakeFileDataAccess());
		_model.Moving += new EventHandler<SnakeFieldEventArgs>(Moving);
		_model.EndGame += new EventHandler<SnakeEventArgs>(ScoreAdvanced);

		_viewModel = new SnakeViewModel(_model);
		_viewModel.NewGame += new EventHandler(NewGame);
		_viewModel.Resume += new EventHandler(Resume);
		_viewModel.Pause += new EventHandler(Pause);*/

		_menu = new MainWindow { /*DataContext = _viewModel*/ };
		//_menu.Closing += new CancelEventHandler(Closing);
		_menu.Show();

		/*_game = new GameWindow { DataContext = _viewModel };
		_game.Closing += new CancelEventHandler(Closing);
		_game.KeyDown += InputConverter;


		_timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.4d) };
		_timer.Tick += new EventHandler(OneStep);*/
	}
}