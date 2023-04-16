/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            var task1 = new Task<int>(() => Sum(CancellationToken.None, 10000));
            var task2 = new Task<int>(() => Sum(CancellationToken.None, 10000));

            task1.ContinueWith(result => Console.WriteLine("Faulted ..."), TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.AttachedToParent);
            task2.ContinueWith(result => Console.WriteLine("Success ..."), TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.AttachedToParent);

            task1.Start();
            task2.Start();

            CancellationTokenSource cancelToken1 = new CancellationTokenSource();
            Task task3 = Task.Factory.StartNew
                    (
                        () =>
                        {
                            throw new Exception("error");
                        }, cancelToken1.Token).ContinueWith(t =>
                        {
                            Console.WriteLine("A " + t.Status);
                        }
                         , cancelToken1.Token
                         , TaskContinuationOptions.NotOnFaulted
                         , TaskScheduler.Current
                      )
                      .ContinueWith
                          (
                             t =>
                             {
                                 Console.WriteLine("B " + t.Status + " " + Task.CurrentId);
                             }
                             , cancelToken1.Token
                           );

            CancellationTokenSource cancelToken2 = new CancellationTokenSource();
            Task task4 = Task.Factory.StartNew
                    (
                        () =>
                        {
                            throw new Exception("error");
                        }, cancelToken2.Token).ContinueWith(t =>
                        {
                            Console.WriteLine("A " + t.Status);
                        }
                         , cancelToken2.Token
                         , TaskContinuationOptions.OnlyOnCanceled
                         , TaskScheduler.Current
                      )
                      .ContinueWith
                          (
                             t =>
                              Task.Run(() =>
                              {
                                  Console.WriteLine("B " + t.Status);
                              })
                             , cancelToken2.Token
                           );

            try
            {
                task1.Wait();
                task2.Wait();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("Exception: {0}", ex.InnerException.Message);
            }

            Console.WriteLine("Press <Enter> to continue ...");
            Console.ReadLine();
        }

        private static int Sum(CancellationToken token, int number)
        {
            throw new Exception("Error ...");

            int sum = 0;
            for (; number > 0; number--)
            {
                token.ThrowIfCancellationRequested();

                checked
                {
                    sum += number;
                }
            }
            return sum;
        }
    }
}
