using System.Collections.Generic;
using System.Linq;

namespace Sinantrop.IMV.UI
{
    public class ChartItem
    {
        public ChartItem(string identity)
        {
            Identity = identity;

            _authors = new Dictionary<string, uint>();
            _authors.Add(identity, 1);
        }

        private Dictionary<string, uint> _authors;

        //there muse be minimum one author
        public string Author { get { return _authors.OrderByDescending(o => o.Value).ToList()[0].Key; } }

        public ulong Count { get; set; }  
        public string Identity { get; internal set; }

        public void AddIfNecessary(string author)
        {
            if (!_authors.ContainsKey(author))
                _authors.Add(author, 0);

            _authors[author]++;
        }
    }
}
