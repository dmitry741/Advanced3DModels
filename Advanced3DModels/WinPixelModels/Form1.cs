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

namespace WinPixelModels
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region === members ===

        Bitmap _bitmap;
        Bitmap _bitmapPixelModel;
        IPixelsModel _ipixelsModel;
        bool _blockEvent = false;
        IEnumerable<ILightSource> _lightSources = null;
        IPoint3D _pointObserverView = null;

        #endregion

        #region === private methods ===

        Bitmap GetModelBitmap(IPixelsModel model, IEnumerable<ILightSource> lightSources, IPoint3D pointObserver, Color backColor)
        {
            RectangleF br = model.BoundRect;

            if (_bitmapPixelModel == null || 
                Convert.ToInt32(br.Width) != _bitmapPixelModel.Width ||
                Convert.ToInt32(br.Height) != _bitmapPixelModel.Height)
            {
                _bitmapPixelModel = new Bitmap(Convert.ToInt32(model.BoundRect.Width), Convert.ToInt32(model.BoundRect.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }

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
                ReflcetionCone = 1200f
            };

            for (int x = 0; x < _bitmapPixelModel.Width; x++)
            {
                for (int y = 0; y < _bitmapPixelModel.Height; y++)
                {
                    int index = y * stride + x * 3;
                    float xm = x + br.X;
                    float ym = y + br.Y;

                    if (model.Contains(xm, ym))
                    {
                        lightModelParameters.Normal = _ipixelsModel.GetNormal(x + br.X, y + br.Y);
                        lightModelParameters.Point = ResolvePoint3D(xm, ym, _ipixelsModel.GetZ(xm, ym));
                        lightModelParameters.BaseColor = _ipixelsModel.GetColor(xm, ym);
                        Color color = LightModel.GetColor(lightModelParameters);

                        rgbValues[index + 0] = color.B;
                        rgbValues[index + 1] = color.G;
                        rgbValues[index + 2] = color.R;
                    }
                    else
                    {
                        rgbValues[index + 0] = backColor.B;
                        rgbValues[index + 1] = backColor.G;
                        rgbValues[index + 2] = backColor.R;
                    }
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, bitmapData.Scan0, bytes);
            _bitmapPixelModel.UnlockBits(bitmapData);

            return _bitmapPixelModel;
        }

        IEnumerable<ILightSource> GetLightSources()
        {
            return new List<ILightSource>
            {
                new PointLightSource() { LightPoint = ResolvePoint3D(-100, -100, -500), Weight = 0.5f },
                new PointLightSource() { LightPoint = ResolvePoint3D(100, 300, -600), Weight = 0.5f }
            };
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
            IPoint3D point3D = ResolvePoint3D(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
            float radius = Math.Min(pictureBox1.Width / 2, pictureBox1.Height / 2) - 120;

            return new Sphere(point3D, radius, Color.DarkGray);
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;

            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            _ipixelsModel = GetModel(0);
            _lightSources = GetLightSources();
            _pointObserverView = ResolvePoint3D(pictureBox1.Width / 2, pictureBox1.Height / 2, -1400);

            // TODO:
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
            Render();
        }
    }
}
