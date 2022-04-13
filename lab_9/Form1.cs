using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab_9
{
    public partial class Form1 : Form
    {
        private Pen MyPen;

        private int n;
        private int size;
        private Point A, B, C;

        private struct Complex
        {
            public double x;
            public double y;
        }
        
        public Form1()
        {
            InitializeComponent();
            MyPen = new Pen(Color.Black);

            n = 30;
            size = 1000;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            InitStartPoints();
        }

        private void InitStartPoints()
        {
            var centerX = ClientSize.Width / 2;
            var centerY = ClientSize.Height / 2;

            A = new Point(centerX - size / 2, centerY - size / 3);
            B = new Point(centerX + size / 2, centerY - size / 3);
            C = new Point(centerX, centerY + size / 3);
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            //SerpTriangle(e.Graphics, n, A, B, C);
            Mandelbrot(e.Graphics, n);
        }

        private void SerpTriangle(Graphics graph, int n, Point a, Point b, Point c)
        {
            graph.DrawPolygon(MyPen, new[]{a, b, c});

            if (n == 0)
            {
                return;
            }
            
            var ab = new Point((a.X + b.X) / 2, (a.Y + b.Y) / 2);
            var bc = new Point((b.X + c.X) / 2, (b.Y + c.Y) / 2);
            var ca = new Point((c.X + a.X) / 2, (c.Y + a.Y) / 2);
            
            SerpTriangle(graph, n - 1, ab, a, ca);
            SerpTriangle(graph, n - 1, b, ab, bc);
            SerpTriangle(graph, n - 1, bc, ca, c);
        }
        
        private void Mandelbrot(Graphics graph, int N)
        {
            int iterations = N, max = 10;
            int xc, yc;            
            int x, y, n;
            double p, q;             
            Complex z, c;         
            xc = (Width-10)/2;                  
            yc = (Height-10)/2;
 
            for(y=-yc; y < yc; y++)
            {
                for (x = -xc; x < xc; x++)
                {
                    n = 0;
                    c.x = x * 0.01 + 1;
                    c.y = y * 0.01;
 
                    z.x = 0.5;
                    z.y = 0;
 
                    while ( (z.x*z.x + z.y*z.y < max) && (n < iterations) )
                    {
                        p = z.x - z.x*z.x + z.y*z.y;
                        q = z.y - 2 * z.x * z.y;
                        z.x = c.x * p - c.y * q;
                        z.y = c.x * q + c.y * p;
                        n++;
                    }
                    if (n < iterations)
                    {
                        MyPen.Color = Color.FromArgb(255, 0, (n * 15) % 255, (n * 20) % 255);
                        graph.DrawRectangle(MyPen, xc + x, yc + y, 1, 1);
                    }
 
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MyPen.Dispose();
        }
    }
}