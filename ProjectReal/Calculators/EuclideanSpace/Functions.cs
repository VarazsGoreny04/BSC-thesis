using ProjectReal.NumberSet;
using System;
using System.Numerics;

namespace Calculators.EuclideanSpace;

/// <summary>
/// Represents a diagonalization function.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> in the matrix.</typeparam>
public class Diagonalize<T> : Function<Matrix<T>>
where T :
	IComparisonOperators<T, T, bool>,
	IEqualityOperators<T, T, bool>,
	IUnaryNegationOperators<T, T>,
	IAdditionOperators<T, T, T>,
	ISubtractionOperators<T, T, T>,
	IMultiplyOperators<T, T, T>,
	IDivisionOperators<T, T, T>,
	IModulusOperators<T, T, T>,
	IPowerOperations<T, T, T>,
	IRootOperations<T, T, T>,
	IAdditiveIdentity<T, T>,
	IMultiplicativeIdentity<T, T>,
	IParsable<T>
{
	#region Properties

	/// <summary>
	/// Gets or sets the first matrix to diagonalize.
	/// </summary>
	public ValueHolder<Matrix<T>> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs an diagonalizer function with the parameter set to <see langword="null"/>.
	/// </summary>
	public Diagonalize() : base([null!]) { }

	/// <summary>
	/// Constructs an diagonalizer function with the given <paramref name="parameter"/> matrix.
	/// </summary>
	/// <param name="parameter">The matrix to diagonalize.</param>
	public Diagonalize(ValueHolder<Matrix<T>> parameter) : base([parameter]) { }

	#endregion

	#region Public methods

	public override Matrix<T> GetValue() => MatrixOperations<T>.Diagonalize(Matrix<T>.ToMatrix(Parameter.GetValue().Value), 3).Eigenvalues;

	public override string Sign() => "diag";

	#endregion
}

/// <summary>
/// Represents a inverse function.
/// </summary>
/// <typeparam name="T">The type of the <see cref="ValueHolder{T}"/> in the matrix.</typeparam>
public class Inverse<T> : Function<Matrix<T>>
where T :
	IComparisonOperators<T, T, bool>,
	IEqualityOperators<T, T, bool>,
	IUnaryNegationOperators<T, T>,
	IAdditionOperators<T, T, T>,
	ISubtractionOperators<T, T, T>,
	IMultiplyOperators<T, T, T>,
	IDivisionOperators<T, T, T>,
	IModulusOperators<T, T, T>,
	IPowerOperations<T, T, T>,
	IRootOperations<T, T, T>,
	IAdditiveIdentity<T, T>,
	IMultiplicativeIdentity<T, T>,
	IParsable<T>
{
	#region Properties

	/// <summary>
	/// Gets or sets the matrix to invert.
	/// </summary>
	public ValueHolder<Matrix<T>> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs an inverse function with the parameter set to <see langword="null"/>.
	/// </summary>
	public Inverse() : base([null!]) { }

	/// <summary>
	/// Constructs an inverse function with the given <paramref name="parameter"/> matrix.
	/// </summary>
	/// <param name="parameter">The matrix to invert.</param>
	public Inverse(ValueHolder<Matrix<T>> parameter) : base([parameter]) { }

	#endregion

	#region Public methods

	public override Matrix<T> GetValue() => MatrixOperations<T>.Inverse(Matrix<T>.ToMatrix(Parameter.GetValue().Value));

	public override string Sign() => "inv";

	#endregion
}