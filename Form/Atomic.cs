using System;
using System.Collections.Generic;
using System.Text;

namespace gentzen_calc.Form
{
    public class Atomic : Formula
    {
        private String variable;

        public Atomic(String variable)
        {
            this.variable = variable;
        }
        public override string ToString()
        {
            return variable;
        }
        public override bool Equals(object o)
        {
            // 1. típuskényszerítés (o-t a saját típusomra)
            if(o != null && o is Atomic)
            {
                // Casting (ugyanaz mint "o as Atomic")
                // Megnézem minden mezőjét (én mezőm ugyanolyan értékű-e mint o mezője)
                Atomic other = (Atomic)o;
                return other.variable == this.variable;
            }
            return false;
        }

        public override int GetHashCode()
        {
            // Fel kell használnom az összes mezőt és azok GetHashCode-ját.
            // Általában prímszámmal szorzom és úgy adom össze.
            return variable.GetHashCode();
        }

        public override Formula GetLeft()
        {
            return null;
        }

        public override Formula GetRight()
        {
            return null;
        }
    }
}
