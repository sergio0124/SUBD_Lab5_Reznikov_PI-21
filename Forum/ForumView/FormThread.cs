using ForumForumBusinessLogic.BindingModels;
using ForumForumBusinessLogic.BusinessLogics;
using ForumForumBusinessLogic.ViewModels;
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
        private PersonLogic person;
        public FormThread(TopicLogic topicLogic, MessageLogic messageLogic, ThreadLogic threadLogic, PersonLogic personLogic)
        {
            InitializeComponent();
            topic = topicLogic;
            message = messageLogic;
            thread = threadLogic;
            person = personLogic;
            List<TopicViewModel> list = topic.Read(null);
            if (list != null)
            {
                comboBox.DisplayMember = "Name";
                comboBox.ValueMember = "Id";
                comboBox.DataSource = list;
                comboBox.SelectedItem = null;
            }
            List<PersonViewModel> listlist = person.Read(null);
            if (listlist != null)
            {
                comboBoxPerson.DisplayMember = "Name";
                comboBoxPerson.ValueMember = "Id";
                comboBoxPerson.DataSource = listlist;
                comboBoxPerson.SelectedItem = null;
            }
            LoadData();
        }

        public void LoadData()
        {
            if (id.HasValue)
            {
                Dictionary<int, string> mes = thread.Read(new ThreadBindingModel { Id = id })?[0].Messages;
                foreach (var m in mes)
                {
                    dataGridView.Rows.Add(m.Key, m.Value );
                }
                ThreadViewModel thr = thread.Read(new ThreadBindingModel { Id = id })?[0];
                textBoxDescription.Text = thr.Description;
                textBoxName.Text = thr.Name;
                comboBoxPerson.Enabled = false;
                comboBox.Enabled = false;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormMessage>();
            form.ThrId = id;
            form.ShowDialog();
            LoadData();
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows == null)
            {
                MessageBox.Show("Выберите сообщение", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var form = Container.Resolve<FormMessage>();
            form.id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
            form.ThrId = id;
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
                        message.Delete(new MessageBindingModel { Id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value) });
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
            if (textBoxName.Text == null)
            {
                MessageBox.Show("Заполните поле Название", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBoxDescription == null)
            {
                MessageBox.Show("Заполните поле Описание", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    thread.CreateOrUpdate(new ThreadBindingModel
                    {
                        Description = textBoxDescription.Text,
                        Name = textBoxName.Text,
                        Id = id
                    });
                }
                else {
                    if (comboBoxPerson.SelectedValue == null)
                    {
                        MessageBox.Show("Заполните поле Person", "Ошибка",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    thread.CreateOrUpdate(new ThreadBindingModel
                    {
                        Description = textBoxDescription.Text,
                        Name = textBoxName.Text,
                        PersonId = Convert.ToInt32(comboBoxPerson.SelectedValue),
                        TopicId = Convert.ToInt32(comboBox.SelectedValue)
                    });
                }              
            }
            catch (Exception ex)
            {
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
    }
}
