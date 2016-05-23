using CDKspUtil.Logic;
using CDKspUtil.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SimpleJetPilot
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class SimpleJetPilot : MonoBehaviour
    {
        private int _windowId;
        private Vessel _vessel;
        private Rect _windowPosition;
        private PIDController _throttleController;
        private Parseable<int> _desiredSpeed;
        private bool _manageThrottle;

        private Parseable<double> _pGain;
        private Parseable<double> _iGain;

        private void Awake()
        {
            _windowId = WindowHelper.NextWindowId("SimpleJetPilot-Test");
            _windowPosition = new Rect(60, 60, 60, 60);
        }

        private void Start()
        {
            _desiredSpeed = new Parseable<int>(10);
            _pGain = new Parseable<double>(0.5);
            _iGain = new Parseable<double>(0.005);

            _throttleController = InitialiseThrottleController();

            _vessel = FlightGlobals.ActiveVessel;
            _vessel.OnFlyByWire -= AutoThrottle;
            _vessel.OnFlyByWire += AutoThrottle;
        }

        private void OnGUI()
        {
            _windowPosition = GUILayout.Window(_windowId, _windowPosition, RenderWindowContent, "Simple Jet Pilot");
        }

        #region GUI Variables

        #endregion

        private void RenderWindowContent(int windowId)
        {
            GUI.skin = HighLogic.Skin;

            GUILayout.BeginHorizontal(GUILayout.Width(250f));
            GUILayout.BeginVertical();

            GUILayout.Label(String.Format("Speed: {0}m/s", _vessel.speed.ToString("n2")));
            GUILayout.Label(String.Format("Set Speed: {0}m/s", _desiredSpeed.Value.ToString("n")));

            GUILayout.BeginHorizontal();
            GUILayout.Label(String.Format("Desired Speed"));
            _desiredSpeed.Text = GUILayout.TextArea(_desiredSpeed.Text);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(String.Format("P"));
            _pGain.Text = GUILayout.TextArea(_pGain.Text);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(String.Format("I"));
            _iGain.Text = GUILayout.TextArea(_iGain.Text);
            GUILayout.EndHorizontal();

            if (_desiredSpeed.Success) _throttleController.SetPoint = (double)_desiredSpeed.Value;
            if (_pGain.Success) _throttleController.Kp = (double)_pGain.Value;
            if (_iGain.Success) _throttleController.Ki = (double)_iGain.Value;

            if (GUILayout.Button(_manageThrottle ? "Off" : "On")) _manageThrottle = !_manageThrottle;

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }

        public void AutoThrottle(FlightCtrlState state)
        {
            if (_manageThrottle)
            {
                state.mainThrottle = (float)_throttleController.Compute(_vessel.speed);
            }
        }

        private PIDController InitialiseThrottleController()
        {
            var controller = new PIDController();

            controller.MaxIntegrator = 250;
            controller.MinIntegrator = -250;

            controller.MaxOutput = 1;
            controller.MinOutput = 0;

            return controller;
        }
    }
}
