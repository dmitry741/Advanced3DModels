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
using System.Numerics;

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
        Matrix4x4 _transformMatrix = Matrix4x4.Identity;
        ILightSource _lightSource = null;

        #endregion

        #region === private methods ===       

        void Render()
        {
            if (_bitmap == null || _model == null)
                return;

            Graphics g = Graphics.FromImage(_bitmap);
            g.Clear(Color.White);

            float xTranslate = pictureBox1.Width / 2;
            float yTranslate = pictureBox1.Height / 2;
            Matrix4x4 translate = Matrix4x4.CreateTranslation(xTranslate, yTranslate, 0f);
            _model.Transform(translate);

            // отрисовка модели
            RenderingModel.Render(g, _model, _lightSource, RenderType.Fill);

            translate = Matrix4x4.CreateTranslation(-xTranslate, -yTranslate, 0f);
            _model.Transform(translate);

            pictureBox1.Image = _bitmap;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            //_model = Model.Cube(200, 20.0f);
            _model = Model.CubeColored(200, 20.0f);

            _lightSource = new PointLightSource()
            {
                LightPoint = new Point3D(0, 0, -500)
            };
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

            if (e.Button == MouseButtons.Left)
            {
                const float cDivider = 100;

                float angleXZ = -(e.X - _startPoint.X) / cDivider;
                float angleYZ = (e.Y - _startPoint.Y) / cDivider;

                Matrix4x4 matrixRotationXZ = Matrix4x4.CreateRotationY(angleXZ);
                Matrix4x4 matrixRotationYZ = Matrix4x4.CreateRotationX(angleYZ);
                Matrix4x4 matrix = matrixRotationXZ * matrixRotationYZ;

                _model.Transform(matrix);

                _transformMatrix *= matrix;

                Render();

                _startPoint.X = e.X;
                _startPoint.Y = e.Y;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _model = Model.Cube(200, 40.0f);
            _model.Transform(_transformMatrix);
            Render();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _model = Model.Cube(200, 8.0f);
            _model.Transform(_transformMatrix);
            Render();
        }
    }
}
