namespace Util
{
    /// <summary>
    ///     Contains Utility methods
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        ///     Gets a prime number higher than the input
        /// </summary>
        /// <param name="n">The input number</param>
        /// <returns>A prime higher than the input number. -1 if no higher prime was found</returns>
        public static int GetHigherPrime(int n)
        {
            var primes = new[] {503, 6029, 12979, 54413, 108727, 385817, 1002403, 10002191, 100003621, 1000002667};

            foreach (var prime in primes)
                if (prime > n)
                    return prime;

            return -1;
        }
    }
}