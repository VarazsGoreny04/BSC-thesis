using System;

namespace Bullseye_Calculator.Model.EuclideanSpace;

public class Diagonalize : Function<Matrix>
{
	public ValueHolder<Matrix> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Diagonalize() : base(1) { }

	public Diagonalize(ValueHolder<Matrix> parameter) : base(1) => Parameter = parameter;

	public override Matrix GetValue() => Matrix.Diagonalize(Matrix.ToRationalMatrix(Parameter?.GetValue().GetValue() ?? throw new FormatException()), 3).Eigenvalues;

	public override string Sign() => "diag";
}

public class Inverse : Function<Matrix>
{
	public ValueHolder<Matrix> Parameter
	{
		get => parameters[0];
		set => parameters[0] = value;
	}

	public Inverse() : base(1) { }

	public Inverse(ValueHolder<Matrix> parameter) : base(1) => Parameter = parameter;

	public override Matrix GetValue() => Matrix.Inverse(Matrix.ToRationalMatrix(Parameter?.GetValue().GetValue() ?? throw new FormatException()));

	public override string Sign() => "inv";
}