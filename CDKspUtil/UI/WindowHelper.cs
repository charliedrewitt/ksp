using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CDKspUtil.UI
{
    public class WindowHelper
    {
        private static Dictionary<string, int> _windowDictionary;
        public static int NextWindowId(string windowKey)
        {
            if (_windowDictionary == null)
            {
                _windowDictionary = new Dictionary<string, int>();
            }

            if (_windowDictionary.ContainsKey(windowKey))
            {
                return _windowDictionary[windowKey];
            }

            var newId = _windowDictionary.Count() + 1;

            _windowDictionary.Add(windowKey, newId);

            return newId;
        }

        #region Rendering Helpers

        public static UIState? RenderTabs(UIState currentState)
        {
            GUILayout.BeginHorizontal();
            {
                var defaultStyle = GUI.skin.button;
                var selectedStyle = GUI.skin.button;
                selectedStyle.normal.textColor = Color.green;

                if (GUILayout.Button("ORT", currentState == UIState.ORT ? selectedStyle : defaultStyle))
                {
                    return UIState.ORT;
                }

                if (GUILayout.Button("SPD", currentState == UIState.SPD ? selectedStyle : defaultStyle))
                {
                    return UIState.SPD;
                }

                if (GUILayout.Button("PID", currentState == UIState.PID ? selectedStyle : defaultStyle))
                {
                    return UIState.PID;
                }
            }
            GUILayout.EndHorizontal();

            return null;
        }

        #region Speed

        public static void RenderSpeedInfo(int desiredSpeed)
        {
            //GUILayout.Label(String.Format("Speed: {0}m/s", vessel.speed.ToString("n2")));
            GUILayout.Label(String.Format("Hold: {0}m/s", desiredSpeed.ToString("n")));
        }

        public static void RenderDesiredSpeedControls(ref bool manageThrottle, ref Parseable<int> desiredSpeed)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(String.Format("Desired Speed"));
                    desiredSpeed.Text = GUILayout.TextArea(desiredSpeed.Text);
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button(manageThrottle ? "Off" : "On")) manageThrottle = !manageThrottle;
            }
            GUILayout.EndVertical();
        }

        #endregion

        #region Orientation

        public static void RenderOrientationInfo(float pitch, float roll, float yaw)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(String.Format("PITCH: {0}", pitch.ToString("n2")));
                GUILayout.Label(String.Format("ROLL: {0}", roll.ToString("n2")));
                GUILayout.Label(String.Format("YAW: {0}", yaw.ToString("n2")));
            }
            GUILayout.EndHorizontal();
        }

        public static void RenderOrientationControls(ref bool holdPitch, ref Parseable<double> pitch)
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(String.Format("Pitch"));
                    pitch.Text = GUILayout.TextArea(pitch.Text);

                    if (GUILayout.Button(holdPitch ? "Off" : "On")) holdPitch = !holdPitch;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        #endregion

        #region PID

        public static void RenderPidControls(ref Parseable<double> pGain, ref Parseable<double> iGain, ref Parseable<double> dGain)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(String.Format("P"));
                pGain.Text = GUILayout.TextArea(pGain.Text);
                GUILayout.Label(String.Format("I"));
                iGain.Text = GUILayout.TextArea(iGain.Text);
                GUILayout.Label(String.Format("D"));
                dGain.Text = GUILayout.TextArea(dGain.Text);
            }
            GUILayout.EndHorizontal();
        }

        #endregion

        #endregion
    }
}
