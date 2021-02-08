using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazFit_Watchface_2
{
    public class ClassMotiomAnimation
    {
        Bitmap _image; // Картинка для анимации
        int _startX; // Стартовая координата X
        int _startY; // Стартовая координата Y
        int _endX; // Конечноя координата X
        int _endY; // Конечноя координата Y
        int _speedAnimation; // Скорость анимации
        int _timeAnimation; // Время анимации
        bool _bounce; // Время анимации
        int _cyclesTime; // длительность цикла
        int _time; // прошедшее время

        /// <summary>Класс для показа анимации с двидением по координатам</summary>
        /// <param name="Image">Картинка для анимации</param>
        /// <param name="StartX">Стартовая координата X</param>
        /// <param name="StartY">Стартовая координата Y</param>
        /// <param name="EndX">Конечноя координата X</param>
        /// <param name="EndY">Конечноя координата Y</param>
        /// <param name="SpeedAnimation">Скорость анимации</param>
        /// <param name="TimeAnimation">Время анимации</param>
        /// <param name="Bounce">Время анимации</param>
        public ClassMotiomAnimation(Bitmap Image, int StartX, int StartY, int EndX, int EndY, int SpeedAnimation,
            int TimeAnimation, bool Bounce)
        {
            _image = Image;
            _startX = StartX;
            _startY = StartY;
            _endX = EndX;
            _endY = EndY;
            _speedAnimation = SpeedAnimation;
            _timeAnimation = TimeAnimation;
            if (_timeAnimation <= 10) _timeAnimation = 0;
             _bounce = Bounce;
            _time = 0;
            _cyclesTime = SpeedAnimation;
            while (_cyclesTime < TimeAnimation)
            {
                _cyclesTime = _cyclesTime + SpeedAnimation;
            }
        }

        /// <summary>Отрисовываем следующий цикл анимации</summary>
        /// <param name="g">Поверхность для рисования</param>
        /// <param name="deltaTime">Шаг приращения времени</param>
        public void DrawMotiomAnimation(Graphics g, int deltaTime)
        {
            if (_bounce)
            {
                DrawMotiomAnimationBonce(g, deltaTime);
                return;
            }
            int newX = _startX;
            int newY = _startY;

            int _newtime = _time;
            if (_newtime > _cyclesTime && _timeAnimation > 0) _newtime = _cyclesTime;
            while (_newtime > _speedAnimation * 2)
            {
                _newtime = _newtime - _speedAnimation * 2;
            }

            if (_newtime > _speedAnimation)
            {
                _newtime = _newtime - _speedAnimation;
                int dX = _startX - _endX;
                int dY = _startY - _endY;

                double proportions = (double)_newtime / _speedAnimation;
                newX = (int)(_endX + dX * proportions);
                newY = (int)(_endY + dY * proportions);
            }
            else
            {
                int dX = _endX - _startX;
                int dY = _endY - _startY;

                double proportions = (double)_newtime / _speedAnimation;
                newX = (int)(_startX + dX * proportions);
                newY = (int)(_startY + dY * proportions);
            }
            if (_image != null)
            {
                g.DrawImage(_image, new Rectangle(newX, newY, _image.Width, _image.Height));
            }
            _time = _time + deltaTime;
        }
        
        private void DrawMotiomAnimationBonce(Graphics g, int deltaTime)
        {
            double TimeCoeff = 0.3;
            int newX = _startX;
            int newY = _startY;

            double _newtime = _time;
            if (_newtime > _cyclesTime && _timeAnimation > 0) _newtime = _cyclesTime;
            while (_newtime > _speedAnimation * 2)
            {
                _newtime = _newtime - _speedAnimation * 2;
            }

            // обратный ход
            if (_newtime > _speedAnimation)
            {
                _newtime = _newtime - _speedAnimation;
                int dX = _startX - _endX;
                int dY = _startY - _endY;

                // конечный отскок
                if (_newtime >= (1 - TimeCoeff) * _speedAnimation)
                {
                    _newtime = _newtime - _speedAnimation;
                    double proportions = _newtime / (_speedAnimation * TimeCoeff);
                    newX = (int)Math.Round((_startX - 0.1f * dX * proportions));
                    newY = (int)Math.Round((_startY - 0.1f * dY * proportions));
                }
                // основной ход
                else if (_newtime >= TimeCoeff * _speedAnimation)
                {
                    _newtime = _newtime - TimeCoeff * _speedAnimation;
                    double proportions = _newtime / (_speedAnimation * (1 - TimeCoeff - TimeCoeff));
                    newX = (int)Math.Round(((_endX - 0.1f * dX )+ 1.2f * dX * proportions));
                    newY = (int)Math.Round(((_endY - 0.1f * dY) + 1.2f * dY * proportions));
                }
                // начальный отскок
                else
                {
                    double proportions = _newtime / (_speedAnimation * TimeCoeff);
                    newX = (int)Math.Round((_endX - 0.1f * dX * proportions));
                    newY = (int)Math.Round((_endY - 0.1f * dY * proportions));
                }


            }
            else // прямой ход
            {
                int dX = _endX - _startX;
                int dY = _endY - _startY;

                // конечный отскок
                if (_newtime >= (1 - TimeCoeff) * _speedAnimation)
                {
                    _newtime = _newtime - _speedAnimation;
                    double proportions = _newtime / (_speedAnimation * TimeCoeff);
                    newX = (int)Math.Round((_endX - 0.1f * dX * proportions));
                    newY = (int)Math.Round((_endY - 0.1f * dY * proportions));
                }
                // основной ход
                else if (_newtime >= TimeCoeff * _speedAnimation)
                {
                    _newtime = _newtime - TimeCoeff * _speedAnimation;
                    double proportions = _newtime / (_speedAnimation * (1 - TimeCoeff - TimeCoeff));
                    newX = (int)Math.Round(((_startX - 0.1f * dX) + 1.2f * dX * proportions));
                    newY = (int)Math.Round(((_startY - 0.1f * dY) + 1.2f * dY * proportions));
                }
                // начальный отскок
                else
                {
                    double proportions = _newtime / (_speedAnimation * TimeCoeff);
                    newX = (int)Math.Round((_startX - 0.1f * dX * proportions));
                    newY = (int)Math.Round((_startY - 0.1f * dY * proportions));
                }

            }
            if (_image != null)
            {
                g.DrawImage(_image, new Rectangle(newX, newY, _image.Width, _image.Height));
            }
            _time = _time + deltaTime;
        }

        /// <summary>Сброс анимации к начальному значению</summary>
        public void ResetDrawMotiomAnimation()
        {
            _time = 0;
        }
    }
}
