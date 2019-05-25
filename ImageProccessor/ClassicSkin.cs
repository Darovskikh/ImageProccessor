using System;

namespace ImageProccessor
{
    class ClassicSkin : Skin
    {
        public override void Clear()
        {
            Console.Clear();
        }

        public override void Render(string text)
        {
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public override void Render(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }
    }
}
