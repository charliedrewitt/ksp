using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleJetPilot
{
    public class SimpleJetPilot : PartModule
    {
        public override void OnStart(StartState state)
        {
            if (state != StartState.Editor)
            {

            }

            base.OnStart(state);
        }
    }
}
