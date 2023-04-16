/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            RunThreadsRecursively(10);
            Console.WriteLine();
            RunThreadsThreadPoolRecursively(10);

            Console.ReadLine();
        }

        static void RunThreadsRecursively(object num)
        {
            int n = (int)num;
            if (n <= 0)
                return;

            Thread t = new Thread(RunThreadsRecursively);
            t.Name = t.ManagedThreadId.ToString();
            t.Start(n - 1);


            Console.Write(" " + n + " ");
            Console.WriteLine(t.Name);

            t.Join();
        }

        static void RunThreadsThreadPoolRecursively(object num)
        {
            int n = (int)num;
            if (n <= 0)
                return;


            Thread.CurrentThread.Name = n.ToString();
            ThreadPool.QueueUserWorkItem(new WaitCallback(RunThreadsThreadPoolRecursively), n - 1);


            Console.Write(" " + n + " ");
            Console.WriteLine(Thread.CurrentThread.Name);
        }
    }
}
