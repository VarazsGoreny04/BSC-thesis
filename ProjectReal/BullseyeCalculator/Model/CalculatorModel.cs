using BullseyeCalculator.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BullseyeCalculator.Model;

public sealed class CalculatorModel
{
	#region Fields

	private readonly CalculatorData data;

	private System.Diagnostics.Process? process;

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

		process = null;
	}

	#endregion

	#region Private methods

	private void OnInputChange() => InputChange?.Invoke(this, EventArgs.Empty);

	private void OnEvaluationChange() => EvaluationChange?.Invoke(this, EventArgs.Empty);

	private void OnResultChange() => ResultChange?.Invoke(this, EventArgs.Empty);

	private void AddEvaluation(List<string> evaluation)
	{
		evaluation.ForEach(data.Evaluation.Add);
		OnEvaluationChange();
	}

	private void ClearEvaluation()
	{
		data.Evaluation.Clear();
		OnEvaluationChange();
	}

	private void ChangeResult(string result)
	{
		data.Result = result;
		OnResultChange();
	}

	#endregion

	#region Public methods

	public void PushInput(string text)
	{
		if (process is not null)
			return;

		data.Input.Add(text);
		OnInputChange();

		if (data.Evaluation.Count > 0)
			ClearEvaluation();

		if (data.Result.Length > 0)
			ChangeResult(string.Empty);
	}

	public void PopInput()
	{
		if (process is not null)
			return;

		if (data.Input.Count > 0)
		{
			data.Input.RemoveAt(data.Input.Count - 1);
			OnInputChange();
		}
	}

	public void ClearInput()
	{
		if (process is not null)
			return;

		data.Input.Clear();
		OnInputChange();
	}

	public async void Evaluate()
	{
		async Task StartEvaluation()
		{
			string input = data.ConcatInput();

			if (input.Length < 1)
				return;

			process = new System.Diagnostics.Process()
			{
				StartInfo = new System.Diagnostics.ProcessStartInfo()
				{
					FileName = Path.Combine(AppContext.BaseDirectory, "BullseyeCalculator.Process.exe"),
					Arguments = $"{(char)data.Mode} {data.FractionalFormat} {data.FractionCalculationLength} \"{input}\"",

					UseShellExecute = false,
					CreateNoWindow = true,

					RedirectStandardOutput = true,
					StandardOutputEncoding = Encoding.Unicode
				}
			};

			ClearEvaluation();
			ChangeResult("Calculating...");

			(string result, List<string> evaluation) = await Task.Run(() =>
			{
				process?.Start();

				string result = process?.StandardOutput.ReadLine() ?? "Cancelled.";

				List<string> evaluation = [];

				while (process?.StandardOutput.ReadLine() is string line)
					evaluation.Add(line);

				return (result, evaluation);
			});

			AddEvaluation(evaluation);
			ChangeResult(result);

			if (process is not null)
			{
				data.Input.Clear();
				data.Input.Add(result);
			}

			process?.Dispose();
			process = null;
		}

		void CancelEvaluation()
		{
			process?.Kill();

			process?.Dispose();
			process = null;

			AddEvaluation([]);
			ChangeResult("Cancelled.");
		}

		if (process is null)
			await StartEvaluation();
		else
			CancelEvaluation();
	}

	#endregion
}