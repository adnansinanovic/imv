using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sinantrop.Helper
{
    public static class ByteArrayExtension
    {
        public static bool CompareByteArrays(this byte[] source, byte[] compare) 
        {
            if (source.Length != compare.Length)
                return false;

            for (int i = 0; i < source.Length; i++)
            {                
                if (source[i] != compare[i])
                    return false;
            }

            return true;
        }
    }
}
