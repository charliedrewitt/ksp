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
        public float Kp { get; set; }
        public float Ki { get; set; }
        public float Kd { get; set; }
        #endregion

        #region Inputs
        public float SetPoint { get; set; }
        #endregion

        #region Limits

        public float MaxOutput { get; set; }
        public float MinOutput { get; set; }

        public float MaxIntegrator { get; set; }
        public float MinIntegrator { get; set; }

        #endregion

        #endregion

        #region Private Fields

        private float _integrator = 0.0f;
        private float _previousError = 0.0f;

        #endregion
        public float Step(float input)
        {
            var error = SetPoint - input;

            _integrator += error;
            _integrator = MathHelper.Clamp<float>(_integrator, MinIntegrator, MaxIntegrator);

            var proportional = Kp * error;
            var integral = Ki * _integrator;
            var derivative = Kd * (_previousError - error);

            //Console.SetCursorPosition(0, 1);
            //Console.Write($"P:{proportional.ToString("F2")}\nI:{integral.ToString("F2")}\nD:{derivative.ToString("F2")}");

            _previousError = error;

            return MathHelper.Clamp<float>(proportional + integral + derivative, MinOutput, MaxOutput);
        }

        #region Helpers



        #endregion
    }
}
