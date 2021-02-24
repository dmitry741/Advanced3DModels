using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Models3DLib;

namespace WinPixelModels
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region === members ===

        Bitmap _bitmap;
        IPixelsModel _ipixelsModel;

        #endregion

        #region === private methods ===

        void Render()
        {
            if (_bitmap == null || _ipixelsModel == null)
                return;

            // создание контекста для рисования
            Graphics g = Graphics.FromImage(_bitmap);

            // отрисовка фона
            g.Clear(Color.White);

            // TODO:

            pictureBox1.Image = _bitmap;
        }

        IPoint3D ResolvePoint3D(float X, float Y, float Z)
        {
            return new Point3D(X, Y, Z);
        }

        IPixelsModel GetModel(int index)
        {
            IPoint3D point3D = ResolvePoint3D(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
            float radius = Math.Min(pictureBox1.Width / 2, pictureBox1.Height / 2) - 80;

            return new Sphere(point3D, radius, Color.DarkGray);
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            _ipixelsModel = GetModel(0);

            // TODO:
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
