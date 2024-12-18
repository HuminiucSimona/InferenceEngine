using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    /// <summary>
    /// clasa pentru testarea vinovatiei intr-un caz de poluare industriala
    /// se demonstreaza utilizarea inferentei pentru determinarea vinovatiei unei companii in caz de poluare industriala
    /// acest test case implementeaza un o solutie logica pentru stabilirea responsabilului in caz de poluare
    /// bazat pe emisii, impactul cu mediul inconjurator si legatura dintre acestea
    /// </summary>
    public class PollutionGuiltDeterminationTest
    {
        /// <summary>
        /// clasa pentru rularea unui test case
        /// ruleaza o demonstratie pentru a determina daca o companie este vinovata de poluare folosind inferenta
        /// baza de cunostinte contine: 
        /// - faptele despre emisii, impactul cu mediul inconjurator si legatura dintre acestea
        /// - regulile pentru stabilirea
        ///   - responsabilitatii pentru poluare la o locatie specifica
        ///   - clasificarii substantelor de poluare
        ///   - determinarea vinovatiei unei companii
        /// se demonstreaza ca o companie este vinovata de poluare dupa fapte si reguli
        /// </summary>
        public static void Run()
        {
            Console.WriteLine("Testarea vinovatiei intr-un caz de poluare industriala\n");

            // se definesc variabilele
            Variable company = new Variable("Company");
            Variable pollutant = new Variable("Pollutant");
            Variable location = new Variable("Location");
            Variable impact = new Variable("Impact");

            // baza de cunostinte si motorul de inferenta
            KnowledgeBase knowledgeBase = new KnowledgeBase();
            InferenceEngine inferenceEngine = new InferenceEngine();

            // propozitia ce trebuie demonstrata
            Predicate alpha = new Predicate("GuiltyOfPollution", new List<Variable> { company });

            // se adauga faptele
            knowledgeBase.Facts.Add(new Predicate("Emits", new List<Variable> { company, pollutant }));
            knowledgeBase.Facts.Add(new Predicate("FoundAtLocation", new List<Variable> { pollutant, location }));
            knowledgeBase.Facts.Add(new Predicate("CausesImpact", new List<Variable> { pollutant, impact }));
            knowledgeBase.Facts.Add(new Predicate("NegativeImpact", new List<Variable> { impact }));

            // regula 1: daca o companie emite un poluant gasit intr-o locatie, este responsabila de poluarea acelei locatii
            Clause responsibilityRule = new Clause();
            responsibilityRule.AddToTheLeft(new Predicate("Emits", new List<Variable> { company, pollutant }));
            responsibilityRule.AddToTheLeft(new Predicate("FoundAtLocation", new List<Variable> { pollutant, location }));
            responsibilityRule.SetConsecvent(new Predicate("ResponsibleForPollution", new List<Variable> { company, location }));
            knowledgeBase.Rules.Add(responsibilityRule);

            // regula 2: daca un poluant cauzeaza un impact negativ, este considerat poluare
            Clause pollutionRule = new Clause();
            pollutionRule.AddToTheLeft(new Predicate("CausesImpact", new List<Variable> { pollutant, impact }));
            pollutionRule.AddToTheLeft(new Predicate("NegativeImpact", new List<Variable> { impact }));
            pollutionRule.SetConsecvent(new Predicate("Pollution", new List<Variable> { pollutant }));
            knowledgeBase.Rules.Add(pollutionRule);

            // regula 3: daca o companie este responsabila pentru poluare, este vinovata
            Clause guiltRule = new Clause();
            guiltRule.AddToTheLeft(new Predicate("ResponsibleForPollution", new List<Variable> { company, location }));
            guiltRule.AddToTheLeft(new Predicate("Pollution", new List<Variable> { pollutant }));
            guiltRule.SetConsecvent(new Predicate("GuiltyOfPollution", new List<Variable> { company }));
            knowledgeBase.Rules.Add(guiltRule);

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

            // se verifica rezultatul
            if (substitution != null)
            {
                Console.WriteLine("Propozitia poate fi demonstrata: Compania este vinovată de poluare.\n");
            }
            else
            {
                Console.WriteLine("Propozitia nu poate fi demonstrata: Compania nu este vinovată de poluare.\n");
            }
        }
    }
}
