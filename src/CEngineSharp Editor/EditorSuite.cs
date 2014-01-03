using System;
using System.Windows.Forms;

namespace CEngineSharp_Editor
{
    public partial class EditorSuite : Form
    {
        private MapEditor mapEditor;
        private ItemEditor itemEditor;
        private NpcEditor npcEditor;

        private string dataPath;

        public EditorSuite()
        {
            InitializeComponent();

            this.IsMdiContainer = true;
        }

        private void _mapEditor_Disposed(object sender, EventArgs e)
        {
            this.mapEditor = null;
        }

        private void itemEditor_Disposed(object sender, EventArgs e)
        {
            this.itemEditor = null;
        }

        private void npcEditor_Disposed(object sender, EventArgs e)
        {
            this.npcEditor = null;
        }

        private void mapEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mapEditor == null)
            {
                if (string.IsNullOrEmpty(this.dataPath))
                    LoadDataPath();

                this.mapEditor = new MapEditor(this.dataPath);
                this.mapEditor.Disposed += _mapEditor_Disposed;
                this.mapEditor.MdiParent = this;
                this.mapEditor.Show();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
                (this.ActiveMdiChild as IChildFormActions).SaveData();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild != null)
                (this.ActiveMdiChild as IChildFormActions).LoadData();
        }

        private void LoadDataPath()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            this.dataPath = dialog.SelectedPath;
        }

        private void itemEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.itemEditor == null)
            {
                if (string.IsNullOrEmpty(this.dataPath))
                    LoadDataPath();

                this.itemEditor = new ItemEditor(this.dataPath);
                this.itemEditor.Disposed += itemEditor_Disposed;
                this.itemEditor.MdiParent = this;
                this.itemEditor.Show();
            }
        }

        private void npcEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.npcEditor == null)
            {
                if (string.IsNullOrEmpty(this.dataPath))
                    LoadDataPath();

                this.npcEditor = new NpcEditor(this.dataPath);
                this.npcEditor.Disposed += npcEditor_Disposed;
                this.npcEditor.MdiParent = this;
                this.npcEditor.Show();
            }
        }


    }
}