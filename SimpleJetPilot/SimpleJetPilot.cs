using CDKspUtil.Logic;
using CDKspUtil.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private UIState _state = UIState.SPD;

        #region Throttle
        private bool _manageThrottle;
        private PIDController _throttleController;
        private Parseable<int> _desiredSpeed;
        #endregion

        #region Orientation

        private bool _holdPitch;
        private PIDController _pitchController;
        private Parseable<int> _desiredPitch;

        #endregion

        #region PID Gains
        private Parseable<double> _pGain;
        private Parseable<double> _iGain;
        private Parseable<double> _dGain;
        #endregion

        private void Awake()
        {
            _windowId = WindowHelper.NextWindowId("SimpleJetPilot-Test");
            _windowPosition = new Rect(60, 60, 60, 60);
        }

        private void Start()
        {
            _desiredSpeed = new Parseable<int>(10);
            _pGain = new Parseable<double>(0.15);
            _iGain = new Parseable<double>(0.01);
            _dGain = new Parseable<double>(0.5);

            _throttleController = InitialiseThrottleController();

            _vessel = FlightGlobals.ActiveVessel;
            _vessel.OnFlyByWire -= AutoThrottle;
            _vessel.OnFlyByWire += AutoThrottle;
        }

        private void OnSave(ConfigNode node)
        {
            LogHelper.Info("OnSave");
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
            {
                GUILayout.BeginVertical();
                {
                    var newState = WindowHelper.RenderTabs(_state);

                    if (newState != null) _state = (UIState)newState;

                    switch (_state)
                    {
                        case UIState.SPD:
                            {
                                WindowHelper.RenderSpeedInfo(_desiredSpeed.Value);

                                WindowHelper.RenderDesiredSpeedControls(ref _manageThrottle, ref _desiredSpeed);

                                if (_desiredSpeed.Success) _throttleController.SetPoint = (double)_desiredSpeed.Value;

                                break;
                            }
                        case UIState.ORT:
                            {
                                var angles = _vessel.transform.rotation.eulerAngles;

                                //WindowHelper.RenderOrientationInfo(angles.y, angles.z, angles.x);

                                break;
                            }
                        case UIState.PID:
                            {
                                WindowHelper.RenderPidControls(ref _pGain, ref _iGain, ref _dGain);

                                //if (_pGain.Success) _throttleController.Kp = (double)_pGain.Value;
                                //if (_iGain.Success) _throttleController.Ki = (double)_iGain.Value;
                                //if (_dGain.Success) _throttleController.Kd = (double)_dGain.Value;

                                break;
                            }
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }

        public void AutoThrottle(FlightCtrlState state)
        {
            if (_manageThrottle)
            {
                state.mainThrottle = (float)_throttleController.Compute(_vessel.speed, Time.deltaTime);
            }
        }

        private PIDController InitialiseThrottleController()
        {
            var controller = new PIDController();

            controller.Kp = 0.3;
            controller.Ki = 0.001;
            controller.Kd = 0.001;

            controller.MaxIntegrator = 250;
            controller.MinIntegrator = -250;

            controller.MaxOutput = 1;
            controller.MinOutput = 0;

            return controller;
        }
    }
}
