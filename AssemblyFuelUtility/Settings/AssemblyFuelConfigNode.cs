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
                string stored = _node.GetValue("FuelModel");

                if (!String.IsNullOrEmpty(stored))
                {
                    string[] vals = stored.Split(':');

                    return new FuelModel
                    {
                        LiquidFuel = float.Parse(vals[0]),
                        Oxidizer = float.Parse(vals[1]),
                        Monoprop = float.Parse(vals[2]),
                    };
                }

                return new FuelModel();
            }
            set
            {
                string serialized = String.Format("{0}:{1}:{2}", value.LiquidFuel, value.Oxidizer, value.Monoprop);

                _node.SetValue("FuelModel", serialized, true);
            }
        }

        #endregion

    }
}
