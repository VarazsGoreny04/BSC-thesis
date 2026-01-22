using Project_Real;

namespace Bullseye_Calculator.Model.Derivatives;

public abstract class Variable : Term<Rational>
{
	protected readonly char sign;
	protected Term<Rational> coefficient;
	protected Term<Rational> power;

	public char Sign => sign;
	public Term<Rational> Coefficient
	{
		get => coefficient;
		set => coefficient = value;
	}

	public Term<Rational> Power
	{
		get => power;
		set => power = value;
	}

	public Variable(char sign, Term<Rational> coefficient, Term<Rational> power)
	{
		this.sign = sign;
		this.coefficient = coefficient;
		this.power = power;
	}

	public override string ToStringByStep(ref int step) => $"{(coefficient is Term<Rational> c ? $"{c}*" : "")}{sign}{(power is Term<Rational> p ? $"^{p}" : "")}";
}

public sealed class X : Variable
{
	private static Rational value = Digit.ZERO;

	public X(Term<Rational>? coefficient, Term<Rational>? power) : base('x', coefficient ?? new Standard.Number(Digit.ONE), power ?? new Standard.Number(Digit.ZERO)) { }

	public override Rational GetValue() => coefficient.GetValue() * (value ^ power.GetValue());
}