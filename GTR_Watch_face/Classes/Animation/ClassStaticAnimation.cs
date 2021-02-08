using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazFit_Watchface_2
{
    public class ClassStaticAnimation
    {
        List<Bitmap> _images = new List<Bitmap>(); // Набор кадров для анимации
        int _x; // Координата X
        int _y; // Координата Y
        int _speedAnimation; // Скорость анимации
        int _timeAnimation; // Время анимации
        int _pause; // Пауза между циклами анимации
        int _cyclesTime; // длительность одного цикла
        int _time; // прошедшее время

        /// <summary>Класс для показа анимации из набора кадров</summary>
        /// <param name="Images">Набор кадров для анимации</param>
        /// <param name="X">Координата X</param>
        /// <param name="Y">Координата Y</param>
        /// <param name="SpeedAnimation">Скорость анимации</param>
        /// <param name="TimeAnimation">Время анимации</param>
        /// <param name="Pause">Пауза между циклами анимации</param>
        public ClassStaticAnimation(List<Bitmap> Images, int X, int Y, int SpeedAnimation, int TimeAnimation, int Pause)
        {
            _images = Images;
            _x = X;
            _y = Y;
            _speedAnimation = SpeedAnimation;
            _timeAnimation = TimeAnimation;
            _pause = Pause;
            _cyclesTime = 0;
            while (_cyclesTime < TimeAnimation)
            {
                _cyclesTime = _cyclesTime + SpeedAnimation * Images.Count;
            }
            if (_cyclesTime == 0) _cyclesTime = SpeedAnimation * Images.Count;
            if (TimeAnimation == 0) _pause = 0;
            _time = 0;
        }

        /// <summary>Отрисовываем следующий цикл анимации</summary>
        /// <param name="g">Поверхность для рисования</param>
        /// <param name="deltaTime">Шаг приращения времени</param>
        public void DrawStaticAnimation(Graphics g, int deltaTime)
        {
            int i = (int)(_time / _speedAnimation);
            while (i >= _images.Count)
            {
                i = i - _images.Count;
            }
            if (_time >= _cyclesTime) i = 0;
            //if (_timeAnimation == 0 && _pause == 0) i = 0;
            Bitmap src = new Bitmap(_images[i]);
            g.DrawImage(src, new Rectangle(_x, _y, src.Width, src.Height));
            src.Dispose();
            _time = _time + deltaTime;
            if ((_time >= _cyclesTime + _pause) && (_pause > 0 || _timeAnimation == 0)) _time = _time - (_cyclesTime + _pause);
        }

        /// <summary>Сброс анимации к начальному значению</summary>
        public void ResetDrawStaticAnimation()
        {
            _time = 0;
        }

    }
}
