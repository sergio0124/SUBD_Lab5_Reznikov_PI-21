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
    public partial class FormPerson : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public int? id;
        private PersonLogic person;
        public FormPerson(PersonLogic personLogic)
        {
            InitializeComponent();
            person = personLogic;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                MessageBox.Show("Заполните поле Логин", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox2.Text == null)
            {
                MessageBox.Show("Заполните поле Статус", "Ошибка",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                person.CreateOrUpdate(new PersonBindingModel
                {
                    Status = textBox2.Text,
                    Name = textBox1.Text,
                    Id = id
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

        private void FormPerson_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                PersonViewModel model = person.Read(new PersonBindingModel { Id = id })?[0];
                textBox1.Text = model.Name;
                textBox2.Text = model.Status;
            }
        }
    }
}
