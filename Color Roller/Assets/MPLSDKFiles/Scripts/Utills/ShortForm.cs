using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MPL.utility.shortForm
{
    public static class ShortForm
    {
       
        static public string GetFormattedString(int value)
        {
            System.Globalization.CultureInfo us = new System.Globalization.CultureInfo("en-IN");
            return value.ToString("N0", us);
        }
        static public string GetFormattedString(long value)
        {
            System.Globalization.CultureInfo us = new System.Globalization.CultureInfo("en-IN");
            return value.ToString("N0", us);
        }

        static public string GetFormattedString(double value)
        {
            System.Globalization.CultureInfo us = new System.Globalization.CultureInfo("en-IN");
            return value.ToString("N0", us);
        }
        static public string GetFormattedString(string value)
        {
            int _parsedNumber;
            bool isParsed = int.TryParse(value, out _parsedNumber);
            if(isParsed)
            {
                System.Globalization.CultureInfo us = new System.Globalization.CultureInfo("en-IN");
                return _parsedNumber.ToString("N0", us);
            }
            else
            {
                return value;
            }
        }
        
    }
}
