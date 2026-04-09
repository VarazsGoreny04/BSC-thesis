using BullseyeCalculator.Model;
using BullseyeCalculator.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BullseyeCalculator.WPF.ViewModel;

public class CalculatorViewModel : ViewModelBase
{
	#region Fields

	private readonly CalculatorModel model;

	private string result;
	private readonly ObservableCollection<string> evaluation;

	private bool showSteps;
	private bool showOptions;

	#endregion

	#region Properties

	public string Input => model.Data.Input;
	public string Result
	{
		get => result;
		set
		{
			result = value;

			OnPropertyChanged(nameof(Result));
		}
	}
	public ObservableCollection<string> Evaluation => evaluation;
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

	public DelegateCommand InputCommand { get; }
	public DelegateCommand BackSpaceCommand { get; }
	public DelegateCommand ClearCommand { get; }
	public DelegateCommand EvaluateCommand { get; }

	public DelegateCommand ShowStepsCommand { get; }
	public DelegateCommand ShowOptionsCommand { get; }

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

		showSteps = false;
		showOptions = false;

		CurrentMode = Mode.Standard;
		result = string.Empty;
		evaluation = [];

		InputCommand = new DelegateCommand(param => PushInput(param?.ToString() ?? throw new FormatException()));
		BackSpaceCommand = new DelegateCommand(_ => PopInput());
		ClearCommand = new DelegateCommand(_ => ClearInput());
		EvaluateCommand = new DelegateCommand(_ => CalculateByInput());

		ShowStepsCommand = new DelegateCommand(_ => ShowSteps = !showSteps);
		ShowOptionsCommand = new DelegateCommand(_ => ShowOptions = !showOptions);

		StandardModeCommand = new DelegateCommand(_ => CurrentMode = Mode.Standard);
		EuclideanModeCommand = new DelegateCommand(_ => CurrentMode = Mode.Matrix);
		InterpolationModeCommand = new DelegateCommand(_ => CurrentMode = Mode.Interpolation);

		IncreaseFractionCalculationLengthCommand = new DelegateCommand(_ => ++FractionCalculationLength);
		DecreaseFractionCalculationLengthCommand = new DelegateCommand(_ => --FractionCalculationLength);
	}

	#endregion

	#region Private methods

	private void PushInput(string text)
	{
		model.PushInput(text);
		OnPropertyChanged(nameof(Input));

		if (evaluation.Count > 0)
		{
			Result = string.Empty;

			evaluation.Clear();
			OnPropertyChanged(nameof(Evaluation));
		}
	}

	private void PopInput()
	{
		model.PopInput();
		OnPropertyChanged(nameof(Input));
	}

	private void ClearInput()
	{
		model.ClearInput();
		OnPropertyChanged(nameof(Input));
	}

	private void CalculateByInput()
	{
		if (Input.Length < 1)
			return;

		OnPropertyChanged(nameof(Input));

		evaluation.Clear();
		OnPropertyChanged(nameof(Evaluation));

		try
		{
			(List<string> evaluation, string result) = model.CalculateByInput();

			evaluation.ForEach(this.evaluation.Add);
			OnPropertyChanged(nameof(Evaluation));

			Result = $"={result}";

			model.ClearInput();
			model.PushInput(result);
		}
		catch (FormatException e)
		{
			Result = e.Message;
		}
	}

	#endregion
}