using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
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
        public void MoveToNextBigger()
        {
            if (currentIndex < Count() - 1)
            {
                TwoWayLinkedListNode current = this[currentIndex];
                int currentValue = current.Data;
                int nextIndex = currentIndex + 1;
                while (nextIndex < Count() && this[nextIndex].Data <= currentValue)
                {
                    nextIndex++;
                }
                currentIndex = nextIndex < Count() ? nextIndex : currentIndex;
            }
        }

        public void MoveToPreviousSmaller()
        {
            if (currentIndex > 0)
            {
                TwoWayLinkedListNode current = this[currentIndex];
                int currentValue = current.Data;
                int previousIndex = currentIndex - 1;
                while (previousIndex >= 0 && this[previousIndex].Data >= currentValue)
                {
                    previousIndex--;
                }
                currentIndex = previousIndex >= 0 ? previousIndex : currentIndex;
            }
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
                {
                    currentIndex = nodes.Count - 1;
                }
                else
                {
                    // После удаления элемента обновляем номер текущего элемента
                    for (int i = currentIndex; i < nodes.Count; i++)
                    {
                        nodes[i].Number--;
                    }
                }
            }
        }


        public void InsertAfterCurrent(int data)
        {
            if (currentIndex >= 0 && currentIndex < nodes.Count)
            {
                int newNumber = currentIndex + 1;
                nodes.Insert(currentIndex + 1, new TwoWayLinkedListNode(data, newNumber));
                // После вставки элемента обновляем номера последующих узлов
                for (int i = currentIndex + 2; i < nodes.Count; i++)
                {
                    nodes[i].Number++;
                }
                currentIndex++;
            }
            else
            {
                // Если текущего элемента нет, вставляем в конец
                int newNumber = nodes.Count + 1;
                nodes.Add(new TwoWayLinkedListNode(data, newNumber));
                currentIndex = nodes.Count - 1;
            }
        }

        public void InsertAtPosition(int data, Point position, int pictureBoxWidth, int pictureBoxHeight)
        {
            int index = GetNodeIndexByPosition(position, pictureBoxWidth, pictureBoxHeight);
            if (index == -1)
            {
                // Если координаты находятся за пределами всех узлов, вставляем в конец списка
                InsertAfterCurrent(data);
            }
            else
            {
                // Вставляем новый элемент перед узлом с указанным индексом
                SetCurrentByIndex(index);
                InsertBeforeCurrent(data);
            }
        }

        public void InsertBeforeCurrent(int data)
        {
            if (currentIndex < 0 || currentIndex >= nodes.Count)
            {
                return; // Если текущий индекс находится вне диапазона допустимых значений, необходимо прекратить выполнение метода
            }

            TwoWayLinkedListNode newNode = new TwoWayLinkedListNode(data);

            if (currentIndex == 0)
            {
                // Вставляем перед первым элементом
                newNode.Next = nodes[currentIndex];
                nodes[currentIndex].Previous = newNode;
                nodes.Insert(0, newNode);
            }
            else
            {
                // Вставляем перед текущим узлом
                TwoWayLinkedListNode current = nodes[currentIndex];
                TwoWayLinkedListNode previous = nodes[currentIndex - 1];

                previous.Next = newNode;
                newNode.Previous = previous;

                newNode.Next = current;
                current.Previous = newNode;

                nodes.Insert(currentIndex, newNode);
            }

            currentIndex++; // Поскольку мы вставили новый элемент перед текущим узлом, мы должны увеличить текущий индекс
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
        private int GetNodeIndexByPosition(Point position, int pictureBoxWidth, int pictureBoxHeight)
        {
            int totalNodes = Count();
            if (totalNodes == 0)
                return -1;

            for (int i = 0; i < totalNodes; i++)
            {
                PointF nodePosition = GetNodePosition(i, pictureBoxWidth, pictureBoxHeight);
                if (Math.Pow(position.X - nodePosition.X, 2) + Math.Pow(position.Y - nodePosition.Y, 2) < Math.Pow(NodeSize / 2, 2))
                    return i;
            }
            return -1;
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
