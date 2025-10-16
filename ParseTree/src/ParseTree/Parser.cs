using System;
using System.Collections.Generic;
using System.Linq;

namespace ParseTreeLang
{
    internal enum TokenType { LParen, RParen, Op, Num, EOF }
    internal readonly record struct Token(TokenType Type, string Lexeme, int Pos);

    internal sealed class Tokenizer
    {
        private readonly string _text;
        private int _i;

        public Tokenizer(string text) => _text = text ?? string.Empty;

        public IEnumerable<Token> Tokenize()
        {
            while (true)
            {
                SkipWs();
                if (_i >= _text.Length) { yield return new Token(TokenType.EOF, "", _i); yield break; }

                char c = _text[_i];

                if (c == '(') { yield return new Token(TokenType.LParen, "(", _i++); continue; }
                if (c == ')') { yield return new Token(TokenType.RParen, ")", _i++); continue; }

                if (c is '+' or '*' or '/' || c == '-')
                {
                    if (c == '-' && _i + 1 < _text.Length && char.IsDigit(_text[_i + 1]))
                    {
                        yield return ReadNumber();
                        continue;
                    }
                    _i++;
                    yield return new Token(TokenType.Op, c.ToString(), _i - 1);
                    continue;
                }

                if (char.IsDigit(c))
                {
                    yield return ReadNumber();
                    continue;
                }

                throw new Exception($"Недопустимый символ '{c}' на позиции {_i}.");
            }
        }

        private Token ReadNumber()
        {
            int start = _i;
            if (_text[_i] == '-') _i++;

            while (_i < _text.Length && char.IsDigit(_text[_i])) _i++;

            return new Token(TokenType.Num, _text[start.._i], start);
        }

        private void SkipWs()
        {
            while (_i < _text.Length && char.IsWhiteSpace(_text[_i])) _i++;
        }
    }

    public sealed class Parser
    {
        private readonly List<Token> _toks;
        private int _p;

        public Parser(string text)
        {
            try { _toks = new Tokenizer(text).Tokenize().ToList(); }
            catch (Exception ex) { throw new ParseException(ex.Message); }
        }

        public INode Parse()
        {
            _p = 0;
            var node = ParseExpr();
            if (Peek().Type != TokenType.EOF)
                throw new ParseException($"Лишние символы после выражения (позиция {Peek().Pos}).");
            return node;
        }

        private INode ParseExpr()
        {
            var t = Peek();
            return t.Type switch
            {
                TokenType.Num => ParseNumber(),
                TokenType.LParen => ParseParenExpr(),
                _ => throw new ParseException($"Ожидалось число или '(' на позиции {t.Pos}.")
            };
        }

        private INode ParseNumber()
        {
            var t = Consume(TokenType.Num, "Ожидалось число");
            if (!int.TryParse(t.Lexeme, out int v))
                throw new ParseException($"Некорректное число '{t.Lexeme}' (позиция {t.Pos}).");
            return new NumberNode(v);
        }

        private INode ParseParenExpr()
        {
            Consume(TokenType.LParen, "Ожидалась '('");

            var opTok = Consume(TokenType.Op, "Ожидался оператор (+ - * /)");
            var op = opTok.Lexeme switch
            {
                "+" => OperatorType.Add,
                "-" => OperatorType.Sub,
                "*" => OperatorType.Mul,
                "/" => OperatorType.Div,
                _ => throw new ParseException($"Неизвестный оператор '{opTok.Lexeme}' (позиция {opTok.Pos}).")
            };

            var left = ParseExpr();
            var right = ParseExpr();

            Consume(TokenType.RParen, "Ожидалась ')'");

            return new BinaryOpNode(op, left, right);
        }

        private Token Peek() => _toks[_p];

        private Token Consume(TokenType type, string msgIfMismatch)
        {
            var t = Peek();
            if (t.Type != type)
                throw new ParseException($"{msgIfMismatch}: позиция {t.Pos}.");
            _p++;
            return t;
        }
    }
}
