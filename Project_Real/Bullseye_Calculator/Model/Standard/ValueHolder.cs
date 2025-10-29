using Project_Real;

namespace Bullseye_Calculator.Model.Standard;

public abstract class ValueHolder : Expression
{
	public virtual Rational Value => GetValue();

	protected static string ParenthesizeIfSigned(Rational value) => Rational.WriteSign || !value.Sign ? $"({value})" : value.ToString();
	public abstract void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder root, ref int step);
	public abstract Rational GetValue();
}