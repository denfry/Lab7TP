using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7TP
{
    internal class TwoWayLinkedListNode
    {
        public int Data { get; set; }

        public TwoWayLinkedListNode(int data)
        {
            Data = data;
        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
