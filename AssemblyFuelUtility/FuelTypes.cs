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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyFuelUtility
{
    public class FuelTypes
    {
        public const string SolidFuel = "SolidFuel";
        public const string LiquidFuel = "LiquidFuel";
        public const string Oxidizer = "Oxidizer";
        public const string Monopropellant = "MonoPropellant";

        public static string[] All = new string[] { LiquidFuel, Oxidizer, SolidFuel, Monopropellant };
    }
}
