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
using System.Threading; // For Dispatcher.

namespace InspectorV1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VideoCapture capture;
        System.Timers.Timer timer;
        bool flagStop = true;

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
            //create a timer that refreshes the webcam feed
            flagStop = false;
            timer = new System.Timers.Timer()
            {
                Interval = 1000 / cameraFps,
                Enabled = true
            };
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);
        }
        private SynchronizationContext _context = SynchronizationContext.Current;

        private UMat cannyExample(Image<Gray, byte> input)
        {
            var cannyImage = new UMat();
            CvInvoke.Canny(input, cannyImage, 150, 50);
            return cannyImage;
        }

        private UMat hsvTresholdExample(Image<Hsv, byte> hsv)
        {
            // 2. Obtain the 3 channels (hue, saturation and value) that compose the HSV image
            return hsv.ToUMat();
        }

        private async void timer_Tick(object sender, ElapsedEventArgs e)
        {

            if (flagStop == true)
            {
                capture.Dispose();
            }
            else
            {
                using (Image<Bgr, byte> frame = capture.QueryFrame().ToImage<Bgr, Byte>())
                {
                    if (frame != null)
                    {
                        this._context.Send(o =>
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
                        },
                        null);
                    }
                }

                using (Image<Gray, byte> frame = capture.QueryFrame().ToImage<Bgr, Byte>().Convert<Gray, byte>())
                {
                    if (frame != null)
                    {
                        this._context.Send(o =>
                        {
                            using (var stream = new MemoryStream())
                            {
                                // My way to display frame 
                                var outFrame = cannyExample(frame);
                                outFrame.Bitmap.Save(stream, ImageFormat.Bmp);

                                BitmapImage bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.StreamSource = new MemoryStream(stream.ToArray());
                                bitmap.EndInit();

                                processedOutput.Source = bitmap;
                            };
                        },
                        null);
                    }
                }
                using (Image<Hsv, byte> hsv = capture.QueryFrame().ToImage<Bgr, Byte>().Convert<Hsv, byte>())
                {
                    if (hsv != null)
                    {
                        this._context.Send(o =>
                        {
                            using (var stream = new MemoryStream())
                            {
                                // My way to display frame 
                                //var outFrame = cannyExample(hsv);
                                hsv.Bitmap.Save(stream, ImageFormat.Bmp);

                                BitmapImage bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.StreamSource = new MemoryStream(stream.ToArray());
                                bitmap.EndInit();

                                processedOutput2.Source = bitmap;
                            };
                        },
                        null);
                    }
                }
            }
        }
        //using (var stream = new MemoryStream())
        //{
        //    // My way to display frame 

        //    var CannyFrame = cannyExample(capture.QueryFrame());

        //    CannyFrame.Bitmap.Save(stream, ImageFormat.Bmp);

        //    BitmapImage bitmap = new BitmapImage();
        //    bitmap.BeginInit();
        //    bitmap.StreamSource = new MemoryStream(stream.ToArray());
        //    bitmap.EndInit();

        //    processedOutput.Source = bitmap;

        //};
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            timer.Enabled = false;
            
        }
    }
}
