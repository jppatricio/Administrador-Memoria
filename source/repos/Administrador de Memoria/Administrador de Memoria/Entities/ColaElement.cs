using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrador_de_Memoria.Entities
{
    class ColaElement
    {
        public TareaEntity te = new TareaEntity();

        public ColaElement()
        {
        }

        public ColaElement(TareaEntity te)
        {
            this.te = te;
        }
    }
}
