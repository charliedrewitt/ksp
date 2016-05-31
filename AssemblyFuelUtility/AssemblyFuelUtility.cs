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

            //GUILayout.BeginArea(new Rect(_windowPosition.x - 10, _windowPosition.y, 10, 10));
            //{ 
            //    if (GUILayout.Button("x"))
            //    {
            //        _toggleOn = false;
            //    }
            //}
            //GUILayout.EndArea();

            GUILayout.BeginHorizontal(GUILayout.Width(250f));
            {
                //GUILayout.Button("x", GUILayout.)

                GUILayout.BeginVertical();
                {
                    var ship = EditorLogic.fetch.ship;

                    if (ShipHasAnyFuelParts(ship))
                    {
                        GUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button("Empty All"))
                            {
                                _fuel.LiquidFuel = 0f;
                                _fuel.Oxidizer = 0f;
                                _fuel.SolidFuel = 0f;
                                _fuel.Monoprop = 0f;
                            }

                            if (GUILayout.Button("Fill All"))
                            {
                                _fuel.LiquidFuel = 1.0f;
                                _fuel.Oxidizer = 1.0f;
                                _fuel.SolidFuel = 1.0f;
                                _fuel.Monoprop = 1.0f;
                            }
                        }
                        GUILayout.EndHorizontal();

                        if (ShipHasAnyPartsContaining(ship, FuelTypes.LiquidFuel))
                        {
                            GUILayout.Label(FuelTypes.LiquidFuel);
                            GUILayout.BeginHorizontal();
                            {
                                _fuel.LiquidFuel = GUILayout.HorizontalSlider(_fuel.LiquidFuel, 0, 1);
                                GUILayout.Label(_fuel.LiquidFuel.ToString("p0"));
                            }
                            GUILayout.EndHorizontal();
                        }

                        if (ShipHasAnyPartsContaining(ship, FuelTypes.Oxidizer))
                        {
                            GUILayout.Label(FuelTypes.Oxidizer);
                            GUILayout.BeginHorizontal();
                            {
                                _fuel.Oxidizer = GUILayout.HorizontalSlider(_fuel.Oxidizer, 0, 1);
                                GUILayout.Label(_fuel.Oxidizer.ToString("p0"));
                            }
                            GUILayout.EndHorizontal();
                        }

                        if (ShipHasAnyPartsContaining(ship, FuelTypes.SolidFuel))
                        {
                            GUILayout.Label(FuelTypes.SolidFuel);
                            GUILayout.BeginHorizontal();
                            {
                                _fuel.SolidFuel = GUILayout.HorizontalSlider(_fuel.SolidFuel, 0, 1);
                                GUILayout.Label(_fuel.SolidFuel.ToString("p0"));
                            }
                            GUILayout.EndHorizontal();
                        }

                        if (ShipHasAnyPartsContaining(ship, FuelTypes.Monopropellant))
                        {
                            GUILayout.Label(FuelTypes.Monopropellant);
                            GUILayout.BeginHorizontal();
                            {
                                _fuel.Monoprop = GUILayout.HorizontalSlider(_fuel.Monoprop, 0, 1);
                                GUILayout.Label(_fuel.Monoprop.ToString("p0"));
                            }
                            GUILayout.EndHorizontal();
                        }

                        if (_fuel.Changed)
                        {
                            _fuel.Apply(ship);

                            _config.FuelModel = _fuel;
                        }
                    }
                    else
                    {
                        GUILayout.Label("Ship currently has no parts containing fuel. Add some.");
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }

        #region Utility

        private bool ShipHasAnyPartsContaining(ShipConstruct ship, string fuelName)
        {
            foreach (var part in ship.parts)
            {
                if (part.Resources.list.Any(r => r.resourceName == fuelName)) return true;
            }

            return false;
        }

        private bool ShipHasAnyFuelParts(ShipConstruct ship)
        {
            foreach (var part in ship.parts)
            {
                if (part.Resources.list.Any(r => FuelTypes.All.Contains(r.resourceName))) return true;
            }

            return false;
        }

        #endregion

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
