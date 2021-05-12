using System;
using System.Collections.Generic;
using System.Text;

namespace gentzen_calc.Form
{
    public class And : Formula
    {
        private Formula a;
        private Formula b;

        public And(Formula a, Formula b)
        {
            this.a = a;
            this.b = b;
        }
        public override string ToString()
        {
            return "(" + a.ToString() + " and " + b.ToString() + ")";
        }

        public override int GetHashCode()
        {
            // Megszorozzuk prímszámmal
            return (a.GetHashCode() + b.GetHashCode()) * 127;
        }

        public override bool Equals(object o)
        {
            if (o != null && o is And)
            {
                // Casting (ugyanaz mint "o as Negate")
                // Megnézem minden mezőjét (én mezőm ugyanolyan értékű-e mint o mezője)
                And other = (And)o;
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
