using System;
using gentzen_calc.Form;

namespace gentzen_calc
{

    class Program
    {
        static void Main(string[] args)
        {
            var atomicVarA = new Atomic("A");
            var atomicVarB = new Atomic("B");
            //var negated = new Negate(atomicVarA);
            //var and = new And(atomicVarA, atomicVarB);

            //// formula (Q and neg(P))
            //Formula form = new And(new Atomic("Q"), new Negate(new Atomic("P")));
            //Formula or = new Or(new Atomic("a"), new Atomic("b"));

            //// formula ((Q and neg(P)) or W)
            Formula form2 = new Or(new And(new Atomic("Q"), new Negate(new Atomic("P"))), new Atomic("W"));
            //Console.WriteLine(form2);
            //Console.WriteLine(or);
            //Console.WriteLine(negated);
            //Console.WriteLine(and);
            //Console.WriteLine(form);


            Formula impl = new Implication(atomicVarA, atomicVarB);
            Console.WriteLine(impl);

            ProofSituation ps1 = new ProofSituation(impl);
            Console.WriteLine(ps1);
            ProofSituation ps2 = ps1.KillImplicationInGoals();
            Console.WriteLine(ps2);

        }
    }
}
