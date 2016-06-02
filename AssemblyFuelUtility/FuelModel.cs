#region Copyright Notice

// Copyright 2016 Charlie Drewitt

// This file is part of Assembly Fuel Utility.

// Assembly Fuel Utility is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// Assembly Fuel Utility is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with Assembly Fuel Utility.  If not, see<http://www.gnu.org/licenses/>.

#endregion
using CDKspUtil.Logic;
using KSP.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AssemblyFuelUtility
{
    public class FuelModel
    {
        private float _liquidFuel;
        public float LiquidFuel
        {
            get
            {
                return _liquidFuel;
            }
            set
            {
                if (value != _liquidFuel)
                {
                    Changed = true;
                }

                _liquidFuel = value;
            }
        }

        private float _oxidizer;
        public float Oxidizer
        {
            get
            {
                return _oxidizer;
            }
            set
            {
                if (value != _oxidizer)
                {
                    Changed = true;
                }

                _oxidizer = value;
            }
        }

        private float _solid;
        public float SolidFuel
        {
            get
            {
                return _solid;
            }
            set
            {
                if (value != _solid)
                {
                    Changed = true;
                }

                _solid = value;
            }
        }

        private float _monoprop;
        public float Monoprop
        {
            get
            {
                return _monoprop;
            }
            set
            {
                if (value != _monoprop)
                {
                    Changed = true;
                }

                _monoprop = value;
            }
        }

        public bool Changed { get; private set; }

        public void Apply(ShipConstruct ship)
        {
            foreach (var part in ship.parts)
            {
                foreach (var resource in part.Resources.list)
                {
                    switch (resource.resourceName)
                    {
                        case FuelTypes.LiquidFuel:
                            {
                                resource.amount = resource.maxAmount * this.LiquidFuel;
                                break;
                            }
                        case FuelTypes.Oxidizer:
                            {
                                resource.amount = resource.maxAmount * this.Oxidizer;
                                break;
                            }
                        case FuelTypes.SolidFuel:
                            {
                                resource.amount = resource.maxAmount * this.SolidFuel;
                                break;
                            }
                        case FuelTypes.Monopropellant:
                            {
                                resource.amount = resource.maxAmount * this.Monoprop;
                                break;
                            }
                    }
                }
            }

            var resourceEditors = EditorLogic.FindObjectsOfType<UIPartActionResourceEditor>();

            foreach (var ed in resourceEditors)
            {
                ed.resourceAmnt.text = ed.Resource.amount.ToString("F1");
                ed.slider.value = (float)(ed.Resource.amount / ed.Resource.maxAmount);
            }

            GameEvents.onEditorShipModified.Fire(ship);

            Changed = false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
