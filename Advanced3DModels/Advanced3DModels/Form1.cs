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
        Bitmap _bitmapLookAt;
        Model _model;
        PointF _startPoint = PointF.Empty;
        Point3D _pointObserver = null;
        Matrix4x4 _transformMatrix = Matrix4x4.Identity;
        AbstractLightSource _lightSource = null;        
        IPerspectiveTransform _iperspectiveTransform = new PerspectiveTransformation();
        IFog _ifog = null;

        bool _blockEvents = false;

        #endregion

        #region === private methods ===
        
        RenderType GetRenderType(int index)
        {
            RenderType renderType;

            if (index == 0)
            {
                renderType = RenderType.Triangulations;
            }
            else if (index == 1)
            {
                renderType = RenderType.FillFull;
            }
            else
            {
                renderType = RenderType.FillWhite;
            }

            return renderType;
        }

        Camera CameraLookAt => cmbLookAt.SelectedItem as Camera;

        void Render()
        {
            if (_bitmap == null || _model == null)
                return;

            Graphics g1 = Graphics.FromImage(_bitmap);
            Color backColor = cmbBack.SelectedIndex == 0 ? Color.White : Color.Black;

            // отрисовка фона
            g1.Clear(backColor);

            // перенос модели в центр окна
            float xTranslate = pictureBox1.Width / 2;
            float yTranslate = pictureBox1.Height / 2;
            Matrix4x4 translate = Matrix4x4.CreateTranslation(xTranslate, yTranslate, 0f);
            _model.Transform(translate);

            // отрисовка модели
            RenderType renderType = GetRenderType(cmbRenderStatus.SelectedIndex);

            Model model = !checkBoxPerspective.Checked ?
                _model : 
                Model.Perspective(_model, _iperspectiveTransform, _pointObserver);

            RenderingModel.Render(g1, model, _lightSource, _pointObserver, _ifog, renderType, backColor);

            // перенос модели в начало координат
            translate = Matrix4x4.CreateTranslation(-xTranslate, -yTranslate, 0f);
            _model.Transform(translate);

            pictureBox1.Image = _bitmap;

            // окно с дополнительной камерой
            Graphics g2 = Graphics.FromImage(_bitmapLookAt);

            // отрисовка фона
            g2.Clear(backColor);

            bool bLookAt = checkBoxCamera.Checked;

            if (bLookAt)
            {
                const float cScaleFactor = 0.5f;
                const float cCameraFactor = 700.0f;

                // создаем матрицу масштабирования
                Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(cScaleFactor);

                // создаем матрицу просмотра
                Vector3 cameraPosition = cCameraFactor * CameraLookAt.VectorLookAt;
                Vector3 cameraTarget = Vector3.Zero;
                Vector3 cameraUpVector = new Vector3(0, 1, 0);

                Matrix4x4 lookAtMatrix = Matrix4x4.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);

                // получаем результирующую матрицу масштабирования и просмотра
                Matrix4x4 matrixView = lookAtMatrix * scaleMatrix;

                // получаем обртаную матрицу
                if (Matrix4x4.Invert(matrixView, out var invertMatrix))
                {
                    // применяем преобразование
                    _model.Transform(matrixView);

                    // перенос модели в центр окна
                    xTranslate = pictureBox2.Width / 2;
                    yTranslate = pictureBox2.Height / 2;
                    translate = Matrix4x4.CreateTranslation(xTranslate, yTranslate, 0f);
                    _model.Transform(translate);

                    // отрисовка модели
                    Point3D observerLookAt = new Point3D(pictureBox2.Width / 2, pictureBox2.Height / 2, _pointObserver.Z);

                    bool bFog = _ifog.Enabled;
                    _ifog.Enabled = false;
                    RenderingModel.Render(g2, _model, _lightSource, observerLookAt, _ifog, renderType, backColor);
                    _ifog.Enabled = bFog;

                    // возвращаем модель в начало координат
                    translate = Matrix4x4.CreateTranslation(-xTranslate, -yTranslate, 0f);
                    _model.Transform(translate);

                    // восстанавливаем модель с помощью обратной матрицы
                    _model.Transform(invertMatrix);
                }
            }

            pictureBox2.Image = _bitmapLookAt;
        }

        ModelQuality GetModelQuality(int index)
        {
            ModelQuality modelQuality;

            if (index == 1)
            {
                modelQuality = ModelQuality.Middle;
            }
            else if (index == 2)
            {
                modelQuality = ModelQuality.High;
            }
            else
            {
                modelQuality = ModelQuality.Low;
            }

            return modelQuality;
        }

        void UpdateModel()
        {
            ModelQuality modelQuality = GetModelQuality(cmbQuality.SelectedIndex);
            int index = cmbModel.SelectedIndex;
            _model = ModelFactory.GetModel(index, modelQuality);
            _model.Transform(_transformMatrix);
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            _bitmapLookAt = new Bitmap(pictureBox2.Width, pictureBox2.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            _blockEvents = true;

            // модель
            cmbModel.BeginUpdate();
            cmbModel.Items.Add("Куб");
            cmbModel.Items.Add("Цветной куб");
            cmbModel.Items.Add("Шахматный столик");
            cmbModel.Items.Add("Куб из кубиков");
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

            // камера
            cmbLookAt.BeginUpdate();
            cmbLookAt.Items.Add(new Camera { VectorLookAt = new Vector3(-1, 0, 0), Description = "Справа" });
            cmbLookAt.Items.Add(new Camera { VectorLookAt = new Vector3(1, 0, 0), Description = "Слева" });
            cmbLookAt.SelectedIndex = 0;
            cmbLookAt.EndUpdate();

            _blockEvents = false;

            _model = ModelFactory.GetModel(1, ModelQuality.Middle);

            _ifog = new FogCorrection(0.008f, -_model.BoundSize)
            {
                Enabled = false
            };

            _lightSource = new PointLightSource()
            {
                LightPoint = new Point3D(0, 0, -500)
            };

            _pointObserver = new Point3D(pictureBox1.Width / 2, pictureBox1.Height / 2, -1400);
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

        private void checkBoxPerspective_CheckedChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            Render();
        }

        private void checkBoxCamera_CheckedChanged(object sender, EventArgs e)
        {
            Render();
        }

        private void cmbLookAt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            Render();
        }

        private void checkBoxFog_CheckedChanged(object sender, EventArgs e)
        {
            _ifog.Enabled = checkBoxFog.Checked;
            Render();
        }
    }
}
