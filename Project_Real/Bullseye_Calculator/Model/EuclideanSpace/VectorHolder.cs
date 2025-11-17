using Bullseye_Calculator.Model.Standard;

namespace Bullseye_Calculator.Model.EuclideanSpace;

public class VectorHolder : Expression
{
	private readonly ValueHolder[] value;

    public VectorHolder(ValueHolder[] value) => this.value = value;

    internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result) => result.Add(this);
	internal override void ToTree(ref Stack<Expression> result) => result.Push(this);
	public override string ToStringByStep(ref int step)
	{
		if (value.Length < 1)
			return "[ ]";

		string text = $"[ {value[0].ToStringByStep(ref step)}";

		for (int i = 1; i < value.Length; ++i)
			text += $", {value[i].ToStringByStep(ref step)}";

		return text + " ]";
	}
}