using ProjectReal.Number;
using Calculators.EuclideanSpace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BullseyeCalculator.Model;
using BullseyeCalculator.Persistence;

namespace BullseyeCalculator.WPF.ViewModel;

public class CalculatorViewModel : ViewModelBase
{
	#region Fields

	private readonly CalculatorModel model;

	private bool start;
	private string result;
	private readonly ObservableCollection<string> evaluation;

	private bool showSteps;

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
	public Mode CurrentMode
	{
		get => model.Data.Mode;
		set
		{
			model.SwitchMode(value);

			OnPropertyChanged(nameof(CurrentMode));
		}
	}
	public static char Separator => Rational.Separator;
	public static char ColumnSeparator => Matrix<Rational>.ColumnSeparator;
	public static char RowSeparator => Matrix<Rational>.RowSeparator;

	#endregion

	#region Commands

	public DelegateCommand InputCommand { get; }
	public DelegateCommand BackSpaceCommand { get; }
	public DelegateCommand ClearCommand { get; }
	public DelegateCommand EvaluateCommand { get; }

	public DelegateCommand ShowStepsCommand { get; }

	public DelegateCommand StandardModeCommand { get; }
	public DelegateCommand EuclideanModeCommand { get; }
	public DelegateCommand InterpolationModeCommand { get; }
	public DelegateCommand IntegralModeCommand { get; }

	public DelegateCommand ShowOptionsCommand { get; }

	#endregion

	#region Constructors

	public CalculatorViewModel()
	{
		Rational.WriteSign = false;
		Rational.FractionalFormat = false;

		model = new CalculatorModel();

		start = true;
		showSteps = false;
		CurrentMode = Mode.Standard;
		result = string.Empty;
		evaluation = [];

		InputCommand = new DelegateCommand(param => PushInput(param?.ToString() ?? throw new FormatException()));
		BackSpaceCommand = new DelegateCommand(_ => PopInput());
		ClearCommand = new DelegateCommand(_ => ClearInput());
		EvaluateCommand = new DelegateCommand(_ => CalculateByInput());

		ShowStepsCommand = new DelegateCommand(_ => ShowSteps = !showSteps);

		StandardModeCommand = new DelegateCommand(_ => CurrentMode = Mode.Standard);
		EuclideanModeCommand = new DelegateCommand(_ => CurrentMode = Mode.Matrix);
		InterpolationModeCommand = new DelegateCommand(_ => CurrentMode = Mode.Interpolation);
		IntegralModeCommand = new DelegateCommand(_ => CurrentMode = Mode.Integral);

		ShowOptionsCommand = new DelegateCommand(_ => { });
	}

	#endregion

	#region Private methods

	private void PushInput(string text)
	{
		model.PushInput(text);
		OnPropertyChanged(nameof(Input));

		if (start)
		{
			Result = string.Empty;

			evaluation.Clear();
			OnPropertyChanged(nameof(Evaluation));

			start = false;
		}
	}

	private void PopInput()
	{
		model.PopInput();
		OnPropertyChanged(nameof(Input));

		start = false;
	}

	private void ClearInput()
	{
		model.ClearInput();
		OnPropertyChanged(nameof(Input));

		start = true;
	}

	private void CalculateByInput()
	{
		if (start)
			return;

		try
		{
			(List<string> evaluation, string result) = model.CalculateByInput();

			evaluation.ForEach(Evaluation.Add);
			Result = $"={result}";

			OnPropertyChanged(nameof(Evaluation));

			model.ClearInput();
			model.PushInput(result);
		}
		catch (FormatException e)
		{
			Result = e.Message;
		}

		start = true;
	}

	#endregion
}