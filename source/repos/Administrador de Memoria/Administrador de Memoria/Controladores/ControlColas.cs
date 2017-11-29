using Administrador_de_Memoria.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Administrador_de_Memoria.Controladores
{
    class ControlColas
    {
        public Queue<ColaElement> queue1 = new Queue<ColaElement>();//baja prioridad
        public Queue<ColaElement> queue2 = new Queue<ColaElement>();//alta prioridad
        public int row1=0, row2 = 0;
        public ControlColas()
        {
            queue1.Clear();
            queue2.Clear();
        }

        public void Enqueue1 (TableLayoutPanel q1, ColaElement ce)
        {
            queue1.Enqueue(ce);
            AñadirUltimoATabla1(q1, ce);
        }
        public ColaElement Dequeue1(TableLayoutPanel q1)
        {
            if(queue1.Count != 0)
            {
                ColaElement q = queue1.Dequeue();
                ActualizarTabla(q1);
                return q;
            }
            return null;
        }
        private void AñadirUltimoATabla1(TableLayoutPanel t, ColaElement ce)
        {
            Random r = new Random();
            Color c = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
            Label numlb = new Label();
            numlb.Text = ce.te.GetNombreTarea();
            numlb.Visible = true;
            numlb.TextAlign = ContentAlignment.MiddleCenter;
            numlb.BackColor = c;
            t.Controls.Add(numlb);
            t.SetCellPosition(numlb, new TableLayoutPanelCellPosition(0, row1));
            row1++;
        }
        private void ActualizarTabla(TableLayoutPanel q1)
        {
            row1 = 0;
            for (int ro = 0; ro <= queue1.Count; ro++)
                q1.Controls.Remove(q1.GetControlFromPosition(0, ro));
            Queue<ColaElement> queue1Temp = new Queue<ColaElement>();
            while (queue1.Count != 0)
            {
                queue1Temp.Enqueue(queue1.Dequeue());
            }
            while (queue1Temp.Count != 0)
            {
                Enqueue1(q1, queue1Temp.Dequeue());
            }
        }
    }
}
