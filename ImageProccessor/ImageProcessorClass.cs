using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using MetadataExtractor;
using Yandex;
using MetadataExtractor.IO;
using MetadataExtractor.Formats;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Util;
using Directory = System.IO.Directory;

namespace ImageProccessor
{
    public class ImageProcessorClass
    {
        public List<FileInfo> FileInfos { get; set; } = new List<FileInfo>();
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
                    FileInfos.Add(file);
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
            foreach (var file in FileInfos)
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
            string newPath = _path + $@"\{drInfo.Name}_AddDateOnPhoto";
            Directory.CreateDirectory(newPath);
            int i = 0;
            foreach (var file in FileInfos)
            {
                Image img = Images[i];
                using (Graphics graphics = Graphics.FromImage(img))
                {
                    graphics.DrawString($"{file.LastWriteTime.ToLongDateString()}_{file.LastWriteTime.ToLongTimeString()}", new Font("Verdana", (float)40), new SolidBrush(Color.White), img.HorizontalResolution + img.Width - 1000, img.VerticalResolution + 50);
                    img.Save($"{newPath}/{file.Name}");
                }
                i++;
            }
        }

        public void SortPhotoByYear()
        {
            DirectoryInfo drInfo = new DirectoryInfo(_path);
            string newPath = _path + $@"\{drInfo.Name}_SortPhotoByYear";
            Directory.CreateDirectory(newPath);
            Dictionary<FileInfo, int> files = new Dictionary<FileInfo, int>();
            foreach (var file in FileInfos)
            {
                int creationDate = file.LastWriteTime.Year;
                files.Add(file, creationDate);
            }
            foreach (var file in files)
            {
                string fullNewPath = $"{newPath}" + $"\\{file.Value}" + $"\\{file.Key.Name}";
                Directory.CreateDirectory($"{newPath}" + $"\\{file.Value}");
                File.Copy($"{file.Key.FullName}", fullNewPath, true);
            }

        }

        public void SortImageByLocation()
        {
            DirectoryInfo drInfo = new DirectoryInfo(_path);
            string newPath = _path + $@"\{drInfo.Name}_SortImageByLocation";
            Directory.CreateDirectory(newPath);
            Dictionary<FileInfo,GeoLocation > filesDictionary = new Dictionary<FileInfo, GeoLocation>();
            Dictionary<FileInfo,string> filesLocationDictionary = new Dictionary<FileInfo, string>();
            foreach (var file in FileInfos)
            {
                var gps = ImageMetadataReader.ReadMetadata(file.FullName).OfType<GpsDirectory>().FirstOrDefault();
                var location = gps?.GetGeoLocation();
                if(location!=null)
                    filesDictionary.Add(file,location);
            }

            foreach (var file in filesDictionary) 
            {
                string responceFromServer = null;
                string placeAdress = null;
                string adress = $"https://geocode-maps.yandex.ru/1.x/?apikey=06513974-5a2a-40aa-8d37-3c1e22e9f3ac&geocode={file.Value.Longitude.ToString()},{file.Value.Latitude.ToString()}";
                WebRequest wtRequest = WebRequest.Create(adress);
                using (HttpWebResponse response = (HttpWebResponse) wtRequest.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(dataStream))
                        {
                            responceFromServer = streamReader.ReadToEnd();
                        }
                    }
                }
                try
                {
                    using (TextReader reader = new StringReader(responceFromServer))
                    {
                        using (XmlReader xmlReader = XmlReader.Create(reader))
                        {
                            xmlReader.MoveToContent();
                            while (xmlReader.Read())
                            {
                                if (xmlReader.Name == "formatted")
                                {
                                    placeAdress = xmlReader.ReadString();
                                    break;
                                }
                            }
                        }
                    }
                    filesLocationDictionary.Add(file.Key, placeAdress);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            foreach (var file in filesLocationDictionary)
            {
                string fullNewPath = $"{newPath}" + $"\\{file.Value}" + $"\\{file.Key.Name}";
                Directory.CreateDirectory($"{newPath}" + $"\\{file.Value}");
                File.Copy($"{file.Key.FullName}", fullNewPath, true);
            }

        }
    }
}
