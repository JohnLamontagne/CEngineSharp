namespace CEngineSharp_Editor
{
    partial class NpcEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.addNpcButton = new System.Windows.Forms.Button();
            this.npcListBox = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.npcTextureScrollBar = new System.Windows.Forms.HScrollBar();
            this.npcSpritePic = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.npcSpritePic)).BeginInit();
            this.SuspendLayout();
            // 
            // addNpcButton
            // 
            this.addNpcButton.Location = new System.Drawing.Point(12, 191);
            this.addNpcButton.Name = "addNpcButton";
            this.addNpcButton.Size = new System.Drawing.Size(120, 23);
            this.addNpcButton.TabIndex = 6;
            this.addNpcButton.Text = "Add Npc";
            this.addNpcButton.UseVisualStyleBackColor = true;
            this.addNpcButton.Click += new System.EventHandler(this.addNpcButton_Click);
            // 
            // npcListBox
            // 
            this.npcListBox.FormattingEnabled = true;
            this.npcListBox.Location = new System.Drawing.Point(12, 12);
            this.npcListBox.Name = "npcListBox";
            this.npcListBox.Size = new System.Drawing.Size(120, 173);
            this.npcListBox.TabIndex = 5;
            this.npcListBox.SelectedIndexChanged += new System.EventHandler(this.npcListBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.npcTextureScrollBar);
            this.groupBox1.Controls.Add(this.npcSpritePic);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textName);
            this.groupBox1.Location = new System.Drawing.Point(138, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(521, 202);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Information";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(304, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Npc Sprite:";
            // 
            // npcTextureScrollBar
            // 
            this.npcTextureScrollBar.Location = new System.Drawing.Point(290, 69);
            this.npcTextureScrollBar.Name = "npcTextureScrollBar";
            this.npcTextureScrollBar.Size = new System.Drawing.Size(83, 17);
            this.npcTextureScrollBar.TabIndex = 4;
            this.npcTextureScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.npcTextureScrollBar_Scroll);
            // 
            // npcSpritePic
            // 
            this.npcSpritePic.Location = new System.Drawing.Point(317, 34);
            this.npcSpritePic.Name = "npcSpritePic";
            this.npcSpritePic.Size = new System.Drawing.Size(32, 32);
            this.npcSpritePic.TabIndex = 3;
            this.npcSpritePic.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(59, 19);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(100, 20);
            this.textName.TabIndex = 1;
            this.textName.TextChanged += new System.EventHandler(this.textName_TextChanged);
            // 
            // NpcEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 226);
            this.Controls.Add(this.addNpcButton);
            this.Controls.Add(this.npcListBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "NpcEditor";
            this.Text = "NpcEditor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.npcSpritePic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addNpcButton;
        private System.Windows.Forms.ListBox npcListBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.HScrollBar npcTextureScrollBar;
        private System.Windows.Forms.PictureBox npcSpritePic;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textName;
    }
}