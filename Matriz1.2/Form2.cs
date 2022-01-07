using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Matriz1._2
{
    public partial class frm_pap : MetroFramework.Forms.MetroForm
    {
        public frm_pap()
        {
            InitializeComponent();
        }

        private void frm_pap_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.UserClosing:
                    e.Cancel = true;
                    this.Hide();
                    break;
            }
        }
    }
}
