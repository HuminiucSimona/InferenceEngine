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
