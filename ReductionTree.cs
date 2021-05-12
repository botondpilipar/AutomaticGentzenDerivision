using System.Collections.Generic;
using System.Linq;


namespace gentzen_calc
{
    // A levezetési fa gyökere, az eredeti bizonyítási szituáció, amelyet egyszerüsítünk átírási szabályokkal
    // Ha a levezetési fa minden levéleleme axióma, akkor bebizonyítottuk az eredeti bizonyítási szituációt.
    // Ha a levezetési fa minden levélelemében már csak atomi formulák vannak és nem minden levélelem axióma
    // akkor sikeretlene a bizonyítás, mivel az eredeti bizonyítási szituáció hamis.
    // Ha egy bizonyítási szituációból több lesz, akkor a fa bővül (több ágra szakad), egyéb esetben mindig
    // csak az aktuális ágat bővítem.
    // Legyegyszerűbb úgy megcsinálni az egészet, hogy az átírási szabályok nem egy bizonyítási szituációt
    // adnak vissza, hanem egy levezetési fát, és akkor mindig csak a levélelemeken kell végigiterálnom,
    // megnézem ki az axióma, ami nem axióma, azt tovább bontom. Ezt úgy teszem, hogy megnézem melyik
    // előfeltétel igaz. Amelyik előfeltétel igaz, az ahhoz tartozó átírási szabályt meghívom.
    class ReductionTree
    {
        private List<List<ProofSituation>> levels;
        private int currentLevel;
        public ReductionTree(ProofSituation situation)
        {
            this.currentLevel = 0;
            this.levels = new List<List<ProofSituation>>();

            var level_1 = new List<ProofSituation>();
            level_1.Add(situation);
            levels.Add(level_1);
        }

        public bool IsAxiome()
        {
            // Is the current level axiomatic and not every proof situation is true

            bool allAtomic = levels[currentLevel].All(proof => proof.IsAtomicSituation());
            if (!allAtomic)
            {
                List<ProofSituation> nextLevel = new List<ProofSituation>();
                foreach (var proof in levels[currentLevel])
                {
                    var levelCandidates = new List<List<ProofSituation>>()
                    {
                        proof.KillAndFromKnowledgeBase(),
                        proof.KillOrFromKnowledgeBase(),
                        proof.KillNegationFromKnowledgeBase(),
                        proof.KillImplicationFromKnowledgeBase(),
                        proof.KillAndInGoals(),
                        proof.KillOrInGoals(),
                        proof.KillImplicationInGoals(),
                        proof.KillNegationInGoals()
                    };
                    var nonNullCandidates = levelCandidates.Where(i => i != null).SelectMany(i => i);
                    nextLevel.AddRange(nonNullCandidates);
                }
                currentLevel += 1;
                levels.Add(nextLevel);
                return IsAxiome();
            }
            else
            {
                return levels[currentLevel].All(proof => proof.IsAxiome());
            }
        }

        public override string ToString()
        {
            return string.Join("----------------------------------------------\n",
                levels.Select(proofs => 
                    string.Join("\n",
                        proofs.Select(p => p.ToString())) + "\n"));
        }
    }
}
