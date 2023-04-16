using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    public class ReadWriteMonitorOperation
    {
        private readonly int _iterations;
        private readonly object _locker = new object();
        private readonly List<int> _items = new List<int>();

        public ReadWriteMonitorOperation(int iterations)
        {
            _iterations = iterations;
        }

        public void Write()
        {
            for (int i = 1; i <= _iterations; i++)
            {
                Monitor.Enter(_locker);

                Console.WriteLine("Task " + Task.CurrentId + " added " + i);
                Console.WriteLine("");

                _items.Add(i);

                Monitor.Pulse(_locker);
                Monitor.Wait(_locker);
            }
        }

        public void Read()
        {
            for (int i = 1; i <= _iterations; i++)
            {
                Monitor.Enter(_locker);

                Console.WriteLine("Task " + Task.CurrentId + " readed ");

                foreach (int item in _items)
                {
                    Console.Write(item + " ");
                }

                Console.WriteLine("");
                Console.WriteLine("");

                Monitor.Pulse(_locker);
                Monitor.Wait(_locker);

                Thread.Sleep(1000);
            }
        }
    }
}