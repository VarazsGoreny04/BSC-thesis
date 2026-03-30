using Bullseye_Calculator.Model;
using Project_Real.Number;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bullseye_Calculator.WPF.ViewModel;

public class CalculatorViewModel : ViewModelBase
{
	#region Fields

	private bool start;
	private readonly List<string> input;
	private string result;
	private readonly ObservableCollection<string> evaluation;
	
	private bool showSteps;
	private Mode currentMode;
	private Calculator calculator;

	#endregion

	#region Properties

	public string Input => string.Join(string.Empty, input);
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
		get => currentMode;
		set
		{
			currentMode = value;

			OnPropertyChanged(nameof(CurrentMode));
		}
	}
	public static char Separator => Rational.Separator;
	public static char ColumnSeparator => Model.EuclideanSpace.Matrix<Rational>.ColumnSeparator;
	public static char RowSeparator => Model.EuclideanSpace.Matrix<Rational>.RowSeparator;

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

		start = true;
		showSteps = false;
		CurrentMode = Mode.Standard;
		calculator = new Model.Standard.StandardCalculator<Rational>();
		input = [];
		result = string.Empty;
		evaluation = [];

		InputCommand = new DelegateCommand(param => PushInput(param?.ToString() ?? throw new FormatException()));
		BackSpaceCommand = new DelegateCommand(_ => PopInput());
		ClearCommand = new DelegateCommand(_ => ClearInput());
		EvaluateCommand = new DelegateCommand(_ => CalculateByInput());

		ShowStepsCommand = new DelegateCommand(_ => ShowSteps = !showSteps);

		StandardModeCommand = new DelegateCommand(_ => ChangeMode(new Model.Standard.StandardCalculator<Rational>(), Mode.Standard));
		EuclideanModeCommand = new DelegateCommand(_ => ChangeMode(new Model.EuclideanSpace.EuclideanSpaceCalculator<Rational>(), Mode.Matrix));
		InterpolationModeCommand = new DelegateCommand(_ => ChangeMode(new Model.Standard.StandardCalculator<Rational>(), Mode.Interpolation));
		IntegralModeCommand = new DelegateCommand(_ => ChangeMode(new Model.Standard.StandardCalculator<Rational>(), Mode.Integral));

		ShowOptionsCommand = new DelegateCommand(_ => { });
	}

	#endregion

	#region Private methods

	private void ChangeMode(Calculator calculator, Mode mode)
	{
		this.calculator = calculator;
		CurrentMode = mode;
	}

	private void PushInput(string text)
	{
		input.Add(text);
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
		if (input.Count > 0)
		{
			input.RemoveAt(input.Count - 1);
			OnPropertyChanged(nameof(Input));
		}
	}

	private void ClearInput()
	{
		input.Clear();
		OnPropertyChanged(nameof(Input));

		start = true;
	}

	private void CalculateByInput()
	{
		if (!start)
		{
			try
			{
				List<(string Calculation, string State)> fullEvaluation = calculator.FullEvaluation(Input);

				ShowFullEvaluation(fullEvaluation);

				string result = fullEvaluation.Count > 0 ? fullEvaluation.Last().State : Input;

				Result = $"={result}";

				input.Clear();
				input.Add(result);
			}
			catch (FormatException e)
			{
				Result = e.Message;
			}

			start = true;
		}
	}

	private void ShowFullEvaluation(List<(string Calculation, string State)> evaluation)
	{
		if (evaluation.Count > 0)
		{
			int maxLength = evaluation.Max(step => step.Calculation.Length);
			evaluation.ForEach(step => this.evaluation.Add($"{step.Calculation}{new string(' ', maxLength - step.Calculation.Length)}  ─→  {step.State}"));

			OnPropertyChanged(nameof(Evaluation));
		}
	}

	#endregion
}