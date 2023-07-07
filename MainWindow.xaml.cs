using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Timers;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing.Imaging;

namespace InspectorV1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VideoCapture capture;
        Timer timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            int cameraFps = 30;
            //init the camera
            capture = new VideoCapture();

            //set the captured frame width and height (default 640x480)
            //capture.Set(CapProp.FrameWidth, 1024);
            //capture.Set(CapProp.FrameHeight, 768);
            //var mat1 = capture.QueryFrame();

            //var bmp = mat1.ToImage().ToBitmap();
            //copy the bitmap to a memorystream
            //display the image on the ui
            // outputFrame.Source = BitmapFrame.Create(bmp);
            using (Image<Bgr, byte> frame = capture.QueryFrame().ToImage<Bgr, Byte>())
            {
                if (frame != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        // My way to display frame 
                        frame.Bitmap.Save(stream, ImageFormat.Bmp);

                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = new MemoryStream(stream.ToArray());
                        bitmap.EndInit();

                        outputFrame.Source = bitmap;
                    };
                }
            }
        }
    }
}
