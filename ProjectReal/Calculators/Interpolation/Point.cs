using Calculators.EuclideanSpace;
using ProjectReal.NumberSet;
using System;
using System.Linq;
using System.Numerics;

namespace Calculators.Interpolation;

/// <summary>
/// Represents a point in the coordinate system.
/// </summary>
/// <typeparam name="T">The type of values within the point.</typeparam>
public abstract class Point<T> where T : IEqualityOperators<T, T, bool>
{
	#region Fields

	protected readonly T[] values;

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Point{T}"/> by the given <paramref name="values"/>.
	/// </summary>
	/// <param name="values">The extent of the <see cref="Point{T}"/> in the coordinate system.</param>
	public Point(T[] values) => this.values = values;

	#endregion

	#region Public methods

	/// <summary>
	/// Returns a string that represents the value of <see langword="this"/> instance.
	/// </summary>
	/// <returns>A <see cref="Point{T}"/> as a <see langword="string"/>.</returns>
	public override string ToString() => $"({string.Join(", ", values)})";

	/// <summary>
	/// Compares two <see cref="Point{T}"/>s.
	/// </summary>
	/// <param name="a">The first <see cref="Point{T}"/>.</param>
	/// <param name="b">The second <see cref="Point{T}"/>.</param>
	/// <returns>
	/// <see langword="true"/> if each element of <paramref name="a"/> is equal to the corresponding value of <paramref name="b"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public static bool Equals(Point<T> a, Point<T> b) => a.values.Length == b.values.Length && a.values.Zip(b.values).All(x => x.First == x.Second);

	#endregion
}

/// <summary>
/// Represents a 2D point in the coordinate system.
/// </summary>
/// <typeparam name="T">The type of values within the point.</typeparam>
public class Point2D<T> : Point<T>
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

	/// <returns>The X value of <see langword="this"/> instance.</returns>
	public T X => values[0];

	/// <returns>The Y value of <see langword="this"/> instance.</returns>
	public T Y => values[1];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Point2D{T}"/> by the given <paramref name="values"/>.
	/// </summary>
	/// <param name="values">The extent of the <see cref="Point2D{T}"/> in the coordinate system.</param>
	/// <exception cref="ArgumentException">The length of <paramref name="values"/> is not 2.</exception>
	protected Point2D(T[] values) : base(values)
	{
		if (values.Length != 2)
			throw new ArgumentException("The length of the given array must have a length of 2!", nameof(values));
	}

	/// <summary>
	/// Constructs a <see cref="Point2D{T}"/> by the given <paramref name="x"/> and <paramref name="y"/> coordinates.
	/// </summary>
	/// <param name="x">The extent of the <see cref="Point2D{T}"/> on the X axis.</param>
	/// <param name="y">The extent of the <see cref="Point2D{T}"/> on the Y axis.</param>
	public Point2D(T x, T y) : base([x, y]) { }

	/// <summary>
	/// Constructs a <see cref="Point2D{T}"/> by the coordinates of the given <paramref name="tuple"/>.
	/// </summary>
	/// <param name="tuple">The extent of the <see cref="Point2D{T}"/>.</param>
	public Point2D((T X, T Y) tuple) : base([tuple.X, tuple.Y]) { }

	#endregion

	#region Public methods

	/// <summary>
	/// Adds two <see cref="Point2D{T}"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Point2D{T}"/> to add.</param>
	/// <param name="right">The second <see cref="Point2D{T}"/> to add.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point2D<T> Add(Point2D<T> left, Point2D<T> right) => new(VectorOperations<T>.Add(left.values, right.values));

	/// <summary>
	/// Subtracts two <see cref="Point2D{T}"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Point2D{T}"/> that represents the minuend.</param>
	/// <param name="right">The <see cref="Point2D{T}"/> that represents the subtrahend.</param>
	/// <returns>The result value and if there was a swap in a tuple.</returns>
	public static Point2D<T> Subtract(Point2D<T> left, Point2D<T> right) => new(VectorOperations<T>.Subtract(left.values, right.values));

	/// <summary>
	/// Calculates the distance of two <see cref="Point2D{T}"/>s.
	/// </summary>
	/// <param name="a">The first <see cref="Point2D{T}"/>.</param>
	/// <param name="b">The second <see cref="Point2D{T}"/>.</param>
	/// <returns>The result of the calculation.</returns>
	public static T Distance(Point2D<T> a, Point2D<T> b)
	{
		T diffX = a.X - b.X;
		T diffY = a.Y - b.Y;

		return ~(diffX * diffX + diffY * diffY);
	}

	/// <summary>
	/// Compares the given <see langword="object"/>? to this instance.
	/// </summary>
	/// <param name="obj">The <see langword="object"/>? to compare to.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="obj"/> is <see cref="Point2D{T}"/> and equal to the value of <see langword="this"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public override bool Equals(object? obj) => obj is Point2D<T> point && Equals(this, point);

	/// <summary>
	/// Throws a <see cref="NotImplementedException"/> because there is no point in implementing this method.
	/// </summary>
	public override int GetHashCode()
	{
		throw new NotImplementedException("This method is not implemented because there are more possible values ​​than the int can handle.");
	}

	#endregion

	#region Operators

	public static implicit operator Point2D<T>((T, T) tuple) => new(tuple);
	public static bool operator ==(Point2D<T> a, Point2D<T> b) => a.Equals(b);
	public static bool operator !=(Point2D<T> a, Point2D<T> b) => !a.Equals(b);
	public static Point2D<T> operator +(Point2D<T> a, Point2D<T> b) => Add(a, b);
	public static Point2D<T> operator -(Point2D<T> a, Point2D<T> b) => Subtract(a, b);

	#endregion
}

/// <summary>
/// Represents a 3D point in the coordinate system.
/// </summary>
/// <typeparam name="T">The type of values within the point.</typeparam>
public class Point3D<T> : Point2D<T>
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

	/// <returns>The Z value of <see langword="this"/> instance.</returns>
	public T Z => values[2];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Point3D{T}"/> by the given <paramref name="values"/>.
	/// </summary>
	/// <param name="values">The extent of the <see cref="Point3D{T}"/> in the coordinate system.</param>
	/// <exception cref="ArgumentException">The length of <paramref name="values"/> is not 3.</exception>
	protected Point3D(T[] values) : base(values)
	{
		if (values.Length != 3)
			throw new ArgumentException("The length of the given array must have a length of 3!", nameof(values));
	}

	/// <summary>
	/// Constructs a <see cref="Point3D{T}"/> by the given <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/> coordinates.
	/// </summary>
	/// <param name="x">The extent of the <see cref="Point3D{T}"/> on the X axis.</param>
	/// <param name="y">The extent of the <see cref="Point3D{T}"/> on the Y axis.</param>
	/// <param name="z">The extent of the <see cref="Point3D{T}"/> on the Z axis.</param>
	public Point3D(T x, T y, T z) : base([x, y, z]) { }

	/// <summary>
	/// Constructs a <see cref="Point3D{T}"/> by the coordinates of the given <paramref name="tuple"/>.
	/// </summary>
	/// <param name="tuple">The extent of the <see cref="Point3D{T}"/>.</param>
	public Point3D((T X, T Y, T Z) tuple) : base([tuple.X, tuple.Y, tuple.Z]) { }

	#endregion

	#region Public methods

	/// <summary>
	/// Adds two <see cref="Point3D{T}"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Point3D{T}"/> to add.</param>
	/// <param name="right">The second <see cref="Point3D{T}"/> to add.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point3D<T> Add(Point3D<T> a, Point3D<T> b) => new(VectorOperations<T>.Add(a.values, b.values));

	/// <summary>
	/// Subtracts two <see cref="Point3D{T}"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Point3D{T}"/> that represents the minuend.</param>
	/// <param name="right">The <see cref="Point3D{T}"/> that represents the subtrahend.</param>
	/// <returns>The result value and if there was a swap in a tuple.</returns>
	public static Point3D<T> Subtract(Point3D<T> left, Point3D<T> right) => new(VectorOperations<T>.Subtract(left.values, right.values));

	/// <summary>
	/// Calculates the distance of two <see cref="Point3D{T}"/>s.
	/// </summary>
	/// <param name="a">The first <see cref="Point3D{T}"/>.</param>
	/// <param name="b">The second <see cref="Point3D{T}"/>.</param>
	/// <returns>The result of the calculation.</returns>
	public static T Distance(Point3D<T> a, Point3D<T> b)
	{
		T diffX = a.X - b.X;
		T diffY = a.Y - b.Y;
		T diffZ = a.Z - b.Z;

		return ~(diffX * diffX + diffY * diffY + diffZ * diffZ);
	}

	/// <summary>
	/// Compares the given <see langword="object"/>? to this instance.
	/// </summary>
	/// <param name="obj">The <see langword="object"/>? to compare to.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="obj"/> is <see cref="Point3D{T}"/> and equal to the value of <see langword="this"/>; 
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public override bool Equals(object? obj) => obj is Point3D<T> point && Equals(this, point);

	/// <summary>
	/// Throws a <see cref="NotImplementedException"/> because there is no point in implementing this method.
	/// </summary>
	public override int GetHashCode()
	{
		throw new NotImplementedException("This method is not implemented because there are more possible values ​​than the int can handle.");
	}

	#endregion

	#region Operators

	public static implicit operator Point3D<T>((T, T, T) tuple) => new(tuple);
	public static Point3D<T> operator +(Point3D<T> a, Point3D<T> b) => Add(a, b);
	public static Point3D<T> operator -(Point3D<T> a, Point3D<T> b) => Subtract(a, b);

	#endregion
}