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


namespace Administrador_de_Memoria
{
    public partial class Form1 : Form
    {
        private ControlTablas ctrl = new ControlTablas();
        public Boolean stop = false;
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
            
            
            if (tareasTB.Text == "")
                tareasTB.Text = "10";
            int numeroUsuarios = Int32.Parse(tareasTB.Text);
            
            Random r = new Random();
            int i = 1;
            //do
            //{
                //DelayAsync();
                TareaEntity t = new TareaEntity("T1", r.Next(250));
                i++;
                ctrl.AgregarElementoTabAreas(this.TablaA, t);
                
            //} while (!stop);
            //Particion p = new Particion();
            //ctrl.AgregarElementoTabPart(p, TablaP);

           



        }
        private async Task DelayAsync()
        {
            await Task.Delay(3000);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            stop = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
