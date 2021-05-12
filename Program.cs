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
            var trueRightSide = 
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

            var firstProofSituation = new ProofSituation(trueRightSide);
            ReductionTree firstTree = new ReductionTree(firstProofSituation);
            bool firstTruth = firstTree.IsAxiome();
            Console.WriteLine(firstTree.ToString());
            Console.WriteLine(firstTruth);

            // |= (a ^ b) => (!a V !b)
            var falseRightSide =
                new Implication(
                    new And(
                        new Atomic("a"),
                        new Atomic("b")),
                    new Or(
                        new Negate(
                            new Atomic("a")),
                        new Negate(
                            new Atomic("b"))));
            ReductionTree secondTree = new ReductionTree(new ProofSituation(falseRightSide));
            bool secondTruth = secondTree.IsAxiome();
            Console.WriteLine(secondTruth);

        }

    } 
}
