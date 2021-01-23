using System;
using System.Collections.Generic;
using System.Text;

namespace SproutClass
{
    public class HardToInstanciateInATestException : Exception
    {
        public HardToInstanciateInATestException(): base("Cannot be instanciated in unittest")
        {
        }
    }
}
