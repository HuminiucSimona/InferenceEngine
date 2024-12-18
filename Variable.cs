using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferenceEngine
{
    /// <summary>
    /// reprezinta o variabila in motorul de inferenta predicativa
    /// clasa este folosita pentru a stoca numele variabilelor in expresiile logice
    /// </summary>  
    public class Variable
    {
        public string Name { get; set; }

        public Variable(string name)
        {
            this.Name = name;
        }
        override public string ToString()
        {
            return Name;
        }
    }
}
