using System.Collections.Generic;

namespace ParseCSVToJson
{
    public class Order
    {

        public Dictionary<string, string> OrderFields { get; set; }
        
        public B  TheB { get; set; }
        public S TheS { get; set; }
        public M TheM { get; set; }
        public T TheT { get; set; }

        public List<L> TheLs { get; set; }


        public Order(List<string[]> lst)
        {
            int NoOfTimes = lst.Count - 1;
            OrderFields = Root.FillDynamicData(lst[0]);
            TheLs = new List<L>();

            for (int i = 1; i <= NoOfTimes; i++)
            {
                string collumnDescriptor = Root.Clean(lst[i][0]);

                DataRow rw = new DataRow(lst[i]);

                switch (collumnDescriptor)
                {
                    case "B":
                        TheB = new B(lst[i]);
                        break;
                    case "S":
                        TheS = new S(lst[i]);
                        break;
                    case "M":
                        TheM = new M(lst[i]);
                        break;
                    case "T":
                        TheT = new T(lst[i]);
                        break;
                    case "L":
                        TheLs.Add( new L(lst[i]));
                        break;
                    default:
                        //What the hell ... :)
                        break;
                }

            }

         
        }
    }
}