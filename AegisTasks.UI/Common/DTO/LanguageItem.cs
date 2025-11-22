using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AegisTasks.Core.Common;

namespace AegisTasks.UI.Common
{
    using LanguageEnum = Core.Common.SupportedLanguage;

    public class LanguageItem
    {
        public string Text { get; }
        public LanguageEnum Value { get; }

        public LanguageItem(LanguageEnum value, string text)
        {
            Value = value;
            Text = text;
        }

        public override string ToString()
        {
            return Text; // Esto es lo que muestra el ComboBox
        }
    }
}
