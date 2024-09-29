using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GreetingService;

namespace WindowsFormsApp
{
    public partial class Greeting : Form
    {
        public Greeting(string username)
        {
            InitializeComponent();
            lblGreeating.Text = new Service().HandleGreeting(username);
        }
    }
}
