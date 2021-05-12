using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using gentzen_calc.Form;

namespace gentzen_calc
{
    class ProofSituation
    {
        // A bizonyítási szituáció két részből áll, tudásbázisból és célokból.
        // A kettő közé a levezetés jelét írjuk (fordított T)
        // A bizonyítási szituáció axióma, ha egy formula előfordul a tudásbázisban
        // és a célok között is, például
        // a-ból a levezethető (A |- A)
        private HashSet<Formula> knowledgeBase;
        private HashSet<Formula> goals;


        public ProofSituation(Formula goal) : this()
        {
            this.goals.Add(goal);
        }

        public ProofSituation()
        {
            this.knowledgeBase = new HashSet<Formula>();
            this.goals = new HashSet<Formula>();
        }
        public bool IsAxiome()
        {
            foreach(Formula f in knowledgeBase)
            {
                // A contains meghívja az equals metódust
                // Ahhoz hogy a contains jól működjön, az egyes formulákban meg
                // kell írni az equals metódust
                if(goals.Contains(f))
                {
                    return true;
                }
            }
            return false;
        }

        // Implikáció eltüntetése a célból:
        // üres |- A=>B  =  A |- B

        public ProofSituation KillImplicationInGoals()
        {
            if(!preKillImplicationInGoals())
            {
                return null;
            }

            var implications = goals.Where(f => f is Implication);
            Implication i = implications.First() as Implication;
            return resolveImplication(i);
        }
        public bool preKillImplicationInGoals()
        {
            return goals.Where(f => f is Implication)
                        .Count() > 0;
        }
        private ProofSituation resolveImplication(Implication i)
        {
            ProofSituation proof = new ProofSituation();
            proof.goals = new HashSet<Formula>(this.goals);
            proof.knowledgeBase = new HashSet<Formula>(this.knowledgeBase);
            proof.goals.Remove(i);
            proof.knowledgeBase.Add(i.GetLeft());
            proof.goals.Add(i.GetRight());

            return proof;
        }

        public override string ToString()
        {
            return String.Join(", ", this.knowledgeBase)
                    + " |- "
                    + String.Join(", ", this.goals);
        }

        // Vagy eltüntetése a célból:
        // Üres |- A vagy B = Üres |- A, B
        // Általánosan: tudásbázis |- Célok, A v B = tudázbázis |- célok, A, B

        public ProofSituation KillOrInGoals()
        {
            if(!preKillOrInGoals())
            {
                return null;
            }

            Formula firstOr = null;
            foreach (Formula f in goals)
            {
                if(f is Or)
                {
                    firstOr = f;
                    break;
                }
            }
            ProofSituation next = new ProofSituation();
            next.knowledgeBase = new HashSet<Formula>(this.knowledgeBase);
            next.goals = new HashSet<Formula>(this.goals);
            next.goals.Remove(firstOr);
            next.goals.Add(firstOr.GetLeft());
            next.goals.Add(firstOr.GetRight());
            return next;
        }

        public bool preKillOrInGoals()
        {
            foreach(Formula f in goals)
            {
                if (f is Or)
                {
                    return true;
                }
            }
            return false;
        }


        // És eltüntetése a tudásbázisból:
        // Példa: A és B |- üres = A, B |- üres
        // Általánosan: tudásbázis, A és B   |- célok 
        //           =  tudásbázis, A,   B   |- célok
        public ProofSituation KillAndFromKnowledgeBase()
        {
            if (!preKillAndFromKnowledgeBase())
            {
                return null;
            }

            Formula firstAnd = null;
            foreach (Formula f in knowledgeBase)
            {
                if (f is And)
                {
                    firstAnd = f;
                    break;
                }
            }
            ProofSituation next = new ProofSituation();
            next.knowledgeBase = new HashSet<Formula>(this.knowledgeBase);
            next.goals = new HashSet<Formula>(this.goals);
            next.knowledgeBase.Remove(firstAnd);
            next.knowledgeBase.Add(firstAnd.GetLeft());
            next.knowledgeBase.Add(firstAnd.GetRight());
            return next;
        }

        private bool preKillAndFromKnowledgeBase()
        {
            foreach (Formula f in knowledgeBase)
            {
                if (f is And)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
