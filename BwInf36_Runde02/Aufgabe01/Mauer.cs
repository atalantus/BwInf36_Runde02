using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Reihe[] Reihen { get; private set; }

        /// <summary>
        /// <seealso cref="https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/indexers/"/>
        /// </summary>
        public Reihe this[int i]
        {
            set { AddReihe(i, value); }
            get { return Reihen[i]; }
        }

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
        /// Erzeugt ein neues <see cref="Mauer"/> Objekt
        /// </summary>
        /// <param name="maxHoehe">Die maximale Hoehe der Mauer</param>
        /// <param name="maxBreite">Die maximale Breite der Mauer</param>
        public Mauer(int maxHoehe, int maxBreite)
        {
            FreieFugen = new bool[maxBreite];
            FreieFugen.FillArray(true);
            Reihen = new Reihe[maxHoehe];
        }

        /// <summary>
        /// Fuegt der Mauer eine neue Reihe hinzu
        /// </summary>
        /// <param name="index">Index der neuen Reihe</param>
        /// <param name="reihe"></param>
        private void AddReihe(int index, Reihe reihe)
        {
            Reihen[index] = reihe;
        }

        /// <summary>
        /// Formatiert die Mauer mit ihren einzelnen Reihen zu einem String
        /// </summary>
        /// <returns>Die Mauer als String</returns>
        public override string ToString()
        {
            var mauer = new StringBuilder();
            for (int i = 0; i < Reihen.Length; i++)
            {
                mauer.Append(Reihen[i].ToString());
                if (i < Reihen.Length - 1) mauer.Append("\n");
            }
            return mauer.ToString();
        }

        #endregion
    }
}
