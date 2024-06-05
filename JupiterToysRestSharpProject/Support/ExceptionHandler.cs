using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterToysRestSharpProject.Support
{
    public class ExceptionHandler
    {
        public static void CheckNullArgument(List<dynamic> arguments){
            arguments.ToList().ForEach(arg => ArgumentNullException.ThrowIfNull(arg));
        }
    }
}
