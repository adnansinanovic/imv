using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinantrop.Logger.Example.Model
{
    class Habitat
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public HabitatType HabitatType { get; set; }
    }
}
