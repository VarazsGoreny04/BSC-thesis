using Project_Real;

namespace Bullseye_Calculator.Model.EuclideanSpace;

public class Point2D
{
	protected readonly Rational[] values;

	public Rational X => values[0];
	public Rational Y => values[1];

	protected Point2D(Rational[] values) => this.values = values;
	public Point2D(Rational x, Rational y) => values = [x, y];
	public Point2D((Rational X, Rational Y) tuple) => values = [tuple.X, tuple.Y];

	public static Point2D Add(Point2D a, Point2D b) => new(Matrix.Add(a.values, b.values));
	public static Point2D Subtract(Point2D a, Point2D b) => new(Matrix.Subtract(a.values, b.values));

	public override string ToString() => $"({X}, {Y})";


	public static implicit operator Point2D((Rational, Rational) tuple) => new(tuple);
	public static Point2D operator +(Point2D a, Point2D b) => Add(a, b);
	public static Point2D operator -(Point2D a, Point2D b) => Subtract(a, b);
}

public class Point3D : Point2D
{
	public Rational Z => values[2];

	protected Point3D(Rational[] values) : base(values) { }
	public Point3D(Rational x, Rational y, Rational z) : base([x, y, z]) { }
	public Point3D((Rational X, Rational Y, Rational Z) tuple) : base([tuple.X, tuple.Y, tuple.Z]) { }

	public static Point3D Add(Point3D a, Point3D b) => new(Matrix.Add(a.values, b.values));
	public static Point3D Subtract(Point3D a, Point3D b) => new(Matrix.Subtract(a.values, b.values));

	public override string ToString() => $"({X}, {Y}, {Z})";


	public static implicit operator Point3D((Rational, Rational, Rational) tuple) => new(tuple);
	public static Point3D operator +(Point3D a, Point3D b) => Add(a, b);
	public static Point3D operator -(Point3D a, Point3D b) => Subtract(a, b);
}