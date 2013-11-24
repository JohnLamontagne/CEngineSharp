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
            this.mapList = new System.Windows.Forms.ListBox();
            this.newMapButton = new System.Windows.Forms.Button();
            this.removeMapButton = new System.Windows.Forms.Button();
            this.fillMapButton = new System.Windows.Forms.Button();
            this.clearMapButton = new System.Windows.Forms.Button();
            this.mapScrollX = new System.Windows.Forms.HScrollBar();
            this.mapScrollY = new System.Windows.Forms.VScrollBar();
            this.attributesGroup = new System.Windows.Forms.GroupBox();
            this.blockedTileCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.mapDisplayPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileSetPicture)).BeginInit();
            this.attributesGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // mapPropertyGrid
            // 
            this.mapPropertyGrid.Location = new System.Drawing.Point(12, 12);
            this.mapPropertyGrid.Name = "mapPropertyGrid";
            this.mapPropertyGrid.Size = new System.Drawing.Size(181, 480);
            this.mapPropertyGrid.TabIndex = 3;
            // 
            // mapDisplayPicture
            // 
            this.mapDisplayPicture.Location = new System.Drawing.Point(657, 12);
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
            this.tileSetForwardButton.Location = new System.Drawing.Point(550, 517);
            this.tileSetForwardButton.Name = "tileSetForwardButton";
            this.tileSetForwardButton.Size = new System.Drawing.Size(58, 25);
            this.tileSetForwardButton.TabIndex = 7;
            this.tileSetForwardButton.Text = "->";
            this.tileSetForwardButton.UseVisualStyleBackColor = true;
            this.tileSetForwardButton.Click += new System.EventHandler(this.tileSetForwardButton_Click);
            // 
            // tileSetBackButton
            // 
            this.tileSetBackButton.Location = new System.Drawing.Point(332, 517);
            this.tileSetBackButton.Name = "tileSetBackButton";
            this.tileSetBackButton.Size = new System.Drawing.Size(58, 25);
            this.tileSetBackButton.TabIndex = 8;
            this.tileSetBackButton.Text = "<-";
            this.tileSetBackButton.UseVisualStyleBackColor = true;
            this.tileSetBackButton.Click += new System.EventHandler(this.tileSetBackButton_Click);
            // 
            // mapList
            // 
            this.mapList.FormattingEnabled = true;
            this.mapList.Location = new System.Drawing.Point(12, 498);
            this.mapList.Name = "mapList";
            this.mapList.Size = new System.Drawing.Size(181, 108);
            this.mapList.TabIndex = 9;
            this.mapList.SelectedIndexChanged += new System.EventHandler(this.mapList_SelectedIndexChanged);
            // 
            // newMapButton
            // 
            this.newMapButton.Location = new System.Drawing.Point(12, 612);
            this.newMapButton.Name = "newMapButton";
            this.newMapButton.Size = new System.Drawing.Size(80, 23);
            this.newMapButton.TabIndex = 10;
            this.newMapButton.Text = "New Map";
            this.newMapButton.UseVisualStyleBackColor = true;
            this.newMapButton.Click += new System.EventHandler(this.newMapButton_Click);
            // 
            // removeMapButton
            // 
            this.removeMapButton.Location = new System.Drawing.Point(111, 612);
            this.removeMapButton.Name = "removeMapButton";
            this.removeMapButton.Size = new System.Drawing.Size(82, 23);
            this.removeMapButton.TabIndex = 11;
            this.removeMapButton.Text = "Remove Map";
            this.removeMapButton.UseVisualStyleBackColor = true;
            this.removeMapButton.Click += new System.EventHandler(this.removeMapButton_Click);
            // 
            // fillMapButton
            // 
            this.fillMapButton.Location = new System.Drawing.Point(657, 519);
            this.fillMapButton.Name = "fillMapButton";
            this.fillMapButton.Size = new System.Drawing.Size(75, 23);
            this.fillMapButton.TabIndex = 12;
            this.fillMapButton.Text = "Fill Map";
            this.fillMapButton.UseVisualStyleBackColor = true;
            this.fillMapButton.Click += new System.EventHandler(this.fillMapButton_Click);
            // 
            // clearMapButton
            // 
            this.clearMapButton.Location = new System.Drawing.Point(749, 519);
            this.clearMapButton.Name = "clearMapButton";
            this.clearMapButton.Size = new System.Drawing.Size(75, 23);
            this.clearMapButton.TabIndex = 13;
            this.clearMapButton.Text = "Clear Map";
            this.clearMapButton.UseVisualStyleBackColor = true;
            this.clearMapButton.Click += new System.EventHandler(this.clearMapButton_Click);
            // 
            // mapScrollX
            // 
            this.mapScrollX.Location = new System.Drawing.Point(657, 494);
            this.mapScrollX.Name = "mapScrollX";
            this.mapScrollX.Size = new System.Drawing.Size(640, 20);
            this.mapScrollX.TabIndex = 14;
            this.mapScrollX.Scroll += new System.Windows.Forms.ScrollEventHandler(this.mapScrollX_Scroll);
            // 
            // mapScrollY
            // 
            this.mapScrollY.Location = new System.Drawing.Point(637, 12);
            this.mapScrollY.Name = "mapScrollY";
            this.mapScrollY.Size = new System.Drawing.Size(17, 480);
            this.mapScrollY.TabIndex = 15;
            this.mapScrollY.Scroll += new System.Windows.Forms.ScrollEventHandler(this.mapScrollY_Scroll);
            // 
            // attributesGroup
            // 
            this.attributesGroup.Controls.Add(this.blockedTileCheckBox);
            this.attributesGroup.Location = new System.Drawing.Point(332, 548);
            this.attributesGroup.Name = "attributesGroup";
            this.attributesGroup.Size = new System.Drawing.Size(276, 117);
            this.attributesGroup.TabIndex = 16;
            this.attributesGroup.TabStop = false;
            this.attributesGroup.Text = "Attributes";
            // 
            // blockedTileCheckBox
            // 
            this.blockedTileCheckBox.AutoSize = true;
            this.blockedTileCheckBox.Location = new System.Drawing.Point(20, 29);
            this.blockedTileCheckBox.Name = "blockedTileCheckBox";
            this.blockedTileCheckBox.Size = new System.Drawing.Size(85, 17);
            this.blockedTileCheckBox.TabIndex = 0;
            this.blockedTileCheckBox.Text = "Blocked Tile";
            this.blockedTileCheckBox.UseVisualStyleBackColor = true;
            this.blockedTileCheckBox.CheckedChanged += new System.EventHandler(this.blockedTileCheckBox_CheckedChanged);
            // 
            // MapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1349, 677);
            this.Controls.Add(this.attributesGroup);
            this.Controls.Add(this.mapScrollY);
            this.Controls.Add(this.mapScrollX);
            this.Controls.Add(this.clearMapButton);
            this.Controls.Add(this.fillMapButton);
            this.Controls.Add(this.removeMapButton);
            this.Controls.Add(this.newMapButton);
            this.Controls.Add(this.mapList);
            this.Controls.Add(this.tileSetBackButton);
            this.Controls.Add(this.tileSetForwardButton);
            this.Controls.Add(this.tileSetScrollX);
            this.Controls.Add(this.tileSetScrollY);
            this.Controls.Add(this.tileSetPicture);
            this.Controls.Add(this.mapPropertyGrid);
            this.Controls.Add(this.mapDisplayPicture);
            this.Name = "MapEditor";
            this.Text = "MapEditor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MapEditor_FormClosed);
            this.Load += new System.EventHandler(this.MapEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mapDisplayPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tileSetPicture)).EndInit();
            this.attributesGroup.ResumeLayout(false);
            this.attributesGroup.PerformLayout();
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
        private System.Windows.Forms.ListBox mapList;
        private System.Windows.Forms.Button newMapButton;
        private System.Windows.Forms.Button removeMapButton;
        private System.Windows.Forms.Button fillMapButton;
        private System.Windows.Forms.Button clearMapButton;
        private System.Windows.Forms.HScrollBar mapScrollX;
        private System.Windows.Forms.VScrollBar mapScrollY;
        private System.Windows.Forms.GroupBox attributesGroup;
        private System.Windows.Forms.CheckBox blockedTileCheckBox;
    }
}