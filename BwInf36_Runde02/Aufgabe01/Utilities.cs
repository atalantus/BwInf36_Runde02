using System.Numerics;

namespace Aufgabe01
{
    /// <summary>
    /// Extra Methoden fuer Aufgabe 1
    /// </summary>
    public static class Utilities
    {
        // Keine Rekursion, wegen StackOverflowException Gefahr
        public static BigInteger FakultaetBerechnen(int n)
        {
            if (n == 0) return 1;
            BigInteger value = 1;
            for (int i = 1; i <= n; i++)
            {
                value *= i;
            }

            return value;
        }
    }
}