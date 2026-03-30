using Project_Real.NumberSet;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Bullseye_Calculator.Model.EuclideanSpace;

public class MatrixHolder<T> : Term<Matrix<T>>
where T :
	IComparisonOperators<T, T, bool>,
	IEqualityOperators<T, T, bool>,
	IUnaryNegationOperators<T, T>,
	IAdditionOperators<T, T, T>,
	ISubtractionOperators<T, T, T>,
	IMultiplyOperators<T, T, T>,
	IDivisionOperators<T, T, T>,
	IPowerOperations<T, T, T>,
	IRootOperations<T, T, T>,
	IAdditiveIdentity<T, T>,
	IMultiplicativeIdentity<T, T>,
	IParsable<T>
{
	private readonly Matrix<T> value;

	public MatrixHolder(Matrix<T> value) => this.value = value;

	public override string ToStringByStep(ref int step)
	{
		List<string> rows = [];

		for (int i = value.Rows; i >= 0; --i)
		{
			string row = $"{value[i, 0].ToStringByStep(ref step)}";

			for (int j = value.Columns; j >= 0; --j)
				row += $";{value[i, j].ToStringByStep(ref step)}";

			rows.Add(row);
		}

		return "[" + string.Join("&", rows) + "]";
	}

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<Matrix<T>> root, ref int step)
	{
		int stepCopy = step;

		foreach (ValueHolder<T> item in value.Value)
			item.FullEvaluation(ref partialValues, item, ref step);

		if (stepCopy != step)
		{
			stepCopy = ++step;
			int toRockBottom = int.MaxValue;

			partialValues.Add(($"{ToString()} = {ToStringByStep(ref toRockBottom)}", root.ToStringByStep(ref stepCopy)));
		}
	}

	public override Matrix<T> GetValue() => value;
}