using System.Text;

namespace Aufgabe01
{
    /// <summary>
    /// Repraesentiert eine Reihe von Klotzen
    /// </summary>
    public struct Reihe
    {
        #region Properties

        /// <summary>
        /// Enthaelt die Kloetze in dieser Reihe
        /// </summary>
        public byte[] Kloetze { get; }

        /// <summary>
        /// Die ID der Reihe
        /// </summary>
        public uint Id { get; private set; }

        /// <summary>
        /// Enthaelt die Freien Fugen in der Reihe
        ///
        /// Beim durchiterieren immer 'Length - 1', 
        /// da das Ende der Mauer die letzte Fuge ist, 
        /// obwohl es eigentlich keine Fuge ist
        /// </summary>
        public bool[] FreieFugen { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Erzeugt ein neues <see cref="Reihe"/> Objekt
        /// </summary>
        /// <param name="n">Anzahl der Kloetze in einer Reihe</param>
        /// <param name="breite">Breite einer Reihe</param>
        /// <param name="id">Index der Reihe in der Permutations Collection</param>
        public Reihe(int n, int breite, uint id)
        {
            Kloetze = new byte[n];
            Kloetze.FillArray((byte) 1); // 0x1; 0b0000_0001
            FreieFugen = new bool[breite];
            FreieFugen.FillArray(true);
            Id = id;
        }

        public Reihe(byte[] kloetze, int breite, uint id)
        {
            Kloetze = kloetze; // HACK: so oder anders (referenz)
            FreieFugen = new bool[breite];
            FreieFugen.FillArray(true);
            ushort zaehler = 0;
            for (var i = 0; i < Kloetze.Length; i++)
            {
                zaehler += Kloetze[i];
                FreieFugen[zaehler - 1] = false;
            }
            Id = id;
        }

        /// <summary>
        /// Checks if this struct has already been initialized
        /// </summary>
        /// <returns>True if the struct has been initialized</returns>
        public bool IsInitialized()
        {
            return Kloetze != null;
        }

        /// <summary>
        /// Ueberprueft, ob zwei Objekte der Klasse <see cref="Reihe"/> gleich sind
        /// </summary>
        /// <param name="other">Die zu vergleichende Reihe</param>
        /// <returns>True wenn die Objekte gleich sind</returns>
        public bool Equals(Reihe other)
        {
            return Equals(Kloetze, other.Kloetze) && Id == other.Id && Equals(FreieFugen, other.FreieFugen);
        }

        /// <summary>
        /// Formatiert die Reihe mit ihren einzelnen Kloetzen zu einem String
        /// </summary>
        /// <returns>Die Reihe als String</returns>
        public override string ToString()
        {
            var reihe = new StringBuilder("|");
            foreach (var klotz in Kloetze)
            {
                reihe.Append($"{klotz}|");
            }
            return reihe.ToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Reihe reihe && Equals(reihe);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Kloetze != null ? Kloetze.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)Id;
                hashCode = (hashCode * 397) ^ (FreieFugen != null ? FreieFugen.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Reihe r1, Reihe r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(Reihe r1, Reihe r2)
        {
            return !r1.Equals(r2);
        }

        #endregion
    }
}