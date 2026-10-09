using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BullseyeCalculator.Persistence;

public sealed class CalculatorData
{
	#region Fields

	private Mode mode;
	private readonly List<string> input;

	private bool fractionalFormat;
	private int fractionCalculationLength;

	private readonly ObservableCollection<string> evaluation;
	private string result;

	#endregion

	#region Properties

	public Mode Mode { get => mode; set => mode = value; }
	public List<string> Input => input;

	public bool FractionalFormat { get => fractionalFormat; set => fractionalFormat = value; }
	public int FractionCalculationLength { get => fractionCalculationLength; set => fractionCalculationLength = value; }

	public ObservableCollection<string> Evaluation => evaluation;
	public string Result { get => result; internal set => result = value; }

	#endregion

	#region Constructors

	public CalculatorData()
	{
		mode = Mode.Standard;
		input = [];

		fractionalFormat = false;
		fractionCalculationLength = 10;

		evaluation = [];
		result = string.Empty;
	}

	#endregion

	#region Public methods

	public string ConcatInput() => string.Concat(input);

	#endregion
}