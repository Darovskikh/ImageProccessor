using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ImageProccessor
{
    class ProgramMain
    {
        private static Skin _skin;
        private static ImageProcessorClass _imageProc;
        static void Main(string[] args)
        {
            _skin = new ClassicSkin();
            _skin.Render("Выберете стиль текста");
            _skin.Render("1. Классический стиль");
            _skin.Render("2. Цветной стиль");
            _skin.Render("3. Рандомный цвет стиль");
            WriteLine();
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D1:
                {
                    _imageProc = new ImageProcessorClass(new ClassicSkin());
                    _skin = ImageProcessorClass.Skin;
                    break;
                }
                case ConsoleKey.NumPad1:
                {
                    _imageProc = new ImageProcessorClass(new ClassicSkin());
                    _skin = ImageProcessorClass.Skin;
                    break;
                }

                case ConsoleKey.D2:
                {
                    WriteLine();
                    _imageProc = new ImageProcessorClass(new ColorSkin(ChooseColor()));
                    _skin = ImageProcessorClass.Skin;
                    break;
                }
                case ConsoleKey.NumPad2:
                {
                    WriteLine();
                    _imageProc = new ImageProcessorClass(new ColorSkin(ChooseColor()));
                    _skin = ImageProcessorClass.Skin;
                    break;
                }
                case ConsoleKey.D3:
                {
                    _imageProc = new ImageProcessorClass(new RandomSkin());
                    _skin = ImageProcessorClass.Skin;
                    break;
                }
                case ConsoleKey.NumPad3:
                {
                    _imageProc = new ImageProcessorClass(new RandomSkin());
                    _skin = ImageProcessorClass.Skin;
                    break;
                }
            }
            _imageProc.ImageLoading += (sender, e) => { _skin.Render(e.Message);};
            _imageProc.LoadPhoto();
            _imageProc.RenamePhotoDate();
            _imageProc.AddDateOnPhoto();
            _imageProc.SortPhotoByYear();
            _imageProc.SortImageByLocation();
            Console.ReadLine();
        }

        private static string ChooseColor()
        {
            _skin.Render("Выберете цвет из списка");
            for (int i = 0; i < 15; i++)
            {
                _skin.Render($"{i + 1}. {Enum.Parse(typeof(ConsoleColor), i.ToString())}");
            }
            WriteLine();
            int number = int.Parse(ReadLine());
            number--;
            return number.ToString();
        }
    }
}
