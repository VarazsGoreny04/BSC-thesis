using ProjectReal.Number;
using Calculators;
using Calculators.Standard;
using Calculators.EuclideanSpace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BullseyeCalculator.Model;
using Calculators.Polynomials;

namespace BullseyeCalculator.WPF.ViewModel;

public class CalculatorViewModel : ViewModelBase
{
	#region Fields

	private readonly Calculator.FunctionToken<Rational>[] standardFunctionTokens;
	private readonly Calculator.FunctionToken<Matrix<Rational>>[] euclideanSpaceFunctionTokens;

	private readonly StandardCalculator<Rational> standardCalculator;
	private readonly EuclideanSpaceCalculator<Rational> euclideanSpaceCalculator;
	private readonly PolynomialCalculator<Rational> polynomialCalculator;

	private bool start;
	private readonly List<string> input;
	private string result;
	private readonly ObservableCollection<string> evaluation;

	private bool showSteps;
	private Mode currentMode;
	private Calculator calculator;

	#endregion

	#region Properties

	public string Input => string.Concat(input);
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

		standardFunctionTokens = [
			new("ceiling", () => new Ceiling()),
			new("round", () => new Round()),
			new("floor", () => new Floor()),
			new("fact", () => new Fact()),
			new("exp", () => new Exp()),
			new("cos", () => new Cos()),
			new("sin", () => new Sin()),
			new("max", () => new Max()),
			new("min", () => new Min()),
			new("abs", () => new Abs()),
			new("ln", () => new Ln()),
			new("pi", () => new PI()),
			new("e", () => new E())
		];
		euclideanSpaceFunctionTokens = [
			new("diag", () => new Diagonalize<Rational>()),
			new("inv", () => new Inverse<Rational>())
		];

		standardCalculator = new StandardCalculator<Rational>(standardFunctionTokens);
		euclideanSpaceCalculator = new EuclideanSpaceCalculator<Rational>(euclideanSpaceFunctionTokens, standardCalculator);
		polynomialCalculator = new PolynomialCalculator<Rational>(standardCalculator);

		calculator = standardCalculator;

		start = true;
		showSteps = false;
		CurrentMode = Mode.Standard;
		input = [];
		result = string.Empty;
		evaluation = [];

		InputCommand = new DelegateCommand(param => PushInput(param?.ToString() ?? throw new FormatException()));
		BackSpaceCommand = new DelegateCommand(_ => PopInput());
		ClearCommand = new DelegateCommand(_ => ClearInput());
		EvaluateCommand = new DelegateCommand(_ => CalculateByInput());

		ShowStepsCommand = new DelegateCommand(_ => ShowSteps = !showSteps);

		StandardModeCommand = new DelegateCommand(_ => ChangeMode(standardCalculator, Mode.Standard));
		EuclideanModeCommand = new DelegateCommand(_ => ChangeMode(euclideanSpaceCalculator, Mode.Matrix));
		InterpolationModeCommand = new DelegateCommand(_ => ChangeMode(polynomialCalculator, Mode.Interpolation));
		IntegralModeCommand = new DelegateCommand(_ => ChangeMode(standardCalculator, Mode.Integral));

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
		if (start)
			return;

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