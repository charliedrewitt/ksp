var UnityEngine = importNamespace('UnityEngine');
var GUILayout = UnityEngine.GUILayout;
var _adjustAll = 0;

function renderMainGui() {
    GUILayout.BeginHorizontal(GUILayout.Width(400));
    {
        GUILayout.BeginVertical();
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label('Use the sliders or [E]mpty and [F]ull buttons top adjust fuel in all tanks.');
                if (GUILayout.Button("X", GUILayout.ExpandWidth(false), GUILayout.Height(_buttonHeight))) {
                    _toggleOn = false;
                }
            }
            GUILayout.EndHorizontal();

            if (ShipHasAnyFuelParts(_ship)) {

                GUILayout.Space(2);

                if (ShipHasAnyPartsContaining(_ship, 'LiquidFuel')) {
                    _fuel.LiquidFuel = renderFuelControlGroup(_fuel.LiquidFuel, 'Liquid Fuel');
                }

                if (ShipHasAnyPartsContaining(_ship, 'Oxidizer')) {
                    _fuel.Oxidizer = renderFuelControlGroup(_fuel.Oxidizer, 'Oxidizer');
                }

                if (ShipHasAnyPartsContaining(_ship, 'SolidFuel')) {
                    _fuel.SolidFuel = renderFuelControlGroup(_fuel.SolidFuel, 'Solid Fuel');
                }

                if (ShipHasAnyPartsContaining(_ship, 'MonoPropellant')) {
                    _fuel.Monoprop = renderFuelControlGroup(_fuel.Monoprop, 'Monoprop');
                }

                //_adjustAll = renderFuelControlGroup(_adjustAll, '');

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Empty All Tanks", GUILayout.Height(_buttonHeight))) {
                        _fuel.LiquidFuel = 0;
                        _fuel.Oxidizer = 0;
                        _fuel.SolidFuel = 0;
                        _fuel.Monoprop = 0;
                    }
                    if (GUILayout.Button("Fill All Tanks", GUILayout.Height(_buttonHeight))) {
                        _fuel.LiquidFuel = 1;
                        _fuel.Oxidizer = 1;
                        _fuel.SolidFuel = 1;
                        _fuel.Monoprop = 1;
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndVertical();
    }
    GUILayout.EndHorizontal();

    return [_toggleOn, _fuel];
}
function renderFuelControlGroup(currentAmount, label) {
    var amount = currentAmount;
    var quickAmount = -1;
    var buttonDimension = 17;

    GUILayout.BeginHorizontal();
    {
        if (label != null && label != '') {
            GUILayout.Label(label, GUILayout.Width(65));
        }

        if (GUILayout.Button("E", GUILayout.Width(buttonDimension), GUILayout.Height(buttonDimension))) { quickAmount = 0; }

        amount = GUILayout.HorizontalSlider(amount, 0, 1);

        if (GUILayout.Button("F", GUILayout.Width(buttonDimension), GUILayout.Height(buttonDimension))) { quickAmount = 1; }

        amount = quickAmount > -1 ? quickAmount : amount;

        GUILayout.Label((amount * 100).toFixed(0) + '%', GUILayout.Width(33));
    }
    GUILayout.EndHorizontal();

    return amount;
}

