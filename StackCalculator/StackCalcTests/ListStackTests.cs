using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stack_Calculator;

namespace StackCalcTests
{
    [TestClass]
    public class ListStackTests
    {
        [TestMethod]
        public void TestCalculator()
        {
            string string1 = "1 2 + 3 *";
            string string2 = "1 1 +";
            int correctNumber1 = 9;
            int correctNumber2 = 2;

            int stackSize = 100;
            ListStack stack = new ListStack(stackSize);

            double number1 = StackCalculator.Calculate(string1, stack);
            double number2 = StackCalculator.Calculate(string2, stack);
            Assert.AreEqual(correctNumber1, number1);
            Assert.AreEqual(correctNumber2, number2);
        }
    }
}