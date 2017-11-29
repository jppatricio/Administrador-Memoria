using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administrador_de_Memoria.Entities
{
    class TareaEntity
    {
        private String nombreTarea;
        private int tamañoTarea, prioridad = 0, tiempo = 0;
        public TareaEntity(String nombreTarea, int tamañoTarea)
        {
            Random r = new Random();
            this.tiempo = r.Next(1, 6);
            this.nombreTarea = nombreTarea;
            this.tamañoTarea = tamañoTarea;
        }

        public TareaEntity()
        {
        }

        public String GetNombreTarea() => this.nombreTarea;
        public void SetNombreTarea(String nombreTarea) => this.nombreTarea = nombreTarea;

        public int GetTamañoTarea() => this.tamañoTarea;
        public void SetTamañoTarea(int tamañoTarea) => this.tamañoTarea = tamañoTarea;

        public int GetPrioridad() => this.prioridad;
        public void SetPrioridad(int prioridad) => this.prioridad = prioridad;
    }
}
