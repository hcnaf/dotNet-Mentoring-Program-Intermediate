/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Task<int[]> task1 = new Task<int[]>(() =>
            {
                var random = new Random();
                var values = new int[10];

                for (int i = 0; i < values.Length; ++i)
                    values[i] = random.Next(1, 100);

                Console.WriteLine(values.ToView());

                Console.WriteLine();

                return values;
            });


            Task<int[]> task2 = task1.ContinueWith(previous =>
            {
                Random random = new Random();
                int randomValue = random.Next(1, 100);

                Console.WriteLine($"Random value: {randomValue}");

                for (int i = 0; i < previous.Result.Length; ++i)
                    previous.Result[i] *= randomValue;

                Console.WriteLine(previous.Result.ToView());

                Console.WriteLine();

                return previous.Result;
            });

            Task<int[]> task3 = task2.ContinueWith(previous =>
            {
                Array.Sort(previous.Result);

                Console.WriteLine(previous.Result.ToView());

                Console.WriteLine($"Current Task: {Task.CurrentId}  Previous Task: {previous.Id}");

                return previous.Result;
            });


            Task task4 = task3.ContinueWith(previous =>
                Console.WriteLine($"Average value: {previous.Result.Average()}"));

            task1.Start();
            task4.Wait();

            Console.ReadLine();
        }

        public static string ToView(this int[] array)
            => string.Join(", ", array);
    }
}
