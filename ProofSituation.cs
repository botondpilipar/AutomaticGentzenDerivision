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


        #region Constructors
        public ProofSituation(Formula goal) : this()
        {
            this.goals.Add(goal);
        }

        public ProofSituation()
        {
            this.knowledgeBase = new HashSet<Formula>();
            this.goals = new HashSet<Formula>();
        }
        #endregion

        #region Predicates
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
        #endregion

        #region Kill Methods
        // Implikáció eltüntetése a célból:
        // üres |- A=>B  =  A |- B

        public List<ProofSituation> KillImplicationInGoals()
        {
            if(!FormExistsInGoals<Implication>())
            {
                return null;
            }

            var implications = goals.Where(f => f is Implication);
            Implication i = implications.First() as Implication;
            ProofSituation proof = new ProofSituation();
            proof.goals = new HashSet<Formula>(this.goals);
            proof.knowledgeBase = new HashSet<Formula>(this.knowledgeBase);
            proof.goals.Remove(i);
            proof.knowledgeBase.Add(i.GetLeft());
            proof.goals.Add(i.GetRight());
            var list = new List<ProofSituation>();
            list.Add(proof);
            return list;
        }

        public List<ProofSituation> KillOrInGoals()
        {
            if (!(FormExistsInGoals<Or>()))
            {
                return null;
            }

            Formula firstOr = null;
            foreach (Formula f in goals)
            {
                if (f is Or)
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
            var list = new List<ProofSituation>();
            list.Add(next);
            return list;
        }

        public List<ProofSituation> KillAndFromKnowledgeBase()
        {
            if (!FormExistsInKnowledgeBase<And>())
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
            var list = new List<ProofSituation>();
            list.Add(next);
            return list;
        }
        #endregion
        
        #region Form checks
        public bool FormExistsInKnowledgeBase<T>()
        {
            return this.knowledgeBase.Where(f => f is T).Count() > 0;
        }

        public bool FormExistsInGoals<T>()
        {
            return this.goals.Where(f => f is T).Count() > 0;
        }

        #endregion

        public override string ToString()
        {
            return String.Join(", ", this.knowledgeBase)
                    + " |- "
                    + String.Join(", ", this.goals);
        }
    }
}
