﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrador_de_Memoria.Entities
{
    class Area
    {
        public TareaEntity t = new TareaEntity();
        public int num, tam, dir;
        public String estado;

        public Area(int num, int tam, int dir,  String estado, TareaEntity t)
        {
            this.t = t;
            this.num = num;
            this.tam = tam;
            this.dir = dir;
            this.estado = estado;
        }
        public Area() { }
    }
}
