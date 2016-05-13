using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleJetPilot.Logic
{
    public class PIDController
    {
        #region Properties

        #region Gains
        public double Kp { get; set; }
        public double Ki { get; set; }
        public double Kd { get; set; }
        #endregion

        #region Inputs
        public double SetPoint { get; set; }
        #endregion

        #region Limits

        public double MaxOutput { get; set; }
        public double MinOutput { get; set; }

        public double MaxIntegrator { get; set; }
        public double MinIntegrator { get; set; }

        #endregion

        #endregion

        #region Private Fields

        private double _integrator = 0.0f;
        private double _previousError = 0.0f;

        #endregion
        public double Compute(double input)
        {
            var error = SetPoint - input;

            _integrator += error;
            _integrator = MathHelper.Clamp<double>(_integrator, MinIntegrator, MaxIntegrator);

            var proportional = Kp * error;
            var integral = Ki * _integrator;
            var derivative = Kd * (_previousError - error);

            //Console.SetCursorPosition(0, 1);
            //Console.Write($"P:{proportional.ToString("F2")}\nI:{integral.ToString("F2")}\nD:{derivative.ToString("F2")}");

            _previousError = error;

            return MathHelper.Clamp<double>(proportional + integral + derivative, MinOutput, MaxOutput);
        }

        #region Helpers



        #endregion
    }
}
