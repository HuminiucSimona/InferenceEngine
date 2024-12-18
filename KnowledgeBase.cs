using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    /// <summary>
    /// clasa pentru reprezentarea unei baze de cunostinte
    /// </summary>
    public class KnowledgeBase
    {
        public List<Predicate> Facts { get; set; }
        public List<Clause> Rules { get; set; }

        public KnowledgeBase()
        {
            Facts = new List<Predicate>();
            Rules = new List<Clause>();
        }
    }
}