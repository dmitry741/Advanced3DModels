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

namespace WinPixelModels
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region === members ===

        Bitmap _bitmap = null;
        Bitmap _bitmapPixelModel = null;
        IPixelsModel _ipixelsModel;
        bool _blockEvent = false;
        IEnumerable<ILightSource> _lightSources = null;
        IPoint3D _pointObserverView = null;

        #endregion

        #region === private methods ===

        Bitmap GetModelBitmap(IPixelsModel model, IEnumerable<ILightSource> lightSources, IPoint3D pointObserver, Color backColor)
        {
            if (_bitmapPixelModel != null)
                return _bitmapPixelModel;
            
            _bitmapPixelModel = new Bitmap(Convert.ToInt32(model.BoundRect.Width), Convert.ToInt32(model.BoundRect.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            Rectangle r = new Rectangle(0, 0, _bitmapPixelModel.Width, _bitmapPixelModel.Height);
            System.Drawing.Imaging.BitmapData bitmapData = _bitmapPixelModel.LockBits(r, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int stride = Math.Abs(bitmapData.Stride);
            int bytes = stride * bitmapData.Height;
            byte[] rgbValues = new byte[bytes];

            LightModelParameters lightModelParameters = new LightModelParameters
            {
                LightSources = lightSources,
                PointObserver = pointObserver,
                ReflectionEnable = true,
                ReflectionBrightness = 80.0f,
                ReflcetionCone = 1000f
            };

            RectangleF br = model.BoundRect;
            Color colorForRender;
            IPoint3D point = ResolvePoint3D(0, 0, 0);

            for (int x = 0; x < _bitmapPixelModel.Width; x++)
            {
                for (int y = 0; y < _bitmapPixelModel.Height; y++)
                {
                    if (model.Contains(x + br.X, y + br.Y))
                    {
                        point.X = x + br.X;
                        point.Y = y + br.Y;
                        point.Z = model.GetZ(point.X, point.Y);

                        lightModelParameters.Normal = model.GetNormal(point.X, point.Y);
                        lightModelParameters.Point = point;
                        lightModelParameters.BaseColor = model.GetColor(point.X, point.Y);
                        colorForRender = LightModel.GetColor(lightModelParameters);
                    }
                    else
                    {
                        colorForRender = backColor;
                    }

                    int index = y * stride + x * 3;
                    rgbValues[index + 0] = colorForRender.B;
                    rgbValues[index + 1] = colorForRender.G;
                    rgbValues[index + 2] = colorForRender.R;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, bitmapData.Scan0, bytes);
            _bitmapPixelModel.UnlockBits(bitmapData);

            return _bitmapPixelModel;
        }

        IEnumerable<ILightSource> GetLightSources()
        {
            List<ILightSource> lightSources = new List<ILightSource>();

            if (checkBox1.Checked) 
                lightSources.Add(new PointLightSource() { LightPoint = ResolvePoint3D(-100, -100, -500), Weight = 0.7f });

            if (checkBox2.Checked)
                lightSources.Add(new PointLightSource() { LightPoint = ResolvePoint3D(100, 300, -600), Weight = 0.7f });

            foreach(ILightSource ls in lightSources)
            {
                ls.Weight = 1.0f / lightSources.Count();
            }

            return lightSources;
        }

        void Render()
        {
            if (_bitmap == null || _ipixelsModel == null)
                return;

            // создание контекста для рисования
            Graphics g = Graphics.FromImage(_bitmap);

            // отрисовка фона
            g.Clear(Color.White);

            // отрисаовка картинки с моделью
            Bitmap bitmapModel = GetModelBitmap(_ipixelsModel, _lightSources, _pointObserverView, Color.White);
            g.DrawImage(bitmapModel, (pictureBox1.Width - bitmapModel.Width) / 2, (pictureBox1.Height - bitmapModel.Height) / 2);

            pictureBox1.Image = _bitmap;
        }

        IPoint3D ResolvePoint3D(float X, float Y, float Z)
        {
            return new Point3D(X, Y, Z);
        }

        IPixelsModel GetModel(int index)
        {
            IPixelsModel pixelsModel;

            if (index == 0)
            {
                IPoint3D point3D = ResolvePoint3D(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
                float radius = Math.Min(pictureBox1.Width / 2, pictureBox1.Height / 2) - 100;
                pixelsModel = new SpherePixelModel(point3D, radius, Color.Gray);
            }
            else
            {
                AbstractConvexPixelModel acpm = new OctahedronPixelModels(300);

                float angleX = Convert.ToSingle(2.25 * Math.PI / 6);
                float angleY = Convert.ToSingle(3.6 * Math.PI / 4);
                Matrix4x4 rotationX = Matrix4x4.CreateRotationX(angleX);
                Matrix4x4 rotationY = Matrix4x4.CreateRotationY(angleY);
                Matrix4x4 rot = rotationX * rotationY;

                acpm.Transform(rot);

                Matrix4x4 trans = Matrix4x4.CreateTranslation(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);

                acpm.Transform(trans);

                pixelsModel = acpm;
            }

            return pixelsModel;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;

            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            _ipixelsModel = GetModel(0);
            _lightSources = GetLightSources();
            _pointObserverView = ResolvePoint3D(pictureBox1.Width / 2, pictureBox1.Height / 2, -1400);

            _blockEvent = true;

            cmbModels.BeginUpdate();
            cmbModels.Items.Add("Сфера");
            cmbModels.Items.Add("Октаэдр");
            cmbModels.SelectedIndex = 0;
            cmbModels.EndUpdate();

            _blockEvent = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void cmbModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_blockEvent)
                return;

            _ipixelsModel = GetModel(cmbModels.SelectedIndex);
            _bitmapPixelModel = null;
            Render();
        }

        private void CheckBoxLightSourceChecked(object sender, EventArgs e)
        {
            _lightSources = GetLightSources();
            _bitmapPixelModel = null;
            Render();
        }
    }
}
