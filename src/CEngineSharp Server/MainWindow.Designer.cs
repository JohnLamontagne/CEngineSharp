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
            this.textServerOutput = new System.Windows.Forms.TextBox();
            this.textCommandInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textServerOutput
            // 
            this.textServerOutput.Location = new System.Drawing.Point(12, 12);
            this.textServerOutput.Multiline = true;
            this.textServerOutput.Name = "textServerOutput";
            this.textServerOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textServerOutput.Size = new System.Drawing.Size(649, 481);
            this.textServerOutput.TabIndex = 0;
            // 
            // textCommandInput
            // 
            this.textCommandInput.Location = new System.Drawing.Point(12, 499);
            this.textCommandInput.Name = "textCommandInput";
            this.textCommandInput.Size = new System.Drawing.Size(649, 20);
            this.textCommandInput.TabIndex = 1;
            this.textCommandInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textCommandInput_KeyDown);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 532);
            this.Controls.Add(this.textCommandInput);
            this.Controls.Add(this.textServerOutput);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(701, 570);
            this.MinimumSize = new System.Drawing.Size(701, 570);
            this.Name = "MainWindow";
            this.Text = "MainWindow";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textServerOutput;
        private System.Windows.Forms.TextBox textCommandInput;
    }
}