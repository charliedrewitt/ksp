using AssemblyFuelUtility.Settings;
using CDKspUtil.Logic;
using CDKspUtil.UI;
using KSP.IO;
using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AssemblyFuelUtility
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class AssemblyFuelUtility : MonoBehaviour
    {
        private AssemblyFuelConfigNode _config;

        private bool _toggleOn = false;

        private int _windowId;
        private Rect _windowPosition;

        private FuelModel _fuel;

        private string _debugString = "";

        private void Awake()
        {
            _config = AssemblyFuelConfigNode.LoadOrCreate(GetEnsuredConfigPath());

            LogHelper.Info("AssemblyFuelUtility Awake");

            GameEvents.onGUIApplicationLauncherReady.Add(OnGUIAppLauncherReady);

            _windowId = WindowHelper.NextWindowId("AssemblyFuelUtility");
            _windowPosition = new Rect(_config.WindowX, _config.WindowY, 250, 250);
            _fuel = _config.FuelModel;
        }

        private void OnDestroy()
        {
            Save();

            //Clean up
            GameEvents.onGUIApplicationLauncherReady.Remove(OnGUIAppLauncherReady);

            if (_afuButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(_afuButton);
            }
        }

        private void OnGUI()
        {
            if (_toggleOn)
            {
                _windowPosition = GUILayout.Window(_windowId, _windowPosition, RenderWindowContent, "Assembly Fuel Utility");
            }
        }

        private void RenderWindowContent(int windowId)
        {
            GUI.skin = HighLogic.Skin;

            GUILayout.BeginHorizontal(GUILayout.Width(250f));
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Empty All"))
                        {
                            _fuel.LiquidFuel = 0f;
                            _fuel.Oxidizer = 0f;
                            _fuel.Monoprop = 0f;
                        }

                        if (GUILayout.Button("Fill All"))
                        {
                            _fuel.LiquidFuel = 1.0f;
                            _fuel.Oxidizer = 1.0f;
                            _fuel.Monoprop = 1.0f;
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Label(FuelTypes.LiquidFuel);
                    GUILayout.BeginHorizontal();
                    {
                        _fuel.LiquidFuel = GUILayout.HorizontalSlider(_fuel.LiquidFuel, 0, 1);
                        GUILayout.Label(((int)(_fuel.LiquidFuel * 100)).ToString("n2"));
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Label(FuelTypes.Oxidizer);
                    GUILayout.BeginHorizontal();
                    {
                        _fuel.Oxidizer = GUILayout.HorizontalSlider(_fuel.Oxidizer, 0, 1);
                        GUILayout.Label(((int)(_fuel.Oxidizer * 100)).ToString("n2"));
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Label(FuelTypes.Monopropellant);
                    GUILayout.BeginHorizontal();
                    {
                        _fuel.Monoprop = GUILayout.HorizontalSlider(_fuel.Monoprop, 0, 1);
                        GUILayout.Label(((int)(_fuel.Monoprop * 100)).ToString("n2"));
                    }
                    GUILayout.EndHorizontal();

                    if (_fuel.Changed)
                    {
                        _fuel.Apply(EditorLogic.fetch.ship);

                        _config.FuelModel = _fuel;
                    }

                    //if (!String.IsNullOrEmpty(_debugString))
                    //{
                    //    _debugString = GUILayout.TextArea(_debugString);
                    //}
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }

        #region Persistance

        private void Save()
        {
            _config.WindowX = _windowPosition.x;
            _config.WindowY = _windowPosition.y;

            _config.Save(GetEnsuredConfigPath());
        }

        private string GetEnsuredConfigPath()
        {
            string path = IOUtils.GetFilePathFor(typeof(AssemblyFuelUtility), "afu_settings.cfg");
            string directory = System.IO.Path.GetDirectoryName(path);

            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
            }

            return path;
        }

        #endregion

        #region App Launcher

        ApplicationLauncherButton _afuButton = null;

        private void OnGUIAppLauncherReady()
        {
            if (ApplicationLauncher.Ready)
            {
                _afuButton = ApplicationLauncher.Instance.AddModApplication(
                    OnAFUToggleOn,
                    OnAFUToggleOff,
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB,
                    (Texture)GameDatabase.Instance.GetTexture("", false));
            }
        }

        private void OnAFUToggleOn()
        {
            _toggleOn = true;
        }

        private void OnAFUToggleOff()
        {
            _toggleOn = false;
        }

        #endregion
    }
}
