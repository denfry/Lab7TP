using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7TP
{
    internal class TwoWayLinkedListNode
    {
        public int Data { get; set; } // Данные узла
        public TwoWayLinkedListNode Next { get; set; } // Ссылка на следующий узел
        public TwoWayLinkedListNode Previous { get; set; } // Ссылка на предыдущий узел
        public int Number {  get; set; } // Номер узла


        public TwoWayLinkedListNode(int data)
        {
            Data = data;
            Next = null;
            Previous = null;
            Number = -1;
        }
        public TwoWayLinkedListNode(int data, int number)
        {
            Data = data;
            Next = null;
            Previous = null;
            Number = number; // Устанавливаем значение Number
        }

        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
