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
			model.SwitchMode(value);

			OnPropertyChanged(nameof(CurrentMode));
		}
	}
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

	public DelegateCommand StandardModeCommand { get; }
	public DelegateCommand EuclideanModeCommand { get; }
	public DelegateCommand InterpolationModeCommand { get; }

	public DelegateCommand IncreaseFractionCalculationLengthCommand { get; }
	public DelegateCommand DecreaseFractionCalculationLengthCommand { get; }

	#endregion

	#region Events

	public event EventHandler<ParamEventArgs>? PushInputEvent;
	public event EventHandler? PopInputEvent;
	public event EventHandler? ClearInputEvent;
	public event EventHandler? EvaluateEvent;

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

		CurrentMode = Mode.Standard;

		PushInputEvent += new EventHandler<ParamEventArgs>(
			(_, e) => model.PushInput(e.Param?.ToString() ?? throw new FormatException("Input format is invalid!"))
		);
		PopInputEvent += new EventHandler((_, _) => model.PopInput());
		ClearInputEvent += new EventHandler((_, _) => model.ClearInput());
		EvaluateEvent += new EventHandler((_, _) => model.Evaluate());

		PushInputCommand = new DelegateCommand(param => PushInputEvent?.Invoke(this, new ParamEventArgs(param)));
		PopInputCommand = new DelegateCommand(_ => PopInputEvent?.Invoke(this, EventArgs.Empty));
		ClearInputCommand = new DelegateCommand(_ => ClearInputEvent?.Invoke(this, EventArgs.Empty));
		EvaluateCommand = new DelegateCommand(_ => EvaluateEvent?.Invoke(this, EventArgs.Empty));

		ShowStepsCommand = new DelegateCommand(_ => ShowSteps = !showSteps);
		ShowOptionsCommand = new DelegateCommand(_ => ShowOptions = !showOptions);

		StandardModeCommand = new DelegateCommand(_ => CurrentMode = Mode.Standard);
		EuclideanModeCommand = new DelegateCommand(_ => CurrentMode = Mode.Matrix);
		InterpolationModeCommand = new DelegateCommand(_ => CurrentMode = Mode.Interpolation);

		IncreaseFractionCalculationLengthCommand = new DelegateCommand(_ => ++FractionCalculationLength);
		DecreaseFractionCalculationLengthCommand = new DelegateCommand(_ => --FractionCalculationLength);
	}

	#endregion
}