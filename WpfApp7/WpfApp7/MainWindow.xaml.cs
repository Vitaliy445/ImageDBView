using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace WpfApp7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr value);
        public MainWindow()
        {
            InitializeComponent();
            Database.createTable();
            updateData();
        }
        void updateData()
        {
            imgList.Items.Clear();
            Database.getImagesMinData().ForEach(imgData => imgList.Items.Add(imgData));
        }

        private void addImg_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Image file(.jpg)|*.jpg";
            fileDialog.Multiselect = true;
            if ((bool)fileDialog.ShowDialog())
            {
                fileDialog.FileNames.ToList().ForEach(
                    filePath =>
                    {
                        using (FileStream fileStream = File.OpenRead(filePath))
                        {
                            byte[] imgBytes = new byte[fileStream.Length];
                            fileStream.Read(imgBytes, 0, (int)fileStream.Length);
                            Database.AddImage(imgBytes, filePath);
                        }
                    });
            }
        }

        private void delImg_Click(object sender, RoutedEventArgs e)
        {

            Database.DeleteImageById((imgList.SelectedItem as Models.ImageMinData).ID);
            updateData();
        }
        private void updateList_Click(object sender, RoutedEventArgs e)
        {
            updateData();
        }
        public static BitmapSource GetImageStream(System.Drawing.Image myImage)
        {
            var bitmap = new System.Drawing.Bitmap(myImage);
            IntPtr bmpPt = bitmap.GetHbitmap();
            BitmapSource bitmapSource =
             System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                   bmpPt,
                   IntPtr.Zero,
                   Int32Rect.Empty,
                   BitmapSizeOptions.FromEmptyOptions());

            bitmapSource.Freeze();
            DeleteObject(bmpPt);

            return bitmapSource;
        }
        private void imgList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (imgList.SelectedItem != null)
                viewIMG.Source = GetImageStream(Database.getImageByID((imgList.SelectedItem as Models.ImageMinData).ID));
            else
                viewIMG.Source = null;
        }
    }
}
