using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    /// <summary>
    /// clasa pentru testarea lemei clestelui
    /// aceats clasa de test arata cum se poate demonstra ca daca o secventa este marginita de doua secvente care au aceeasi limita,
    /// atunci limita secventei este aceeasi cu limita celor doua secvente
    /// </summary>
    public class PincerLemmaTest
    {
        /// <summary>
        /// ruleaza demostratia pentru lema clestelui folosind inferenta
        /// baza de cunostinta contine:
        /// - faptele despre convergenta unei secvente si ordonare
        /// - regulile despre legea clestelui
        /// - variabile pentru secvente: xn, yn, zn si limita: a
        /// se demonstreze ca yn converge la a
        /// </summary>
        public static void Run()
        {
            Console.WriteLine("Lema clestelui\n");

            // se definesc variabilele
            Variable xn = new Variable("xn");
            Variable yn = new Variable("yn");
            Variable zn = new Variable("zn");
            Variable a = new Variable("a");
            Variable infinit = new Variable("infinit");

            // baza de cunostinte si motorul de inferenta
            KnowledgeBase knowledgeBase = new KnowledgeBase();
            InferenceEngine inferenceEngine = new InferenceEngine();

            // propozitia ce trebuie demonstrata
            Predicate alpha = new Predicate("lim", new List<Variable> { yn, a });

            // se adauga faptele
            knowledgeBase.Facts.Add(new Predicate("lim", new List<Variable> { xn, a }));
            knowledgeBase.Facts.Add(new Predicate("lim", new List<Variable> { zn, a }));
            knowledgeBase.Facts.Add(new Predicate("MaiMare", new List<Variable> { yn, xn }));
            knowledgeBase.Facts.Add(new Predicate("MaiMare", new List<Variable> { zn, yn }));

            // regula 1: daca `lim(xn, a)` este adevarat, atunci `valoare(xn, infinit, a)` este adevarata
            Clause rule1 = new Clause();
            rule1.AddToTheLeft(new Predicate("lim", new List<Variable> { xn, a }));
            rule1.SetConsecvent(new Predicate("valoare", new List<Variable> { xn, infinit, a }));
            knowledgeBase.Rules.Add(rule1);

            // regula 2: lema clestelui - combinate mai multe fapte duc la `lim(yn, a)`
            Clause rule2 = new Clause();
            rule2.AddToTheLeft(new Predicate("MaiMare", new List<Variable> { yn, xn }));
            rule2.AddToTheLeft(new Predicate("MaiMare", new List<Variable> { zn, yn }));
            rule2.AddToTheLeft(new Predicate("valoare", new List<Variable> { xn, infinit, a }));
            rule2.AddToTheLeft(new Predicate("valoare", new List<Variable> { zn, infinit, a }));
            rule2.SetConsecvent(new Predicate("lim", new List<Variable> { yn, a }));
            knowledgeBase.Rules.Add(rule2);

            // regula 3: `valoare(xn, infinit, a)` este adevarata pentru orice infinit
            Clause rule3 = new Clause();
            rule3.AddToTheLeft(new Predicate("lim", new List<Variable> { zn, a }));
            rule3.SetConsecvent(new Predicate("valoare", new List<Variable> { zn, infinit, a }));
            knowledgeBase.Rules.Add(rule3);

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
                Console.WriteLine("Propozitia poate fi demonstrata.");
            }
            else
            {
                Console.WriteLine("Propozitia nu poate fi demonstrata.");
            }
        }
    }

}
