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
        AbstractLightSource _lightSource = null;
        Point3D _pointObserver = null;

        bool _blockEvents = false;

        #endregion

        #region === private methods ===       

        void Render()
        {
            if (_bitmap == null || _model == null)
                return;

            Graphics g = Graphics.FromImage(_bitmap);

            if (cmbBack.SelectedIndex == 0)
                g.Clear(Color.White);
            else
                g.Clear(Color.Black);

            float xTranslate = pictureBox1.Width / 2;
            float yTranslate = pictureBox1.Height / 2;
            Matrix4x4 translate = Matrix4x4.CreateTranslation(xTranslate, yTranslate, 0f);
            _model.Transform(translate);

            // отрисовка модели
            RenderType renderType;

            if (cmbRenderStatus.SelectedIndex == 0)
            {
                renderType = RenderType.Triangulations;
            }
            else if (cmbRenderStatus.SelectedIndex == 1)
            {
                renderType = RenderType.FillFull;
            }
            else
            {
                renderType = RenderType.FillWhite;
            }

            RenderingModel.Render(g, _model, _lightSource, _pointObserver, renderType);

            translate = Matrix4x4.CreateTranslation(-xTranslate, -yTranslate, 0f);
            _model.Transform(translate);

            pictureBox1.Image = _bitmap;
        }

        void UpdateModel()
        {           
            ModelQuality modelQuality;

            if (cmbQuality.SelectedIndex == 1)
            {
                modelQuality = ModelQuality.Middle;
            }
            else if (cmbQuality.SelectedIndex == 2)
            {
                modelQuality = ModelQuality.High;
            }
            else
            {
                modelQuality = ModelQuality.Low;
            }

            int index = cmbModel.SelectedIndex;
            _model = ModelFactory.GetModel(index, 200, modelQuality);
            _model.Transform(_transformMatrix);
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            _blockEvents = true;

            // модель
            cmbModel.BeginUpdate();
            cmbModel.Items.Add("Куб");
            cmbModel.Items.Add("Цветной куб");
            cmbModel.Items.Add("Шахматная доска");
            cmbModel.SelectedIndex = 1;
            cmbModel.EndUpdate();

            // качество
            cmbQuality.BeginUpdate();
            cmbQuality.Items.Add("Низкое");
            cmbQuality.Items.Add("Среднее");
            cmbQuality.Items.Add("Высокое");
            cmbQuality.SelectedIndex = 1;
            cmbQuality.EndUpdate();

            // источник света
            cmbLightSource.BeginUpdate();
            cmbLightSource.Items.Add("Точечный");
            cmbLightSource.Items.Add("Прожектор");
            cmbLightSource.SelectedIndex = 0;
            cmbLightSource.EndUpdate();

            // отображение
            cmbRenderStatus.BeginUpdate();
            cmbRenderStatus.Items.Add("Триангуляция");
            cmbRenderStatus.Items.Add("Полное");
            cmbRenderStatus.Items.Add("Триангуляция. Белая модель.");
            cmbRenderStatus.SelectedIndex = 1;
            cmbRenderStatus.EndUpdate();

            // фон
            cmbBack.BeginUpdate();
            cmbBack.Items.Add("Белый");
            cmbBack.Items.Add("Черный");
            cmbBack.SelectedIndex = 0;
            cmbBack.EndUpdate();

            _blockEvents = false;

            _model = ModelFactory.GetModel(1, 200, ModelQuality.Middle);

            _lightSource = new PointLightSource()
            {
                LightPoint = new Point3D(0, 0, -500)
            };

            _pointObserver = new Point3D(pictureBox1.Width / 2, pictureBox1.Height / 2, -2000);
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

        private void cmbModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            _transformMatrix = Matrix4x4.Identity;
            UpdateModel();            
            Render();
        }

        private void cmbQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            UpdateModel();
            Render();
        }

        private void cmbLightSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            if (cmbLightSource.SelectedIndex == 1)
            {
                _lightSource = new ProjectorLightSource
                {
                    VectorLightSource = new Vector3(-0.05f, -0.05f, -1.0f)
                };
            }
            else
            {
                _lightSource = new PointLightSource()
                {
                    LightPoint = new Point3D(0, 0, -500)
                };
            }

            Render();
        }

        private void cmbRenderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            Render();
        }

        private void cmbBack_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            Render();
        }
    }
}
