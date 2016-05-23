using AssemblyFuelUtility.Settings;
using CDKspUtil.Logic;
using CDKspUtil.UI;
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
        private AFUSettings _settings;

        private bool _toggleOn = false;

        private int _windowId;
        private Rect _windowPosition;

        private FuelModel _fuel;

        private string _debugString = "";

        private void Awake()
        {
            _settings = AFUSettings.Load();

            LogHelper.Info("AssemblyFuelUtility Awake");

            GameEvents.onGUIApplicationLauncherReady.Add(OnGUIAppLauncherReady);
            
            _windowId = WindowHelper.NextWindowId("AssemblyFuelUtility");
            _windowPosition = new Rect(_settings.WindowY, _settings.WindowY, 250, 250);
        }

        void OnDestroy()
        {
            //Clean up
            GameEvents.onGUIApplicationLauncherReady.Remove(OnGUIAppLauncherReady);

            if (_afuButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(_afuButton);
            }
        }

        private void Start()
        {
            _fuel = new FuelModel();
        }

        private void OnGUI()
        {
            if (_toggleOn) { 
                _windowPosition = GUILayout.Window(_windowId, _windowPosition, RenderWindowContent, "Assembly Fuel Utility");

                if (_windowPosition.x != _settings.WindowX || _windowPosition.y != _settings.WindowY)
                {
                    _settings.WindowX = (int)_windowPosition.x;
                    _settings.WindowY = (int)_windowPosition.y;

                    AFUSettings.Save(_settings);
                }
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

                            ApplyFuelModel(_fuel, EditorLogic.fetch.ship);
                        }

                        if (GUILayout.Button("Fill All"))
                        {
                            _fuel.LiquidFuel = 1.0f;
                            _fuel.Oxidizer = 1.0f;
                            _fuel.Monoprop = 1.0f;

                            ApplyFuelModel(_fuel, EditorLogic.fetch.ship);
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

                    if (GUILayout.Button("Apply"))
                    {
                        ApplyFuelModel(_fuel, EditorLogic.fetch.ship);
                    }

                    if (!String.IsNullOrEmpty(_debugString))
                    {
                        _debugString = GUILayout.TextArea(_debugString);
                    }

                    
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }

        private void ApplyFuelModel(FuelModel model, ShipConstruct ship)
        {
            foreach (var part in ship.parts)
            {
                foreach (var resource in part.Resources.list)
                {
                    switch (resource.resourceName)
                    {
                        case FuelTypes.LiquidFuel:
                            {
                                resource.amount = resource.maxAmount * _fuel.LiquidFuel;
                                break;
                            }
                        case FuelTypes.Oxidizer:
                            {
                                resource.amount = resource.maxAmount * _fuel.Oxidizer;
                                break;
                            }
                        case FuelTypes.Monopropellant:
                            {
                                resource.amount = resource.maxAmount * _fuel.Monoprop;
                                break;
                            }

                    }
                }
            }
        }

        #region Persistance



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
