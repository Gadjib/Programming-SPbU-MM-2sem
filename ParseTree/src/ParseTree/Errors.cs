using System;

namespace ParseTreeLang
{
    public sealed class ParseException : Exception
    {
        public ParseException(string message) : base(message) { }
    }

    public sealed class EvaluationException : Exception
    {
        public EvaluationException(string message) : base(message) { }
    }
}
