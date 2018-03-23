using System;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.CompilerServices;
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
        public int[] Kloetze { get; }

        /// <summary>
        /// Die ID der Reihe
        /// </summary>
        public int ID { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Erzeugt ein neues <see cref="Reihe"/> Objekt
        /// </summary>
        /// <param name="n">Anzahl der Kloetze in einer Reihe</param>
        /// <param name="id">Index der Reihe in der Permutations Collection</param>
        public Reihe(int n, int id)
        {
            Kloetze = new int[n];
            Kloetze.FillArray(1);
            ID = id;
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

        #endregion
    }
}