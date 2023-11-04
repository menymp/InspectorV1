using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorV1
{
    class RoiSelect
    {
        /// <summary>
        /// Convert the coordinates for the image's SizeMode.
        /// </summary>
        /// http://csharphelper.com/blog/2014/10/select-parts-of-a-scaled-image-picturebox-different-sizemode-values-c/</a>
        /// http://csharphelper.com/blog/2014/10/select-parts-of-a-scaled-image-picturebox-different-sizemode-values-c/</a>
        /// <param name="pic"></param>
        /// <param name="X0">out X coordinate</param>
        /// <param name="Y0">out Y coordinate</param>
        /// <param name="x">atual coordinate</param>
        /// <param name="y">atual coordinate</param>
        /*
        public static void ConvertCoordinates(PictureBox pic,
            out int X0, out int Y0, int x, int y)
        {
            int pic_hgt = pic.ClientSize.Height;
            int pic_wid = pic.ClientSize.Width;
            int img_hgt = pic.Image.Height;
            int img_wid = pic.Image.Width;

            X0 = x;
            Y0 = y;
            switch (pic.SizeMode)
            {
                case PictureBoxSizeMode.AutoSize:
                case PictureBoxSizeMode.Normal:
                    // These are okay. Leave them alone.
                    break;
                case PictureBoxSizeMode.CenterImage:
                    X0 = x - (pic_wid - img_wid) / 2;
                    Y0 = y - (pic_hgt - img_hgt) / 2;
                    break;
                case PictureBoxSizeMode.StretchImage:
                    X0 = (int)(img_wid * x / (float)pic_wid);
                    Y0 = (int)(img_hgt * y / (float)pic_hgt);
                    break;
                case PictureBoxSizeMode.Zoom:
                    float pic_aspect = pic_wid / (float)pic_hgt;
                    float img_aspect = img_wid / (float)img_wid;
                    if (pic_aspect > img_aspect)
                    {
                        // The PictureBox is wider/shorter than the image.
                        Y0 = (int)(img_hgt * y / (float)pic_hgt);

                        // The image fills the height of the PictureBox.
                        // Get its width.
                        float scaled_width = img_wid * pic_hgt / img_hgt;
                        float dx = (pic_wid - scaled_width) / 2;
                        X0 = (int)((x - dx) * img_hgt / (float)pic_hgt);
                    }
                    else
                    {
                        // The PictureBox is taller/thinner than the image.
                        X0 = (int)(img_wid * x / (float)pic_wid);

                        // The image fills the height of the PictureBox.
                        // Get its height.
                        float scaled_height = img_hgt * pic_wid / img_wid;
                        float dy = (pic_hgt - scaled_height) / 2;
                        Y0 = (int)((y - dy) * img_wid / pic_wid);
                    }
                    break;
            }

                private Point RectStartPoint;
        private Rectangle Rect = new Rectangle();
        private Rectangle RealImageRect = new Rectangle();
        private Brush selectionBrush = new SolidBrush(Color.FromArgb(128, 64, 64, 64));
        private int thickness = 3;

        /// <summary>
        /// Start Rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Determine the initial rectangle coordinates...
            RectStartPoint = e.Location;
            Invalidate();
        }

        /// <summary>
        /// Draw Rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            #region SETS COORDINATES AT INPUT IMAGE BOX
            int X0, Y0;
            Utilities.ConvertCoordinates(imageBoxInput, out X0, out Y0, e.X, e.Y);
            labelPostionXY.Text = "Last Position: X:" + X0 + "  Y:" + Y0;

            //Coordinates at input picture box
            if (e.Button != MouseButtons.Left)
                return;
            Point tempEndPoint = e.Location;
            Rect.Location = new Point(
                Math.Min(RectStartPoint.X, tempEndPoint.X),
                Math.Min(RectStartPoint.Y, tempEndPoint.Y));
            Rect.Size = new Size(
                Math.Abs(RectStartPoint.X - tempEndPoint.X),
                Math.Abs(RectStartPoint.Y - tempEndPoint.Y));
            #endregion

            #region SETS COORDINATES AT REAL IMAGE
            //Coordinates at real image - Create ROI
            Utilities.ConvertCoordinates(imageBoxInput, out X0, out Y0, 
            RectStartPoint.X, RectStartPoint.Y);
            int X1, Y1;
            Utilities.ConvertCoordinates(imageBoxInput, out X1, out Y1, tempEndPoint.X, tempEndPoint.Y);
            RealImageRect.Location = new Point(
                Math.Min(X0, X1),
                Math.Min(Y0, Y1));
            RealImageRect.Size = new Size(
                Math.Abs(X0 - X1),
                Math.Abs(Y0 - Y1));

            imgEntrada = new Image<Bgr, byte>("lena.jpg");
            imgEntrada.Draw(RealImageRect, new Bgr(Color.Red), thickness);
            imageBoxOutputROI.Image = imgEntrada;
            #endregion

            ((PictureBox)sender).Invalidate();
        }

        /// <summary>
        /// Desenha retângulo
        /// </summary>
        /// http://stackoverflow.com/questions/11088154/graphics-fillrectangle-except-specified-area-net-gdi
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            // Draw the rectangle...
            if (imageBoxInput.Image != null)
            {
                if (Rect != null && Rect.Width > 0 && Rect.Height > 0)
                {
                    //Seleciona a ROI
                    e.Graphics.SetClip(Rect, System.Drawing.Drawing2D.CombineMode.Exclude);
                    e.Graphics.FillRectangle(selectionBrush, new Rectangle
            (0, 0, ((PictureBox)sender).Width, ((PictureBox)sender).Height));
                    //e.Graphics.FillRectangle(selectionBrush, Rect);
                }
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            //Define ROI. Valida altura e largura para evitar index range exception.
            if (RealImageRect.Width > 0 && RealImageRect.Height > 0)
            {
                imgEntrada.ROI = RealImageRect;
                imageBoxROI.Image = imgEntrada;
            }

        }

        private void pictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Rect = new Rectangle();
            ((PictureBox)sender).Invalidate();
        }
        */
    }
}
