using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        #endregion

        #region === private methods ===

        void RenderSphere(Graphics g, int Xc, int Yc, int radius)
        {
            // TODO:
        }

        void Render()
        {
            if (_bitmap == null)
                return;

            Graphics g = Graphics.FromImage(_bitmap);
            g.Clear(Color.White);

            RenderSphere(g, pictureBox1.Width / 2, pictureBox1.Height / 2, pictureBox1.Width / 2 - pictureBox1.Width / 10);

            pictureBox1.Image = _bitmap;
        }

        bool CreateBackground()
        {
            if (pictureBox1.Width < 1 || pictureBox1.Height < 1)
            {
                _bitmap = null;
                return false;
            }

            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            return true;
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.White;
            CreateBackground();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
