using Project_Real;
using System.Collections.Generic;

namespace Bullseye_Calculator.Model;

public abstract class ValueHolder<T> : Expression
{
	public virtual T Value => GetValue();

	protected static string ParenthesizeIfSigned(Rational value) => Rational.WriteSign || !value.Sign ? $"({value})" : value.ToString();
	public abstract void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<T> root, ref int step);
	public abstract T GetValue();
}