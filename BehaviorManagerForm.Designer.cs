namespace EscapeRoomControlPanel
{
    partial class BehaviorManagerForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ListBox behaviorListBox;
        private System.Windows.Forms.Button addBehaviorButton;

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
            this.behaviorListBox = new System.Windows.Forms.ListBox();
            this.addBehaviorButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // behaviorListBox
            // 
            this.behaviorListBox.FormattingEnabled = true;
            this.behaviorListBox.Location = new System.Drawing.Point(12, 12);
            this.behaviorListBox.Name = "behaviorListBox";
            this.behaviorListBox.Size = new System.Drawing.Size(260, 186);
            this.behaviorListBox.TabIndex = 0;
            // 
            // addBehaviorButton
            // 
            this.addBehaviorButton.Location = new System.Drawing.Point(12, 204);
            this.addBehaviorButton.Name = "addBehaviorButton";
            this.addBehaviorButton.Size = new System.Drawing.Size(260, 45);
            this.addBehaviorButton.TabIndex = 1;
            this.addBehaviorButton.Text = "Add Behavior";
            this.addBehaviorButton.UseVisualStyleBackColor = true;
            this.addBehaviorButton.Click += new System.EventHandler(this.addBehaviorButton_Click);
            // 
            // BehaviorManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.addBehaviorButton);
            this.Controls.Add(this.behaviorListBox);
            this.Name = "BehaviorManagerForm";
            this.Text = "Behavior Manager";
            this.ResumeLayout(false);

        }
    }
}
