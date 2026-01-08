using Project_Real;

namespace Bullseye_Calculator.Model.Derivatives;

public abstract class Variable : Term<Rational>
{
	private readonly char sign;
	private readonly Rational? coefficient;

	public char Sign => sign;

	public Variable(char sign, Rational? coefficient = null)
	{
		this.sign = sign;
		this.coefficient = coefficient;
	}

	public override string ToStringByStep(ref int step) => $"{coefficient}*{sign}";
}