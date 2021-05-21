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
    public partial class FormMessage : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly MessageLogic logic;
        public int? id;
        public int ThreadId;
        public FormMessage(MessageLogic logics, PersonLogic personLogic)
        {
            InitializeComponent();
            logic = logics;
            List<PersonViewModel> list = personLogic.Read(null);
            if (list != null)
            {
                comboBoxPerson.DisplayMember = "Name";
                comboBoxPerson.ValueMember = "Id";
                comboBoxPerson.DataSource = list;
                comboBoxPerson.SelectedItem = null;
            }
            List<MessageViewModel> messageViews = logic.Read(null);
            if (id.HasValue)
            {
                messageViews.Remove(new MessageViewModel { Id = (int)id });
            }
            if (list != null)
            {
                comboBoxMessage.DisplayMember = "Text";
                comboBoxMessage.ValueMember = "Id";
                comboBoxMessage.DataSource = list;
                comboBoxMessage.SelectedItem = null;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Заполните текст", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (!comboBoxPerson.Focused)
            {
                MessageBox.Show("Выберите пользователя", "Ошибка", MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new MessageBindingModel
                {
                    Id = id,
                    Text = textBox1.Text,
                    DateCreate = DateTime.Now,
                    PersonId = Convert.ToInt32(comboBoxPerson.Text),
                    MessageId = Convert.ToInt32(comboBoxMessage.Text),
                    ThreadId = ThreadId
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }

        private void FormMessage_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new MessageBindingModel { Id = id })?[0];
                    if (view != null)
                    {
                        textBox1.Text = view.Text;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
        }
    }
}
