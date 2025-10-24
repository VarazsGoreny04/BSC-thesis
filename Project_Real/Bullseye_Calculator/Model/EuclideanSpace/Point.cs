using Project_Real;

namespace Bullseye_Calculator.Model.EuclideanSpace;

/// <summary>
/// Represents a point in the coordinate system.
/// </summary>
public abstract class Point
{
	#region Fields

	protected readonly Rational[] values;

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Point"/> by the given <paramref name="values"/>.
	/// </summary>
	/// <param name="values">The extent of the <see cref="Point"/> in the coordinate system.</param>
	public Point(Rational[] values) => this.values = values;

	#endregion

	#region Public methods

	/// <summary>
	/// Returns a string that represents the value of <see langword="this"/> instance.
	/// </summary>
	/// <returns>A <see cref="Point"/> as a <see langword="string"/>.</returns>
	public override string ToString() => $"({string.Join(", ", values)})";

	public override bool Equals(object? obj) => obj is Point point && values.Length == point.values.Length && values.Zip(point.values).All(x => x.First == x.Second);

	public override int GetHashCode() => throw new NotImplementedException();

	#endregion
}

/// <summary>
/// Represents a 2D point in the coordinate system.
/// </summary>
public class Point2D : Point
{
	#region Properties

	/// <returns>The X value of <see langword="this"/> instance.</returns>
	public Rational X => values[0];

	/// <returns>The Y value of <see langword="this"/> instance.</returns>
	public Rational Y => values[1];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Point2D"/> by the given <paramref name="values"/>.
	/// </summary>
	/// <param name="values">The extent of the <see cref="Point2D"/> in the coordinate system.</param>
	/// <exception cref="ArgumentException">The length of <paramref name="values"/> is not 2.</exception>
	protected Point2D(Rational[] values) : base(values) { if (values.Length != 2) throw new ArgumentException(); }

	/// <summary>
	/// Constructs a <see cref="Point2D"/> by the given <paramref name="x"/> and <paramref name="y"/> coordinates.
	/// </summary>
	/// <param name="x">The extent of the <see cref="Point2D"/> on the X axis.</param>
	/// <param name="y">The extent of the <see cref="Point2D"/> on the Y axis.</param>
	public Point2D(Rational x, Rational y) : base([x, y]) { }

	/// <summary>
	/// Constructs a <see cref="Point2D"/> by the coordinates of the given <paramref name="tuple"/>.
	/// </summary>
	/// <param name="tuple">The extent of the <see cref="Point2D"/>.</param>
	public Point2D((Rational X, Rational Y) tuple) : base([tuple.X, tuple.Y]) { }

	#endregion

	#region Public methods

	/// <summary>
	/// Adds two <see cref="Point2D"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Point2D"/> to add.</param>
	/// <param name="right">The second <see cref="Point2D"/> to add.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point2D Add(Point2D left, Point2D right) => new(Matrix.Add(left.values, right.values));

	/// <summary>
	/// Subtracts two <see cref="Point2D"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Point2D"/> that represents the minuend.</param>
	/// <param name="right">The <see cref="Point2D"/> that represents the subtrahend.</param>
	/// <returns>The result value and if there was a swap in a tuple.</returns>
	public static Point2D Subtract(Point2D left, Point2D right) => new(Matrix.Subtract(left.values, right.values));

	/// <summary>
	/// Calculates the distance of two <see cref="Point2D"/>s.
	/// </summary>
	/// <param name="a">The first <see cref="Point2D"/>.</param>
	/// <param name="b">The second <see cref="Point2D"/>.</param>
	/// <returns>The result of the calculation.</returns>
	public static Rational Distance(Point2D a, Point2D b) => Rational.SquareRoot( Rational.SecondPower(a.X - b.X) + Rational.SecondPower(a.Y - b.Y));

	#endregion

	#region Operators

	public static implicit operator Point2D((Rational, Rational) tuple) => new(tuple);
	public static bool operator ==(Point2D a, Point2D b) => a.Equals(b);
	public static bool operator !=(Point2D a, Point2D b) => !a.Equals(b);
	public static Point2D operator +(Point2D a, Point2D b) => Add(a, b);
	public static Point2D operator -(Point2D a, Point2D b) => Subtract(a, b);

	#endregion
}

/// <summary>
/// Represents a 3D point in the coordinate system.
/// </summary>
public class Point3D : Point2D
{
	#region Properties

	/// <returns>The Z value of <see langword="this"/> instance.</returns>
	public Rational Z => values[2];

	#endregion

	#region Constructors

	/// <summary>
	/// Constructs a <see cref="Point3D"/> by the given <paramref name="values"/>.
	/// </summary>
	/// <param name="values">The extent of the <see cref="Point3D"/> in the coordinate system.</param>
	/// <exception cref="ArgumentException">The length of <paramref name="values"/> is not 3.</exception>
	protected Point3D(Rational[] values) : base(values) { if (values.Length != 3) throw new ArgumentException(); }

	/// <summary>
	/// Constructs a <see cref="Point3D"/> by the given <paramref name="x"/>, <paramref name="y"/> and <paramref name="z"/> coordinates.
	/// </summary>
	/// <param name="x">The extent of the <see cref="Point3D"/> on the X axis.</param>
	/// <param name="y">The extent of the <see cref="Point3D"/> on the Y axis.</param>
	/// <param name="z">The extent of the <see cref="Point3D"/> on the Z axis.</param>
	public Point3D(Rational x, Rational y, Rational z) : base([x, y, z]) { }

	/// <summary>
	/// Constructs a <see cref="Point3D"/> by the coordinates of the given <paramref name="tuple"/>.
	/// </summary>
	/// <param name="tuple">The extent of the <see cref="Point3D"/>.</param>
	public Point3D((Rational X, Rational Y, Rational Z) tuple) : base([tuple.X, tuple.Y, tuple.Z]) { }

	#endregion

	#region Public methods

	/// <summary>
	/// Adds two <see cref="Point3D"/>s.
	/// </summary>
	/// <param name="left">The first <see cref="Point3D"/> to add.</param>
	/// <param name="right">The second <see cref="Point3D"/> to add.</param>
	/// <returns>The result of the calculation.</returns>
	public static Point3D Add(Point3D a, Point3D b) => new(Matrix.Add(a.values, b.values));

	/// <summary>
	/// Subtracts two <see cref="Point3D"/>s.
	/// </summary>
	/// <param name="left">The <see cref="Point3D"/> that represents the minuend.</param>
	/// <param name="right">The <see cref="Point3D"/> that represents the subtrahend.</param>
	/// <returns>The result value and if there was a swap in a tuple.</returns>
	public static Point3D Subtract(Point3D left, Point3D right) => new(Matrix.Subtract(left.values, right.values));

	/// <summary>
	/// Calculates the distance of two <see cref="Point3D"/>s.
	/// </summary>
	/// <param name="a">The first <see cref="Point3D"/>.</param>
	/// <param name="b">The second <see cref="Point3D"/>.</param>
	/// <returns>The result of the calculation.</returns>
	public static Rational Distance(Point3D a, Point3D b)
	{
		return Rational.SquareRoot(Rational.SecondPower(a.X - b.X) + Rational.SecondPower(a.Y - b.Y) + Rational.SecondPower(a.Z - b.Z));
	}

	#endregion

	#region Operators

	public static implicit operator Point3D((Rational, Rational, Rational) tuple) => new(tuple);
	public static Point3D operator +(Point3D a, Point3D b) => Add(a, b);
	public static Point3D operator -(Point3D a, Point3D b) => Subtract(a, b);

	#endregion
}