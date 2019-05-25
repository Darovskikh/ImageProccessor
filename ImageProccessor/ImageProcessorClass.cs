using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ImageProccessor
{
    public class ImageProcessorClass
    {
        private string _path;
        public delegate void ShowMessageImageProcHandler(ImageProcessorClass sender, ImageProcEventArgs e);

        public event ShowMessageImageProcHandler ImageLoading;
        public List<Image> Images { get; set; } = new List<Image>();

        public static Skin Skin { get; set; }

        public ImageProcessorClass() { }

        public ImageProcessorClass(Skin skin)
        {
            Skin = skin;
        }

        public void LoadPhoto()
        {
            Console.WriteLine();
            ImageLoading?.Invoke(this, new ImageProcEventArgs() { Message = "Укажите путь к папке с изображениями" });
            List<FileInfo> fileInfos = new List<FileInfo>();
            Thread thread = new Thread(() =>
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        _path = fbd.SelectedPath;
                    }
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(_path);
                foreach (var file in directoryInfo.GetFiles("*.jpg"))
                {
                    Images.Add(Image.FromFile(file.FullName));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ImageLoading?.Invoke(this, new ImageProcEventArgs() { Message = e.Message });
            }

        }
        // Добавить пул потоков для ускорения
        public void RenamePhotoDate()
        {
            DirectoryInfo drInfo = new DirectoryInfo(_path);
            Directory.CreateDirectory(_path + $@"\{drInfo.Name}_RenamePhotoDate");
            string newPath = _path + $@"\{drInfo.Name}_RenamePhotoDate";
            foreach (var file in drInfo.GetFiles("*.jpg"))
            {
                string newpath1 = $@"\{file.LastWriteTime.ToShortDateString()}_{file.LastWriteTime.ToLongTimeString()}.jpg";
                newpath1 = newpath1.Replace(':', '.');
                string fullnewpath = newPath + newpath1;
                File.Copy($"{file.FullName}", fullnewpath, true);
            }
        }

        public void AddDateOnPhoto()
        {
            DirectoryInfo drInfo = new DirectoryInfo(_path);
            Directory.CreateDirectory(_path + $@"\{drInfo.Name}_RenamePhotoDate");
            string newPath = _path + $@"\{drInfo.Name}_RenamePhotoDate";
            int i = 0;
            foreach (var file in drInfo.GetFiles("*jpg"))
            {
                Image img = Images[i];
                using (Graphics graphics = Graphics.FromImage(img))
                {
                    graphics.DrawString($"{file.LastWriteTime.ToLongDateString()}_{file.LastWriteTime.ToLongTimeString()}",new Font("Verdana",(float)40),new SolidBrush(Color.White),img.HorizontalResolution+img.Width-1000,img.VerticalResolution+50);
                    img.Save($"{newPath}/{file.Name}");
                }

                i++;
            }
        }
    }
}
