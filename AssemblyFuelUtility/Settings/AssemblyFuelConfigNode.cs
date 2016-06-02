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
using CDKspUtil.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AssemblyFuelUtility.Settings
{
    public class AssemblyFuelConfigNode
    {
        #region Static

        public static AssemblyFuelConfigNode LoadOrCreate(string fileFullPath)
        {
            var internalNode = ConfigNode.Load(fileFullPath);

            if (internalNode == null)
            {
                internalNode = new ConfigNode();

                return new AssemblyFuelConfigNode(internalNode)
                {
                    WindowX = 100,
                    WindowY = 100
                };
            }

            return new AssemblyFuelConfigNode(internalNode);
        }

        #endregion

        private ConfigNode _node;
        public AssemblyFuelConfigNode(ConfigNode node)
        {
            _node = node;
        }

        public void Save(string fileFullPath)
        {
            _node.Save(fileFullPath);
        }

        #region Window Position

        public float WindowX
        {
            get
            {
                string stored = _node.GetValue("WindowX");

                var val = new Parseable<float>(stored);

                return val.Success ? val.Value : 0;
            }
            set
            {
                _node.SetValue("WindowX", value.ToString(), true);
            }
        }

        public float WindowY
        {
            get
            {
                string stored = _node.GetValue("WindowY");

                var val = new Parseable<float>(stored);

                return val.Success ? val.Value : 0;
            }
            set
            {
                _node.SetValue("WindowY", value.ToString(), true);
            }
        }

        #endregion

        #region FuelModel

        public FuelModel FuelModel
        {
            get
            {
                try
                {
                    string stored = _node.GetValue("FuelModel");

                    if (!String.IsNullOrEmpty(stored))
                    {
                        string[] vals = stored.Split(':');

                        return new FuelModel
                        {
                            LiquidFuel = float.Parse(vals[0]),
                            Oxidizer = float.Parse(vals[1]),
                            SolidFuel = float.Parse(vals[2]),
                            Monoprop = float.Parse(vals[3]),
                        };
                    }
                }
                catch(Exception ex)
                {
                    LogHelper.Error("Unable to load FuelModel from settings.\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                }

                return new FuelModel();
            }
            set
            {
                string serialized = String.Format("{0}:{1}:{2}:{3}", value.LiquidFuel, value.Oxidizer, value.SolidFuel, value.Monoprop);

                _node.SetValue("FuelModel", serialized, true);
            }
        }

        #endregion

    }
}
