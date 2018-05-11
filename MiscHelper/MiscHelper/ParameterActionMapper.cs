using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiscHelper
{
    public class ParameterActionMapper<T>
    {
        static Dictionary<String, T> _ParameterMap = new Dictionary<string, T>();

        public static void Register(String parameter, Func<T> dataObject)
        {
            _ParameterMap.Add(parameter.Trim().ToLower(), dataObject());
        }

        public static List<T> Run(String[] args)
        {
            T val;
            SortedDictionary<String ,T> output = new SortedDictionary<string, T>();

            foreach (var item in args)
            {
                if (item.Trim() != "")
                {
                    if (_ParameterMap.TryGetValue(item.Trim().ToLower(), out val) == true)
                    {
                        output.Add(item , val);
                    }
                }
            }

            return output.ToList().Select( kv => kv.Value).ToList();

            //_actionParameterMap[runKey](args);
        }
    }
}
