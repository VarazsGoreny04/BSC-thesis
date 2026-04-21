using BullseyeCalculator.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BullseyeCalculator.Model;

public class CalculatorModel
{
	#region Fields

	private readonly CalculatorData data;
	private CancellationTokenSource? tokenSource;

	#endregion

	#region Properties

	public CalculatorData Data => data;

	public event EventHandler? InputChange;
	public event EventHandler? EvaluationChange;
	public event EventHandler? ResultChange;

	#endregion

	#region Constructors

	public CalculatorModel()
	{
		data = new CalculatorData();
		tokenSource = null;
	}

	#endregion

	#region Private methods

	private async Task<List<(string Calculation, string State)>> FullEvaluationAsync(string input) => await Task.Run(() => data.Calculator.FullEvaluation(input));
	private static List<string> FormatEvaluation(List<(string Calculation, string State)> evaluation)
	{
		List<string> result = [];

		if (evaluation.Count < 1)
			return result;

		int maxLength = evaluation.Max(step => step.Calculation.Length);
		evaluation.ForEach(step => result.Add($"{step.Calculation}{new string(' ', maxLength - step.Calculation.Length)}  ─→  {step.State}"));

		return result;
	}

	private void OnInputChange() => InputChange?.Invoke(this, EventArgs.Empty);

	private void OnEvaluationChange() => EvaluationChange?.Invoke(this, EventArgs.Empty);

	private void OnResultChange() => ResultChange?.Invoke(this, EventArgs.Empty);

	#endregion

	#region Public methods

	public void SwitchMode(Mode mode)
	{
		data.mode = mode;
		data.calculator = mode switch
		{
			Mode.Standard => data.standardCalculator,
			Mode.Matrix => data.euclideanSpaceCalculator,
			Mode.Interpolation => data.polynomialCalculator,
			_ => throw new NotImplementedException("No corresponding calculator found for this mode!")
		};
	}

	public void PushInput(string text)
	{
		if (tokenSource is not null)
			return;

		data.input.Add(text);
		OnInputChange();

		if (data.evaluation.Count > 0)
			ClearEvaluation();

		if (data.result.Length > 0)
			ChangeResult(string.Empty);
	}

	public void PopInput()
	{
		if (tokenSource is not null)
			return;

		if (data.input.Count > 0)
		{
			data.input.RemoveAt(data.input.Count - 1);
			OnInputChange();
		}
	}

	public void ClearInput()
	{
		if (tokenSource is not null)
			return;

		data.input.Clear();
		OnInputChange();
	}

	public void AddEvaluation(List<string> evaluation)
	{
		if (tokenSource is not null)
			return;

		evaluation.ForEach(data.evaluation.Add);
		OnEvaluationChange();
	}

	public void ClearEvaluation()
	{
		if (tokenSource is not null)
			return;

		data.evaluation.Clear();
		OnEvaluationChange();
	}

	public void ChangeResult(string result)
	{
		data.result = result;
		OnResultChange();
	}

	public async void Evaluate()
	{
		if (tokenSource is not null)
			return;

		string input = data.Input;

		if (input.Length > 0)
		{
			ClearEvaluation();
			ChangeResult("Calculating...");

			try
			{
				tokenSource = new CancellationTokenSource();

				List<(string Calculation, string State)> fullEvaluation = await FullEvaluationAsync(input);

				tokenSource = null;

				string result = fullEvaluation.Count > 0 ? fullEvaluation.Last().State : input;

				AddEvaluation(FormatEvaluation(fullEvaluation));
				ChangeResult(result);

				data.input.Clear();
				data.input.Add(result);
			}
			catch (Exception e)
			{
				ChangeResult(e.Message);
			}
		}
	}

	#endregion
}