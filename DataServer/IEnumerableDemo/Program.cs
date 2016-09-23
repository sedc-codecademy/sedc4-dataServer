using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IEnumerableDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new NumberCollection();

            //var last = GetSomePrimes().Last();

            var primes = GetAllPrimes().Take(10).ToList();

            Console.WriteLine("Generated Primes");
            foreach (var prime in primes)//.TakeWhile(n => n < 1000))
            {
                Console.WriteLine(prime);
            }

            Console.WriteLine("------------");

            foreach (var item in new Foreachable())
            {
                Console.WriteLine(item);
            }
        }

        static IEnumerable<int> GetSomePrimes()
        {
            Console.WriteLine("Starting prime generation");
            yield return 2;
            Console.WriteLine("Returning 3");
            yield return 3;
            Console.WriteLine("Returning 5");
            yield return 5;
            Console.WriteLine("Returning 7");
            yield return 7;
            Console.WriteLine("Finishing up");
        }

        static IEnumerable<int> GetAllPrimes()
        {
            yield return 2;
            var pprime = 3;
            while (true)
            {
                var sqrt = (int)Math.Sqrt(pprime);
                var isPrime = true;
                for (int i = 3; i < sqrt; i += 2)
                {
                    if (pprime % i == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                    yield return pprime;
                pprime += 2;
                Thread.Sleep(100);
            }
        }

        class Foreachable
        {
            public class Whatever
            {
                public Whatever(int number) {
                    Current = number;
                    returned = false;
                }

                public bool returned;

                public int Current { get; set; }
                public bool MoveNext()
                {
                    if (!returned)
                    {
                        returned = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            public Whatever GetEnumerator()
            {
                return new Whatever(14);
            }
        }
    }

}
