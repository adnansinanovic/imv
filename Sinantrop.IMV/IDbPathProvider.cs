using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinantrop.IMV
{
    public interface IDbPathProvider
    {
        string GetPath(string skypeUserName);

        string GetPath();
    }
}
