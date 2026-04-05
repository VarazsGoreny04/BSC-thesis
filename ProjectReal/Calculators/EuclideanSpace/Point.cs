using ProjectReal.NumberSet;
using System;
using System.Linq;
using System.Numerics;

namespace Calculators.EuclideanSpace;

/// <summary>
/// Represents a Point<T> in the coordinate system.
/// </summary>
public abstract class Point<T> where T : IEqualityOperators<T, T, bool>
{
	#region Fields

	protected readonly T[] values;

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Point"/> by the given <paramref name="values"/>.
	/// </summary>
	/// <param name="values">The extent of the <see cref="Point"/> in the coordinate system.</param>
	public Point(T[] values) => this.values = values;

	#endregion

	#region Public methods

	/// <summary>
	/// Returns a string that represents the value of <see langword="this"/> instance.
	/// </summary>
	/// <returns>A <see cref="Point"/> as a <see langword="string"/>.</returns>
	public override string ToString() => $"({string.Join(", ", values)})";

	public static bool Equals(Point<T> a, Point<T> b) => a.values.Length == b.values.Length && a.values.Zip(b.values).All(x => x.First == x.Second);

	#endregion
}

/// <summary>
/// Represents a 2D Point<T> in the coordinate system.
/// </summary>
public class Point2D<T> : Point<T>
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
	#region Properties

	/// <returns>The X value of <see langword="this"/> instance.</returns>
	public T X => values[0];

	/// <returns>The Y value of <see langword="this"/> instance.</returns>
	public T Y => values[1];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Point2D"/> by the given <paramref name="values"/>.
	/// </summary>
	/// <param name="values">The extent of the <see cref="Point2D"/> in the coordinate system.</param>
	/// <exception cref="ArgumentException">The length of <paramref name="values"/> is not 2.</exception>
	protected Point2D(T[] values) : base(values) { if (values.Length != 2) throw new ArgumentException(); }

	/// <summary>
	/// Constructs a <see cref="Point2D"/> by the given <paramref name="x"/> and <paramref name="y"/> coordinates.
	/// </summary>
	/// <param name="x">The extent of the <see cref="Point2D"/> on the X axis.</param>
	/// <param name="y">The extent of the <see cref="Point2D"/> on the Y axis.</param>
	public Point2D(T x, T y) : base([x, y]) { }

	/// <summary>
	/// Constructs a <see cref="Point2D"/> by the coordinates of the given <paramref name="tuple"/>.
	/// </summary>
	/// <param name="tuple">The extent of the <see cref="Point2D"/>.</param>
	public Point2D((T X, T Y) tuple) : base([tuple.X, tuple.Y]) { }

	#endregion

	#region Public methods

	/// <summary>
	/// Adds two <see cref="Point2D"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Point2D"/> to add.</param>
	/// <param name="right">The second <see cref="Point2D"/> to add.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point2D<T> Add(Point2D<T> left, Point2D<T> right) => new(Matrix<T>.Add(left.values, right.values));

	/// <summary>
	/// Subtracts two <see cref="Point2D"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Point2D"/> that represents the minuend.</param>
	/// <param name="right">The <see cref="Point2D"/> that represents the subtrahend.</param>
	/// <returns>The result value and if there was a swap in a tuple.</returns>
	public static Point2D<T> Subtract(Point2D<T> left, Point2D<T> right) => new(Matrix<T>.Subtract(left.values, right.values));

	/// <summary>
	/// Calculates the distance of two <see cref="Point2D"/>s.
	/// </summary>
	/// <param name="a">The first <see cref="Point2D"/>.</param>
	/// <param name="b">The second <see cref="Point2D"/>.</param>
	/// <returns>The result of the calculation.</returns>
	public static T Distance(Point2D<T> a, Point2D<T> b)
	{
		T diffX = a.X - b.X;
		T diffY = a.Y - b.Y;

		return ~(diffX * diffX + diffY * diffY);
	}

	public override bool Equals(object? obj) => obj is Point2D<T> point && Equals(this, point);
	
	public override int GetHashCode() => throw new NotImplementedException();

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
/// Represents a 3D Point<T> in the coordinate system.
/// </summary>
public class Point3D<T> : Point2D<T>
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
	#region Properties

	/// <returns>The Z value of <see langword="this"/> instance.</returns>
	public T Z => values[2];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Point3D"/> by the given <paramref name="values"/>.
	/// </summary>
	/// <param name="values">The extent of the <see cref="Point3D"/> in the coordinate system.</param>
	/// <exception cref="ArgumentException">The length of <paramref name="values"/> is not 3.</exception>
	protected Point3D(T[] values) : base(values) { if (values.Length != 3) throw new ArgumentException(); }

	/// <summary>
	/// Constructs a <see cref="Point3D"/> by the given <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/> coordinates.
	/// </summary>
	/// <param name="x">The extent of the <see cref="Point3D"/> on the X axis.</param>
	/// <param name="y">The extent of the <see cref="Point3D"/> on the Y axis.</param>
	/// <param name="z">The extent of the <see cref="Point3D"/> on the Z axis.</param>
	public Point3D(T x, T y, T z) : base([x, y, z]) { }

	/// <summary>
	/// Constructs a <see cref="Point3D"/> by the coordinates of the given <paramref name="tuple"/>.
	/// </summary>
	/// <param name="tuple">The extent of the <see cref="Point3D"/>.</param>
	public Point3D((T X, T Y, T Z) tuple) : base([tuple.X, tuple.Y, tuple.Z]) { }

	#endregion

	#region Public methods

	/// <summary>
	/// Adds two <see cref="Point3D"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Point3D"/> to add.</param>
	/// <param name="right">The second <see cref="Point3D"/> to add.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point3D<T> Add(Point3D<T> a, Point3D<T> b) => new(Matrix<T>.Add(a.values, b.values));

	/// <summary>
	/// Subtracts two <see cref="Point3D"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Point3D"/> that represents the minuend.</param>
	/// <param name="right">The <see cref="Point3D"/> that represents the subtrahend.</param>
	/// <returns>The result value and if there was a swap in a tuple.</returns>
	public static Point3D<T> Subtract(Point3D<T> left, Point3D<T> right) => new(Matrix<T>.Subtract(left.values, right.values));

	/// <summary>
	/// Calculates the distance of two <see cref="Point3D"/>s.
	/// </summary>
	/// <param name="a">The first <see cref="Point3D"/>.</param>
	/// <param name="b">The second <see cref="Point3D"/>.</param>
	/// <returns>The result of the calculation.</returns>
	public static T Distance(Point3D<T> a, Point3D<T> b)
	{
		T diffX = a.X - b.X;
		T diffY = a.Y - b.Y;
		T diffZ = a.Z - b.Z;

		return ~(diffX * diffX + diffY * diffY + diffZ * diffZ);
	}

	public override bool Equals(object? obj) => obj is Point3D<T> point && Equals(this, point);

	public override int GetHashCode() => throw new NotImplementedException();

	#endregion

	#region Operators

	public static implicit operator Point3D<T>((T, T, T) tuple) => new(tuple);
	public static Point3D<T> operator +(Point3D<T> a, Point3D<T> b) => Add(a, b);
	public static Point3D<T> operator -(Point3D<T> a, Point3D<T> b) => Subtract(a, b);

	#endregion
}