using FizzBuzz.Logic;

namespace FizzBuzz.Tests
{
    public class Tests
    {
        Logic.FizzBuzz fb;

        [SetUp]
        public void Setup()
        {
            fb = new Logic.FizzBuzz();
        }

        [Test]
        public void PrintNumbers_ShouldReturnFizz_WhenNumberIsDivisibleByThree()
        {
            var result = fb.GetResult(3);

            Assert.That(result, Is.EqualTo("Fizz"));
        }

        [Test]
        public void PrintNumbers_ShouldReturnBuzz_WhenNumberIsDivisibleByFive()
        {
            var result = fb.GetResult(5);

            Assert.That(result, Is.EqualTo("Buzz"));
        }

        [Test]
        public void PrintNumbers_ShouldReturnFizzBuzz_WhenNumberIsDivisibleByThreeAndFive()
        {
            var result = fb.GetResult(15);

            Assert.That(result, Is.EqualTo("FizzBuzz"));
        }

        [Test]
        public void PrintNumbers_ShouldThrowException_WhenNumberIsOutOfRange()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => fb.GetResult(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => fb.GetResult(101));
        }
    }

}
