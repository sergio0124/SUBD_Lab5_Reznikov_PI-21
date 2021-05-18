using ForumBusinessLogic.BindingModels;
using ForumBusinessLogic.BusinessLogics;
using ForumBusinessLogic.ViewModels;
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
        public FormTopic(TopicLogic topicLogic)
        {
            InitializeComponent();
            topic = topicLogic;
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
                topic.CreateOrUpdate(new TopicBindingModel
                {
                    Id = id,
                    Name = textBox.Text,
                    ObjectId=Convert.ToInt32(comboBoxObject.SelectedValue),
                    TopicId= Convert.ToInt32(comboBoxTopic.SelectedValue)
                }); 
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

        private void FormTopic_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                TopicViewModel model = topic.Read(new TopicBindingModel { Id = id })?[0];
                textBox.Text = model.Name;
                comboBoxObject.SelectedIndex = (int)model.ObjectId;
                comboBoxTopic.SelectedIndex = (int)model.TopicId;
            }
        }
    }
}
