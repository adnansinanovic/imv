using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinantrop.Logger.Example.Model
{
    class Animal
    {
        public Habitat Habitat { get; set; }

        public DateTime BirthDate { get; set; }

        public string Nickname { get; set; }

        public decimal Weight { get; set; }

        public double Height { get; set; }

        public bool IsHappy { get; set; }
    }
}
