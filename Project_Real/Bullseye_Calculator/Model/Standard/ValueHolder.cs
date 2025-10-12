using Project_Real;

namespace Bullseye_Calculator.Model.Standard;

public abstract class ValueHolder : Expression
{
	protected Rational? value;

	public Rational Value => value ??= GetValue();

	public abstract void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder root, ref int step);
	public abstract Rational GetValue();
}