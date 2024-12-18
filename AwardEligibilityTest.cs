using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    /// <summary>
    /// clasa pentru testarea eligibilitatii pentru un premiu
    /// se demonstreaza utilizarea inferentei pentru a determina daca o persoana este eligibila pentru un premiu
    /// acest test case implementeaza o solutie logica pentru a demonstra eligibilitatea unei persoane pentru un premiu
    /// bazat pe fapte despre performante academice, voluntariat si abilitati de conducere
    /// </summary>
    public class AwardEligibilityTest
    {
        /// <summary>
        /// ruleaza testul pentru eligibilitatea unei persoane pentru un premiu
        /// baza de cunostinte contine:
        /// - faptele despre realizarile personale
        /// - regulile pentru stabilirea
        ///     - calificare academica pe baza performantelor
        ///     - implicare in voluntariat
        ///     - abilitati de conducere
        ///     - eligibilitate pentru un premiu bazat pe toate criteriile mentionate
        /// se demonstreaza ca o persoana este eligibila pentru un premiu dupa fapte si reguli
        /// </summary>
        public static void Run()
        {
            Console.WriteLine("Testarea eligibilitatii pentru un premiu\n");

            // se definesc variabilele
            Variable person = new Variable("Person");

            // baza de cunoștinte si motorul de inferenta
            KnowledgeBase knowledgeBase = new KnowledgeBase();
            InferenceEngine inferenceEngine = new InferenceEngine();

            // propozitia ce trebuie demonstrata
            Predicate alpha = new Predicate("EligibleForAward", new List<Variable> { person });

            // se adauga faptele
            knowledgeBase.Facts.Add(new Predicate("HasExcellentPerformance", new List<Variable> { person }));
            knowledgeBase.Facts.Add(new Predicate("ParticipatesInCommunityService", new List<Variable> { person }));
            knowledgeBase.Facts.Add(new Predicate("DemonstratesLeadership", new List<Variable> { person }));

            // regula 1: daca o persoana are performanțe academice excelente, este calificata academic
            Clause academicRule = new Clause();
            academicRule.AddToTheLeft(new Predicate("HasExcellentPerformance", new List<Variable> { person }));
            academicRule.SetConsecvent(new Predicate("AcademicallyQualified", new List<Variable> { person }));
            knowledgeBase.Rules.Add(academicRule);

            // regula 2: daca o persoana participa la activitati de voluntariat, este implicata comunitar
            Clause communityRule = new Clause();
            communityRule.AddToTheLeft(new Predicate("ParticipatesInCommunityService", new List<Variable> { person }));
            communityRule.SetConsecvent(new Predicate("CommunityEngaged", new List<Variable> { person }));
            knowledgeBase.Rules.Add(communityRule);

            // regula 3: daca o persoana demonstreaza abilitati de conducere, este lider
            Clause leadershipRule = new Clause();
            leadershipRule.AddToTheLeft(new Predicate("DemonstratesLeadership", new List<Variable> { person }));
            leadershipRule.SetConsecvent(new Predicate("Leader", new List<Variable> { person }));
            knowledgeBase.Rules.Add(leadershipRule);

            // regula 4: daca o persoana este calificata academic, implicata comunitar și lider, este eligibila pentru premiu
            Clause eligibilityRule = new Clause();
            eligibilityRule.AddToTheLeft(new Predicate("AcademicallyQualified", new List<Variable> { person }));
            eligibilityRule.AddToTheLeft(new Predicate("CommunityEngaged", new List<Variable> { person }));
            eligibilityRule.AddToTheLeft(new Predicate("Leader", new List<Variable> { person }));
            eligibilityRule.SetConsecvent(new Predicate("EligibleForAward", new List<Variable> { person }));
            knowledgeBase.Rules.Add(eligibilityRule);

            // se afiseaza faptele
            Console.WriteLine("Fapte:");
            foreach (var fact in knowledgeBase.Facts)
            {
                Console.WriteLine(fact.ToString());
            }

            // se afiseaza regulile
            Console.WriteLine("\nReguli:");
            foreach (var rule in knowledgeBase.Rules)
            {
                Console.WriteLine(rule.ToString());
            }
            Console.WriteLine();

            // se apeleaza algoritmul FOL-FC-ASK pentru baza de cunostinte actuala
            var substitution = inferenceEngine.FOL_FC_Ask(knowledgeBase, alpha);

            // se afiseaza rezultatul
            if (substitution != null)
            {
                Console.WriteLine("Propozitia poate fi demonstrata: Persoana este eligibila pentru premiu.\n");
            }
            else
            {
                Console.WriteLine("Propozitia nu poate fi demonstrata: Persoana nu este eligibila pentru premiu.\n");
            }
        }
    }
}
