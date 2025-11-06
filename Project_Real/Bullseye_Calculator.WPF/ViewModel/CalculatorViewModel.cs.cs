using Bullseye_Calculator.Model;
using Bullseye_Calculator.Model.Standard;
using Project_Real;
using System.Collections.ObjectModel;

namespace Bullseye_Calculator.WPF.ViewModel;

public class CalculatorViewModel : ViewModelBase
{
	private bool start;
	private Mode currentMode;

	Calculator calculator;
	private readonly List<string> input;
	private string result;
	private readonly ObservableCollection<string> evaluation;

	public Mode CurrentMode
	{
		get => currentMode;
		set
		{
			currentMode = value;

			OnPropertyChanged(nameof(CurrentMode));
		}
	}
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
	public char Separator
	{
		get => Rational.Separator;
		set
		{
			Rational.Separator = value;

			OnPropertyChanged(nameof(Separator));
		}
	}

	public DelegateCommand InputCommand { get; private set; }
	public DelegateCommand BackSpaceCommand { get; private set; }
	public DelegateCommand ClearCommand { get; private set; }
	public DelegateCommand EvaluateCommand { get; private set; }
	public DelegateCommand ChangeModeCommand { get; private set; }

	public CalculatorViewModel()
	{
		Rational.WriteSign = false;
		Rational.FractionalFormat = false;

		CurrentMode = Mode.Standard;
		start = true;

		calculator = new StandardCalculator();
		input = [];
		result = string.Empty;
		evaluation = [];

		InputCommand = new DelegateCommand(param => PushInput(param?.ToString() ?? throw new FormatException()));
		BackSpaceCommand = new DelegateCommand(_ => PopInput());
		ClearCommand = new DelegateCommand(_ => ClearInput());
		EvaluateCommand = new DelegateCommand(_ => CalculateByInput());
		ChangeModeCommand = new DelegateCommand(param => CurrentMode = Enum.Parse<Mode>(param?.ToString() ?? throw new FormatException()));
	}

	public void PushInput(string text)
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

	public void PopInput()
	{
		if (input.Count > 0)
		{
			input.RemoveAt(input.Count - 1);
			OnPropertyChanged(nameof(Input));
		}
	}

	public void ClearInput()
	{
		input.Clear();
		OnPropertyChanged(nameof(Input));

		start = true;
	}

	public void CalculateByInput()
	{
		if (!start)
		{
			try
			{
				ValueHolder valueHolder = Calculator.Evaluate(Input, calculator);

				ShowFullEvaluation(Calculator.FullEvaluation(valueHolder));

				string result = valueHolder.Value.ToString();

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
}