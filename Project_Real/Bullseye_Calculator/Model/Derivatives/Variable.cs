using Project_Real;

namespace Bullseye_Calculator.Model.Derivatives;

public abstract class Variable : Term<Term<Rational>>
{
	protected readonly char sign;
	protected readonly Term<Rational>? coefficient;
	protected readonly Term<Rational>? power;

	public char Sign => sign;

	public Variable(char sign, Term<Rational>? coefficient, Term<Rational>? power)
	{
		this.sign = sign;
		this.coefficient = coefficient;
		this.power = power;
	}

	public override string ToStringByStep(ref int step) => $"{(coefficient is Term<Rational> c? $"{c}*" : "")}{sign}{(power is Term<Rational> p ? $"^{p}" : "")}";
}

public sealed class X : Variable
{
	private static Rational value = Digit.ZERO;

	public X(Term<Rational>? coefficient, Term<Rational>? power) : base('x', coefficient, power) { }

	public override Term<Rational> GetValue()
	{
		return new Standard.Number(coefficient is Term<Rational> c ? c.GetValue() * (power is Term<Rational> p ? value ^ power.GetValue() : Digit.ONE) : Digit.ZERO);
	}
}