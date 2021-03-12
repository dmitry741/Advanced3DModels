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
using System.Drawing.Drawing2D;

namespace WinShadow
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
        ProjectorLightSource _lightSource = null;
        IPerspectiveTransform _iperspectiveTransform = new PerspectiveTransformation();
        PointF _startPoint;

        bool _blockEvents = false;

        #endregion

        #region === private methods ===

        Model GetModel(int index)
        {
            const float cSizePrimitive = 16;

            Model model;

            if (index == 0)
            {
                model = PresetsModel.Cube(120.0f, cSizePrimitive);
            }
            else
            {
                const float cScaleFactor = 0.5f;
                model = PresetsModel.Arrow(200 * cScaleFactor, 60 * cScaleFactor, 60 * cScaleFactor, 100 * cScaleFactor, 24, cSizePrimitive, Color.DarkGoldenrod);
            }

            return model;
        }

        IPoint3D ResolvePoint3D(float X, float Y, float Z)
        {
            return new Point3D(X, Y, Z);
        }

        void RenderString(Graphics g)
        {
            const string cScript = "Отображение теней";
            Font font = new Font("Arial", 36f, FontStyle.Regular);
            SizeF sz = g.MeasureString(cScript, font);

            g.DrawString(cScript, font, Brushes.DarkGray, (pictureBox1.Width - sz.Width) / 2, (pictureBox1.Height - sz.Height) / 2);
        }

        void RenderShadow(Graphics g, Model model, ProjectorLightSource lightSource)
        {
            const float z = 1900;

            Vector3 point1 = new Vector3(0, 0, z);
            Vector3 point2 = new Vector3(1, 0, z);
            Vector3 point3 = new Vector3(0, 1, z);

            System.Numerics.Plane plane = System.Numerics.Plane.CreateFromVertices(point1, point2, point3);
            Matrix4x4 shadowMatrix = Matrix4x4.CreateShadow(-lightSource.VectorLightSource, plane);
            Region region = new Region();
            region.MakeEmpty();

             foreach (Models3DLib.Plane p in model.Planes)
             {
                 foreach(Triangle triangle in p.Triangles)
                 {
                     IEnumerable<Vector3> shadowPoints = triangle.Point3Ds.Select(point => Vector3.Transform(point.ToVector3(), shadowMatrix));
                     PointF[] shadow = shadowPoints.Select(point => new PointF(point.X, point.Y)).ToArray();
                     GraphicsPath gp = new GraphicsPath();
                     gp.AddPolygon(shadow);
                     region.Union(gp);
                 }
             }

             Brush brush = new SolidBrush(Color.FromArgb(128, 0, 0, 0));
             g.FillRegion(brush, region);
        }

        void RenderModel(Graphics g, Model model, ILightSource lightSource, IPoint3D pointObserver)
        {
            LightModelParameters lightModelParameters = new LightModelParameters
            {
                LightSources = new List<ILightSource> { lightSource },
                PointObserver = pointObserver,
            };

            IEnumerable<Triangle> triangles = model.GetTrianglesForRender(RenderModelType.FillFull);
            Color[] colors = new Color[3];

            foreach (Triangle triangle in triangles)
            {
                lightModelParameters.Normal = triangle.Normal;
                lightModelParameters.ReflectionEnable = triangle.ReflectionEnable;
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

            RenderString(g);

            // запомнили состояние модели
            _model.SaveState();

            // перенос модели в центр окна
            float xTranslate = 3 * pictureBox1.Width / 8;
            float yTranslate = 3 * pictureBox1.Height / 8;
            Matrix4x4 translate = Matrix4x4.CreateTranslation(xTranslate, yTranslate, 0f);
            _model.Transform(translate);

            // отрисовка модели
            bool bPerspective = false;

            if (bPerspective)
            {
                // перспективное преобразование
                _model.Transform(_iperspectiveTransform, _pointObserver);
            }

            // отрисовка тени
            RenderShadow(g, _model, _lightSource);

            // отрисовка в главном окне
            RenderModel(g, _model, _lightSource, _pointObserver);

            // восстановили сохраненное состояние
            _model.RestoreState();

            pictureBox1.Image = _bitmap;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            _blockEvents = true;

            comboBox1.BeginUpdate();
            comboBox1.Items.AddRange(new string[] { "Куб", "Стрелка" });
            comboBox1.SelectedIndex = 0;
            comboBox1.EndUpdate();

            _blockEvents = false;

            _model = GetModel(0);

            _lightSource = new ProjectorLightSource
            {
                VectorLightSource = new Vector3(-0.05f, -0.05f, -1.0f)
            };

            _pointObserver = ResolvePoint3D(pictureBox1.Width / 2, pictureBox1.Height / 2, -1400);

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            _model = GetModel(comboBox1.SelectedIndex);
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

                Render();

                _startPoint.X = e.X;
                _startPoint.Y = e.Y;
            }
        }
    }
}
