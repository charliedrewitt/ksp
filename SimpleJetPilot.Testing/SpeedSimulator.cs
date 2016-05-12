using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimpleJetPilot.Logic;

namespace SimpleJetPilot.Testing
{
    public class SpeedSimulator
    {
        #region Control Variables

        private bool _autoThrottle = false;

        #endregion

        #region Simulation Variables

        private float _throttlePercentage = 0.0f;
        private float _maxAcceleration = 20f;
        private float _airResistance = 0.1f;

        private float _currentSpeed = 0.0f;

        #endregion

        private PIDController _controller;

        public void Start()
        {
            _controller = new PIDController();

            _controller.Kp = 0.5f;
            _controller.Ki = 0.005f;
            _controller.Kd = 0.5f;
            _controller.MaxIntegrator = 250;
            _controller.MinIntegrator = -250;

            _controller.SetPoint = 65f;

            _controller.MaxOutput = 1;
            _controller.MinOutput = 0;

            var timer = Stopwatch.StartNew();

            while (true)
            {
                var elapsedSeconds = (float)timer.ElapsedTicks / (float)Stopwatch.Frequency;
                timer = Stopwatch.StartNew();

                if (_autoThrottle)
                {
                    _throttlePercentage = _controller.Step(_currentSpeed);
                }

                _throttlePercentage = MathHelper.Clamp<float>(_throttlePercentage, 0, 1);

                var acceleration = ((_maxAcceleration * _throttlePercentage) * elapsedSeconds);
                var resistance = ((_airResistance * _currentSpeed) * elapsedSeconds);

                var deltaV = acceleration - resistance;

                _currentSpeed += deltaV;

                Console.SetCursorPosition(0, 0);
                Console.Write("\r{2}Throttle: {0}%, Speed: {1}m/s                                ",
                    (_throttlePercentage * 100).ToString("n0"),
                    _currentSpeed.ToString("n2"),
                    _autoThrottle ? "Auto" : ""
                    );
            }
        }

        public void OnKeyPress(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.T:
                    {
                        _autoThrottle = !_autoThrottle; break;
                    }
                case ConsoleKey.Z: { if (!_autoThrottle) _throttlePercentage += 0.05f; break; }
                case ConsoleKey.X: { if (!_autoThrottle) _throttlePercentage -= 0.05f; break; }
            }
        }
    }
}
