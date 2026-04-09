using BullseyeCalculator.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BullseyeCalculator.Model;

public class CalculatorModel
{
	private readonly CalculatorData data;

	public CalculatorData Data => data;

	public CalculatorModel() => data = new CalculatorData();

	#region Private methods

	private static List<string> FormatEvaluation(List<(string Calculation, string State)> evaluation)
	{
		List<string> result = [];

		if (evaluation.Count < 1)
			return result;

		int maxLength = evaluation.Max(step => step.Calculation.Length);
		evaluation.ForEach(step => result.Add($"{step.Calculation}{new string(' ', maxLength - step.Calculation.Length)}  ─→  {step.State}"));

		return result;
	}

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
			Mode.Options => data.calculator,
			_ => throw new NotImplementedException()
		};
	}

	public void PushInput(string text) => data.input.Add(text);

	public void PopInput()
	{
		if (data.input.Count > 0)
			data.input.RemoveAt(data.input.Count - 1);
	}

	public void ClearInput() => data.input.Clear();

	public (List<string> Evaluation, string Result) CalculateByInput()
	{
		string input = string.Concat(data.Input);

		List<(string Calculation, string State)> fullEvaluation = data.Calculator.FullEvaluation(input);

		return (FormatEvaluation(fullEvaluation), fullEvaluation.Count > 0 ? fullEvaluation.Last().State : input);
	}

	#endregion
}