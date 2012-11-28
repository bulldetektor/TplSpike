using FluentAssertions;
using NUnit.Framework;

namespace TplSpike
{
    [TestFixture]
    public class CalculatorTester
    {
        [TestFixture]
        public class When_adding_numbers
        {
            [Test]
            public void It_should_add_numbers()
            {
                var calc = new Calculator();

                var sum = calc.Add(1, 1);

                sum.Should().Be(2);
            }

            [Test]
            public void It_should_add_numbers_async()
            {
                var calc = new Calculator { TaskScheduler = new CurrentThreadTaskScheduler() };

                calc.AddAsync(1, 1);

                calc.GetLastSum().Should().Be(2);
            }

            [Test]
            public void It_should_summarize_range_of_numbers()
            {
                var calc = new Calculator();

                var sum = calc.Summarize(new[] { 1, 2, 3 });

                sum.Should().Be(6);
            }

            [Test]
            public void It_should_summarize_range_of_numbers_async()
            {
                var calc = new Calculator { TaskScheduler = new CurrentThreadTaskScheduler() };

                calc.SummarizeAsync(new[] { 1, 1, 1, 1 });

                calc.GetLastSum().Should().Be(4);
            }
        }

        [TestFixture]
        public class When_subtracting_numbers
        {
            [Test]
            public void It_should_subtract_numbers()
            {
                var calc = new Calculator();

                int answer = calc.Subtract(2, 1);

                answer.Should().Be(1);
            }

            [Test]
            public void It_should_subtract_numbers_async()
            {
                var calc = new Calculator { TaskScheduler = new CurrentThreadTaskScheduler() };

                calc.SubtractAsync(2, 1);

                calc.GetLastDiff().Should().Be(1);
            }
        }

        [TestFixture]
        public class When_adding_and_then_subtracting_numbers
        {
            [Test]
            public void It_should_add_and_subtract_numbers()
            {
                var calc = new Calculator();

                int sum = calc.Add(1, 1);
                int answer = calc.Subtract(sum, 1);

                answer.Should().Be(1);
            }

            [Test]
            public void It_should_add_and_subtract_numbers_async()
            {
                var calc = new Calculator { TaskScheduler = new CurrentThreadTaskScheduler() };

                calc.AddAndSubtractAsync(1, 2, 3, 4);

                calc.GetLastSum().Should().Be(3);
                calc.GetLastDiff().Should().Be(-1);
            }

        }


    }
}