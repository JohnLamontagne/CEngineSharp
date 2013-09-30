using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEngineSharp_Server.Utilities
{
    public class Rect
    {
        public Rect(int left, int top, int height, int width)
        {
            this.Left = left;
            this.Top = top;
            this.Height = height;
            this.Width = width;
        }

        public int Left;
        public int Top;
        public int Height;
        public int Width;
    }
}