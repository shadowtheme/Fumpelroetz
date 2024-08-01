namespace EscapeRoomControlPanel
{
    partial class EinstellungenForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnAddScene;
        private System.Windows.Forms.Button preshowButton;
        private System.Windows.Forms.Panel preShowPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnAddScene = new System.Windows.Forms.Button();
            this.preshowButton = new System.Windows.Forms.Button();
            this.preShowPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnAddScene
            // 
            this.btnAddScene.Location = new System.Drawing.Point(132, 22);
            this.btnAddScene.Name = "btnAddScene";
            this.btnAddScene.Size = new System.Drawing.Size(100, 30);
            this.btnAddScene.TabIndex = 0;
            this.btnAddScene.Text = "Szene hinzufügen";
            this.btnAddScene.UseVisualStyleBackColor = true;
            this.btnAddScene.Click += new System.EventHandler(this.btnAddScene_Click);
            // 
            // preshowButton
            // 
            this.preshowButton.Location = new System.Drawing.Point(26, 22);
            this.preshowButton.Name = "preshowButton";
            this.preshowButton.Size = new System.Drawing.Size(100, 30);
            this.preshowButton.TabIndex = 1;
            this.preshowButton.Text = "Pre-Show";
            this.preshowButton.UseVisualStyleBackColor = true;
            this.preshowButton.Click += new System.EventHandler(this.preshowButton_Click);
            // 
            // preShowPanel
            // 
            this.preShowPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.preShowPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.preShowPanel.Location = new System.Drawing.Point(12, 66);
            this.preShowPanel.Name = "preShowPanel";
            this.preShowPanel.Size = new System.Drawing.Size(760, 400);
            this.preShowPanel.TabIndex = 0;
            // 
            // EinstellungenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.preshowButton);
            this.Controls.Add(this.btnAddScene);
            this.Controls.Add(this.preShowPanel);
            this.Name = "EinstellungenForm";
            this.Text = "Einstellungen";
            this.ResumeLayout(false);

        }
    }
}
