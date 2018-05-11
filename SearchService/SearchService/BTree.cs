using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService
{
    public class BTree<T>
    {
        public char Key { get; private set; }
        public int ChildrenCount { get { return _nodeDictionary.Count; } }

        Dictionary<Char, List<T>> _charDictionary = new Dictionary<char, List<T>>(); // Using a list instead of a singular ... To handle duplicates
        Dictionary<Char, BTree<T>> _nodeDictionary = new Dictionary<char, BTree<T>>();


        // This is more Memory intensive . Pointers to children are stored and created at load
        // I could create a Cpu intensive version  .. That dynamically created a list of children as needed
        List< Tuple<String,T>> _children_lst = new List<Tuple<String, T>>();

        public BTree() { } // root node

        public BTree(Char key)
        {
            Key = key;
        }

        public BTree(List<Tuple<String, T>> tupleList)
        {
            Add(tupleList);
        }

        public void  Add(List<T> daList, Func<T,String> selector)
        {
            foreach (var item in daList)
            {
                String val = selector(item);
                Add(val, item);
            }
        }

        public void Add(List<Tuple<String, T>> valueList)
        {
            foreach (var item in valueList)
            {
                Add(item.Item1, item.Item2);
            }
        }

        public void Add(String target, T data)
        {
            target = target.ToLower().Trim();
            InternalAdd(target, target, data);
        }

        public bool Remove(String target)
        {
            target = target.ToLower().Trim();
            return InternalRemove(target, target);
        }

        public bool Seek(string target, out List<T> result)
        {
            target = target.ToLower().Trim();
            return InternalSeek(target, out result);
        }

        private bool InternalSeek(string target, out List<T> result)
        {
            result = null;
            if (target == null || target.Length <= 0) return false;

            if (target.Length == 1)
            {
                if (false == _charDictionary.TryGetValue(target[0], out result))
                {
                    return false;
                }
                return true;
            }
            else
            {
                char searchChar = target[0];
                string remaining = target.Substring(1);
                BTree<T> nodeToSearch;

                if (false == _nodeDictionary.TryGetValue(searchChar, out nodeToSearch))
                {
                    return false;
                }

                return nodeToSearch.Seek(remaining, out result);

            }
        }

        public List<T> Children(string target)
        {
            target = target.ToLower().Trim();
            return InternalSuggestions(target);
        }

        private List<T> InternalSuggestions(string target)
        {
            if (target == null || target.Length <= 0) return new List<T>();

            if (target.Length == 1)
            {
                char searchChar = target[0];
                BTree<T> nodeToSearch;

                if (false == _nodeDictionary.TryGetValue(searchChar, out nodeToSearch))
                {
                    return _children_lst.Select( t => t.Item2).ToList();
                }
                else
                {
                    return nodeToSearch._children_lst.Select(t => t.Item2).ToList();
                }
            }
            else
            {
                char searchChar = target[0];
                string remaining = target.Substring(1);
                BTree<T> nodeToSearch;

                if (false == _nodeDictionary.TryGetValue(searchChar, out nodeToSearch))
                {
                    return _children_lst.Select(t => t.Item2).ToList();
                }

                return nodeToSearch.Children(remaining);

            }
        }

        private void InternalAdd(string original, string target, T data)
        {
            if (target == null || target.Length <= 0) return;

            if (target.Length == 1)
            {
                AddToCharDictionary(target, data);
            }
            else
            {
                BTree<T> nodeToAddTo = AddOrGetFromNodeDictionary(target[0]);
                nodeToAddTo.InternalAdd(original, target.Substring(1), data);
            }
            _children_lst.Add(new Tuple<string, T>(original,data));
        }

        private bool InternalRemove(String original, String target)
        {
            bool retVal = false;

            if (target == null || target.Length <= 0) return false;

            if (target.Length == 1)
            {
                return RemoveFromCharDictionary(original, target);
            }
            else
            {
                BTree<T> nodeChild;

                if (false == _nodeDictionary.TryGetValue(target[0], out nodeChild))
                {  // No Children
                    return RemoveFromCharDictionary(original, target);
                }

                retVal = nodeChild.Remove(target.Substring(1));

                if (true == retVal)
                {
                    _children_lst.RemoveAll ( t=> t.Item1== original);

                    if (target.Length == 2)
                    {
                        if (nodeChild.ChildrenCount == 0)
                        {
                            this._nodeDictionary.Remove(nodeChild.Key);
                            _children_lst.RemoveAll(t => t.Item1 == original);
                        }
                    }
                }
            }
            return retVal;
        }

        private bool RemoveFromCharDictionary(string original, string target)
        {

            List<T> ret;
            if (true == _charDictionary.TryGetValue(target[0], out ret))
            {
                _charDictionary.Remove(target[0]);
                _children_lst.RemoveAll(t => t.Item1 == original);
                return true;
            }
            else
            {
                return false;
            }
        }

        private BTree<T> AddOrGetFromNodeDictionary(char searchChar)
        {
            BTree<T> ret;

            if (false == _nodeDictionary.TryGetValue(searchChar, out ret))
            {
                ret = new BTree<T>(searchChar);
                _nodeDictionary.Add(searchChar, ret);
            }

            return ret;
        }

        private void AddToCharDictionary(string search, T data)
        {
            List<T> lst;

            if (true == _charDictionary.TryGetValue(search[0], out lst))
            {
                lst.Add(data);
            }
            else
            {
                lst = new List<T>() { data };
                _charDictionary.Add(search[0], lst);
            }
        }
    }
}
