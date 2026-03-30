using Project_Real.NumberSet;
using System;
using System.Numerics;

namespace Bullseye_Calculator.Model.EuclideanSpace;

public class Diagonalize<T> : Function<Matrix<T>>
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
	public ValueHolder<Matrix<T>> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Diagonalize() : base([null!]) { }

	public Diagonalize(ValueHolder<Matrix<T>> parameter) : base([parameter]) { }

	public override Matrix<T> GetValue() => Matrix<T>.Diagonalize(Matrix<T>.ToMatrix(Parameter?.GetValue().Value ?? throw new FormatException()), 3).Eigenvalues;

	public override string Sign() => "diag";
}

public class Inverse<T> : Function<Matrix<T>>
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
	public ValueHolder<Matrix<T>> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Inverse() : base([null!]) { }

	public Inverse(ValueHolder<Matrix<T>> parameter) : base([parameter]) { }

	public override Matrix<T> GetValue() => Matrix<T>.Inverse(Matrix<T>.ToMatrix(Parameter?.GetValue().Value ?? throw new FormatException()));

	public override string Sign() => "inv";
}