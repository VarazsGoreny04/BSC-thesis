using Bullseye_Calculator.Model.Standard;

namespace Bullseye_Calculator.Model.EuclideanSpace;

public class MatrixHolder : Expression
{
    private readonly ValueHolder[,] value;

    public MatrixHolder(ValueHolder[,] value) => this.value = value;

    internal override void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result) => result.Add(this);
    internal override void ToTree(ref Stack<Expression> result) => result.Push(this);
	public override string ToStringByStep(ref int step)
    {
        int rowCount = value.GetLength(0);
        int colCount = value.GetLength(1);

        if (rowCount < 1 || colCount < 1)
            return "[ ]";

        string text = "[\n";

        for (int i = 0; i < rowCount; ++i)
        {
            text += $"\t{value[i, 0].ToStringByStep(ref step)}";

            for (int j = 1; j < colCount; ++j)
                text += $",\t{value[i, j].ToStringByStep(ref step)}";

            text += ";\n";
        }

        return text + "]";
    }
}