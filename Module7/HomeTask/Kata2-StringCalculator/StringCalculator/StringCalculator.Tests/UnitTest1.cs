using StringCalculator.Logic;

namespace StringCalculator.Tests
{
    public class Tests
    {
        [Test]
        public void Add_ShouldReturnZero_WhenInputIsEmptyString()
        {
            var result = Calculator.Add("");
            
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Add_ShouldReturnNumber_WhenInputHasSingleNumber()
        {

            var result = Calculator.Add("1");

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void Add_ShouldReturnSum_WhenInputHasTwoNumbers()
        {

            var result = Calculator.Add("1,2");

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void Add_ShouldHandleNewLineDelimiter()
        {

            var result = Calculator.Add("1\n2,3");

            Assert.That(result, Is.EqualTo(6));
        }

        [Test]
        public void Add_ShouldSupportCustomDelimiter()
        {

            var result = Calculator.Add("//;\n1;2");

            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void Add_ShouldThrowException_WhenNegativeNumbersAreProvided()
        {
            var ex = Assert.Throws<ArgumentException>(() => Calculator.Add("1,-2,-3"));
            Assert.That(ex.Message, Is.EqualTo("Negatives not allowed: -2, -3"));
        }

        [Test]
        public void Add_ShouldIgnoreNumbersGreaterThan1000()
        {

            var result = Calculator.Add("2,1001");

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void Add_ShouldSupportCustomDelimitersOfAnyLength()
        {

            var result = Calculator.Add("//[***]\n1***2***3");

            Assert.That(result, Is.EqualTo(6));
        }

        [Test]
        public void Add_ShouldSupportMultipleDelimiters()
        {

            var result = Calculator.Add("//[*][%]\n1*2%3");

            Assert.That(result, Is.EqualTo(6));
        }

        [Test]
        public void Add_ShouldSupportMultipleDelimitersWithDifferentLengths()
        {

            var result = Calculator.Add("//[***][%%]\n1***2%%3");

            Assert.That(result, Is.EqualTo(6));
        }
    }
}