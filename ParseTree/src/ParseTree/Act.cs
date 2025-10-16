namespace ParseTreeLang
{
    public interface INode
    {
        int Evaluate();
        string Print();
    }

    public sealed class NumberNode : INode
    {
        public int Value { get; }
        public NumberNode(int value) => Value = value;
        public int Evaluate() => Value;
        public string Print() => Value.ToString();
    }

    public enum OperatorType { Add, Sub, Mul, Div }

    public sealed class BinaryOpNode : INode
    {
        public OperatorType Operator { get; }
        public INode Left { get; }
        public INode Right { get; }

        public BinaryOpNode(OperatorType op, INode left, INode right)
        {
            Operator = op;
            Left = left;
            Right = right;
        }

        public int Evaluate()
        {
            int l = Left.Evaluate();
            int r = Right.Evaluate();
            return Operator switch
            {
                OperatorType.Add => l + r,
                OperatorType.Sub => l - r,
                OperatorType.Mul => l * r,
                OperatorType.Div => r == 0
                    ? throw new EvaluationException("Деление на ноль.")
                    : l / r,
                _ => throw new EvaluationException("Неизвестная операция.")
            };
        }

        public string Print()
        {
            string op = Operator switch
            {
                OperatorType.Add => "+",
                OperatorType.Sub => "-",
                OperatorType.Mul => "*",
                OperatorType.Div => "/",
                _ => "?"
            };

            return $"( {op} {Left.Print()} {Right.Print()} )";
        }
    }
}
