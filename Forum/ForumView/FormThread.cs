using ForumBusinessLogic.BindingModels;
using ForumBusinessLogic.BusinessLogics;
using ForumBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace ForumView
{
    public partial class FormThread : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public int? id;
        private TopicLogic topic;
        private MessageLogic message;
        private ThreadLogic thread;
        public FormThread(TopicLogic topicLogic, MessageLogic messageLogic, ThreadLogic threadLogic, int threadid)
        {
            InitializeComponent();
            topic = topicLogic;
            message = messageLogic;
            thread = threadLogic;
            id = threadid;
            List<TopicViewModel> list = topic.Read(null);
            if (list != null)
            {
                comboBox.DisplayMember = "Name";
                comboBox.ValueMember = "Id";
                comboBox.DataSource = list;
                comboBox.SelectedItem = null;
            }
            LoadData();
        }

        private void LoadData() {
            if (id.HasValue)
            {
                Dictionary<int, string> mes = thread.Read(new ThreadBindingModel { Id = id })?[0].Messages;
                foreach (var message in mes)
                {
                    dataGridView.Rows.Add(new object[] { message.Key, message.Value });
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormMessage>();
            form.ShowDialog();
            LoadData();
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            if (comboBox.SelectedValue==null) {
                MessageBox.Show("Выберите сообщение", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var form = Container.Resolve<FormMessage>();
            form.id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
            form.ThreadId = (int)id;
            form.ShowDialog();
            LoadData();
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        message.Delete(new MessageBindingModel { Id= Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value) });
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

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text == null) {
                MessageBox.Show("Заполните поле Название", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBoxDescription == null) {
                MessageBox.Show("Заполните поле Описание", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                thread.CreateOrUpdate(new ThreadBindingModel
                {
                    Description = textBoxDescription.Text,
                    Name = textBoxName.Text,
                    Id = id,
                    TopicId = Convert.ToInt32(comboBox.SelectedValue)
                });
            }
            catch (Exception ex) {
                MessageBox.Show("Exception occured", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FormThread_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
