using Administrador_de_Memoria.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Administrador_de_Memoria
{
    class ControlTablas
    {
        private Particion p;
        private LinkedList<Particion> llPart = new LinkedList<Particion>();
        private LinkedList<Area> llArea = new LinkedList<Area>();
        private List<int> filaLibreAreas = new List<int>();
        private int numPart = 1, numArea = 1, row = 1;
        

        public ControlTablas()
        {
        }

        public void AgregarElementoTabPart(TableLayoutPanel t)
        {
            p.num = numPart;
            numPart++;
            llPart.AddLast(p);
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                Label ll = new Label();
                ll.Text = "hola " + i;
                ll.Visible = true;
                ll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                ll.BackColor = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                t.Controls.Add(ll);
                t.SetCellPosition(ll, new TableLayoutPanelCellPosition(0, row: i + 1));
            }
        }

        public void AgregarElementoTabAreas(TableLayoutPanel t, TareaEntity tarea)
        {
            if(filaLibreAreas.Count != 0)
            {
                // Revisa si hay espacios disponibles si no, inserta uno nuevo
                Area prueba = new Area();
                foreach (int x in filaLibreAreas)
                {
                    prueba.num = x;
                    prueba = llArea.Find(prueba).Value;
                    if(prueba.tam == tarea.GetTamañoTarea())
                    {
                        llArea.AddAfter(llArea.Find(prueba), new LinkedListNode<Area> (new Area(x, tarea.GetTamañoTarea(), prueba.dir, "A")));
                        llArea.Remove(prueba);
                    }
                    else if(prueba.tam > tarea.GetTamañoTarea())
                    {
                        // Falta!!!!---------------vvvv
                        llArea.AddBefore(llArea.Find(prueba), new LinkedListNode<Area>(new Area(x, tarea.GetTamañoTarea(), prueba.dir, "A")));
                        llArea.AddAfter(llArea.Find(prueba), new LinkedListNode<Area>(new Area(x, tarea.GetTamañoTarea(), prueba.dir, "A")));
                    }
                }
                
            }
            else if (llArea.First == null)
            {
                llArea.AddFirst(new Area(numArea, tarea.GetTamañoTarea(), 0, "A"));
                numArea++;
            } 
            else
            {
                llArea.AddLast(new Area(numArea,tarea.GetTamañoTarea(),llArea.Last.Value.dir + llArea.Last.Value.tam,"A"));
                numArea++;
            }

            Random r = new Random();
            
            Area a = new Area();
            a = llArea.Last.Value;

            Color c = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
            Label numlb = new Label();
            numlb.Text = a.num.ToString();
            numlb.Visible = true;
            numlb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            numlb.BackColor = c;
            t.Controls.Add(numlb);
            t.SetCellPosition(numlb, new TableLayoutPanelCellPosition(0, row));

            Label tamlb = new Label();
            tamlb.Text = a.tam.ToString();
            tamlb.Visible = true;
            tamlb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            tamlb.BackColor = c;
            t.Controls.Add(tamlb);
            t.SetCellPosition(tamlb, new TableLayoutPanelCellPosition(1, row));

            Label dirlb = new Label();
            dirlb.Text = a.dir.ToString();
            dirlb.Visible = true;
            dirlb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            dirlb.BackColor = c;
            t.Controls.Add(dirlb);
            t.SetCellPosition(dirlb, new TableLayoutPanelCellPosition(2, row));

            Label estlb = new Label();
            estlb.Text = a.estado;
            estlb.Visible = true;
            estlb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            estlb.BackColor = c;
            t.Controls.Add(estlb);
            t.SetCellPosition(estlb, new TableLayoutPanelCellPosition(3, row));
            row++;
        }

        public void DejarVacioElementoTabAreas(TableLayoutPanel t, int dirInicio)
        {
            //Columna 2 esta la dir inicio
            Area prueba = new Area();
            prueba.dir = dirInicio;
            prueba = llArea.Find(prueba).Value;
            if(prueba == null)
            {
                Console.WriteLine("No se encontró");
            }
            else
            {
                filaLibreAreas.Add(prueba.num);
            }
        }
    }

}
