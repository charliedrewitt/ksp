using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public void ChangesApplied()
        {
            Changed = false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
