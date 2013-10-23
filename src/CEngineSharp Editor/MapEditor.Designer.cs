namespace CEngineSharp_Editor
{
    partial class MapEditor
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
            this.mapPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.mapDisplayPicture = new System.Windows.Forms.PictureBox();
            this.tileSetPicture = new System.Windows.Forms.PictureBox();
            this.tileSetScrollY = new System.Windows.Forms.VScrollBar();
            this.tileSetScrollX = new System.Windows.Forms.HScrollBar();
            this.tileSetForwardButton = new System.Windows.Forms.Button();
            this.tileSetBackButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mapDisplayPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileSetPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // mapPropertyGrid
            // 
            this.mapPropertyGrid.Location = new System.Drawing.Point(12, 12);
            this.mapPropertyGrid.Name = "mapPropertyGrid";
            this.mapPropertyGrid.Size = new System.Drawing.Size(181, 607);
            this.mapPropertyGrid.TabIndex = 3;
            // 
            // mapDisplayPicture
            // 
            this.mapDisplayPicture.Location = new System.Drawing.Point(614, 12);
            this.mapDisplayPicture.Name = "mapDisplayPicture";
            this.mapDisplayPicture.Size = new System.Drawing.Size(640, 480);
            this.mapDisplayPicture.TabIndex = 2;
            this.mapDisplayPicture.TabStop = false;
            // 
            // tileSetPicture
            // 
            this.tileSetPicture.Location = new System.Drawing.Point(352, 12);
            this.tileSetPicture.Name = "tileSetPicture";
            this.tileSetPicture.Size = new System.Drawing.Size(256, 480);
            this.tileSetPicture.TabIndex = 4;
            this.tileSetPicture.TabStop = false;
            // 
            // tileSetScrollY
            // 
            this.tileSetScrollY.Location = new System.Drawing.Point(332, 12);
            this.tileSetScrollY.Name = "tileSetScrollY";
            this.tileSetScrollY.Size = new System.Drawing.Size(17, 480);
            this.tileSetScrollY.TabIndex = 5;
            this.tileSetScrollY.Scroll += new System.Windows.Forms.ScrollEventHandler(this.tileSetScrollY_Scroll);
            // 
            // tileSetScrollX
            // 
            this.tileSetScrollX.Location = new System.Drawing.Point(352, 495);
            this.tileSetScrollX.Name = "tileSetScrollX";
            this.tileSetScrollX.Size = new System.Drawing.Size(256, 19);
            this.tileSetScrollX.TabIndex = 6;
            this.tileSetScrollX.Scroll += new System.Windows.Forms.ScrollEventHandler(this.tileSetScrollX_Scroll);
            // 
            // tileSetForwardButton
            // 
            this.tileSetForwardButton.Location = new System.Drawing.Point(550, 528);
            this.tileSetForwardButton.Name = "tileSetForwardButton";
            this.tileSetForwardButton.Size = new System.Drawing.Size(58, 25);
            this.tileSetForwardButton.TabIndex = 7;
            this.tileSetForwardButton.Text = "->";
            this.tileSetForwardButton.UseVisualStyleBackColor = true;
            this.tileSetForwardButton.Click += new System.EventHandler(this.tileSetForwardButton_Click);
            // 
            // tileSetBackButton
            // 
            this.tileSetBackButton.Location = new System.Drawing.Point(332, 528);
            this.tileSetBackButton.Name = "tileSetBackButton";
            this.tileSetBackButton.Size = new System.Drawing.Size(58, 25);
            this.tileSetBackButton.TabIndex = 8;
            this.tileSetBackButton.Text = "<-";
            this.tileSetBackButton.UseVisualStyleBackColor = true;
            this.tileSetBackButton.Click += new System.EventHandler(this.tileSetBackButton_Click);
            // 
            // MapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1266, 642);
            this.Controls.Add(this.tileSetBackButton);
            this.Controls.Add(this.tileSetForwardButton);
            this.Controls.Add(this.tileSetScrollX);
            this.Controls.Add(this.tileSetScrollY);
            this.Controls.Add(this.tileSetPicture);
            this.Controls.Add(this.mapPropertyGrid);
            this.Controls.Add(this.mapDisplayPicture);
            this.Name = "MapEditor";
            this.Text = "MapEditor";
            ((System.ComponentModel.ISupportInitialize)(this.mapDisplayPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileSetPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid mapPropertyGrid;
        private System.Windows.Forms.PictureBox mapDisplayPicture;
        private System.Windows.Forms.PictureBox tileSetPicture;
        private System.Windows.Forms.VScrollBar tileSetScrollY;
        private System.Windows.Forms.HScrollBar tileSetScrollX;
        private System.Windows.Forms.Button tileSetForwardButton;
        private System.Windows.Forms.Button tileSetBackButton;
    }
}