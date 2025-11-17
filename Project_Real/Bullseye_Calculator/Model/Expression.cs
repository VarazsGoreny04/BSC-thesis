namespace Bullseye_Calculator.Model;

public abstract class Expression
{
    internal abstract void ToPostfix(ref Stack<Expression> functions, ref List<Expression> result);
    internal abstract void ToTree(ref Stack<Expression> result);
    public abstract string ToStringByStep(ref int step);

    public override string ToString()
    {
        int step = 1;
        return ToStringByStep(ref step);
    }
}