using ForumForumBusinessLogic.BindingModels;
using ForumForumBusinessLogic.BusinessLogics;
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
    public partial class FormThreads : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private ThreadLogic threadLogic;
        public FormThreads(ThreadLogic thread)
        {
            InitializeComponent();
            threadLogic = thread;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var list = threadLogic.Read(null);
                if (list != null)
                {
                    dataGridView.DataSource = list;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[5].Visible = false;
                    dataGridView.Columns[6].Visible = false;
                    dataGridView.Columns[7].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormThread>();
            form.ShowDialog();
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormThread>();
                form.id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                form.LoadData();
                form.ShowDialog();
                LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                    try
                    {
                        threadLogic.Delete(new ThreadBindingModel { Id = id });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void FormThreads_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
