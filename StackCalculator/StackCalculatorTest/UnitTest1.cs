namespace StackCalculatorTest;
using Stack_Calculator;

[TestClass]
public class ArrayStackTests
{
    [TestMethod]
    public void TestCalculator()
    {
        string string1 = "1 2 + 3 *";
        string string2 = "1 1 +";
        double correctNumber1 = 9;
        double correctNumber2 = 2;

        int stackSize = 100;
        ArrayStack stack = new ArrayStack(stackSize);
        
        double number1 = StackCalculator.Calculate(string1, stack);
        double number2 = StackCalculator.Calculate(string2, stack);
        Assert.IsTrue(StackCalculator.nearlyEqual(correctNumber1, number1));
        Assert.IsTrue(StackCalculator.nearlyEqual(correctNumber2, number2));
    }
}