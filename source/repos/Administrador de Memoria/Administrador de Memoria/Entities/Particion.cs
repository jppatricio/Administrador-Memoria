using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrador_de_Memoria.Entities
{
    class Particion
    {
        public int num, tam, dir;
        public String proceso, estado; 

        public Particion(int num, int tam, int dir, String proceso, String estado)
        {
            this.num = num;
            this.tam = tam;
            this.dir = dir;
            this.proceso = proceso;
            this.estado = estado;
        }
        
    }
}
