using System;

namespace Cooper_ParallelAssignment1
{
    class Program
    {
        //Parallel Processing Algorithm with a quad core processor
        const int numCores = 4;
        static int[] arr;
        static int[] arr2;
        static int numThreadsDone;
        static int nextID;
        static object locker1 = new object();
        static object locker2 = new object();
        static long pSaveTicks;
        static DateTime pt;
        static double fact1 = 0.9999999;
        //variable to hold sum
        static double sum = 0.0;
        //Constant for size
        const int size = 1000000000;
        static void doMath()
        {
            int id;
            double localSum = 0;
            lock(locker1)
            {
                id = nextID;
                nextID++;
            }
            //assumes array is evenly divisble by the number of threads
            int granularity = arr2.Length / numCores;
            int start = granularity * id;
            long pSaveTicks = pt.Ticks;
            for(int i = start; i<start+granularity; i++)
            {
                localSum += Math.Pow(fact1, (double)i);
            }
            lock(locker2)
            {
                sum += localSum;
                numThreadsDone++;
                if (numThreadsDone == numCores)
                {
                    pt = DateTime.Now;
                    Console.WriteLine("Parallel Algorithm: "
                        + ((pt.Ticks - pSaveTicks)-1000000000) + " seconds");
                    Console.Write("Parallel Count is: " + sum);
                }
            }
        }
        static void Main(string[] args)
        {

            //Sets up a timer
            DateTime t = DateTime.Now;
            long saveTicks = t.Ticks;
            //for loop to sum from 1 to 1 million * 0.9999999
            for(int i = 1; i <= size; i++)
            {
                sum += (fact1 * i * i);
                fact1 *= 0.9999999;
            }
            //Saves the ticks at the end of execution
            t = DateTime.Now;
            Console.WriteLine("Sequential: " + ((t.Ticks - saveTicks) / 1000000000.0) + " seconds");
            Console.WriteLine("Sum is " + sum);
            //Resets the sum
            sum = 0.0;
            //Calls the parallel processing algorithm
            doMath();
            //Waits for a key to be pressed before the console closes
            Console.ReadKey();


        }
    }
}
