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
        private int numPart = 1, numArea = 1, row = 1, encontrado = 0;
        

        public ControlTablas()
        { 
        }

        public void AgregarElementoTabPart(TableLayoutPanel t)//------------FALTA POR HACER
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
        }//________________________________________________________________________----------------------------

        public void AgregarElementoTabAreas(TableLayoutPanel t, TareaEntity tarea)
        {
            if (filaLibreAreas.Count != 0 && encontrado == 0)
            {
                // Revisa si hay espacios disponibles si no, inserta uno nuevo
                Area prueba = new Area();
                foreach (int x in filaLibreAreas)
                {
                    prueba = null;
                    for (var node = llArea.First; node != null; node = node.Next)
                    {
                        if (node.Value.num == x)
                        {
                            prueba = node.Value;
                            break;
                        }
                    }

                    if (prueba.tam == tarea.GetTamañoTarea())
                    {
                        llArea.AddAfter(llArea.Find(prueba), new LinkedListNode<Area>(new Area(x, tarea.GetTamañoTarea(), prueba.dir, "A")));
                        llArea.Remove(prueba);
                        ActualizarTabla(t);
                        filaLibreAreas.Remove(x);
                        encontrado = 0;
                        break;
                    }
                    else if (prueba.tam > tarea.GetTamañoTarea())
                    {
                        LinkedListNode<Area> p = llArea.Find(prueba);
                        llArea.AddBefore(p, new LinkedListNode<Area>(new Area(x, tarea.GetTamañoTarea(), prueba.dir, "A")));
                        p.Value.dir = p.Previous.Value.tam + prueba.dir;
                        p.Value.tam = prueba.tam - p.Previous.Value.tam;
                        ActualizarTabla(t);
                        filaLibreAreas.Remove(x);
                        encontrado = 0;
                        filaLibreAreas.Add(x + 1);
                        numArea++;
                        break;
                    }
                    else
                    {
                        encontrado = 1;
                        AgregarElementoTabAreas(t, tarea);
                    }
                }


            }
            else if (llArea.First == null)
            {
                llArea.AddFirst(new Area(numArea, tarea.GetTamañoTarea(), 0, "A"));
                numArea++;
                AñadirUltimoATabla(t);
                DejarVacioElementoTabAreas(t, 0);
            } 
            else
            {
                llArea.AddLast(new Area(numArea,tarea.GetTamañoTarea(),llArea.Last.Value.dir + llArea.Last.Value.tam,"A"));
                numArea++;
                AñadirUltimoATabla(t);
            }
            encontrado = 0;

        }

        public void DejarVacioElementoTabAreas(TableLayoutPanel t, int dirInicio)
        {
            //Columna 2 esta la dir inicio
            Area prueba = new Area();
            prueba = null;
            for (var node = llArea.First; node != null; node = node.Next)
            {
                if (node.Value.dir == dirInicio)
                {
                    prueba = node.Value;
                    break;
                }
            }
            if (prueba == null)
            {
                Console.WriteLine("No se encontró");
            }
            else
            {
                filaLibreAreas.Add(prueba.num);
                llArea.Find(prueba).Value.estado = "V";
                LinkedList<Area> llAreaTemp = new LinkedList<Area>();
                for (var node = llArea.First; node != null; node = node.Next)
                {
                    llArea.Remove(node);
                    llAreaTemp.AddLast(node);
                }
                    foreach (Area a in llAreaTemp)
                {
                    llArea.AddLast(a);
                }

                ActualizarTabla(t);
            }
        }

        private void ActualizarTabla(TableLayoutPanel t)
        {
            row = 1;
            for(int c = 0; c <= 3; c++)
            {
                for(int ro = 1; ro <= 20; ro++)
                    t.Controls.Remove(t.GetControlFromPosition(c, ro));
            }
            LinkedList<Area> llAreaTemp = new LinkedList<Area>();
            int i = 1;
            while(llArea.First != null)
            {
                var node = llArea.First;
                node.Value.num = i;
                llArea.Remove(llArea.First);
                llAreaTemp.AddLast(node);
                i++;
            }
            while (llAreaTemp.First != null)
            {
                var node = llAreaTemp.First;
                llAreaTemp.Remove(llAreaTemp.First);
                llArea.AddLast(node);
                AñadirUltimoATabla(t);
            }
            //for (var node = llAreaTemp.First; node != null; node = node.Next)
            //{
            //    llAreaTemp.Remove(node);
            //    llArea.AddLast(node);
            //    AñadirUltimoATabla(t);
            //}
        }

        private void AñadirUltimoATabla(TableLayoutPanel t)
        {
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
    }

}
