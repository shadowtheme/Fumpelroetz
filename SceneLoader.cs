using System.Drawing;
using System.Windows.Forms;

namespace EscapeRoomControlPanel
{
    public static class SceneLoader
    {
        public static void AddLoadedScene(EinstellungenForm form, SceneData sceneData)
        {
            Panel newScenePanel = new Panel
            {
                Size = new Size(1400, 700),
                Location = new Point(sceneData.LocationX, sceneData.LocationY),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = SystemColors.ControlLight,
                Name = sceneData.Name
            };

            Button newSceneButton = new Button
            {
                Size = new Size(100, 30),
                Text = sceneData.Name
            };
            newSceneButton.Click += (s, e) => form.ShowScene(newScenePanel);

            Panel headerPanel = new Panel
            {
                Size = new Size(760, 50),
                Dock = DockStyle.Top,
                BackColor = Color.Gray
            };

            TextBox sceneNameTextBox = new TextBox
            {
                Text = sceneData.Name,
                Dock = DockStyle.Left,
                TextAlign = HorizontalAlignment.Left,
                BorderStyle = BorderStyle.None,
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, 0),
                Width = 700
            };
            sceneNameTextBox.Enter += (s, e) => sceneNameTextBox.SelectAll();
            sceneNameTextBox.Leave += (s, e) => form.RenameScene(newScenePanel, newSceneButton, sceneNameTextBox.Text);
            sceneNameTextBox.Click += (s, e) =>
            {
                sceneNameTextBox.Focus();
                sceneNameTextBox.SelectAll();
            };
            sceneNameTextBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                {
                    form.ActiveControl = null; // Remove focus from TextBox
                    form.RenameScene(newScenePanel, newSceneButton, sceneNameTextBox.Text);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            };

            // Add click event to the panel to remove focus from TextBox when clicking outside
            newScenePanel.Click += form.EinstellungenForm_Click;
            headerPanel.Click += form.EinstellungenForm_Click;

            // Create Delete Button
            Button deleteButton = new Button
            {
                Text = "Szene löschen",
                Dock = DockStyle.Right,
                Width = 80
            };
            deleteButton.Click += (s, e) => form.DeleteScene(newScenePanel, newSceneButton);

            // Create Behavior Button
            Button addBehaviorButton = new Button
            {
                Text = "Verhalten hinzufügen",
                Location = new Point(10, 60),
                Width = 150,
                Height = 30
            };
            addBehaviorButton.Click += (s, e) => form.ShowBehaviorManager(newScenePanel);

            // Create Vertical Separator
            Panel separatorPanel = new Panel
            {
                Location = new Point(170, 50),
                Size = new Size(2, 700),
                BackColor = Color.Gray
            };

            headerPanel.Controls.Add(sceneNameTextBox);
            headerPanel.Controls.Add(deleteButton);

            newScenePanel.Controls.Add(headerPanel);
            newScenePanel.Controls.Add(addBehaviorButton);
            newScenePanel.Controls.Add(separatorPanel);

            form.Controls.Add(newScenePanel);
            form.Controls.Add(newSceneButton);

            form.scenePanels.Add(newScenePanel);
            form.szeneButtons.Add(newSceneButton);

            form.UpdateSceneButtonPositions();
        }

    }
}
