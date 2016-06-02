using AssemblyFuelUtility.Settings;
using CDKspUtil.Logic;
using CDKspUtil.UI;
using JEngine = Jint.Engine;
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
        private const float _buttonHeight = 20f;
        private GUIStyle _buttonStyle;

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
            _windowPosition = new Rect(_config.WindowX, _config.WindowY, 0, 0);
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
                if (Event.current.type == EventType.Layout)
                {
                    _windowPosition.height = 0;
                    _windowPosition.width = 0;
                }

                _windowPosition = GUILayout.Window(_windowId, _windowPosition, RenderWindowContent, "Assembly Fuel Utility");
            }
        }

        private void RenderWindowContent(int windowId)
        {
            try
            {
                var ship = EditorLogic.fetch.ship;
                var functionsSource = System.IO.File.ReadAllText(IOUtils.GetFilePathFor(typeof(AssemblyFuelUtility), "afu_scripts.js"));

                var jEngine = CreateJintEngine();

                //Variables
                jEngine.SetValue("_fuel", _fuel);
                jEngine.SetValue("_ship", ship);
                jEngine.SetValue("_toggleOn", _toggleOn);
                jEngine.SetValue("_buttonHeight", _buttonHeight);

                //Functions
                jEngine.SetValue("ShipHasAnyFuelParts", new Func<ShipConstruct, bool>(ShipHasAnyFuelParts));
                jEngine.SetValue("ShipHasAnyPartsContaining", new Func<ShipConstruct, string, bool>(ShipHasAnyPartsContaining));

                jEngine.Execute(functionsSource);

                var state = (object[])jEngine.Execute("renderMainGui();").GetCompletionValue().ToObject();

                _toggleOn = (bool)state[0];
                _fuel = (FuelModel)state[1];

                if (_fuel.Changed)
                {
                    _fuel.Apply(ship);

                    _config.FuelModel = _fuel;
                }
            }
            catch (Exception ex)
            {
                GUILayout.BeginVertical();
                {
                    GUILayout.Label("Unable to render GUI. Perhaps there is a mistake in the .js.");
                    GUILayout.Label(ex.Message);
                    GUILayout.Label(ex.StackTrace);
                }
                GUILayout.EndVertical();
            }

            GUI.DragWindow();
        }

        private float RenderFuelControlGroup(float currentAmount, string label)
        {
            float amount = currentAmount;
            float quickAmount = -1;

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(label);

                if (GUILayout.Button("E", GUILayout.Height(_buttonHeight))) { quickAmount = 0; }

                amount = GUILayout.HorizontalSlider(amount, 0, 1);

                if (GUILayout.Button("F", GUILayout.Height(_buttonHeight))) { quickAmount = 1; }

                amount = quickAmount > -1 ? quickAmount : amount;

                GUILayout.Label(amount.ToString("p0"), GUILayout.Width(40f));
            }
            GUILayout.EndHorizontal();

            return amount;
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

        private JEngine CreateJintEngine()
        {
            return new JEngine(cfg => cfg.
                AllowClr().
                AllowClr(typeof(GUILayout).Assembly).
                AllowClr(typeof(AssemblyFuelUtility).Assembly));
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
                    OnAFUToggle,
                    OnAFUToggle,
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.SPH | ApplicationLauncher.AppScenes.VAB,
                    (Texture)GameDatabase.Instance.GetTexture("AssemblyFuelUtility/AFUIcon", false));
            }
        }

        private void OnAFUToggle()
        {
            _toggleOn = !_toggleOn;
        }

        #endregion
    }
}
