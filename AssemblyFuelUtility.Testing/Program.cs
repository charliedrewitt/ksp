using Jint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AssemblyFuelUtility.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            TestJavascriptExecution();
        }

        private static void TestJavascriptExecution()
        {
            string source = File.ReadAllText(@"..\..\..\AssemblyFuelUtility\GameData\AssemblyFuelUtility\Scripts\\RenderFuelControl.js");

            var engine = new Engine(cfg => cfg.AllowClr().AllowClr(typeof(GUILayout).Assembly)).Execute(source);

            engine.Execute("renderTestAmountLabel(5);");
        }
    }
}
