using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FormCapture
{
    public sealed partial class FormPanel : UserControl
    {
        public FormPanel(List<FormElement> list)
        {
            this.InitializeComponent();
            foreach (var element in list)
            {
                switch (element.Type)
                {
                    case FormElementType.MultiLineText:
                        {
                            var component = new TextBox();
                            break;
                        }
                    case FormElementType.SingleLineText:
                        {

                            break;
                        }
                    case FormElementType.Slider:
                        {

                            break;
                        }
                }
            }
        }
    }
}
