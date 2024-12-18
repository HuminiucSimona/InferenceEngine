using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    /// <summary>
    /// reprezinta o propozitie logica in motorul de inferenta predicativa
    /// clasa este folosita pentru a stoca numele propozitiei si argumentele sale
    /// </summary>
    public class Predicate
    {
        public string Name { get; set; }
        public List<Variable> Arguments { get; set; }

        public Predicate()
        {
            Name = string.Empty;
            Arguments = new List<Variable>();
        }

        public Predicate(string name, List<Variable> arguments)
        {
            Name = name;
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        public override string ToString()
        {
            return $"{Name}({string.Join(", ", Arguments)})";
        }

        /// <summary>
        /// metoda override pentru a compara doua propozitii
        /// </summary>
        /// <param name="obj">propozitia cu care se compara</param>
        /// <returns>true daca propozitiile sunt egale, altfel false</returns>
        public override bool Equals(object obj)
        {
            if (obj is Predicate otherPredicate)
            {
                return Name == otherPredicate.Name && Arguments.Count == otherPredicate.Arguments.Count && Arguments.SequenceEqual(otherPredicate.Arguments);
            }
            return false;
        }
    }
}
