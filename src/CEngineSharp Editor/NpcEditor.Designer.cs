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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textHP = new System.Windows.Forms.TextBox();
            this.textMP = new System.Windows.Forms.TextBox();
            this.textStrength = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.npcSpritePic)).BeginInit();
            this.groupBox2.SuspendLayout();
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
            this.groupBox1.Controls.Add(this.groupBox2);
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
            this.label2.Location = new System.Drawing.Point(317, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Npc Sprite:";
            // 
            // npcTextureScrollBar
            // 
            this.npcTextureScrollBar.Location = new System.Drawing.Point(307, 107);
            this.npcTextureScrollBar.Name = "npcTextureScrollBar";
            this.npcTextureScrollBar.Size = new System.Drawing.Size(83, 17);
            this.npcTextureScrollBar.TabIndex = 4;
            this.npcTextureScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.npcTextureScrollBar_Scroll);
            // 
            // npcSpritePic
            // 
            this.npcSpritePic.Location = new System.Drawing.Point(332, 72);
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textStrength);
            this.groupBox2.Controls.Add(this.textMP);
            this.groupBox2.Controls.Add(this.textHP);
            this.groupBox2.Location = new System.Drawing.Point(21, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(247, 127);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stats";
            // 
            // textHP
            // 
            this.textHP.Location = new System.Drawing.Point(56, 19);
            this.textHP.Name = "textHP";
            this.textHP.Size = new System.Drawing.Size(100, 20);
            this.textHP.TabIndex = 0;
            this.textHP.TextChanged += new System.EventHandler(this.textHP_TextChanged);
            // 
            // textMP
            // 
            this.textMP.Location = new System.Drawing.Point(56, 45);
            this.textMP.Name = "textMP";
            this.textMP.Size = new System.Drawing.Size(100, 20);
            this.textMP.TabIndex = 1;
            this.textMP.TextChanged += new System.EventHandler(this.textMP_TextChanged);
            // 
            // textStrength
            // 
            this.textStrength.Location = new System.Drawing.Point(56, 71);
            this.textStrength.Name = "textStrength";
            this.textStrength.Size = new System.Drawing.Size(100, 20);
            this.textStrength.TabIndex = 2;
            this.textStrength.TextChanged += new System.EventHandler(this.textStrength_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "HP:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "MP:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Strength:";
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textStrength;
        private System.Windows.Forms.TextBox textMP;
        private System.Windows.Forms.TextBox textHP;
    }
}