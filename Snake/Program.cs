using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Snake {
    class Program {

        [STAThread]
        static void Main(string[] args) {

            SnakeForm MainForm = new SnakeForm();
            MainForm.ShowDialog();

        }
    }
}
