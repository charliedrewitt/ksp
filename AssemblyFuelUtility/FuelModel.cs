using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyFuelUtility
{
    public class FuelModel
    {
        public float LiquidFuel { get; set; }
        public float Oxidizer { get; set; }
        public float Monoprop { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is FuelModel)
            {
                var otherModel = (FuelModel)obj;

                return this.LiquidFuel == otherModel.LiquidFuel &&
                        this.Oxidizer == otherModel.Oxidizer &&
                        this.Monoprop == otherModel.Monoprop;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
