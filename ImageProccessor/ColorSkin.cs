using System;

namespace ImageProccessor
{
    class ColorSkin : Skin
    {
        public string Color { get; private set; }
        private ConsoleColor _color;
        public ColorSkin()
        {

        }
        public ColorSkin(string color )
        {
            //string fl = Convert.ToString(color[0]);
            //fl = fl.ToUpper();
            //Color = color.Replace(color[0], Convert.ToChar(fl));
            GetColor(color);
        }
        public override void Clear()
        {
            Console.Clear();
        }

        public override void Render(string text)
        {
            Console.ForegroundColor = _color;
            Console.WriteLine(text);
            Console.ResetColor();

        }
        public override void Render(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();

        }

        protected void GetColor(string clr)
        {
            _color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), clr);
            if (_color == ConsoleColor.Black)
            {
                _color = ConsoleColor.Blue;
            }
            else if (_color == ConsoleColor.DarkBlue)
            {
                _color = ConsoleColor.Blue;
            }
        }
    }
}
