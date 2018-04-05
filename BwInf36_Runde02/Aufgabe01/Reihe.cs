using System.Collections.Generic;
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
        /// Die ID der Reihe (Index in der Reihen Matrix)
        /// </summary>
        public uint Id { get; }

        /// <summary>
        /// Enthaelt die Indizes der besetzten Fugen
        /// </summary>
        public HashSet<byte> BesetzteFugen { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Erzeugt eine neue Reihe
        /// </summary>
        /// <param name="kloetze">Die Kloetze der Reihe</param>
        /// <param name="id">Die ID der Reihe (Index in der Reihen Matrix)</param>
        public Reihe(byte[] kloetze, uint id)
        {
            Kloetze = kloetze;
            BesetzteFugen = new HashSet<byte>();
            byte zaehler = 0;
            for (var i = 0; i < Kloetze.Length - 1; i++)
            {
                zaehler += Kloetze[i];
                BesetzteFugen.Add(zaehler);
            }
            Id = id;
        }

        /// <summary>
        /// Checks if this struct has already been initialized
        /// </summary>
        /// <returns>True if the struct has been initialized</returns>
        public bool IsInitialized()
        {
            return Kloetze != null && BesetzteFugen != null;
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