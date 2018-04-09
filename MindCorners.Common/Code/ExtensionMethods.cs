using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Common.Code
{
    public static class ExtensionMethods
    {public static void AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source.ContainsKey(key))
                source[key] = value;
            else
                source.Add(key, value);
        }
    }
}
