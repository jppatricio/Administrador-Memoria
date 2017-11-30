using Administrador_de_Memoria.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using Administrador_de_Memoria.Controladores;

namespace Administrador_de_Memoria
{
    class ControlTablas
    {
        private LinkedList<Particion> llPart = new LinkedList<Particion>();
        private LinkedList<Area> llArea = new LinkedList<Area>();
        private List<int> filaLibreAreas = new List<int>();
        private ControlColas cc = new ControlColas();
        private int numPart = 1, numArea = 1, row = 1, rowPart = 1, encontrado = 0, espacioDisponible = 2500, segundaPas = 0;
        private int memPart = 45;//45px


        public ControlTablas()
        {
        }
        public Area ObtenerTarea(TableLayoutPanel t)
        {
            if (llArea.Count != 0)
            {
                for (var node = llArea.First; node != null; node = node.Next)
                {
                    if (node.Value.t.GetPrioridad() >= 0)
                    {
                        node.Value.t.SetPrioridad(-1);
                        return (node.Value);
                    }
                }
            }
            return null;
        }
        public void AgregarElementoTabPart(TableLayoutPanel t, Area a)
        {
            Particion p = new Particion();
            p.dir = a.dir;
            p.estado = "A";
            p.num = numPart;
            p.proceso = a.t.GetNombreTarea();
            p.tam = a.tam;
            llPart.AddLast(p);
            numPart++;
            AñadirUltimoATablaParticion(t);
        }//________________________________________________________________________----------------------------

        public ColaElement AgregarElementoTabAreas(TableLayoutPanel t, TareaEntity tarea)
        {
            //foreach (Particion a in llPart)
            //{
            //    Console.WriteLine(a.num + "|" + a.tam + "|" + a.dir + "|" + a.proceso + "|" + a.estado);
            //}
            //Console.WriteLine("\n----------------------------------\n");
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
                        llArea.AddAfter(llArea.Find(prueba), new LinkedListNode<Area>(new Area(x, tarea.GetTamañoTarea(), prueba.dir, "A", tarea)));
                        llArea.Remove(prueba);
                        ActualizarTabla(t);
                        filaLibreAreas.Remove(x);
                        encontrado = 0;
                        break;
                    }
                    else if (prueba.tam > tarea.GetTamañoTarea())
                    {
                        LinkedListNode<Area> p = llArea.Find(prueba);
                        llArea.AddBefore(p, new LinkedListNode<Area>(new Area(x, tarea.GetTamañoTarea(), prueba.dir, "A", tarea)));
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
                llArea.AddFirst(new Area(numArea, tarea.GetTamañoTarea(), 0, "A", tarea));
                numArea++;
                AñadirUltimoATabla(t);
                //DejarVacioElementoTabAreas(t, 0);  PARA PRUEBA UNITARIA
            }
            else
            {
                if (llArea.Last.Value.dir + llArea.Last.Value.tam + tarea.GetTamañoTarea() > espacioDisponible)
                {
                    tarea.SetPrioridad(tarea.GetPrioridad() + 1);
                    return new ColaElement(tarea);
                }
                llArea.AddLast(new Area(numArea, tarea.GetTamañoTarea(), llArea.Last.Value.dir + llArea.Last.Value.tam, "A", tarea));
                numArea++;
                AñadirUltimoATabla(t);
            }
            encontrado = 0;
            tarea.SetPrioridad(0);// tarea fue añadida a Tabla de Areas Disp.
            return new ColaElement(tarea);
        }

        public void ModAreaPart(TableLayoutPanel t)
        {
            int axelesgay = 0;
            List<Area> arealist = new List<Area>();
            foreach(Particion p in llPart)
            {
                if(p.estado == "V")
                {
                    foreach (Area a2 in llArea)
                    {
                        if (a2.dir >= p.dir && a2.dir <= p.tam + p.dir)
                            arealist.Add(a2);
                    }
                    foreach (Area a2 in arealist)
                    {
                        Console.WriteLine(llArea.Remove(a2));
                        llArea.Remove(a2);
                    }
                }
                //if (p.estado == "V")
                //    foreach(Area a in llArea)
                //    {
                //        if (a.t.GetNombreTarea() == p.proceso)
                //        {
                //            a.t.SetTamañoTarea(p.tam);
                //            a.tam = p.tam;
                //            axelesgay = 1;
                //            foreach (Area a2 in llArea)
                //            {
                //                if (a2 != a)
                //                {
                //                    if (a2.dir > p.dir && a2.dir < p.tam + p.dir)
                //                        arealist.Add(a2);
                //                }
                //            }
                //            foreach (Area a2 in arealist)
                //            {
                //                Console.WriteLine(llArea.Remove(a2));
                //                llArea.Remove(a2);
                //            }

                //            break;
                //        }
                //    }
                if (axelesgay == 1)
                    DejarVacioElementoTabAreas(t, p.dir);
                axelesgay = 0;
            }
        }

        public void DejarVacioElementoTabAreas(TableLayoutPanel t, int dirInicio)
        {

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

        public void DejarVacioElementoTabPart(TableLayoutPanel t,List<TareaEntity> listaTareasTerminadas)//------------------------------------------------------------|||||||||||||
        {
            foreach(TareaEntity tarea in listaTareasTerminadas)
            {
                Particion prueba = new Particion();
                prueba = null;
                for (var node = llPart.First; node != null; node = node.Next)
                {
                    if (node.Value.proceso == tarea.GetNombreTarea())
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
                    llPart.Find(prueba).Value.estado = "V";
                    ActualizarTablaPart(t);
                }
            }
        }

        private void ActualizarTabla(TableLayoutPanel t)
        {
            row = 1;
            for (int c = 0; c <= 3; c++)
            {
                for (int ro = 1; ro <= 20; ro++)
                    t.Controls.Remove(t.GetControlFromPosition(c, ro));
            }
            LinkedList<Area> llAreaTemp = new LinkedList<Area>();
            int i = 1;
            while (llArea.First != null)
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

        private void AñadirUltimoATablaParticion(TableLayoutPanel t)
        {
            Particion a = new Particion();
            a = llPart.Last.Value;
            Random r = new Random();
            Color c = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
            Label numlb = new Label();
            numlb.Text = a.num.ToString();
            numlb.Visible = true;
            numlb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            numlb.BackColor = c;
            t.Controls.Add(numlb);
            t.SetCellPosition(numlb, new TableLayoutPanelCellPosition(0, rowPart));

            Label tamlb = new Label();
            tamlb.Text = a.tam.ToString();
            tamlb.Visible = true;
            tamlb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            tamlb.BackColor = c;
            t.Controls.Add(tamlb);
            t.SetCellPosition(tamlb, new TableLayoutPanelCellPosition(1, rowPart));

            Label dirlb = new Label();
            dirlb.Text = a.dir.ToString();
            dirlb.Visible = true;
            dirlb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            dirlb.BackColor = c;
            t.Controls.Add(dirlb);
            t.SetCellPosition(dirlb, new TableLayoutPanelCellPosition(2, rowPart));

            Label prolb = new Label();
            prolb.Text = a.proceso;
            prolb.Visible = true;
            prolb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            prolb.BackColor = c;
            t.Controls.Add(prolb);
            t.SetCellPosition(prolb, new TableLayoutPanelCellPosition(3, rowPart));

            Label estlb = new Label();
            estlb.Text = a.estado;
            estlb.Visible = true;
            estlb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            estlb.BackColor = c;
            t.Controls.Add(estlb);
            t.SetCellPosition(estlb, new TableLayoutPanelCellPosition(4, rowPart));
            rowPart++;
        }
        public void AgregarAMemoria(Splitter splitter1, TareaEntity t, int dir)
        {
            Random r = new Random();
            Label estlb = new Label();
            Color c = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
            estlb.Text = t.GetNombreTarea();
            estlb.Visible = true;
            estlb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            estlb.BackColor = c;
            splitter1.Controls.Add(estlb);
            t.SetYPos(dir/4);
            estlb.SetBounds(0, t.GetYPos(), 307, t.GetTamañoTarea()/4);
        }
        public List<TareaEntity> QuitarDeMemoria(Splitter sp1)
        {
            List<TareaEntity> tareasTerminadas = new List<TareaEntity>();
            foreach (Area par in llArea)
            {
                TareaEntity t = par.t;
                if (t.GetTiempo() <= 0)
                {
                    foreach (Control c in sp1.Controls)
                    {
                        if (c.Text == t.GetNombreTarea())
                        {
                            sp1.Controls.Remove(c);
                            tareasTerminadas.Add(t);
                        }
                    }
                }
                else
                    t.SetTiempo(t.GetTiempo() - 1);
            }
            return tareasTerminadas;

        }

        public void ActualizarTablaMemoria(Splitter sp1)
        {
            memPart = 45;
            foreach (Control c in sp1.Controls)
            {
                if (c is Label)
                {
                    c.SetBounds(0, c.Location.Y, 307, c.Size.Height);
                    memPart = memPart + c.Size.Height;
                }
            }
        }
        private void ActualizarTablaPart(TableLayoutPanel t)
        {
            rowPart = 1;
            for (int c = 0; c <= 4; c++)
            {
                for (int ro = 1; ro <= llPart.Count + 2; ro++)
                    t.Controls.Remove(t.GetControlFromPosition(c, ro));
            }
            //llPart.OrderBy(p => p.dir);
            for (var node = llPart.First; node != null; node = node.Next) // hay que acumular espacios vacios que estén juntos
            {
                if (node.Value.estado == "V" && node.Next != null && node.Next.Value.estado == "V")
                {
                    node.Value.proceso = "NADA";
                    if (node == llPart.First)
                    {
                        node.Value.tam = node.Value.tam + llPart.First.Next.Value.tam;
                        llPart.Remove(llPart.First.Next);
                    }
                    else
                    {
                        node.Value.tam = node.Value.tam + node.Next.Value.tam;
                        llPart.Remove(node.Next);
                        //Console.WriteLine(node.Next.Value.proceso + " | " + node.Next.Value.dir);
                    }
                }
            }
            foreach (Particion a in llPart)
            {
                Console.WriteLine(a.num + "|" + a.tam + "|" + a.dir + "|" + a.proceso + "|" + a.estado);
            }
            Console.WriteLine("\n----------------------------------\n");
            LinkedList<Particion> llPartTemp = new LinkedList<Particion>();
            int i = 1;
            while (llPart.First != null)
            {
                var node = llPart.First;
                node.Value.num = i;
                llPart.Remove(llPart.First);
                llPartTemp.AddLast(node);
                i++;
            }
            while (llPartTemp.First != null)
            {
                var node = llPartTemp.First;
                llPartTemp.Remove(llPartTemp.First);
                llPart.AddLast(node);
                AñadirUltimoATablaParticion(t);
            }
        }
    }
}
