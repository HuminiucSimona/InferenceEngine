using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    /// <summary>
    /// reprezinta motorul de inferenta predicativa
    /// clasa contine metode pentru inferenta si unificare
    /// </summary>
    public class InferenceEngine
    {
        /// <summary>
        /// metoda pentru inferenta folosind metoda inlatuirii inaite (forward chaining) pentru gasirea
        /// unei substitutii pentru o propozitie data
        /// </summary>
        /// <param name="KB">baza de cunostinte</param>
        /// <param name="alpha">propozitia ce trebuie demonstrata</param>
        /// <returns>substitutiile daca propozitia este demonstrabila, altfel null</returns>
        public Dictionary<string, object>? FOL_FC_Ask(KnowledgeBase KB, Predicate alpha)
        {
            var newPredicates = new List<Predicate>();
            do
            {
                newPredicates.Clear();
                foreach (Clause r in KB.Rules)
                {
                    var subst_list = FindSubstitutions(KB.Facts, r);
                    foreach (var theta in subst_list)
                    {
                        Predicate q_prime = SubstPredicate(theta, r.Consecvent);
                        if (!IsRenaming(q_prime, newPredicates) && !IsRenaming(q_prime, KB.Facts))
                        {
                            Console.WriteLine(q_prime.ToString());
                            newPredicates.Add(q_prime);
                            var phi = Unify(q_prime, alpha, new Dictionary<string, object>());
                            if (phi != null)
                            {
                                return phi;
                            }
                        }
                    }
                }
                KB.Facts.AddRange(newPredicates);
            }
            while (newPredicates.Count > 0);
            return null;
        }

        /// <summary>
        /// metoda pentru gasirea substitutiilor pentru un antecedent si un consecvent
        /// </summary>
        /// <param name="facts">lista de fapte</param>
        /// <param name="clause">clauza</param>
        /// <returns>lista de substitutii</returns>
        private List<Dictionary<string, object>>? FindSubstitutions(List<Predicate> facts, Clause clause)
        {
            var substitutionsList = new List<Dictionary<string, object>>();

            foreach (var antecedentPredicate in clause.Antecedent)
            {
                foreach (var fact in facts)
                {
                    if (fact.Name == antecedentPredicate.Name)
                    {
                        var theta = Unify(antecedentPredicate, fact, new Dictionary<string, object>());
                        if (theta != null)
                        {
                            substitutionsList.Add(theta);
                        }
                    }
                }
            }
            return substitutionsList;
        }

        /// <summary>
        /// metoda pentru aplicarea unei substitutii la un predicat
        /// </summary>
        /// <param name="theta">substitutiile</param>
        /// <param name="predicate">predicatul</param>
        /// <returns>predicatul cu substitutii</returns>
        private Predicate SubstPredicate(Dictionary<string, object> theta, Predicate predicate)
        {
            var substitutes = new List<Variable>();
            foreach (var arg in predicate.Arguments)
            {
                if (arg is Variable var && theta.TryGetValue(var.Name, out var substitution))
                {
                    if (substitution is Variable subVar)
                    {
                        substitutes.Add(subVar);
                    }
                    else
                    {
                        substitutes.Add(new Variable(substitution.ToString()));
                    }
                }
                else
                {
                    substitutes.Add(arg);
                }
            }

            return new Predicate(predicate.Name, substitutes);
        }

        /// <summary>
        /// metoda pentru verificarea daca un predicat este o redenumire din lista
        /// </summary>
        /// <param name="predicate">predicatul</param>
        /// <param name="Arguments">lista de predicate</param>
        /// <returns>true daca predicatul este o redenumire, altfel false</returns>
        private bool IsRenaming(Predicate predicate, List<Predicate> Arguments)
        {
            return Arguments.Any(existingPredicate => predicate.Equals(existingPredicate));
        }

        /// <summary>
        /// metoda pentru unificarea a doua expresii
        /// </summary>
        /// <param name="x">expresia 1</param>
        /// <param name="y">expresia 2</param>
        /// <param name="theta">substitutiile</param>
        /// <returns>substitutiile daca unificarea este posibila, altfel null</returns>
        public Dictionary<string, object>? Unify(object x, object y, Dictionary<string, object> theta)
        {
            if (theta == null)
            {
                return null;
            }
            else if (x.Equals(y))
            {
                return theta;
            }
            else if (IsVariable(x))
            {
                return UnifyVar(x as Variable, y, theta);
            }
            else if (IsVariable(y))
            {
                return UnifyVar(y as Variable, x, theta);
            }
            else if (IsCompound(x) && IsCompound(y))
            {
                return Unify((x as Predicate).Arguments, (y as Predicate).Arguments, Unify((x as Predicate).Name, (y as Predicate).Name, theta));
            }
            else if (x is List<Variable> listX && y is List<Variable> listY)
            {
                if (listX.Count != listY.Count)
                {
                    return null;
                }

                for (int i = 0; i < listX.Count; i++)
                {
                    theta = Unify(listX[i], listY[i], theta);
                    if (theta == null)
                    {
                        return null;
                    }
                }
                return theta;
            }
            else return null;
        }

        /// <summary>
        /// metoda pentru unificarea unei variabile cu o expresie
        /// </summary>
        /// <param name="var">variabila</param>
        /// <param name="x">expresia</param>
        /// <param name="theta">substitutia</param>
        /// <returns>substitutia daca unificarea este posibila, altfel null</returns>
        public Dictionary<string, object>? UnifyVar(Variable var, object x, Dictionary<string, object> theta)
        {
            if (x == null)
            {
                return null;
            }
            if (theta.TryGetValue(var.Name, out object? val))
            {
                return Unify(val, x, theta);
            }
            else if (theta.TryGetValue(x.ToString(), out object? valX))
            {
                return Unify(var, valX, theta);
            }
            else if (OccurCheck(var, x))
            {
                return null;
            }
            else
            {
                theta.Add(var.Name, x);
                return theta;
            }
        }

        /// <summary>
        /// metoda pentru verificarea daca o variabila apare in expresie
        /// </summary>
        /// <param name="var">variabila</param>
        /// <param name="x">expresia</param>
        /// <returns>true daca variabila apare in expresie, altfel false</returns>
        private bool OccurCheck(Variable var, object x)
        {
            return x is Predicate predicate && predicate.Arguments.Contains(var);
        }

        /// <summary>
        /// metoda pentru verificarea daca un obiect este o variabila
        /// </summary>
        /// <param name="obj">obiectul</param>
        /// <returns>true daca obiectul este o variabila, altfel false</returns>
        private bool IsVariable(object obj)
        {
            return obj is Variable;
        }

        /// <summary>
        /// metoda pentru verificarea daca un obiect este un predicat compus (cu argumente)
        /// </summary>
        /// <param name="obj">obiectul</param>
        /// <returns>true daca obiectul este un predicat compus, altfel false</returns>
        private bool IsCompound(object obj)
        {
            return obj is Predicate && (obj as Predicate).Arguments != null;
        }
    }
}
