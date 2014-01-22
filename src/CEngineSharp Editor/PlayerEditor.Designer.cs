namespace CEngineSharp_Editor
{
    partial class PlayerEditor
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textPassword = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textMP = new System.Windows.Forms.TextBox();
            this.textHP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.playerTextureScrollBar = new System.Windows.Forms.HScrollBar();
            this.playerSpritePic = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textName = new System.Windows.Forms.TextBox();
            this.playerListBox = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playerSpritePic)).BeginInit();
            this.SuspendLayout();
            // 
            // addNpcButton
            // 
            this.addNpcButton.Location = new System.Drawing.Point(-181, 208);
            this.addNpcButton.Name = "addNpcButton";
            this.addNpcButton.Size = new System.Drawing.Size(120, 23);
            this.addNpcButton.TabIndex = 9;
            this.addNpcButton.Text = "Add Npc";
            this.addNpcButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textPassword);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.playerTextureScrollBar);
            this.groupBox1.Controls.Add(this.playerSpritePic);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textName);
            this.groupBox1.Location = new System.Drawing.Point(138, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(562, 202);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Information";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Password:";
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(77, 43);
            this.textPassword.Name = "textPassword";
            this.textPassword.Size = new System.Drawing.Size(100, 20);
            this.textPassword.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textMP);
            this.groupBox2.Controls.Add(this.textHP);
            this.groupBox2.Location = new System.Drawing.Point(21, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(247, 87);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stats";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "HP:";
            // 
            // textMP
            // 
            this.textMP.Location = new System.Drawing.Point(56, 45);
            this.textMP.Name = "textMP";
            this.textMP.Size = new System.Drawing.Size(100, 20);
            this.textMP.TabIndex = 1;
            //this.textMP.TextChanged += new System.EventHandler(this.textMP_TextChanged);
            // 
            // textHP
            // 
            this.textHP.Location = new System.Drawing.Point(56, 19);
            this.textHP.Name = "textHP";
            this.textHP.Size = new System.Drawing.Size(100, 20);
            this.textHP.TabIndex = 0;
            //this.textHP.TextChanged += new System.EventHandler(this.textHP_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(317, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Player Sprite:";
            // 
            // playerTextureScrollBar
            // 
            this.playerTextureScrollBar.Location = new System.Drawing.Point(307, 107);
            this.playerTextureScrollBar.Name = "playerTextureScrollBar";
            this.playerTextureScrollBar.Size = new System.Drawing.Size(83, 17);
            this.playerTextureScrollBar.TabIndex = 4;
            //this.playerTextureScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.playerTextureScrollBar_Scroll);
            // 
            // playerSpritePic
            // 
            this.playerSpritePic.Location = new System.Drawing.Point(332, 72);
            this.playerSpritePic.Name = "playerSpritePic";
            this.playerSpritePic.Size = new System.Drawing.Size(32, 32);
            this.playerSpritePic.TabIndex = 3;
            this.playerSpritePic.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // textName
            // 
            this.textName.Location = new System.Drawing.Point(77, 17);
            this.textName.Name = "textName";
            this.textName.Size = new System.Drawing.Size(100, 20);
            this.textName.TabIndex = 1;
            // 
            // playerListBox
            // 
            this.playerListBox.FormattingEnabled = true;
            this.playerListBox.Location = new System.Drawing.Point(12, 12);
            this.playerListBox.Name = "playerListBox";
            this.playerListBox.Size = new System.Drawing.Size(120, 199);
            this.playerListBox.TabIndex = 10;
            // 
            // PlayerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 222);
            this.Controls.Add(this.playerListBox);
            this.Controls.Add(this.addNpcButton);
            this.Controls.Add(this.groupBox1);
            this.Name = "PlayerEditor";
            this.Text = "AccountEditor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playerSpritePic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addNpcButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textMP;
        private System.Windows.Forms.TextBox textHP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.HScrollBar playerTextureScrollBar;
        private System.Windows.Forms.PictureBox playerSpritePic;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textPassword;
        private System.Windows.Forms.ListBox playerListBox;
    }
}