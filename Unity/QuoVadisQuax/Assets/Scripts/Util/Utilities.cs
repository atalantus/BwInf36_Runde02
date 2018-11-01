namespace Util
{
    public static class Utilities
    {
        public static int GetHigherPrime(int n)
        {
            var primes = new int[] {503, 6029, 12979, 54413, 108727, 385817, 1002403, 10002191, 100003621, 1000002667};

            for (int i = 0; i < primes.Length; i++)
            {
                if (primes[i] > n) return primes[i];
            }

            return -1;
        }
    }
}