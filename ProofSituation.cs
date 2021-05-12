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

        #region Members
        private HashSet<Formula> knowledgeBase;
        private HashSet<Formula> goals;
        #endregion

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
            if(!IsAtomicSituation())
            {
                return false;
            }

            return knowledgeBase.Any(f => goals.Contains(f));
        }

        public bool IsAtomicSituation()
        {
            return this.knowledgeBase.All(f => f is Atomic) &&
                this.goals.All(f => f is Atomic);
        }
        #endregion

        #region Kill in Goals
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
            ProofSituation proof = GetNext();
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
            ProofSituation next = GetNext();
            next.goals.Remove(firstOr);
            next.goals.Add(firstOr.GetLeft());
            next.goals.Add(firstOr.GetRight());
            var list = new List<ProofSituation>();
            list.Add(next);
            return list;
        }

        public List<ProofSituation> KillNegationInGoals()
        {
            if (!FormExistsInGoals<Negate>())
            {
                return null;
            }

            Negate negation = (Negate)this.goals.Where(f => f is Negate).First();
            var next = GetNext();
            next.goals.Remove(negation);
            next.knowledgeBase.Add(negation.GetRight());
            var list = new List<ProofSituation>();
            list.Add(next);
            return list;
        }

        public List<ProofSituation> KillAndInGoals()
        {
            if (!FormExistsInGoals<And>())
            {
                return null;
            }

            And and = (And)this.goals.Where(f => f is And).First();

            var nextFirst = GetNext();
            nextFirst.goals.Remove(and);
            nextFirst.goals.Add(and.GetLeft());

            var nextSecond = GetNext();
            nextSecond.goals.Remove(and);
            nextSecond.goals.Add(and.GetRight());
            var list = new List<ProofSituation>();
            list.Add(nextFirst);
            list.Add(nextSecond);
            return list;
        }
        #endregion

        #region Kill in Knowledgebase
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
            ProofSituation next = GetNext();
            next.knowledgeBase.Remove(firstAnd);
            next.knowledgeBase.Add(firstAnd.GetLeft());
            next.knowledgeBase.Add(firstAnd.GetRight());
            var list = new List<ProofSituation>();
            list.Add(next);
            return list;
        }

        public List<ProofSituation> KillOrFromKnowledgeBase()
        {
            if (!FormExistsInKnowledgeBase<Or>())
            {
                return null;
            }

            Or firstOr = (Or)this.knowledgeBase.Where(f => f is Or).First();

            ProofSituation nextFirst = GetNext();
            ProofSituation nextSecond = GetNext();

            nextFirst.knowledgeBase.Remove(firstOr);
            nextFirst.knowledgeBase.Add(firstOr.GetLeft());
            nextSecond.knowledgeBase.Remove(firstOr);
            nextSecond.knowledgeBase.Add(firstOr.GetRight());
            
            var list = new List<ProofSituation>();
            list.Add(nextFirst);
            list.Add(nextSecond);
            return list;
        }

        public List<ProofSituation> KillImplicationFromKnowledgeBase()
        {
            if (!FormExistsInKnowledgeBase<Implication>())
            {
                return null;
            }

            Implication firstImplication = (Implication)this.knowledgeBase.Where(f => f is Implication).First();

            ProofSituation nextFirst = GetNext();
            ProofSituation nextSecond = GetNext();

            nextFirst.knowledgeBase.Remove(firstImplication);
            nextFirst.goals.Add(firstImplication.GetLeft());
            nextSecond.knowledgeBase.Remove(firstImplication);
            nextSecond.goals.Add(firstImplication.GetRight());

            var list = new List<ProofSituation>();
            list.Add(nextFirst);
            list.Add(nextSecond);
            return list;
        }

        public List<ProofSituation> KillNegationFromKnowledgeBase()
        {
            if (!FormExistsInKnowledgeBase<Negate>())
            {
                return null;
            }

            Negate firstNegation = (Negate)this.knowledgeBase.Where(f => f is Negate).First();

            ProofSituation next = GetNext();
            next.knowledgeBase.Remove(firstNegation);
            next.goals.Add(firstNegation.GetRight());

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

        #region Overrides
        public override string ToString()
        {
            return String.Join(", ", this.knowledgeBase)
                    + " |- "
                    + String.Join(", ", this.goals);
        }
        #endregion

        #region Private methods
        private ProofSituation GetNext()
        {
            var newProofSituation = new ProofSituation();
            newProofSituation.goals = new HashSet<Formula>(this.goals);
            newProofSituation.knowledgeBase = new HashSet<Formula>(this.knowledgeBase);
            return newProofSituation;
        }
        #endregion
    }
}
