using System.Collections.Generic;

namespace ParseCSVToJson
{
    public class DataRow
    {
        public Dictionary<string, string> DataFields { get; set; }

        public DataRow(string[] cols)
        {
            DataFields = Root.FillDynamicData(cols);
        }
    }

    public class B : DataRow
    {
        public B(string[] cols) : base(cols)
        {
        }
    }

    public class S : DataRow
    {
        public S(string[] cols) : base(cols)
        {
        }
    }


    public class M : DataRow
    {
        public M(string[] cols) : base(cols)
        {
        }
    }
    public class T : DataRow
    {
        public T(string[] cols) : base(cols)
        {
        }
    }

    public class L : DataRow
    {
        public L(string[] cols) : base(cols)
        {
        }
    }
}