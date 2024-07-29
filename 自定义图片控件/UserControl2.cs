using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 自定义图片控件
{
    public partial class UserControl2 : UserControl
    {
        private Image Image = null;
        private float scaleFactor = 1.0f; //每一次缩放的大小

        public UserControl2()
        {
            InitializeComponent();
            Image = Image.FromFile("C:\\Users\\29185\\Desktop\\img\\img\\template.bmp");
            this.pictureBox1.Image = Image;
            this.pictureBox1.MouseWheel += PictureBox1_MouseWheel;
            this.pictureBox1.Paint += pictureBox1_Paint;
        }

        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                // 放大
                scaleFactor *= 1.1f;
                //触发重绘事件，实时更新图像位置
                pictureBox1.Invalidate();

            }
            else if (e.Delta < 0)
            {
                // 缩小
                scaleFactor /= 1.1f;
                //触发重绘事件，实时更新图像位置
                pictureBox1.Invalidate();

            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (Image == null)
                return;

            int newWidth = (int)(Image.Width * scaleFactor);
            int newHeight = (int)(Image.Height * scaleFactor);

            //在PictureBox中显示包含偏移的图像
            e.Graphics.Clear(Color.White);
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //在指定的位置 (imageOffset.X, imageOffset.Y) 绘制图像 Image，并将其缩放到指定的宽度 (newWidth) 和高度 (newHeight)
            e.Graphics.DrawImage(Image, 0, 0, newWidth, newHeight);
        }
    }
}
