using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Lab7TP.MainForm;

namespace Lab7TP
{
    internal class TwoWayLinkedList
    {
        
        private List<TwoWayLinkedListNode> nodes = new List<TwoWayLinkedListNode>();
        protected internal int currentIndex = -1;

        public TwoWayLinkedListNode Current => currentIndex >= 0 && currentIndex < nodes.Count ? nodes[currentIndex] : null;

        public int Count()
        {
            return nodes.Count;
        }

        public TwoWayLinkedListNode this[int index] => nodes[index];

        public void SetCurrentByIndex(int index)
        {
            if (index >= 0 && index < nodes.Count)
                currentIndex = index;
        }

        public void MoveNext()
        {
            if (currentIndex < nodes.Count - 1)
                currentIndex++;
        }

        public void MovePrevious()
        {
            if (currentIndex > 0)
                currentIndex--;
        }

        public void DeleteCurrent()
        {
            if (currentIndex >= 0 && currentIndex < nodes.Count)
            {
                nodes.RemoveAt(currentIndex);
                if (currentIndex >= nodes.Count)
                    currentIndex = nodes.Count - 1;
            }
        }

        public void InsertAfterCurrent(int data)
        {
            if (currentIndex >= 0 && currentIndex < nodes.Count)
            {
                nodes.Insert(currentIndex + 1, new TwoWayLinkedListNode(data));
                currentIndex++;
            }
            else
            {
                nodes.Add(new TwoWayLinkedListNode(data));
                currentIndex = nodes.Count - 1;
            }
        }

        public PointF GetNodePosition(int index, int formWidth, int formHeight)
        {
            if (formWidth == 0 || formHeight == 0)
                return PointF.Empty; // или что-то еще, чтобы обработать эту ситуацию

            int numColumns = formWidth / (NodeSize + NodeMargin);
            int x = index % numColumns;
            int y = index / numColumns;

            float centerX;
            float centerY;

            if (y % 2 == 0)
            {
                centerX = x * (NodeSize + NodeMargin) + NodeSize / 2;
            }
            else
            {
                centerX = formWidth - (x + 1) * (NodeSize + NodeMargin) + NodeSize / 2;
            }

            centerY = y * (NodeSize + NodeMargin) + NodeSize / 2;

            return new PointF(centerX, centerY);
        }

        public bool ReachedEnd(int formWidth, int formHeight)
        {
            if (nodes.Count == 0)
                return false;

            PointF lastNodePosition = GetNodePosition(nodes.Count - 1, formWidth, formHeight);
            return lastNodePosition.Y + NodeSize / 2 >= formHeight;
        }
    }
}
