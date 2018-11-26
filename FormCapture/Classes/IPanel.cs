using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FormCapture.Classes.Enumerations;

namespace FormCapture.Classes
{
    interface IPanel
    {
        void Save(SaveFormOptions saveBehavior);
    }
}
