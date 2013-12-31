namespace CEngineSharp_Server
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textCommandInput = new System.Windows.Forms.TextBox();
            this.textServerOutput = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.playersDataGrid = new System.Windows.Forms.DataGridView();
            this.dataGridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlayerLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccessLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playersDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(673, 481);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.textCommandInput);
            this.tabPage1.Controls.Add(this.textServerOutput);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(665, 455);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Console";
            // 
            // textCommandInput
            // 
            this.textCommandInput.Location = new System.Drawing.Point(6, 429);
            this.textCommandInput.Name = "textCommandInput";
            this.textCommandInput.Size = new System.Drawing.Size(649, 20);
            this.textCommandInput.TabIndex = 2;
            this.textCommandInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textCommandInput_KeyDown);
            // 
            // textServerOutput
            // 
            this.textServerOutput.Location = new System.Drawing.Point(6, 6);
            this.textServerOutput.Multiline = true;
            this.textServerOutput.Name = "textServerOutput";
            this.textServerOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textServerOutput.Size = new System.Drawing.Size(649, 417);
            this.textServerOutput.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.playersDataGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(665, 455);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Players";
            // 
            // playersDataGrid
            // 
            this.playersDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.playersDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.PlayerName,
            this.PlayerLevel,
            this.IP,
            this.AccessLevel,
            this.HP,
            this.MP});
            this.playersDataGrid.Location = new System.Drawing.Point(6, 6);
            this.playersDataGrid.Name = "playersDataGrid";
            this.playersDataGrid.ReadOnly = true;
            this.playersDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.playersDataGrid.Size = new System.Drawing.Size(644, 443);
            this.playersDataGrid.TabIndex = 0;
            // 
            // dataGridContextMenu
            // 
            this.dataGridContextMenu.Name = "dataGridContextMenu";
            this.dataGridContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // Index
            // 
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            // 
            // PlayerName
            // 
            this.PlayerName.HeaderText = "Player Name";
            this.PlayerName.Name = "PlayerName";
            this.PlayerName.ReadOnly = true;
            // 
            // PlayerLevel
            // 
            this.PlayerLevel.HeaderText = "Player Level";
            this.PlayerLevel.Name = "PlayerLevel";
            this.PlayerLevel.ReadOnly = true;
            // 
            // IP
            // 
            this.IP.HeaderText = "IP";
            this.IP.Name = "IP";
            this.IP.ReadOnly = true;
            // 
            // AccessLevel
            // 
            this.AccessLevel.HeaderText = "Access Level";
            this.AccessLevel.Name = "AccessLevel";
            this.AccessLevel.ReadOnly = true;
            // 
            // HP
            // 
            this.HP.HeaderText = "HP";
            this.HP.Name = "HP";
            this.HP.ReadOnly = true;
            // 
            // MP
            // 
            this.MP.HeaderText = "MP";
            this.MP.Name = "MP";
            this.MP.ReadOnly = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 500);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(704, 539);
            this.MinimumSize = new System.Drawing.Size(704, 539);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.playersDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox textServerOutput;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textCommandInput;
        private System.Windows.Forms.DataGridView playersDataGrid;
        private System.Windows.Forms.ContextMenuStrip dataGridContextMenu;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlayerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlayerLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccessLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn HP;
        private System.Windows.Forms.DataGridViewTextBoxColumn MP;
    }
}