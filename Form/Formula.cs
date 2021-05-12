using System;
using System.Collections.Generic;
using System.Text;

namespace gentzen_calc
{
    public abstract class Formula
    {
        // Az Equals metódust felül kell írnia minden formulának
        // Ezt úgy érjük el, hogy abstractá tesszük.
        // Equals metódusa minden osztálynak van, mert az Object osztályból minden
        // osztály megörökli az Equals metódust
        // A hook metódus:
        // Virtuális, van törzse, de az üres, vagy csak egy return van benne
        // Ha hook-ot írok felül, azzal nem szegem meg az OCP-t (Open Closed Principle)
        // OCP: Ne használj override-ot, csak abstract vagy hook metódus felülírására

        public abstract override bool Equals(Object o);
        public abstract override int GetHashCode();
        public abstract Formula GetLeft();
        public abstract Formula GetRight();
    }
}
