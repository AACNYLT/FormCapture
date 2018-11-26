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

namespace FormCapture.Controls
{
    public sealed partial class PodioConfigDialog : UserControl
    {
        public event FieldMappingCompleteHandler FieldMappingComplete;
        public delegate void FieldMappingCompleteHandler(string FirstNameFieldName, string LastNameFieldName);

        public PodioConfigDialog()
        {
            this.InitializeComponent();
        }

        private void Continue(object sender, RoutedEventArgs e)
        {
            FieldMappingComplete(FirstNameField.Text, LastNameField.Text);
        }
    }
}
