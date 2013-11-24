using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CEngineSharp_Server.Utilities
{
    public class TextBoxStreamWriter : TextWriter
    {
        private TextBox _output = null;

        private delegate void WriteDelegate(char value);

        public TextBoxStreamWriter(TextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            if (_output.InvokeRequired)
                _output.Invoke(new WriteDelegate(Write), value);
            else
            {
                base.Write(value);
                _output.AppendText(value.ToString());
            }
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}