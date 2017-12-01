using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormCapture
{
    public class FormElement
    {
        public FormElementType Type { get; set; }
        public object Value { get; set; }
        public string Label { get; set; }
    }

    public enum FormElementType
    {
        SingleLineText,
        Slider,
        MultiLineText
    }
}
