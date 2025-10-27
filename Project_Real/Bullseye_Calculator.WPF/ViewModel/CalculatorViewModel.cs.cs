using Bullseye_Calculator.Model.Standard;
using Project_Real;

namespace Bullseye_Calculator.WPF.ViewModel;

public class CalculatorViewModel : ViewModelBase
{
	private readonly List<string> input;
	private readonly List<string> evaluation;

	public string Input => string.Join(string.Empty, input);
	public List<string> Evaluation => evaluation;

	public DelegateCommand InputCommand { get; private set; }
	public DelegateCommand BackSpaceCommand { get; private set; }
	public DelegateCommand ClearCommand { get; private set; }
	public DelegateCommand EvaluateCommand { get; private set; }

	public CalculatorViewModel()
	{
		Rational.WriteSign = false;
		Rational.FractionalFormat = false;

		input = [];
		evaluation = [];

		InputCommand = new DelegateCommand(param => PushInput(param?.ToString() ?? throw new FormatException()));
		BackSpaceCommand = new DelegateCommand(_ => PopInput());
		ClearCommand = new DelegateCommand(_ => ClearInput());
		EvaluateCommand = new DelegateCommand(_ => CalculateByInput());
	}

	public void PushInput(string text)
	{
		input.Add(text);

		OnPropertyChanged(nameof(Input));
	}

	public void PopInput()
	{
		if (input.Count > 0)
			input.RemoveAt(input.Count - 1);

		OnPropertyChanged(nameof(Input));
	}

	public void ClearInput()
	{
		if (input.Count > 0)
			input.Clear();
		
		OnPropertyChanged(nameof(Input));
	}

	public void CalculateByInput()
	{
		if (input.Count > 0)
		{
			try
			{
				ValueHolder valueHolder = Calculator.Evaluate(Input);

				ResultToString(valueHolder);

				FullEvaluationToString(Calculator.FullEvaluation(valueHolder));
			}
			catch (FormatException e)
			{
				input.Clear();
				input.Add(e.Message);

				OnPropertyChanged(nameof(Input));

				input.Clear();
			}
		}
	}

	private void ResultToString(ValueHolder valueHolder)
	{
		input.Clear();
		input.Add($"{valueHolder}\n={valueHolder.Value}");

		OnPropertyChanged(nameof(Input));

		input.Clear();
		input.Add(valueHolder.Value.ToString());
	}

	private void FullEvaluationToString(List<(string Calculation, string State)> evaluation)
	{
		this.evaluation.Clear();

		if (evaluation.Count > 0)
		{
			int maxLength = evaluation.Max(step => step.Calculation.Length);
			evaluation.ForEach(step => this.evaluation.Add($"{step.Calculation}{new string(' ', maxLength - step.Calculation.Length)}\t{step.State}"));
		}

		OnPropertyChanged(nameof(Evaluation));
	}
}