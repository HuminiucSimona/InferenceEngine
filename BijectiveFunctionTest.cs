using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    /// <summary>
    /// clasa pentru testarea bijectivitatii unei functii
    /// se demonstreaza utilizarea inferentei pentru a determina daca o functie este bijectiva
    /// acest test case implementeaza o solutie logica pentru a demonstra bijectivitatea unei functii
    /// bazat pe fapte despre functii si proprietatea de injectivitate si surjectivitate
    /// </summary>
    public class BijectiveFunctionTest
    {
        /// <summary>
        /// ruleaza testul pentru bijectivitatea unei functii
        /// se demonstreaza ca o functie este bijectiva folosind inferenta
        /// baza de cunostinte contine:
        /// - faptele despre imaginea functiei si valori distincte
        /// - regulile pentru stabilirea
        ///     - injectivitatii (valorile diferite au imagini diferite )
        ///     - surjectivitatii (fiecare valoare are o imagine in functie)
        ///     - bijectivitatii (functia este injectiva si surjectiva)
        /// se demonstreaza ca o functie este bijectiva dupa fapte si reguli
        /// </summary>
        public static void Run()
        {
            Console.WriteLine("Bijectivitatea unei functii\n");
            Variable x = new Variable("X");
            Variable y = new Variable("Y");
            Variable z = new Variable("Z");
            Variable f = new Variable("F");

            KnowledgeBase knowledgeBase = new KnowledgeBase();
            InferenceEngine inferenceEngine = new InferenceEngine();

            // propozitia ce trebuie demonstrata
            Predicate alpha = new Predicate("Bijective", new List<Variable> { f });

            // se adauga faptele
            knowledgeBase.Facts.Add(new Predicate("Distinct", new List<Variable> { new Variable("1"), new Variable("2") }));
            knowledgeBase.Facts.Add(new Predicate("Map", new List<Variable> { f, new Variable("1"), new Variable("10") }));
            knowledgeBase.Facts.Add(new Predicate("Map", new List<Variable> { f, new Variable("2"), new Variable("20") }));

            // se definesc regulile pentru bijectivitate
            Clause injectiveRule = new Clause();
            injectiveRule.AddToTheLeft(new Predicate("Distinct", new List<Variable> { x, y }));
            injectiveRule.AddToTheLeft(new Predicate("Map", new List<Variable> { f, x, z }));
            injectiveRule.AddToTheLeft(new Predicate("Map", new List<Variable> { f, y, z }));
            injectiveRule.SetConsecvent(new Predicate("Injective", new List<Variable> { f }));
            knowledgeBase.Rules.Add(injectiveRule);

            // regula 2: daca fiecare valoare are o imagine în functie, functia este surjectiva
            Clause surjectiveRule = new Clause();
            surjectiveRule.AddToTheLeft(new Predicate("Map", new List<Variable> { f, x, z }));
            surjectiveRule.SetConsecvent(new Predicate("Surjective", new List<Variable> { f }));
            knowledgeBase.Rules.Add(surjectiveRule);

            // regula 3: daca functia este injectiva si surjectiva, este bijectiva
            Clause bijectiveRule = new Clause();
            bijectiveRule.AddToTheLeft(new Predicate("Injective", new List<Variable> { f }));
            bijectiveRule.AddToTheLeft(new Predicate("Surjective", new List<Variable> { f }));
            bijectiveRule.SetConsecvent(new Predicate("Bijective", new List<Variable> { f }));
            knowledgeBase.Rules.Add(bijectiveRule);

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
                Console.WriteLine("Propozitia poate fi demonstrata: Functia este bijectiva.\n");
            }
            else
            {
                Console.WriteLine("Propozitia nu poate fi demonstrata: Functia nu este bijectiva.\n");
            }
        }
    }

}
