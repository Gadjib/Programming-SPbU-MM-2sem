using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParseTreeLang;

namespace ParseTree.Tests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void Sample_Works()
        {
            var p = new Parser("(* (+ 1 1) 2)");
            var root = p.Parse();

            Assert.AreEqual("( * ( + 1 1 ) 2 )", root.Print());
            Assert.AreEqual(4, root.Evaluate());
        }

        [TestMethod]
        [DataRow("(* 1)")]
        [DataRow("(+ 1)")]
        [DataRow("(* (+ 1 1) 2")]
        public void Invalid_Syntax_Throws(string input)
        {
            var p = new Parser(input);
            Assert.ThrowsException<ParseException>(() => new Parser(input).Parse());
        }

        [TestMethod]
        public void Division_By_Zero_Throws()
        {
            var p = new Parser("(/ 10 0)");
            var root = p.Parse();
            Assert.ThrowsException<EvaluationException>(() => root.Evaluate());
        }

        [TestMethod]
        public void Negative_Number_Literal_Works()
        {
            var p = new Parser("(* -3 (+ 1 2))");
            var root = p.Parse();
            Assert.AreEqual("( * -3 ( + 1 2 ) )", root.Print());
            Assert.AreEqual(-9, root.Evaluate());
        }
    }
}
