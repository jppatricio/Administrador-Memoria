using System;
using Administrador_de_Memoria.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Administrador_de_Memoria.Controladores;

namespace Administrador_de_Memoria
{
    public partial class Form1 : Form
    {
        private ControlTablas ctrl = new ControlTablas();
        private ControlColas ctrlqueue = new ControlColas();
        private ColaElement colaElement = new ColaElement();
        public Boolean stop = false, freeQueue = true;
        public int timeInt = 0, i = 1, clickedStop = 1, secInt = 0;
        public int min = 0, seg = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void AutomaticoBTN_Click(object sender, EventArgs e)
        {

            if (stop == false)
            {
                if (tareasTB.Text == "")
                    tareasTB.Text = "10";
                tareasTB.ReadOnly = true;
                timer1.Start();
                timer2.Start();
                button3.Visible = true;
                AutomaticoBTN.Visible = false;
            }
            if (stop == true)
            {
                //timeInt++;
                AddTime();
                int numeroUsuarios = Int32.Parse(tareasTB.Text);

                Random r = new Random();

                TareaEntity t = new TareaEntity("T" + i, r.Next(1,250));
                i++;
                ColaElement c = ctrl.AgregarElementoTabAreas(this.TablaA, t);
                if(c.te.GetPrioridad() != 0)
                {
                    freeQueue = false;
                    ctrlqueue.Enqueue1(this.Cola1tbl, c);
                }//--------------------------------------

                AddTime();
                colaElement = ctrlqueue.Dequeue1(this.Cola1tbl);
                if (colaElement == null)
                {
                    freeQueue = true;
                }
                else
                {
                    ColaElement ce = ctrl.AgregarElementoTabAreas(this.TablaA, colaElement.te);
                    if (ce.te.GetPrioridad() != 0)
                    {
                        ctrlqueue.Enqueue1(this.Cola1tbl, ce);
                    }
                }

                Area ar = ctrl.ObtenerTarea(this.TablaA);
                if (ar != null)
                {
                    ctrl.AgregarElementoTabPart(this.TablaP, ar);// obtiene tarea y la mete a tabPart
                    ctrl.AgregarAMemoria(this.splitter1, ar.t, ar.dir);
                }

                List<TareaEntity> listTareasTerminadas = ctrl.QuitarDeMemoria(this.splitter1);//Actualiza Memoria
                ctrl.DejarVacioElementoTabPart(TablaP, listTareasTerminadas);
                ctrl.ModAreaPart(this.TablaA);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (clickedStop == 1)
            {
                stop = true;
                clickedStop = 0;
            }
            else
            {
                stop = false;
                button3.Text = "DETENER";
                AutomaticoBTN.Visible = false;
                timer1.Start();
                timer2.Start();
                clickedStop = 1;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (stop == true)
            {
                timer1.Stop();// Detiene el programa
                timer2.Stop();
                button3.Text = "Automático detenido";
                AutomaticoBTN.Visible = true;
                AutomaticoBTN.Text = "Añadir 1 nueva tarea (+1 seg)";
            }
            else
            {
                button3.Text = "DETENER";
                AutomaticoBTN.Visible = false;
                int numeroUsuarios = Int32.Parse(tareasTB.Text);

                Random r = new Random();

                TareaEntity t = new TareaEntity("T" + i, r.Next(1,250));
                i++;
                if(freeQueue == true)
                {
                    ColaElement c = ctrl.AgregarElementoTabAreas(this.TablaA, t);
                    if (c.te.GetPrioridad() != 0)
                    {
                        freeQueue = false;
                        ctrlqueue.Enqueue1(this.Cola1tbl, c);
                    }
                }
                else
                {
                    ColaElement c = new ColaElement(t);
                    ctrlqueue.Enqueue1(this.Cola1tbl, c);
                }

                ctrl.ModAreaPart(this.TablaA);
                //timeInt++;
            }
        }
        private void timer2_Tick(object sender, EventArgs e)// Este thread se encarga de obtener tareas de las colas e intentar insertarlas en Tabla Areas
                                                            // Al igual que atender las que están en tablas
        {
            AddTime();
            colaElement = ctrlqueue.Dequeue1(this.Cola1tbl);
            if (colaElement == null)
            {
                freeQueue = true;
            }
            else
            {
                ColaElement c = ctrl.AgregarElementoTabAreas(this.TablaA, colaElement.te);
                if (c.te.GetPrioridad() != 0)
                {
                    ctrlqueue.Enqueue1(this.Cola1tbl, c);
                }
            }

            Area ar = ctrl.ObtenerTarea(this.TablaA);
            if (ar != null)
            {
                ctrl.AgregarElementoTabPart(this.TablaP, ar);// obtiene tarea y la mete a tabPart
                ctrl.AgregarAMemoria(this.splitter1, ar.t, ar.dir);
            }

            List<TareaEntity> listTE = ctrl.QuitarDeMemoria(this.splitter1);//Actualiza Memoria
            ctrl.DejarVacioElementoTabPart(TablaP,listTE);
            //ctrl.ModAreaPart(this.TablaA);
        }

        private void AddTime()//para medir tiempo transcurrido
        {
            if (seg + 1 == 60)
            {
                min++;
                seg = 0;
            }
            else
                seg++;
            if (seg < 10)
                timeLB.Text = "0" + min + ":" + "0" + seg;
            else
                timeLB.Text = "0" + min + ":" + seg;
        }

    }
}
