using ForumForumBusinessLogic.BindingModels;
using ForumForumBusinessLogic.BusinessLogics;
using ForumForumBusinessLogic.ViewModels;
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
    public partial class FormTopic : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public int? id;
        private TopicLogic topic;
        private ObjectLogic objects;
        public FormTopic(TopicLogic topicLogic, ObjectLogic objectLogic)
        {
            InitializeComponent();
            topic = topicLogic;
            objects = objectLogic;
            List<ObjectViewModel> list = objects.Read(null);
            if (list != null)
            {
                comboBoxObject.DisplayMember = "Name";
                comboBoxObject.ValueMember = "Id";
                comboBoxObject.DataSource = list;
                comboBoxObject.SelectedItem = null;
            }
            List<TopicViewModel> listobj = topic.Read(null);
            if (listobj != null)
            {
                comboBoxTopic.DisplayMember = "Name";
                comboBoxTopic.ValueMember = "Id";
                comboBoxTopic.DataSource = listobj;
                comboBoxTopic.SelectedItem = null;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBox.Text == null)
            {
                MessageBox.Show("Заполните поле Логин", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    topic.CreateOrUpdate(new TopicBindingModel
                    {
                        Id = id,
                        Name = textBox.Text,
                        ObjectId = Convert.ToInt32(comboBoxObject.SelectedValue),
                        TopicId = Convert.ToInt32(comboBoxTopic.SelectedValue)
                    });
                }
                else
                {
                    topic.CreateOrUpdate(new TopicBindingModel
                    {
                        Name = textBox.Text,
                        ObjectId = Convert.ToInt32(comboBoxObject.SelectedValue),
                        TopicId = Convert.ToInt32(comboBoxTopic.SelectedValue)
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occured", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FormTopic_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                TopicViewModel model = topic.Read(new TopicBindingModel { Id = id })?[0];
                textBox.Text = model.Name;
                comboBoxObject.Enabled = false;
                comboBoxTopic.Enabled = false;
            }
        }
    }
}
