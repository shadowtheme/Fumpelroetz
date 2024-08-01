using System;
using System.Windows.Forms;

namespace EscapeRoomControlPanel
{
    public partial class BehaviorManagerForm : Form
    {
        public string SelectedBehavior { get; private set; }

        public BehaviorManagerForm()
        {
            InitializeComponent();
            LoadBehaviors();
        }

        private void LoadBehaviors()
        {
            behaviorListBox.Items.Add("PinStatus abfragen");
            // Weitere Verhaltensweisen können hier hinzugefügt werden
        }

        private void addBehaviorButton_Click(object sender, EventArgs e)
        {
            if (behaviorListBox.SelectedItem != null)
            {
                SelectedBehavior = behaviorListBox.SelectedItem.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie ein Verhalten aus.");
            }
        }
    }
}

