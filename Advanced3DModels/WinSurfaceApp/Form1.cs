using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Models3DLib;

/* Урок "Трехмерное моделирование. Построения в пространстве."
 * Все уроки на http://digitalmodels.ru
 * 
 */

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
        Matrix4x4 _transformMatrix;
        readonly IPerspectiveTransform _iperspectiveTransform = new PerspectiveTransformation();
        readonly IRenderPipeline _irenderPipeline = new RenderPipeline();
        Surface _surface = null;
        ILightSource _lightSource = null;
        PointF _startPoint;
        Palette[] _palettes = null;
        Color _surfaceColor = Color.LightGreen;

        bool _blockEvents = false;

        enum ModelQuality
        {
            Low,
            Middle,
            High,
            Extra
        }

        #endregion

        #region === private ===

        Model GetModel()
        {
            int index = cmbModel.SelectedIndex;
            ModelQuality modelQuality = GetModelQuality(cmbQuality.SelectedIndex);

            float sizePrimitive;

            if (modelQuality == ModelQuality.Low)
            {
                sizePrimitive = 16;
            }
            else if (modelQuality == ModelQuality.Middle)
            {
                sizePrimitive = 12;
            }
            else if (modelQuality == ModelQuality.High)
            {
                sizePrimitive = 8;
            }
            else
            {
                sizePrimitive = 6;
            }

            const float cScaleCompFactor = 208;
            const float cScaleRealFactor = 2;

            IPoint3D point1 = ResolvePoint3D(-cScaleCompFactor, -cScaleCompFactor, 0);
            IPoint3D point2 = ResolvePoint3D(cScaleCompFactor, -cScaleCompFactor, 0);
            IPoint3D point3 = ResolvePoint3D(cScaleCompFactor, cScaleCompFactor, 0);
            IPoint3D point4 = ResolvePoint3D(-cScaleCompFactor, cScaleCompFactor, 0);

            Function3D function3D;
            float ZMin, Zmax;

            if (index == 0)
            {
                function3D = (x, y) => 1.0f / (1.0f + x * x + y * y);
                ZMin = -80;
                Zmax = 80;
            }
            else if (index == 1)
            {
                function3D = (x, y) => x * x - y * y;
                ZMin = -100;
                Zmax = 100;
            }
            else if (index == 2)
            {
                function3D = (x, y) => Convert.ToSingle(2 * Math.Exp(-(x * x + y * y)) * Math.Cos(2 * x * x + 2 * y * y));
                ZMin = -80;
                Zmax = 80;
            }
            else if (index == 3)
            {
                function3D = (x, y) => x * x + y * y;
                ZMin = -90;
                Zmax = 90;
            }
            else if (index == 4)
            {
                function3D = (x, y) => x * x * x + y * y * y - 3 * x * y;
                ZMin = -110;
                Zmax = 110;
            }
            else if (index == 5)
            {
                function3D = (x, y) => 1.0f / (1.0f + 4 * x * x + y * y);
                ZMin = -80;
                Zmax = 80;
            }
            else if (index == 6)
            {
                function3D = (x, y) => Convert.ToSingle(2 * Math.Exp(-(x * x + y * y) / 8) * (Math.Sin(x * x) + Math.Cos(y * y)));
                ZMin = -80;
                Zmax = 80;
            }
            else if (index == 7)
            {
                function3D = (x, y) => Convert.ToSingle(Math.Sin(2 * x) + Math.Cos(2 * y));
                ZMin = -60;
                Zmax = 60;
            }
            else if (index == 8)
            {
                function3D = (x, y) => Convert.ToSingle(4 * x * Math.Exp(-(x * x + y * y)));
                ZMin = -100;
                Zmax = 100;
            }
            else
            {
                function3D = (x, y) => Convert.ToSingle(x + y);
                ZMin = -60;
                Zmax = 60;
            }

            _surface = new Surface(point1, point2, point3, point4, sizePrimitive);
           
            RectangleF realBr = new RectangleF(-cScaleRealFactor, -cScaleRealFactor, 2 * cScaleRealFactor, 2 * cScaleRealFactor);

            if (cmbColors.SelectedIndex == 0)
            {
                _surface.CreateSurface(realBr, function3D, ZMin, Zmax, _surfaceColor);
            }
            else
            {
                List<Color> colors = _palettes[cmbPalette.SelectedIndex].CreatePalette();
                _surface.CreateSurface(realBr, function3D, ZMin, Zmax, colors.ToArray());
            }

            return new Model
            {
                Planes = new List<Models3DLib.Plane> { _surface }
            };
        }

        Palette CreateMountainsPalette()
        {
            return new Palette
            {
                BaseColors = new List<Color>
                {
                    Color.LightGreen,
                    Color.DarkGreen,
                    Color.Brown,
                    Color.Blue,
                    Color.LightGray
                }
            };
        }

        Palette CreateHotColdPalette()
        {
            return new Palette
            {
                BaseColors = new List<Color>
                {
                    Color.Blue,
                    Color.Red,
                    Color.Gold
                },

                GradientCount = 4                
            };
        }

        RenderModelType GetRenderType(int index)
        {
            RenderModelType renderType;

            if (index == 0)
            {
                renderType = RenderModelType.Triangulations;
            }
            else if (index == 1)
            {
                renderType = RenderModelType.FillFull;
            }
            else
            {
                renderType = RenderModelType.FillSolidColor;
            }

            return renderType;
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
            else if (index == 3)
            {
                modelQuality = ModelQuality.Extra;
            }
            else
            {
                modelQuality = ModelQuality.Low;
            }

            return modelQuality;
        }

        Matrix4x4 ZeroTransform
        {
            get
            {
                float m11 = -0.0135017429f;
                float m12 = 0.5386155f;
                float m13 = -0.8424442f;
                float m14 = 0;

                float m21 = 0.9974066f;
                float m22 = 0.06683127f;
                float m23 = 0.0267435964f;
                float m24 = 0;

                float m31 = 0.07070663f;
                float m32 = -0.8398974f;
                float m33 = -0.538120866f;
                float m34 = 0;

                float m41 = 0;
                float m42 = 0;
                float m43 = 0;
                float m44 = 0;

                return new Matrix4x4(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);
            }
        }

        IPoint3D ResolvePoint3D(float X, float Y, float Z)
        {
            return new Point3D(X, Y, Z);
        }

        void RenderModel(Graphics g, IEnumerable<Triangle> triangles, RenderFillTriangle renderFillTriangle, RenderModelType renderType)
        {
            if (renderType == RenderModelType.Triangulations)
            {
                foreach (Triangle triangle in triangles)
                {
                    g.DrawPolygon(Pens.Black, triangle.Points);
                }
            }
            else if (renderType == RenderModelType.FillFull)
            {
                foreach (Triangle triangle in triangles)
                {
                    if (renderFillTriangle == RenderFillTriangle.Flat0 || !triangle.AllowToGouraudMethod)
                    {
                        Color color = triangle.RenderColor0;
                        Brush brush = new SolidBrush(color);

                        g.FillPolygon(brush, triangle.Points);
                    }
                    else if (renderFillTriangle == RenderFillTriangle.Flat3)
                    {
                        int R = Convert.ToInt32(triangle.RenderColors.Average(x => x.R));
                        int G = Convert.ToInt32(triangle.RenderColors.Average(x => x.G));
                        int B = Convert.ToInt32(triangle.RenderColors.Average(x => x.B));

                        Color color = Color.FromArgb(R, G, B);
                        Brush brush = new SolidBrush(color);

                        g.FillPolygon(brush, triangle.Points);
                    }
                    else
                    {
                        Color[] surroundColors = triangle.RenderColors;
                        PointF[] points = triangle.Points;

                        PathGradientBrush pthGrBrush = new PathGradientBrush(points)
                        {
                            SurroundColors = surroundColors,
                            CenterPoint = points[0],
                            CenterColor = surroundColors[0]
                        };

                        g.FillPolygon(pthGrBrush, points);

                        for (int i = 0; i < 3; i++)
                        {
                            Vector2 v1 = new Vector2(points[i].X, points[i].Y);
                            Vector2 v2 = new Vector2(points[(i + 1) % 3].X, points[(i + 1) % 3].Y);
                            float len = Vector2.Distance(v1, v2);

                            float x1 = (points[(i + 1) % 3].X - points[i].X) * (-1) / len + points[i].X;
                            float y1 = (points[(i + 1) % 3].Y - points[i].Y) * (-1) / len + points[i].Y;

                            float x2 = (points[(i + 1) % 3].X - points[i].X) * (len + 1) / len + points[i].X;
                            float y2 = (points[(i + 1) % 3].Y - points[i].Y) * (len + 1) / len + points[i].Y;

                            PointF point1 = new PointF(x1, y1);
                            PointF point2 = new PointF(x2, y2);

                            Brush brush = new LinearGradientBrush(point1, point2, surroundColors[i], surroundColors[(i + 1) % 3]);
                            g.DrawLine(new Pen(brush), points[i], points[(i + 1) % 3]);
                        }
                    }
                }
            }
            else if (renderType == RenderModelType.FillSolidColor)
            {
                foreach (Triangle triangle in triangles)
                {
                    g.FillPolygon(Brushes.White, triangle.Points);
                    g.DrawPolygon(Pens.Black, triangle.Points);
                }
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
            _model.PushState();

            // перенос модели в центр окна
            bool perspectiveEnable = checkBoxPerspective.Checked;
            RenderModelType renderType = GetRenderType(cmbRenderStatus.SelectedIndex);
            RenderFillTriangle renderFillTriangle = RenderFillTriangle.Flat3;

            IRenderPipelineParameters irpp = new RenderPipelineParameters
            {
                TranslateX = pictureBox1.Width / 2,
                TranslateY = pictureBox1.Height / 2,
                TranslateZ = 0,
                renderModelType = renderType,
                PerspectiveEnable = perspectiveEnable,
                PerspectiveTransform = _iperspectiveTransform,
                CenterPerspective = _pointObserver,
                FogEnable = false,
                lightSources = new List<ILightSource> { _lightSource },
                pointObserver = _pointObserver,
                renderFillTriangle = renderFillTriangle,
                InvertNormal = true
            };

            // отрисовка модели
            RenderModel(g, _irenderPipeline.RenderPipeline(_model, irpp), renderFillTriangle, renderType);

            // восстановили сохраненное состояние
            _model.PopState();

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
                _transformMatrix *= matrix;

                Render();

                _startPoint.X = e.X;
                _startPoint.Y = e.Y;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox2.BackColor = _surfaceColor;
            cmbPalette.Visible = label5.Visible = false;
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            _blockEvents = true;

            // модели
            cmbModel.BeginUpdate();
            cmbModel.Items.Add("Шляпа");
            cmbModel.Items.Add("Седло");
            cmbModel.Items.Add("Затухающие колебания");
            cmbModel.Items.Add("Параболоид");
            cmbModel.Items.Add("Скат");
            cmbModel.Items.Add("Плавник");
            cmbModel.Items.Add("Волны");
            cmbModel.Items.Add("Холмы");
            cmbModel.Items.Add("Холм и яма");
            cmbModel.Items.Add("Плоскость");
            cmbModel.SelectedIndex = 0;
            cmbModel.EndUpdate();

            // качество
            cmbQuality.BeginUpdate();
            cmbQuality.Items.Add("Низкое");
            cmbQuality.Items.Add("Среднее");
            cmbQuality.Items.Add("Высокое");
            cmbQuality.Items.Add("Экстра");
            cmbQuality.SelectedIndex = 1;
            cmbQuality.EndUpdate();

            // отображение
            cmbRenderStatus.BeginUpdate();
            cmbRenderStatus.Items.Add("Каркас");
            cmbRenderStatus.Items.Add("Полное");
            cmbRenderStatus.Items.Add("Каркас. Без цвета.");
            cmbRenderStatus.SelectedIndex = 1;
            cmbRenderStatus.EndUpdate();

            // раскраска
            cmbColors.BeginUpdate();
            cmbColors.Items.Add("Один цвет");
            cmbColors.Items.Add("Палитра");
            cmbColors.SelectedIndex = 0;
            cmbColors.EndUpdate();

            // палитры
            cmbPalette.BeginUpdate();
            cmbPalette.Items.Add("Горы");
            cmbPalette.Items.Add("Холодное-горячее");
            cmbPalette.SelectedIndex = 0;
            cmbPalette.EndUpdate();

            _blockEvents = false;

            _palettes = new Palette[] { CreateMountainsPalette(), CreateHotColdPalette() };

            _model = GetModel();
            Matrix4x4 zeroTansform = ZeroTransform;
            _model.Transform(zeroTansform);
            _transformMatrix = zeroTansform;

            _lightSource = new PointLightSource() { LightPoint = ResolvePoint3D(0, 0, -500), Weight = 0.9f };
            _pointObserver = ResolvePoint3D(pictureBox1.Width / 2, pictureBox1.Height / 2, -1400);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            _model = GetModel();
            Matrix4x4 zeroTansform = ZeroTransform;
            _model.Transform(zeroTansform);
            _transformMatrix = zeroTansform;

            Render();
        }

        private void checkBoxPerspective_CheckedChanged(object sender, EventArgs e)
        {
            Render();
        }

        private void cmbQuality_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            _model = GetModel();
            _model.Transform(_transformMatrix);

            Render();
        }

        private void cmbRenderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            Render();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            Vector3 axis = Vector3.Normalize(_surface.Normal);
            float angle = Convert.ToSingle(-Math.PI / 2 / 16);

            Matrix4x4 matrix = Matrix4x4.CreateFromAxisAngle(axis, angle);
            _model.Transform(matrix);
            _transformMatrix *= matrix;

            Render();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            Vector3 axis = Vector3.Normalize(_surface.Normal);
            float angle = Convert.ToSingle(Math.PI / 2 / 16);

            Matrix4x4 matrix = Matrix4x4.CreateFromAxisAngle(axis, angle);
            _model.Transform(matrix);
            _transformMatrix *= matrix;

            Render();
        }

        private void cmbColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            if (cmbColors.SelectedIndex == 0)
            {
                pictureBox2.Visible = btnOneColor.Visible = true;
                cmbPalette.Visible = label5.Visible = false;
            }
            else
            {
                pictureBox2.Visible = btnOneColor.Visible = false;
                cmbPalette.Visible = label5.Visible = true;
            }

            _model = GetModel();
            _model.Transform(_transformMatrix);

            Render();
        }

        private void btnOneColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog
            {
                Color = _surfaceColor
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.BackColor = dialog.Color;
                _surfaceColor = dialog.Color;
                _surface.SetColor(dialog.Color);
                Render();
            }
        }

        private void cmbPalette_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            _model = GetModel();
            _model.Transform(_transformMatrix);

            Render();
        }
    }
}
