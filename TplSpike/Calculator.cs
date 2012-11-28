using System;
using System.Linq;
using System.Threading.Tasks;

namespace TplSpike
{
    public class Calculator
    {
        public event EventHandler<int> CalculationCompleted;

        public TaskScheduler TaskScheduler
        {
            get
            {
                return _taskScheduler
                    ?? (_taskScheduler = TaskScheduler.Default);
            }
            set { _taskScheduler = value; }
        }
        private TaskScheduler _taskScheduler;

        public TaskFactory TaskFactory
        {
            get
            {
                return _taskFactory
                    ?? (_taskFactory = new TaskFactory(TaskScheduler));
            }
            set { _taskFactory = value; }
        }
        private TaskFactory _taskFactory;

        public int Add(int augend, int addend)
        {
            _lastSum = augend + addend;
            return _lastSum;
        }

        public Task<int> AddAsync(int augend, int addend)
        {
            return TaskFactory.StartNew(() => Add(augend, addend));
        }

        public int GetLastSum()
        {
            return _lastSum;
        }


        public int Subtract(int minuend, int subtrahend)
        {
            _lastDiff = minuend - subtrahend;
            return _lastDiff;
        }

        public Task<int> SubtractAsync(int minuend, int subtrahend)
        {
            return TaskFactory.StartNew(() => Subtract(minuend, subtrahend));
        }

        public int GetLastDiff()
        {
            return _lastDiff;
        }


        public void AddAndSubtractAsync(int augend, int addend, int minuend, int subtrahend)
        {
            var addTask = AddAsync(augend, addend);
            var subtractTask = SubtractAsync(minuend, subtrahend);

            TaskFactory.ContinueWhenAll(
                new[] { addTask, subtractTask },
                tasks => CalculationCompleted(this, 0));
        }


        public int Summarize(int[] addends)
        {
            return addends.Sum();
        }

        public void SummarizeAsync(int[] addends)
        {
            var work = new Task<int>[2];

            work[0] = new TaskFactory<int>(_taskScheduler)
                .StartNew(() => Summarize(addends.Take(addends.Length / 2).ToArray()));
            work[1] = new TaskFactory<int>(_taskScheduler)
                .StartNew(() => Summarize(addends.Skip(addends.Length / 2).ToArray()));

            TaskFactory
                .ContinueWhenAll(
                    work,
                    antecendants =>
                    {
                        _lastSum = work[0].Result + work[1].Result;
                    });
        }

        
        private int _lastSum;
        private int _lastDiff;
    }
}