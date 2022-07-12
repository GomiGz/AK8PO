using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TP_MAUI.Models
{
    static class InputValidator
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsNumeric(string text)
        {
            return _regex.IsMatch(text);
        }
    }
}
