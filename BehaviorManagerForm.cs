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
            behaviorListBox.Items.Add("PinStatus");
            behaviorListBox.Items.Add("Szene gestartet");
            behaviorListBox.Items.Add("Audio startet");
            behaviorListBox.Items.Add("Audio stoppt");
            behaviorListBox.Items.Add("Aktionbutton");
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

        public static void ShowBehaviorManager(EinstellungenForm form, Panel scenePanel)
        {
            using (var behaviorManagerForm = new BehaviorManagerForm())
            {
                if (behaviorManagerForm.ShowDialog() == DialogResult.OK)
                {
                    // Hier können Sie das ausgewählte Verhalten zur Szene hinzufügen
                    MessageBox.Show($"Verhalten hinzugefügt: {behaviorManagerForm.SelectedBehavior}");
                }
            }
        }
    }
}
