using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Models3DLib;

namespace WinSurfaceApp
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
        IPoint3D _pointObserver = null;
        Matrix4x4 _transformMatrix = Matrix4x4.Identity;
        ILightSource _lightSource = null;
        PointF _startPoint;
        Surface _surface = null;

        bool _blockEvents = false;

        #endregion

        #region === private ===

        IPoint3D ResolvePoint3D(float X, float Y, float Z)
        {
            return new Point3D(X, Y, Z);
        }

        Model GetModel()
        {           
            const float cSizePrimitive = 10;
            const float cScaleFactor = 200;

            IPoint3D point1 = ResolvePoint3D(-cScaleFactor, -cScaleFactor, 0);
            IPoint3D point2 = ResolvePoint3D(cScaleFactor, -cScaleFactor, 0);
            IPoint3D point3 = ResolvePoint3D(cScaleFactor, cScaleFactor, 0);
            IPoint3D point4 = ResolvePoint3D(-cScaleFactor, cScaleFactor, 0);

            _surface = new Surface(point1, point2, point3, point4, cSizePrimitive, Color.Gray, string.Empty);

            RectangleF realBr = new RectangleF(-2, -2, 4, 4);
            //Function3D function3D = (x, y) => 1.0f / (1.0f + x * x + y * y);
            Function3D function3D = (x, y) => x * x - y * y;

            _surface.CreateSurface(realBr, function3D, -120, 120);

            return new Model
            {
                NeedToSort = true,
                Planes = new List<Models3DLib.Plane> { _surface }
            };
        }

        void RenderModel(Graphics g, Model model, ILightSource lightSource, IPoint3D pointObserver)
        {
            LightModelParameters lightModelParameters = new LightModelParameters
            {
                LightSources = new List<ILightSource> { lightSource },
                PointObserver = pointObserver,
                ReflectionEnable = true
            };

            IEnumerable<Triangle> triangles = model.GetTrianglesForRender(RenderModelType.FillFull);
            Color[] colors = new Color[3];

            foreach (Triangle triangle in triangles)
            {
                Vector3 normal = triangle.Normal;
                lightModelParameters.Normal = (normal.Z < 0) ? normal : -normal;                
                lightModelParameters.ReflectionBrightness = triangle.ReflectionBrightness;
                lightModelParameters.ReflcetionCone = triangle.ReflectionCone;
                lightModelParameters.BaseColor = triangle.BaseColor;

                for (int i = 0; i < 3; i++)
                {
                    lightModelParameters.Point = triangle.Point3Ds[i];
                    colors[i] = LightModel.GetColor(lightModelParameters);
                }

                int R = Convert.ToInt32(colors.Average(x => x.R));
                int G = Convert.ToInt32(colors.Average(x => x.G));
                int B = Convert.ToInt32(colors.Average(x => x.B));

                Color color = Color.FromArgb(R, G, B);
                Brush brush = new SolidBrush(color);

                g.FillPolygon(brush, triangle.Points);
            }
        }

        void Render()
        {
            if (_bitmap == null || _model == null)
                return;

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

            // отрисовка в главном окне
            RenderModel(g, _model, _lightSource, _pointObserver);

            // восстановили сохраненное состояние
            _model.RestoreState();

            pictureBox1.Image = _bitmap;
        }

        #endregion

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Render();
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
                _surface.Transform(matrix);

                _transformMatrix *= matrix;

                Render();

                _startPoint.X = e.X;
                _startPoint.Y = e.Y;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            _model = GetModel();
            _lightSource = new PointLightSource() { LightPoint = ResolvePoint3D(0, 0, -500) };
            _pointObserver = ResolvePoint3D(pictureBox1.Width / 2, pictureBox1.Height / 2, -1400);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
