using Models3DLib;
using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

/* Урок "Трехмерное моделирование. Построения в пространстве."
 * Все уроки на http://digitalmodels.ru
 * 
 */

namespace WinTextureModels
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region === members ===

        Bitmap _bitmap = null;
        PointF _startPoint = PointF.Empty;
        IPoint3D _pointObserver = null;
        ILightSource _lightSource = null;
        readonly IPerspectiveTransform _iperspectiveTransform = new PerspectiveTransformation();
        Model _model = null;

        #endregion

        #region === private ===

        Model GetModel()
        {
            float sizePrimitive = 4;

            Color back = Color.FromArgb(94, 27, 44);
            Color[] colors = new Color[] { back, back, back, back, back, back };
            bool[] panels = new bool[] { true, true, true, true, true, true };
            Model model = PresetsModel.Parallelepiped(353, 219, 36, sizePrimitive, panels, colors);

            int iterator = 0;

            foreach(Models3DLib.Plane plane in model.Planes)
            {
                plane.Name = iterator.ToString();
                iterator++;
            }

            Bitmap texture = Properties.Resources.sweets;
            model.SetTexture("5", texture, true);

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
            g.Clear(Color.Gray);

            // запомнили состояние модели
            _model.PushState();

            // перенос модели в центр окна
            float xTranslate = pictureBox1.Width / 2;
            float yTranslate = pictureBox1.Height / 2;
            Matrix4x4 translate = Matrix4x4.CreateTranslation(xTranslate, yTranslate, 0f);
            _model.Transform(translate);

            bool bPerspective = checkBox1.Checked;

            // перспективное преобразование
            if (bPerspective)
            {
                _model.Transform(_iperspectiveTransform, _pointObserver);
            }

            // отрисовка в главном окне
            RenderingModel.Render(g, _model, _lightSource, _pointObserver, RenderModelType.FillFull);

            // восстановили сохраненное состояние
            _model.PopState();

            pictureBox1.Image = _bitmap;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;

            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            _model = GetModel();
            _lightSource = new PointLightSource { LightPoint = ResolvePoint3D(0, 0, -500) };
            _pointObserver = ResolvePoint3D(pictureBox1.Width / 2, pictureBox1.Height / 2, -1400);
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

            Render();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            float angle = Convert.ToSingle(-Math.PI / 2 / 15);
            Matrix4x4 matrixRotation = Matrix4x4.CreateRotationY(angle);

            _model.Transform(matrixRotation);

            Render();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            float angle = Convert.ToSingle(Math.PI / 2 / 15);
            Matrix4x4 matrixRotation = Matrix4x4.CreateRotationX(angle);

            _model.Transform(matrixRotation);

            Render();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            float angle = Convert.ToSingle(Math.PI / 2 / 15);
            Matrix4x4 matrixRotation = Matrix4x4.CreateRotationY(angle);

            _model.Transform(matrixRotation);

            Render();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Render();
        }
    }
}
