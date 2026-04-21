using ProjectReal.NumberSet;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Calculators.EuclideanSpace;

/// <summary>
/// Represents a matrix holding node.
/// </summary>
/// <typeparam name="T">The type of the values in the matrix to hold.</typeparam>
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
	#region Fields

	private readonly Matrix<T> value;

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="MatrixHolder{T}"/> by the given <paramref name="value"/>.
	/// </summary>
	/// <param name="value">The given value.</param>
	public MatrixHolder(Matrix<T> value) => this.value = value;

	#endregion

	#region Public methods

	public override string ToStringByStep(ref int step)
	{
		List<string> rows = [];

		for (int i = 0; i < value.Rows; ++i)
		{
			List<string> row = [];

			for (int j = 0; j < value.Columns; ++j)
				row.Add(value[i, j].ToStringByStep(ref step));

			rows.Add(string.Join(Matrix<T>.ColumnSeparator, row));
		}

		return $"[{string.Join(Matrix<T>.RowSeparator, rows)}]";
	}

	public override void FullEvaluation(ref List<(string, string)> partialValues, ValueHolder<Matrix<T>> root, ref int step)
	{
		int stepCopy = step;

		foreach (ValueHolder<T> item in value.Value)
			item.FullEvaluation(ref partialValues, item, ref step);

		if (stepCopy != step)
		{
			stepCopy = step;

			partialValues.Add(($"{ToString()} = {GetValue()}", root.ToStringByStep(ref stepCopy)));
		}
	}

	public override Matrix<T> GetValue() => new(Matrix<T>.ToMatrix(value));

	#endregion
}