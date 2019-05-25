using System;

namespace ImageProccessor
{
    class RandomSkin : ColorSkin
    {
        private Random _random;
        private int _rndColor = 0;

        public RandomSkin()
        {
            GetColor();
        }

        protected void GetColor()
        {
            // If для того чтобы каждую строчку был новый цвет, т.к. строки появляеются меньше чем за секунду и не генерируется новое рандомное число
            if (_rndColor != 0 && _rndColor <15)
            {
                _rndColor++;
                string clrName = Enum.GetName(typeof(ConsoleColor), _rndColor);
                base.GetColor(clrName);
            }
            else if (_rndColor != 0 && _rndColor > 15)
            {
                _rndColor--;
                string clrName = Enum.GetName(typeof(ConsoleColor), _rndColor);
                base.GetColor(clrName);
            }
            else
            {
                _random = new Random();
                _rndColor = _random.Next(0, 16);
                string clrName = Enum.GetName(typeof(ConsoleColor), _rndColor);
                base.GetColor(clrName);
            }
        }

        public override void Render(string text)
        {
            base.Render(text);
            GetColor();
        }

    }
}
