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

namespace 图片控件
{
    public partial class UserControl1 : UserControl
    {
        Image image;
        double Seale = 1.0; // 初始缩放因子
        Point point1 = new Point(); // 开始坐标
        Point point2 = new Point(); // 移动后的坐标
        bool isClick = false;
        Bitmap bitmap = null;

        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = Image.FromFile("C:\\Users\\29185\\Desktop\\img\\img\\template.bmp");
            image = this.pictureBox1.Image;
            bitmap = new Bitmap(image);
            this.pictureBox1.MouseWheel += PictureBox1_MouseWheel;
            this.pictureBox1.Paint += pictureBox1_Paint;
            this.pictureBox1.MouseDown += PictureBox1_MouseDown;
            this.pictureBox1.MouseUp += PictureBox1_MouseUp;
            this.pictureBox1.MouseMove += PictureBox1_MouseMove;
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isClick)
            {
                // 通过减去上一次记录的鼠标位置(point1)来计算鼠标位置的变化量(dx 和 dy)这实际上是将图像移动了鼠标移动的距离
                int dx = e.Location.X - point1.X;
                int dy = e.Location.Y - point1.Y;

                //更新 point2
                point2.X += dx;
                point2.Y += dy;

                //将 lastMousePos 更新为当前的鼠标位置，以便在下一次移动时计算新的增量
                point1 = e.Location;

                pictureBox1.Invalidate();
            }
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isClick = false;
            }
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //当鼠标左键按下时，设置isClick为true，并记录当前鼠标位置
            if (e.Button == MouseButtons.Left)
            {
                isClick = true;
                point1 = e.Location;
            }
        }

        private void PictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                Seale *= 1.1;
                // 放大
                pictureBox1.Invalidate();
            }
            if (e.Delta < 0)
            {
                // 缩小
                Seale /= 1.1;
                pictureBox1.Invalidate();
            }


            //下面更加卡
            //原因：每次缩放都会重新创建 Bitmap 对象，这会导致较大的内存开销和性能消耗，因为图像重新创建和内存分配是耗时的操作，新的代码则没有创建新的图像对象，而是直接在 Paint 事件中进行绘制，避免了不必要的内存分配，性能较高。

            /* 
             *  使用 Graphics 对图像进行处理
                当我们说使用 Graphics 对图像进行处理时，实际上是指对图像的显示进行处理，而不是直接修改原始图像对象。具体来说：
                原始图像 (image)：这是从文件加载的图像，保持不变。
                处理后的图像 (bitmap)：这是一个临时的图像对象，用于存储处理后的结果。

                处理过程
                原始图像 (image)：始终保持不变，作为绘制的基础。
                处理后的图像 (bitmap)：每次缩放操作时，创建一个新的 Bitmap 对象，并使用 Graphics 对象将原始图像绘制到新的 Bitmap 上。

                为什么需要两个图像对象
                原始图像 (image)：提供高质量的源图像，确保每次缩放操作都基于原始图像进行，避免质量损失。
                处理后的图像 (bitmap)：用于显示在 PictureBox 中，反映当前的缩放状态。
             
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //在指定的位置 (imageOffset.X, imageOffset.Y) 绘制图像 originalImage，并将其缩放到指定的宽度 (newWidth) 和高度 (newHeight)
            graphics.DrawImage(image, 0, 0, image.Width * (float)Seale, image.Height * (float)Seale);
            this.pictureBox1.Image = bitmap;
            */
        }


        
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //在指定的位置 (imageOffset.X, imageOffset.Y) 绘制图像 originalImage，并将其缩放到指定的宽度 (newWidth) 和高度 (newHeight)
            e.Graphics.DrawImage(image, point2.X, point2.Y, image.Width * (float)Seale, image.Height * (float)Seale);
        }

        private void 还原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 重置缩放因子
            Seale = 1.0;

            // 重置移动坐标
            point2 = new Point(0, 0);

            // 重绘 PictureBox
            pictureBox1.Invalidate();


            // 重新设置图像(不行)
            /*
              * 直接设置 this.pictureBox1.Image 不会使图像框还原到原始状态：
                1.自定义绘制逻辑：
                你在代码中重写了 PictureBox 的默认绘制行为。通常，PictureBox 会直接显示其 Image 属性中的图像。但是，你添加了一个自定义的 Paint 事件处理程序（pictureBox1_Paint），这改变了 PictureBox 的绘制方式。

                2.Paint 事件的优先级：
                当你为控件添加 Paint 事件处理程序时，它会覆盖控件的默认绘制行为。这意味着无论 Image 属性是什么，控件都会使用你的自定义绘制逻辑。

                3.自定义绘制中的变量：
                在你的 pictureBox1_Paint 方法中，你使用了 Seale（缩放因子）和 point2（平移坐标）来控制图像的显示。这些变量不会因为更改 Image 属性而自动重置。

                4.绘制过程：
                你的 pictureBox1_Paint 方法中的绘制代码如下：
                e.Graphics.DrawImage(image, point2.X, point2.Y, image.Width * (float)Seale, image.Height * (float)Seale);

                这段代码会根据 point2 和 Seale 的当前值来绘制图像，无论 image 是什么。
                因此，即使你更新了 pictureBox1.Image，由于自定义的 Paint 事件处理程序仍在使用未重置的 Seale 和 point2 值，图像的显示不会改变。
              */
            // this.pictureBox1.Image = Image.FromFile("C:\\Users\\29185\\Desktop\\img\\img\\template.bmp");

        }
    }
}
