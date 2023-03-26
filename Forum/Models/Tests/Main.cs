using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Forum.Models.Tests
{
    internal sealed class Main
    {
        internal static void Start()
        {
            ControllerTest.ThreadTest();
            ControllerTest.SectionTest();
            ControllerTest.EndPointTest();
        }
    }
}