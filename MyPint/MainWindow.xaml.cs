using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
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
using System.Xml.Linq;

namespace MyPint
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<MyImage> images =new ObservableCollection<MyImage>();
        public MyImage temp = new MyImage();
       
        string s = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"image\";
        public MainWindow()
        {
            InitializeComponent();          
            SetImages();
            MyList.ItemsSource = images;
            //MyGrid.DataContext = temp;
            //MyPanel.DataContext = temp;
        }
        public void SetImages()//Заполнение из папки
        {
            Random rand = new Random();
            string[] df = Directory.GetFiles(s);
            foreach (var item in df)
            {
                string url = String.Join(s,item);
               // Text1.Text = url;
                Image im = new Image();
                BitmapImage bm = new BitmapImage(new Uri(url));
                im.Source = bm;
                MyImage image = new MyImage();
                string name = System.IO.Path.GetFileName(item);
                int like = rand.Next(1000);
                var height =(int) bm.Height;
                var width = (int) bm.Width;
                images.Add(new MyImage { Name = name, MySize = height.ToString()+"X"+ width.ToString(), Like = like, Url = url });
            }
        }

        private void Add_Image_Click(object sender, RoutedEventArgs e)//Добавление картинки
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();//диалоговое окно
            dialog.FileName = "Image"; // Default file name
            dialog.DefaultExt = ".jpg"; // Default file extension
            dialog.Filter = "Image (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                FileInfo f = new FileInfo(dialog.FileName);
               string path = f.FullName;
                // Path = dialog.FileName;
                BitmapImage save = new BitmapImage(new Uri(path));
                JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();//сохранение картинки в папке программы
                jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(save));
                using (FileStream fileStream = new FileStream(@"image\" + System.IO.Path.GetFileName(path), FileMode.Create))//bin debug
                jpegBitmapEncoder.Save(fileStream);
                var height = (int)save.Height;
                var width = (int)save.Width;
                images.Add(new MyImage { Name = System.IO.Path.GetFileName(path), MySize = height.ToString()+"X"+ width.ToString(), Like = 0, Url = s+ System.IO.Path.GetFileName(path) });
            }
        }

        private void Click_Image(object sender, MouseButtonEventArgs e)
        {
            MyGrid.Visibility = Visibility.Visible;
            var item = (sender as FrameworkElement).DataContext;
            int ind = MyList.Items.IndexOf(item);
            temp = new MyImage {Name= (item as MyImage).Name, Like=(item as MyImage).Like, MySize=(item as MyImage).MySize, Url=(item as MyImage).Url };
            //temp = item as MyImage;
           // Text1.Text = temp.ToString();
            MyPanel.DataContext = temp;
        }

        private void Close_click(object sender, RoutedEventArgs e)
        {
            temp = new MyImage();
            MyGrid.Visibility = Visibility.Hidden;
        }
    }
}
