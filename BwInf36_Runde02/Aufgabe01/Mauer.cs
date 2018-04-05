using System;
using System.Collections.Generic;
using System.Text;

namespace Aufgabe01
{
    /// <summary>
    /// Repraesentiert eine Mauer mit mehreren <see cref="Reihe"/>n
    /// </summary>
    public struct Mauer
    {
        #region Properties

        /// <summary>
        /// Einzelnen Reihen der Mauer
        /// </summary>
        public Reihe[] Reihen { get; }

        /// <summary>
        /// Enthaelt die Indizes der besetzten Fugen
        /// </summary>
        public HashSet<byte> BesetzteFugen { get; }

        /// <summary>
        /// Ist die Mauer fertig/voll (Sind alle Reihen gesetzt)
        /// </summary>
        public bool Fertig { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Erzeugt ein neues <see cref="Mauer"/> Objekt
        /// </summary>
        /// <param name="maxHoehe">Die maximale Hoehe der Mauer</param>
        public Mauer(int maxHoehe)
        {
            BesetzteFugen = new HashSet<byte>();
            Reihen = new Reihe[maxHoehe];
            Fertig = false;
        }

        /// <summary>
        /// Erzeugt eine neues <see cref="Mauer"/> Objekt
        /// </summary>
        /// <param name="mauer">Die Mauer, die "geklont" wird</param>
        public Mauer(Mauer mauer)
        {
            BesetzteFugen = new HashSet<byte>();
            Reihen = new Reihe[mauer.Reihen.Length];
            Fertig = false;

            for (var i = 0; i < mauer.Reihen.Length; i++)
            {
                if (mauer.Reihen[i].IsInitialized()) AddReihe(mauer.Reihen[i]);
            }
        }

        /// <summary>
        /// Fuegt der Mauer eine neue Reihe hinzu, wenn noch Platz ist
        /// </summary>
        /// <param name="newReihe">Die Reihe, die hinzugefuegt werden soll</param>
        public Mauer AddReihe(Reihe newReihe)
        {
            if (!newReihe.IsInitialized()) return this;
            for (var i = 0; i < Reihen.Length; i++)
            {
                if (!Reihen[i].IsInitialized())
                {
                    Reihen[i] = newReihe;

                    BesetzteFugen.UnionWith(newReihe.BesetzteFugen);

                    if (i == Reihen.Length - 1) // Letzte Reihe wurde gesetzt. Mauer ist voll
                        Fertig = true;

                    return this;
                }
            }

            return this;
        }

        /// <summary>
        /// Formatiert die Mauer mit ihren einzelnen Reihen zu einem String
        /// </summary>
        /// <returns>Die Mauer als String</returns>
        public override string ToString()
        {
            var mauer = new StringBuilder();
            for (var i = 0; i < Reihen.Length; i++)
            {
                mauer.Append(Reihen[i].ToString());
                if (i < Reihen.Length - 1) mauer.Append("\n");
            }
            return mauer.ToString();
        }

        #endregion
    }
}
