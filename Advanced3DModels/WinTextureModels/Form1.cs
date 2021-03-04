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

namespace WinTextureModels
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region === members ===

        enum ModelQuality
        {
            Low,
            Middle,
            High
        }

        Bitmap _bitmap = null;
        //bool _blockEvent = false;
        PointF _startPoint = PointF.Empty;
        IPoint3D _pointObserver = null;
        Matrix4x4 _transformMatrix = Matrix4x4.Identity;
        ILightSource _lightSource = null;
        IPerspectiveTransform _iperspectiveTransform = new PerspectiveTransformation();
        Model _model = null;

        #endregion

        #region === private ===

        Model GetModel(ModelQuality modelQuality)
        {
            float sizePrimitive = 12;

            /*switch (modelQuality)
            {
                case ModelQuality.Middle:
                    sizePrimitive = 12;
                    break;
                case ModelQuality.High:
                    sizePrimitive = 8;
                    break;
                default:
                    sizePrimitive = 16;
                    break;
            }*/

            Model model = PresetsModel.Cube(300.0f, sizePrimitive);

            foreach(Models3DLib.Plane plane in model.Planes)
            {
                plane.Name = "side";
            }

            return model;
        }

        IPoint3D ResolvePoint3D(float X, float Y, float Z)
        {
            return new Point3D(X, Y, Z);
        }

        void Render()
        {
            if (_bitmap == null || _model == null)
                return;

            // создание контекста для рисования
            Graphics g = Graphics.FromImage(_bitmap);

            // отрисовка фона
            g.Clear(Color.White);

            // запомнили состояние модели
            _model.SaveState();

            // перенос модели в центр окна
            float xTranslate = pictureBox1.Width / 2;
            float yTranslate = pictureBox1.Height / 2;
            Matrix4x4 translate = Matrix4x4.CreateTranslation(xTranslate, yTranslate, 0f);
            _model.Transform(translate);

            bool bPerspective = false;

            // перспективное преобразование
            if (bPerspective)
            {
                _model.Transform(_iperspectiveTransform, _pointObserver);
            }

            // отрисовка в главном окне
            RenderingModel.Render(g, _model, _lightSource, _pointObserver, RenderModelType.FillFull);

            // восстановили сохраненное состояние
            _model.RestoreState();

            pictureBox1.Image = _bitmap;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;

            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            _model = GetModel(ModelQuality.Middle);
            _lightSource = new PointLightSource { LightPoint = ResolvePoint3D(0, 0, -500) };
            _pointObserver = ResolvePoint3D(pictureBox1.Width / 2, pictureBox1.Height / 2, -1400);

            // TODO
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void OnMouseDownEvent(object sender, MouseEventArgs e)
        {
            _startPoint = new PointF(e.X, e.Y);
        }

        private void OnMouseMoveEvent(object sender, MouseEventArgs e)
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

        private void btnUp_Click(object sender, EventArgs e)
        {
            float angle = Convert.ToSingle(-Math.PI / 2 / 15);
            Matrix4x4 matrixRotation = Matrix4x4.CreateRotationX(angle);

            _model.Transform(matrixRotation);
            _transformMatrix *= matrixRotation;

            Render();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            float angle = Convert.ToSingle(-Math.PI / 2 / 15);
            Matrix4x4 matrixRotation = Matrix4x4.CreateRotationY(angle);

            _model.Transform(matrixRotation);
            _transformMatrix *= matrixRotation;

            Render();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            float angle = Convert.ToSingle(Math.PI / 2 / 15);
            Matrix4x4 matrixRotation = Matrix4x4.CreateRotationX(angle);

            _model.Transform(matrixRotation);
            _transformMatrix *= matrixRotation;

            Render();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            float angle = Convert.ToSingle(Math.PI / 2 / 15);
            Matrix4x4 matrixRotation = Matrix4x4.CreateRotationY(angle);

            _model.Transform(matrixRotation);
            _transformMatrix *= matrixRotation;

            Render();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap texture = Properties.Resources.wood1;
            _model.SetTexture("side", texture, true);

            Render();
        }
    }
}
