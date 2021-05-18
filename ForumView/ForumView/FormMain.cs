using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace ForumView
{
    public partial class FormMain : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public FormMain()
        {
            InitializeComponent();
        }

        private void объектыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormObjects>();
            form.ShowDialog();
        }

        private void пользователиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormPersons>();
            form.ShowDialog();
        }

        private void обсужденияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormThreads>();
            form.ShowDialog();
        }

        private void темыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormTopics>();
            form.ShowDialog();
        }
    }
}
