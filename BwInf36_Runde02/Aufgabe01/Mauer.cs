using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aufgabe01
{
    public struct Mauer
    {
        //public bool hallo { get; set; }

        public Reihe[] Reihen { get; set; }

        /// <summary>
        /// HACK: Ist diese Fuge frei (BruteForce)
        /// </summary>
        public bool[] FreieFugen { get; private set; }

        public Mauer(int hoehe, int breite)
        {
            //hallo = false;
            FreieFugen = new bool[breite];
            FreieFugen.FillArray(true);
            Reihen = new Reihe[hoehe];
        }

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
    }
}
