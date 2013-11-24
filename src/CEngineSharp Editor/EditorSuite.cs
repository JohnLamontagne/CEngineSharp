using System;
using System.Windows.Forms;

namespace CEngineSharp_Editor
{
    public partial class EditorSuite : Form
    {
        private MapEditor _mapEditor;

        private string dataPath;

        public EditorSuite()
        {
            InitializeComponent();

            this.IsMdiContainer = true;
        }

        private void _mapEditor_Disposed(object sender, EventArgs e)
        {
            _mapEditor = null;
        }

        private void mapEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_mapEditor == null)
            {
                if (dataPath == null || dataPath == "")
                    LoadDataPath();

                _mapEditor = new MapEditor(this.dataPath);
                _mapEditor.Disposed += _mapEditor_Disposed;
                _mapEditor.MdiParent = this;
                _mapEditor.Show();
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
    }
}