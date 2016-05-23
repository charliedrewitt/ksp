using CDKspUtil.UI;
using CDKspUtil.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleJetPilot.Logic.ControllerManagers
{
    public class OrientationManager
    {
        public OrientationManager()
        {
            PitchController = InitialisePitchController();
        }

        public bool PitchEnabled { get; set; }
        public PIDController PitchController { get; set; }
        public Parseable<float> PitchSetpoint { get; set; }

        private PIDController InitialisePitchController()
        {
            return new PIDController
            {
                Kp = 0.15,
                Ki = 0.01,
                Kd = 0.2,
                MaxIntegrator = 250,
                MinIntegrator = -250,
                MaxOutput = 360,
                MinOutput = 0,
            };
        }
    }
}
