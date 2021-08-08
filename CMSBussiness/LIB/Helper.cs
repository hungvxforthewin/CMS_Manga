using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CRMBussiness.LIB
{
    public class Helper
    {
        public static string GenerateNextCode(string maximumCode)
        {
            string prefix = Regex.Match(maximumCode, @"[A-Z]+").Value;
            int postfix = int.Parse(Regex.Match(maximumCode, @"[^A-Z]+").Value);
            string nextCode = prefix;
            for (int i = 3; i >= 0; i--)
            {
                if (postfix+1 >= Math.Pow(10, i))
                {
                    string zeroString = "";
                    for (int j = 0; j < 3 - i; j++)
                    {
                        zeroString += "0";
                    }
                    nextCode += zeroString + (postfix + 1);
                    break;
                }
            }
            return nextCode;
        }
    }
}
