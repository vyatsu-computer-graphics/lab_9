﻿using System;
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
        
        const int iter = 15;
        const double min = 1e-6;
        const double max = 1e+6;

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
            //Mandelbrot(e.Graphics, n);
            //Tree(e.Graphics, 200, Math.PI / 4);
            //Dragon(e.Graphics);
            Newton(e.Graphics);
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
            xc = (Width - 10) / 2;                  
            yc = (Height - 10) / 2;
 
            for(y = -yc; y < yc; y++)
            {
                for (x = -xc; x < xc; x++)
                {
                    n = 0;
                    c.x = x * 0.01 + 1;
                    c.y = y * 0.01;
 
                    z.x = 0.5;
                    z.y = 0;
 
                    while (z.x * z.x + z.y * z.y < max && n < iterations)
                    {
                        p = z.x - z.x * z.x + z.y * z.y;
                        q = z.y - 2 * z.x * z.y;
                        z.x = c.x * p - c.y * q;
                        z.y = c.x * q + c.y * p;
                        n++;
                    }

                    if (n >= iterations) continue;
                    
                    MyPen.Color = Color.FromArgb(255, 0, n * 15 % 255, n * 20 % 255);
                    graph.DrawRectangle(MyPen, xc + x, yc + y, 1, 1);
                }
            }
        }

        private void Tree(Graphics graph, double a, double b)
        {
            MyPen.Color = Color.GreenYellow;
            DrawTree(graph, (int)(ClientSize.Width / 2 - a), (int)(ClientSize.Height / 2 + a), a, b);
            MyPen.Color = Color.Black;
        }

        private void DrawTree(Graphics graph, int x, int y, double a, double b)
        {
            if (a <= 1) return;
            DrawFirstLine(graph, x, y, a, b);
            x = (int)Math.Round(x + a * Math.Cos(b));
            y = (int)Math.Round(y - a * Math.Sin(b));
            DrawTree(graph, x, y, a * 0.4, b - 14 * Math.PI / 60);
            DrawTree(graph, x, y, a * 0.4, b + 14 * Math.PI / 60);
            DrawTree(graph, x, y, a * 0.7, b + Math.PI / 20);
        }

        private void DrawFirstLine(Graphics graph, int x, int y, double a, double b)
        {
            graph.DrawLine(MyPen, x, y, (int)Math.Round(x + a * Math.Cos(b)), (int)Math.Round(y - a * Math.Sin(b)));
        }

        private void Dragon(Graphics graph)
        {
            int x1, y1, x2, y2, n;
 
            x1 = ClientSize.Width / 2;
            y1 = ClientSize.Height / 2;
            x2 = ClientSize.Width / 3;
            y2 = ClientSize.Width / 3;
            n  = 10;
            
            DrawDragon(graph, x1, y1, x2, y2,n);
        }
        
        void DrawDragon(Graphics graph, int x1, int y1, int x2, int y2, int n)
        {
            int xn, yn;
 
            if (n > 0)
            {
                xn = (x1 + x2) / 2 + (y2 - y1) / 2;
                yn = (y1 + y2) / 2 - (x2 - x1) / 2 ;
 
                DrawDragon(graph, x2, y2, xn, yn, n - 1);
                DrawDragon(graph, x1, y1, xn, yn, n - 1);
            }
 
            var point1 = new Point(x1, y1);
            var point2 = new Point(x2, y2);
            graph.DrawLine(MyPen, point1, point2);
        }

        private void Newton(Graphics graph)
        {
            var mx = ClientSize.Width; 
            var my = ClientSize.Height; 
            DrawNewton(graph, mx, my);
        }

        private void DrawNewton(Graphics graph, int mx1, int my1)
        {
            int n, mx, my;
            double p;
            Complex z, t, d = new();
 
            mx = mx1 / 2;
            my = my1 / 2;
 
            for (int y = -my; y < my; y++)
            for (int x = -mx; x < mx; x++)
            {
                n = 0;
                z.x = x * 0.005;
                z.y = y * 0.005;
                d = z;
 
                while (Math.Pow(z.x, 2) + Math.Pow(z.y, 2) < max && Math.Pow(d.x, 2) + Math.Pow(d.y, 2) > min && n < iter)
                {
                    t = z;
                    p = Math.Pow(Math.Pow(t.x, 2) + Math.Pow(t.y, 2), 2);
                    z.x = 2 / 3 * t.x + (Math.Pow(t.x, 2) - Math.Pow(t.y, 2)) / (3*p);
                    z.y = 2 / 3 * t.y * (1 - t.x / p);
                    d.x = Math.Abs(t.x - z.x);
                    d.y = Math.Abs(t.y - z.y);
                    n++;
                }
                MyPen.Color = Color.FromArgb(255, n * 100 % 255, 0, n * 100 % 255);
                graph.DrawRectangle(MyPen, mx + x, my + y, 1, 1);
            }
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MyPen.Dispose();
        }
    }
}