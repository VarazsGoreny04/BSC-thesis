using System.Collections.Generic;

namespace Bullseye_Calculator.Model.EuclideanSpace;

public class MatrixHolder : ValueHolder<Matrix>
{
	private readonly Matrix value;

	public MatrixHolder(Matrix value) => this.value = value;

	internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result) => result.Add(this);
	internal override void ToTree(ref Stack<Expression> result) => result.Push(this);
	public override string ToStringByStep(ref int step) => value.ToStringByStep(ref step);

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<Matrix> root, ref int step)
	{
		value.FullEvaluation(ref partialValues, value, ref step);
	}

	public override Matrix GetValue() => value;
}