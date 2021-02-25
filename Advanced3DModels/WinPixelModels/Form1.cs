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

        Bitmap GetModelBitmap(IPixelsModel model, IEnumerable<ILightSource> lightSources, IPoint3D observerView, Color backColor)
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

            int stride = bitmapData.Stride;
            int bytes = Math.Abs(stride) * bitmapData.Height;
            byte[] rgbValues = new byte[bytes];

            // TODO:


            for (int x = Convert.ToInt32(br.X); x < br.X + br.Width; x++)
            {
                for (int y = Convert.ToInt32(br.Y); y < br.Y + br.Height; y++)
                {
                    if (model.Contains(x, y))
                    {

                    }
                    else
                    {

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
                new PointLightSource() { LightPoint = ResolvePoint3D(0, 0, -500), Weight = 0.5f },
                new PointLightSource() { LightPoint = ResolvePoint3D(0, 200, -600), Weight = 0.5f }
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

            // TODO:

            pictureBox1.Image = _bitmap;
        }

        IPoint3D ResolvePoint3D(float X, float Y, float Z)
        {
            return new Point3D(X, Y, Z);
        }

        IPixelsModel GetModel(int index)
        {
            IPoint3D point3D = ResolvePoint3D(pictureBox1.Width / 2, pictureBox1.Height / 2, 0);
            float radius = Math.Min(pictureBox1.Width / 2, pictureBox1.Height / 2) - 80;

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
