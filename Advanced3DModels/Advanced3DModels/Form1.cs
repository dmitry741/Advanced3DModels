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

namespace Advanced3DModels
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region === members ===

        Bitmap _bitmap;
        Model _model;
        PointF _startPoint = PointF.Empty;

        #endregion

        #region === private methods ===       

        void Render()
        {
            if (_bitmap == null || _model == null)
                return;

            Graphics g = Graphics.FromImage(_bitmap);
            g.Clear(Color.White);

            // отрисовка модели
            RenderingModel.Render(g, _model, RenderType.Edges);

            pictureBox1.Image = _bitmap;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            _model = Model.Cube(Math.Min(pictureBox1.Width, pictureBox1.Height) / 2, 16.0f);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _startPoint = new PointF(e.X, e.Y);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_model == null)
                return;

            /*if (e.Button == MouseButtons.Left)
            {
                PointF point = new PointF(e.X, e.Y);
                const double cDivider = 64;

                double angleXZ = (point.X - _startPoint.X) / cDivider;
                double angleYZ = (point.Y - _startPoint.Y) / cDivider;




                Render();

                _startPoint = point;
            }*/
        }
    }
}
