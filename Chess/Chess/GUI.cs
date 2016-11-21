using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logic;
using Entities;

namespace Chess
{
    public partial class GUI : Form
    {
        private GameMoveEntity gameMove;
        private GameLogic gameLogic;
        public GUI(GameLogic logic)
        {
            gameLogic = logic;
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.WriteLine("ABOUT");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("BUTTON1");
        }

        private void label16_Click(object sender, EventArgs e)
        {
            Console.WriteLine("LABEL16");
        }

        private void label17_Click(object sender, EventArgs e)
        {
            Console.WriteLine("LABEL17");
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }
    }
}
