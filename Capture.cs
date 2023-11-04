using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing.Imaging;

namespace InspectorV1
{
    class Capture
    {
        VideoCapture capture;
        public Capture(int index = 0)
        {
            this.capture = new VideoCapture(index);
        }
        public BitmapImage read()
        {
            BitmapImage bitmap = new BitmapImage();
            bool success = false;
            using (Image<Bgr, byte> frame = capture.QueryFrame().ToImage<Bgr, Byte>())
            {
                if (frame != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        // My way to display frame 
                        frame.Bitmap.Save(stream, ImageFormat.Bmp);

                        bitmap.BeginInit();
                        bitmap.StreamSource = new MemoryStream(stream.ToArray());
                        bitmap.EndInit();
                        success = true;
                    }
                }
            }
            return success ? bitmap : null;
        }

        public void Dispose()
        {
            capture.Dispose();
        }
        ~Capture()
        {
            capture.Dispose();
        }

    }
}
