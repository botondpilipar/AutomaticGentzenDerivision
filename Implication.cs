using System;
using System.Collections.Generic;
using System.Text;

namespace gentzen_calc
{
    public class Implication : Formula
    {
        public Formula a { get; set; }
        public Formula b { get; set; }

        public Implication(Formula a, Formula b)
        {
            this.a = a;
            this.b = b;
        }

        public override string ToString()
        {
            return "(" + a.ToString() + " => " + b.ToString() + ")";
        }

        public override int GetHashCode()
        {
            // Megszorozzuk prímszámmal
            return (a.GetHashCode() + b.GetHashCode()) * 769;
        }

        public override bool Equals(object o)
        {
            if (o != null && o is Implication)
            {
                // Casting (ugyanaz mint "o as Negate")
                // Megnézem minden mezőjét (én mezőm ugyanolyan értékű-e mint o mezője)
                Implication other = (Implication)o;
                return other.a.Equals(this.a) &&
                        other.b.Equals(this.b);
            }
            return false;
        }

        public override Formula GetLeft()
        {
            return a;
        }

        public override Formula GetRight()
        {
            return b;
        }
    }
}
