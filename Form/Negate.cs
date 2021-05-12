using System;
using System.Collections.Generic;
using System.Text;

namespace gentzen_calc.Form
{
    // Transparent wrapping
    class Negate : Formula
    {
        private Formula a;

        public Negate(Formula a)
        {
            this.a = a;
        }

        public override string ToString()
        {
            return "neg(" + a.ToString() + ")";
        }

        public override int GetHashCode()
        {
            return this.a.GetHashCode();
        }

        public override bool Equals(object o)
        {
            if (o != null && o is Negate)
            {
                // Casting (ugyanaz mint "o as Negate")
                // Megnézem minden mezőjét (én mezőm ugyanolyan értékű-e mint o mezője)
                Negate other = (Negate)o;
                return other.a.Equals(this.a);
            }
            return false;
        }

        public override Formula GetLeft()
        {
            return null;
        }

        public override Formula GetRight()
        {
            return a;
        }
    }
}
