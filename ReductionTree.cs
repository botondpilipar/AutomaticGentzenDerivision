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
        public ReductionTree()
        {

        }
    }
}
