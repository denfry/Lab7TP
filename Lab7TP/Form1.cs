using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab7TP
{
    public partial class MainForm : Form
    {
        protected internal const int NodeSize = 30; // Размер узла (круга)
        protected internal const int NodeMargin = 20; // Отступ между узлами
        protected internal const int ArrowSize = 5; // Размер стрелки
        TwoWayLinkedList list = new TwoWayLinkedList();
        Graphics graphics;
        public MainForm()
        {
            InitializeComponent();
            pictureBox1.Paint += PictureBoxList_Paint;
            pictureBox1.MouseClick += PictureBoxList_MouseClick;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
        }
        private void PictureBoxList_Paint(object sender, PaintEventArgs e)
        {
            graphics = e.Graphics;
            DrawList();
        }

        private void PictureBoxList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int clickedIndex = GetClickedNodeIndex(e.Location);
                if (clickedIndex != -1)
                {
                    list.SetCurrentByIndex(clickedIndex);
                    pictureBox1.Refresh();
                    DisplayCurrentNode();
                }
                else
                {
                    int newItem = new Random().Next(101);
                    list.InsertAfterCurrent(newItem);
                    pictureBox1.Refresh();
                    DisplayCurrentNode();

                    if (list.ReachedEnd(pictureBox1.Width, pictureBox1.Height))
                    {
                        MessageBox.Show("Место закончилось", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        private void DrawList()
        {
            int totalNodes = list.Count();
            if (totalNodes == 0)
                return;

            for (int i = 0; i < totalNodes; i++)
            {
                PointF nodePosition = list.GetNodePosition(i, pictureBox1.Width, pictureBox1.Height);
                DrawNode(nodePosition.X, nodePosition.Y, list[i].ToString());

                if (i == list.currentIndex)
                {
                    graphics.DrawRectangle(Pens.Red, nodePosition.X - NodeSize / 2, nodePosition.Y - NodeSize / 2, NodeSize, NodeSize);
                }

                if (i < totalNodes - 1)
                {
                    PointF nextNodePosition = list.GetNodePosition(i + 1, pictureBox1.Width, pictureBox1.Height);
                    DrawArrow(nodePosition, nextNodePosition);
                }
            }
        }

        private void DrawNode(float x, float y, string text)
        {
            RectangleF nodeRect = new RectangleF(x - NodeSize / 2, y - NodeSize / 2, NodeSize, NodeSize);
            graphics.FillEllipse(Brushes.LightBlue, nodeRect);
            graphics.DrawEllipse(Pens.Black, nodeRect);
            graphics.DrawString(text, DefaultFont, Brushes.Black, x - 8, y - 8);
        }

        private void DrawArrow(PointF start, PointF end)
        {
            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            float length = (float)Math.Sqrt(dx * dx + dy * dy);
            float unitDx = dx / length;
            float unitDy = dy / length;

            // Определяем точки начала и конца стрелки
            PointF arrowStart = new PointF(start.X + unitDx * NodeSize / 2, start.Y + unitDy * NodeSize / 2);
            PointF arrowEnd = new PointF(end.X - unitDx * NodeSize / 2, end.Y - unitDy * NodeSize / 2);

            // Определяем, если элементы находятся на разных уровнях
            bool onDifferentLevels = Math.Abs(start.Y - end.Y) > NodeSize + NodeMargin;

            // Если элементы находятся на разных уровнях, привязываем стрелку к верхнему краю следующего элемента
            if (onDifferentLevels && start.X != end.X)
            {
                if (start.X < end.X)
                {
                    arrowEnd.Y -= NodeSize / 2;
                }
                else
                {
                    arrowStart.Y -= NodeSize / 2;
                }
            }

            // Вычисляем координаты точек базы стрелки
            PointF arrowBase1 = new PointF(arrowEnd.X - unitDx * ArrowSize * 2 - unitDy * ArrowSize, arrowEnd.Y - unitDy * ArrowSize * 2 + unitDx * ArrowSize);
            PointF arrowBase2 = new PointF(arrowEnd.X - unitDx * ArrowSize * 2 + unitDy * ArrowSize, arrowEnd.Y - unitDy * ArrowSize * 2 - unitDx * ArrowSize);

            // Отрисовываем стрелку
            graphics.DrawLine(Pens.Black, arrowStart, arrowEnd);
            PointF[] arrowPoints = { arrowBase1, arrowEnd, arrowBase2 };
            graphics.FillPolygon(Brushes.Black, arrowPoints);
        }

        private int GetClickedNodeIndex(Point location)
        {
            int totalNodes = list.Count();
            if (totalNodes == 0)
                return -1;

            for (int i = 0; i < totalNodes; i++)
            {
                PointF nodePosition = list.GetNodePosition(i, pictureBox1.Width, pictureBox1.Height);  
                if (Math.Pow(location.X - nodePosition.X, 2) + Math.Pow(location.Y - nodePosition.Y, 2) < Math.Pow(NodeSize / 2, 2))
                    return i;
            }
            return -1;
        }


        private void DisplayCurrentNode()
        {
            if (list.Count() > 0)
            {
                int currentIndex = list.currentIndex + 1; // Увеличиваем индекс на 1, чтобы считать с 1, а не с 0
                label2.Text = $"Количество элементов: {list.Count()}, Текущий элемент: {currentIndex}";
            }
            else
            {
                label2.Text = "Список пуст";
            }
        }
       
        private void buttonMoveNext_Click(object sender, EventArgs e)
        {
            list.MoveNext();
            pictureBox1.Refresh();
            DisplayCurrentNode();
        }

        private void buttonMovePrevious_Click(object sender, EventArgs e)
        {
            list.MovePrevious();
            pictureBox1.Refresh();
            DisplayCurrentNode();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            list.DeleteCurrent();
            pictureBox1.Refresh();
            DisplayCurrentNode();
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            int newItem = new Random().Next(101);
            list.InsertAfterCurrent(newItem);
            pictureBox1.Refresh();
            DisplayCurrentNode();
            if (list.ReachedEnd(pictureBox1.Width, pictureBox1.Height))
            {
                MessageBox.Show("Место закончилось", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            label3.Text = $"Координаты мыши: X: {e.X}, Y: {e.Y}";
        }
    }
}
