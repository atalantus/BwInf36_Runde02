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
        public Reihe[] Reihen { get; private set; }

        /// <summary>
        /// Enthaelt die Freien Fugen in der Mauer
        ///
        /// Beim durchiterieren immer 'Length - 1', 
        /// da das Ende der Mauer die letzte Fuge ist, 
        /// obwohl es eigentlich keine Fuge ist
        /// </summary>
        public bool[] FreieFugen { get; private set; }

        /// <summary>
        /// Die Id der Mauer. Mauern mit den gleichen Reihen haben auch die gleiche ID
        /// </summary>
        public uint Id { get; private set; }

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
            Id = 0;
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
                    for (var j = 0; j < newReihe.FreieFugen.Length - 1; j++)
                    {
                        if (!newReihe.FreieFugen[j]) FreieFugen[j] = false;
                    }

                    Id += newReihe.Id;
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
