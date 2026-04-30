using BullseyeCalculator.Model;
using BullseyeCalculator.Persistence;
using System;
using System.Collections.ObjectModel;

namespace BullseyeCalculator.WPF.ViewModel;

public class CalculatorViewModel : ViewModelBase
{
	#region Fields

	private readonly CalculatorModel model;

	private bool showSteps;
	private bool showOptions;
	private Panel panel;

	#endregion

	#region Properties

	public string Input => model.Data.Input;
	public ObservableCollection<string> Evaluation => model.Data.Evaluation;
	public string Result => model.Data.Result;
	public bool ShowSteps
	{
		get => showSteps;
		set
		{
			showSteps = value;

			OnPropertyChanged(nameof(ShowSteps));
		}
	}
	public bool ShowOptions
	{
		get => showOptions;
		set
		{
			showOptions = value;

			OnPropertyChanged(nameof(ShowOptions));
		}
	}
	public Mode CurrentMode
	{
		get => model.Data.Mode;
		set
		{
			model.Data.SwitchMode(value);

			OnPropertyChanged(nameof(CurrentMode));
		}
	}
	public Panel CurrentPanel
	{
		get => panel;
		set
		{
			panel = value;

			OnPropertyChanged(nameof(CurrentPanel));
			OnPropertyChanged(nameof(CurrentPanelName));
		}
	}
	public string CurrentPanelName => $"⨍{(int)CurrentPanel}";
	public static char Separator => CalculatorData.Separator;
	public static char ColumnSeparator => CalculatorData.ColumnSeparator;
	public static char RowSeparator => CalculatorData.RowSeparator;
	public bool FractionalFormat
	{
		get => CalculatorData.FractionalFormat;
		set
		{
			CalculatorData.FractionalFormat = value;

			OnPropertyChanged(nameof(FractionalFormat));
		}
	}
	public int FractionCalculationLength
	{
		get => CalculatorData.FractionCalculationLength;
		set
		{
			if (value < 0)
				return;

			CalculatorData.FractionCalculationLength = value;

			OnPropertyChanged(nameof(FractionCalculationLength));
		}
	}

	#endregion

	#region Commands

	public DelegateCommand PushInputCommand { get; }
	public DelegateCommand PopInputCommand { get; }
	public DelegateCommand ClearInputCommand { get; }
	public DelegateCommand EvaluateCommand { get; }

	public DelegateCommand ShowStepsCommand { get; }
	public DelegateCommand ShowOptionsCommand { get; }

	public DelegateCommand CyclePanelCommand { get; }

	public DelegateCommand StandardModeCommand { get; }
	public DelegateCommand EuclideanModeCommand { get; }
	public DelegateCommand InterpolationModeCommand { get; }

	public DelegateCommand IncreaseFractionCalculationLengthCommand { get; }
	public DelegateCommand DecreaseFractionCalculationLengthCommand { get; }

	#endregion

	#region Constructors

	public CalculatorViewModel()
	{
		FractionalFormat = false;
		FractionCalculationLength = 10;

		model = new CalculatorModel();
		model.InputChange += new EventHandler((_, _) => OnPropertyChanged(nameof(Input)));
		model.EvaluationChange += new EventHandler((_, _) => OnPropertyChanged(nameof(Evaluation)));
		model.ResultChange += new EventHandler((_, _) => OnPropertyChanged(nameof(Result)));

		showSteps = false;
		showOptions = false;

		CurrentPanel = Panel.ExponentialFunctions;

		CurrentMode = Mode.Standard;

		PushInputCommand = new DelegateCommand(param => model.PushInput(param?.ToString() ?? throw new FormatException("Input format is invalid!")));
		PopInputCommand = new DelegateCommand(_ => model.PopInput());
		ClearInputCommand = new DelegateCommand(_ => model.ClearInput());
		EvaluateCommand = new DelegateCommand(_ => model.Evaluate());

		ShowStepsCommand = new DelegateCommand(_ => ShowSteps = !showSteps);
		ShowOptionsCommand = new DelegateCommand(_ => ShowOptions = !showOptions);

		CyclePanelCommand = new DelegateCommand(_ => CyclePanel());

		StandardModeCommand = new DelegateCommand(_ => CurrentMode = Mode.Standard);
		EuclideanModeCommand = new DelegateCommand(_ => CurrentMode = Mode.Matrix);
		InterpolationModeCommand = new DelegateCommand(_ => CurrentMode = Mode.Interpolation);

		IncreaseFractionCalculationLengthCommand = new DelegateCommand(_ => ++FractionCalculationLength);
		DecreaseFractionCalculationLengthCommand = new DelegateCommand(_ => --FractionCalculationLength);
	}

	#endregion

	#region Private methods

	private void CyclePanel()
	{
		CurrentPanel = CurrentPanel switch
		{
			Panel.ExponentialFunctions => Panel.StandardFunctions,
			Panel.StandardFunctions => Panel.SpecialFunctions,
			Panel.SpecialFunctions => Panel.ExponentialFunctions,
			_ => throw new NotImplementedException("This panel state is not implemented!")
		};
	}

	#endregion
}