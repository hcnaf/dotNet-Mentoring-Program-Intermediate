using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    public class ReadWriteLockSlimOperation
    {
        private readonly int _iterations;
        private readonly List<int> _items = new List<int>();
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

        public ReadWriteLockSlimOperation(int iterations) { _iterations = iterations; }

        public void Write()
        {
            for (int i = 1; i <= _iterations; i++)
            {
                _rwLock.EnterWriteLock();
                int newNumber = i;

                _items.Add(newNumber);

                Console.WriteLine("");
                Console.WriteLine("Thread " + Task.CurrentId + " added " + newNumber);

                _rwLock.ExitWriteLock();
            }
        }

        public void Read()
        {
            while (_items.Count < _iterations)
            {
                _rwLock.EnterReadLock();

                Console.WriteLine("Thread " + Task.CurrentId + " readed ");

                foreach (int item in _items)
                {
                    Console.Write(" " + item + " ");
                }

                Console.WriteLine("");
                Console.WriteLine("");

                _rwLock.ExitReadLock();
            }
        }
    }
}
