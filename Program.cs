using System;
using gentzen_calc.Form;
using System.Collections.Generic;

namespace gentzen_calc
{

    class Program
    {
        static void Main(string[] args)
        {
            //  |= (!b V c ) => ((a ^ b ) => c)
            var rightSide = 
                new Implication(
                    new Or(
                        new Negate(
                            new Atomic("b")),
                        new Atomic("c")),
                    new Implication(
                        new And(
                            new Atomic("a"),
                            new Atomic("b")),
                        new Atomic("c")));

            var proofSituation = new ProofSituation(rightSide);
            ReductionTree tree = new ReductionTree(proofSituation);
            bool truth = tree.IsAxiome();
            Console.WriteLine(tree.ToString());
        }

    } 
}
