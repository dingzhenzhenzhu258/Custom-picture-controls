using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace 自定义图片控件
{
    public partial class UserControl1 : UserControl
    {
        private Image originalImage; 
        private float scaleFactor = 1.0f; // 初始缩放因子
        private Point lastMousePos; // 记录最后一次鼠标位置
        private Point imageOffset; // 记录图像的偏移量
        private bool isDragging = false; // 标记是否在拖拽

        public UserControl1()
        {
            InitializeComponent();
            // 加载原始图像
            originalImage = Image.FromFile("C:\\Users\\29185\\Desktop\\img\\img\\template.bmp");
            this.pictureBox1.Image = originalImage;

            this.pictureBox1.MouseWheel += pictureBox1_MouseWheel;
            this.pictureBox1.MouseDown += pictureBox1_MouseDown;
            this.pictureBox1.MouseMove += pictureBox1_MouseMove;
            this.pictureBox1.MouseUp += pictureBox1_MouseUp;
            this.pictureBox1.Paint += pictureBox1_Paint;
        }

        // 处理 PictureBox 的 MouseWheel 事件
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                // 放大
                scaleFactor *= 1.1f;
            }
            else if (e.Delta < 0)
            {
                // 缩小
                scaleFactor /= 1.1f;
            }
            //触发重绘事件，更新缩放图像
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //当鼠标左键按下时，设置isDragging为true，并记录当前鼠标位置
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastMousePos = e.Location;
            }
        }

        // 在拖拽时计算鼠标位置的变化量并更新图像的偏移量，然后触发重绘事件。
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // 通过减去上一次记录的鼠标位置(lastMousePos)来计算鼠标位置的变化量(dx 和 dy)这实际上是将图像移动了鼠标移动的距离
                int dx = e.Location.X - lastMousePos.X;
                int dy = e.Location.Y - lastMousePos.Y;

                //更新 imageOffset。
                imageOffset.X += dx;
                imageOffset.Y += dy;

                //将 lastMousePos 更新为当前的鼠标位置，以便在下一次移动时计算新的增量
                lastMousePos = e.Location;

                //触发重绘事件，实时更新图像位置
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            // MouseUp事件处理程序，当鼠标左键松开时，设置isDragging为false，停止拖拽
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (originalImage == null)
                return;

            int newWidth = (int)(originalImage.Width * scaleFactor);
            int newHeight = (int)(originalImage.Height * scaleFactor);


            // 使用Graphics重新绘制图像

            //在PictureBox中显示包含偏移的图像
            e.Graphics.Clear(Color.White);
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //在指定的位置 (imageOffset.X, imageOffset.Y) 绘制图像 originalImage，并将其缩放到指定的宽度 (newWidth) 和高度 (newHeight)
            e.Graphics.DrawImage(originalImage, imageOffset.X, imageOffset.Y, newWidth, newHeight);
        }
    }
}
