using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba3_Hopfild.Core
{
    internal sealed class Neuron
    {
        public int State { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Neuron && this.Equals(obj as Neuron);
        }

        public bool Equals(Neuron neuron)
        {
            return this.State == neuron.State;
        }
    }
}
