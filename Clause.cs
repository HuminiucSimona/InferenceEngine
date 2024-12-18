using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    /// <summary>
    /// reprezinta o clauza in motorul de inferenta predicativa
    /// clasa este folosita pentru a stoca antecedentul si consecventul unei clauze
    /// </summary>
    public class Clause
    {
        public List<Predicate> Antecedent { get; set; }
        public Predicate Consecvent { get; set; }

        public Clause()
        {
            Antecedent = new List<Predicate>();
            Consecvent = new Predicate();
        }

        public Clause(List<Predicate> antecedent, Predicate consecvent)
        {
            Antecedent = antecedent ?? throw new ArgumentNullException(nameof(antecedent));
            Consecvent = consecvent ?? throw new ArgumentNullException(nameof(consecvent));
        }

        /// <summary>
        /// adauga un predicat la antecedent
        /// </summary>
        /// <param name="predicate">predicatul care se adauga</param>
        public void AddToTheLeft(Predicate predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            Antecedent.Add(predicate);
        }

        /// <summary>
        /// seteaza consecventul unei clauze
        /// </summary>
        /// <param name="consecvent">predicatul care se seteaza</param>
        public void SetConsecvent(Predicate consecvent)
        {
            if (consecvent == null)
                throw new ArgumentNullException(nameof(consecvent));
            Consecvent = consecvent;
        }

        public override string ToString()
        {
            string antecedentStr = Antecedent.Count > 0 ? string.Join("^", Antecedent) : "True";
            return $"{antecedentStr} => {Consecvent}";
        }
    }
}