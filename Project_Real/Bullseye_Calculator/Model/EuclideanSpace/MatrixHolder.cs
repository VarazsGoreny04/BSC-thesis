using System.Collections.Generic;

namespace Bullseye_Calculator.Model.EuclideanSpace;

public class MatrixHolder : Term<Matrix>
{
	private readonly Matrix value;

	public MatrixHolder(Matrix value) => this.value = value;

	public override string ToStringByStep(ref int step) => value.ToStringByStep(ref step);

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<Matrix> root, ref int step)
	{
		value.FullEvaluation(ref partialValues, value, ref step);
	}

	public override Matrix GetValue() => value;
}