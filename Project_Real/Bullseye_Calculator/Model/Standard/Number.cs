using Project_Real;

namespace Bullseye_Calculator.Model.Standard;

public sealed class Number(string token) : Term
{
	private readonly string token = token;
	public string Token => token;

	public override Rational GetValue() => new(token);
	public override string ToStringByStep(ref int _) => token;
}