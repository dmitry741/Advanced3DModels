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
        IPoint3D _pointObserver = null;
        Matrix4x4 _transformMatrix = Matrix4x4.Identity;
        ILightSource _lightSource = null;        
        readonly IPerspectiveTransform _iperspectiveTransform = new PerspectiveTransformation();
        readonly IRenderPipeline _irenderPipeline = new RenderPipeline();
        readonly IFog _ifog = new FogCorrection(0.00005f, -200);

        bool _blockEvents = false;

        #endregion

        #region === private methods ===

        IPoint3D ResolvePoint3D(float X, float Y, float Z)
        {
            return new Point3D(X, Y, Z);
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

        Camera CameraLookAt => cmbLookAt.SelectedItem as Camera;

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

            // создание графического контекста
            Graphics g1 = Graphics.FromImage(_bitmap);

            // отрисовка фона
            g1.Clear(Color.White);

            // запомнили состояние модели
            _model.PushState();

            bool perspectiveEnable = checkBoxPerspective.Checked;
            RenderModelType renderType = GetRenderType(cmbRenderStatus.SelectedIndex);
            RenderFillTriangle renderFillTriangle = GetRenderFillTriangle(cmbTriRender.SelectedIndex);

            IRenderPipelineParameters irpp1 = new RenderPipelineParameters
            {
                TranslateX = pictureBox1.Width / 2,
                TranslateY = pictureBox1.Height / 2,
                TranslateZ = 0,
                renderModelType = renderType,
                PerspectiveEnable = perspectiveEnable,
                PerspectiveTransform = _iperspectiveTransform,
                CenterPerspective = _pointObserver,
                FogEnable = _ifog.Enabled,
                Fog = _ifog,
                lightSources = new List<ILightSource> { _lightSource },
                pointObserver = _pointObserver,
                renderFillTriangle = renderFillTriangle
            };

            // отрисовка модели
            RenderModel(g1, _irenderPipeline.RenderPipeline(_model, irpp1), renderFillTriangle, renderType);

            // восстановили сохраненное состояние
            _model.PopState();

            // обновляем изображение модели
            pictureBox1.Image = _bitmap;

            // окно с дополнительной камерой
            Graphics g2 = Graphics.FromImage(_bitmapLookAt);

            // отрисовка фона
            g2.Clear(Color.White);

            bool bLookAt = checkBoxCamera.Checked;

            if (bLookAt)
            {
                // запомнили состояние модели
                _model.PushState();

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

                // применяем преобразование
                _model.Transform(matrixView);

                // отрисовка модели
                IPoint3D observerLookAt = ResolvePoint3D(pictureBox2.Width / 2, pictureBox2.Height / 2, _pointObserver.Z);

                IRenderPipelineParameters irpp2 = new RenderPipelineParameters
                {
                    TranslateX = pictureBox2.Width / 2,
                    TranslateY = pictureBox2.Height / 2,
                    TranslateZ = 0,
                    renderModelType = renderType,
                    PerspectiveEnable = false,
                    FogEnable = false,
                    lightSources = new List<ILightSource> { _lightSource },
                    pointObserver = observerLookAt,
                    renderFillTriangle = renderFillTriangle
                };

                // отрисовка модели - вид с дополнительной камерой
                RenderModel(g2, _irenderPipeline.RenderPipeline(_model, irpp2), renderFillTriangle, renderType);

                // восстановили сохраненное состояние
                _model.PopState();
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

        RenderFillTriangle GetRenderFillTriangle(int index)
        {
            RenderFillTriangle renderFillTriangle;

            if (index == 0)
            {
                renderFillTriangle = RenderFillTriangle.Flat0;
            }
            else if (index == 1)
            {
                renderFillTriangle = RenderFillTriangle.Flat3;
            }
            else
            {
                renderFillTriangle = RenderFillTriangle.Gouraud;
            }

            return renderFillTriangle;
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
            cmbModel.Items.Add("Прозрачный столик");
            cmbModel.Items.Add("Октаэдр");
            cmbModel.Items.Add("Стрелка");
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
            cmbRenderStatus.Items.Add("Каркас");
            cmbRenderStatus.Items.Add("Полное");
            cmbRenderStatus.Items.Add("Каркас. Без цвета.");
            cmbRenderStatus.SelectedIndex = 1;
            cmbRenderStatus.EndUpdate();

            // треугольники
            cmbTriRender.BeginUpdate();
            cmbTriRender.Items.Add("Плоское по одной точке");
            cmbTriRender.Items.Add("Плоское по трем точкам");
            cmbTriRender.Items.Add("Метод Гуро");
            cmbTriRender.SelectedIndex = 0;
            cmbTriRender.EndUpdate();

            // камера
            cmbLookAt.BeginUpdate();
            cmbLookAt.Items.Add(new Camera { VectorLookAt = new Vector3(-1, 0, 0), Description = "Справа" });
            cmbLookAt.Items.Add(new Camera { VectorLookAt = new Vector3(1, 0, 0), Description = "Слева" });
            cmbLookAt.SelectedIndex = 0;
            cmbLookAt.EndUpdate();

            _blockEvents = false;

            _model = ModelFactory.GetModel(1, ModelQuality.Middle);

            _ifog.Enabled = false;

            _lightSource = new PointLightSource()
            {
                LightPoint = ResolvePoint3D(0, 0, -500), // расположение источника света
                Weight = 0.9f
            };

            _pointObserver = ResolvePoint3D(pictureBox1.Width / 2, pictureBox1.Height / 2, -1400);
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

        private void cmbModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            _transformMatrix = Matrix4x4.Identity;
            _ifog.Enabled = checkBoxFog.Checked;

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
                    LightPoint = ResolvePoint3D(0, 0, -500)
                };
            }

            Render();
        }

        private void cmbRenderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

            checkBoxFog.Enabled = cmbRenderStatus.SelectedIndex == 1;
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

        private void cmbTriRender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvents)
                return;

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

        private void btnRight_Click(object sender, EventArgs e)
        {
            float angle = Convert.ToSingle(-Math.PI / 2 / 15);
            Matrix4x4 matrixRotation = Matrix4x4.CreateRotationY(angle);

            _model.Transform(matrixRotation);
            _transformMatrix *= matrixRotation;

            Render();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            float angle = Convert.ToSingle(-Math.PI / 2 / 15);
            Matrix4x4 matrixRotation = Matrix4x4.CreateRotationX(angle);

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

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
