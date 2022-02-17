using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonSnappableTypes;

namespace CSharpSnapIn
{
    [CompanyInfo(CompanyName = "Greed corp", CompanyUrl = "http://Greed.corp.org")]
    public class CsharpModule : IAppFunctionality
    {
        void IAppFunctionality.DoIt()
        {
            Console.WriteLine("You have just used the C# snap-in");
        }
    }
}
