using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Tests
{
    // Тесты бизнес-логики калькулятора.
    [TestClass]
    public class EngineTests
    {
        [TestMethod]
        public void Immediate_Addition_Works()
        {
            var e = new CalculatorEngine();
            e.PressDigit('7');
            e.PressOperator(CalcOp.Add);
            e.PressDigit('3');
            e.PressOperator(CalcOp.Add);
            Assert.AreEqual("10", e.Display);
        }

        [TestMethod]
        public void Chain_Operations_Without_Priority()
        {
            var e = new CalculatorEngine();
            e.PressDigit('2');
            e.PressOperator(CalcOp.Add);
            e.PressDigit('3');
            e.PressOperator(CalcOp.Mul);
            e.PressDigit('4');
            e.PressEquals();
            Assert.AreEqual("20", e.Display);
        }

        [TestMethod]
        public void Replace_Operator_Before_Second_Operand()
        {
            var e = new CalculatorEngine();
            e.PressDigit('8');
            e.PressOperator(CalcOp.Mul);
            e.PressOperator(CalcOp.Sub);
            e.PressDigit('5');
            e.PressEquals();
            Assert.AreEqual("3", e.Display);
        }

        [TestMethod]
        public void Decimal_Input_And_Result()
        {
            var e = new CalculatorEngine();
            e.PressDigit('1');
            e.PressDot();
            e.PressDigit('5');
            e.PressOperator(CalcOp.Add);
            e.PressDigit('2');
            e.PressDot();
            e.PressDigit('5');
            e.PressEquals();
            Assert.AreEqual("4", e.Display);
        }

        [TestMethod]
        public void Sign_Toggle_Works()
        {
            var e = new CalculatorEngine();
            e.PressDigit('4');
            e.PressSign();
            e.PressOperator(CalcOp.Add);
            e.PressDigit('1');
            e.PressEquals();
            Assert.AreEqual("-3", e.Display);
        }

        [TestMethod]
        public void Backspace_Works()
        {
            var e = new CalculatorEngine();
            e.PressDigit('1');
            e.PressDigit('2');
            e.PressDigit('3');
            e.PressBackspace();
            Assert.AreEqual("12", e.Display);
        }

        [TestMethod]
        public void Divide_By_Zero_Shows_Error()
        {
            var e = new CalculatorEngine();
            e.PressDigit('9');
            e.PressOperator(CalcOp.Div);
            e.PressDigit('0');
            e.PressEquals();
            Assert.AreEqual("Error", e.Display);
        }

        [TestMethod]
        public void Equals_With_Pending_Operator_Reuses_Right()
        {
            var e = new CalculatorEngine();
            e.PressDigit('5');
            e.PressOperator(CalcOp.Add);
            e.PressEquals();
            Assert.AreEqual("10", e.Display);
        }

        [TestMethod]
        public void Clear_Resets_State()
        {
            var e = new CalculatorEngine();
            e.PressDigit('9');
            e.PressOperator(CalcOp.Mul);
            e.PressDigit('9');
            e.PressClear();
            Assert.AreEqual("0", e.Display);
        }
    }
}
