using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Disskjra
{
    public partial class Form1 : Form
    {
        private List<Point> vertices = new List<Point>(); // Danh sách đỉnh
        private List<Tuple<Point, Point, int>> edges = new List<Tuple<Point, Point, int>>(); // Danh sách cạnh
        private Point? selectedVertex = null; // Đỉnh đang chọn để nối cạnh
        private Point currentMousePosition;
        private Dictionary<Point, string> vertexNames = new Dictionary<Point, string>();
        public Form1()
        {
            InitializeComponent();
            this.Paint += Form1_Paint;
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

      

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Vẽ các cạnh trước
            Font font = new Font("Arial", 8);
            Brush brush = Brushes.Red;

            foreach (var edge in edges)
            {
                Point p1 = edge.Item1;
                Point p2 = edge.Item2;
                int weight = edge.Item3;

                g.DrawLine(Pens.Black, p1, p2);

                // Tính vị trí trung điểm của cạnh để vẽ số liệu
                Point midPoint = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                g.DrawString(weight.ToString(), font, brush, midPoint);
            }

            // Vẽ đường tạm thời khi kéo chuột
            if (selectedVertex != null)
            {
                g.DrawLine(Pens.Gray, selectedVertex.Value, currentMousePosition);
            }

            // Vẽ các đỉnh
            foreach (var vertex in vertices)
            {
                g.FillEllipse(Brushes.Blue, vertex.X - 10, vertex.Y - 10, 20, 20);
                g.DrawEllipse(Pens.Black, vertex.X - 10, vertex.Y - 10, 20, 20);
                if (vertexNames.ContainsKey(vertex))
                {
                    g.DrawString(vertexNames[vertex], font, Brushes.Black, vertex.X + 10, vertex.Y - 10);
                }
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // Kiểm tra xem có nhấp vào một đỉnh không
            foreach (var vertex in vertices)
            {
                if (IsInsideVertex(e.Location, vertex))
                {
                    selectedVertex = vertex;
                    currentMousePosition = e.Location;
                    this.Invalidate();
                    return;
                }
            }

            // Nếu không nhấp vào đỉnh nào, thêm một đỉnh mới
            vertices.Add(e.Location);
            // Nhập tên cho đỉnh
            string vertexName = Microsoft.VisualBasic.Interaction.InputBox(
                "Nhập tên của đỉnh:", "Đặt tên đỉnh", "V" + (vertices.Count));

            if (!string.IsNullOrWhiteSpace(vertexName))
            {
                vertexNames[e.Location] = vertexName;
            }
            this.Invalidate();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedVertex != null)
            {
                // Cập nhật vị trí chuột để vẽ đường tạm thời
                currentMousePosition = e.Location;
                this.Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (selectedVertex != null)
            {
                // Kiểm tra xem có nhả chuột lên một đỉnh khác không
                foreach (var vertex in vertices)
                {
                    if (IsInsideVertex(e.Location, vertex) && vertex != selectedVertex)
                    {
                        // Tạo cạnh giữa hai đỉnh
                        string input = Microsoft.VisualBasic.Interaction.InputBox(
    "Nhập trọng số của cạnh:", "Nhập Trọng Số", "1");

                        // Kiểm tra xem người dùng có nhập đúng số không
                        if (int.TryParse(input, out int weight))
                        {
                            // Kiểm tra xem cạnh đã tồn tại chưa
                            var existingEdge = edges.FirstOrDefault(edge =>
                                (edge.Item1 == selectedVertex.Value && edge.Item2 == vertex) ||
                                   (edge.Item1 == vertex && edge.Item2 == selectedVertex.Value));

                            if (existingEdge != null)
                            {
                                // Nếu cạnh tồn tại, cập nhật trọng số
                                edges.Remove(existingEdge);
                            }

                            // Thêm cạnh mới với trọng số
                            edges.Add(new Tuple<Point, Point, int>(selectedVertex.Value, vertex, weight));                         
                        }
                        break;
                    }
                }
            }
            selectedVertex = null;
            this.Invalidate();
        }
        private bool IsInsideVertex(Point mouse, Point vertex)
        {
            int radius = 10;
            return Math.Sqrt(Math.Pow(mouse.X - vertex.X, 2) + Math.Pow(mouse.Y - vertex.Y, 2)) <= radius;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            foreach(var item in vertexNames)
            {
                
            }

        }
    }
}
