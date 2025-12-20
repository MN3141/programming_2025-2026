using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerVision
{
    class ImageBlock
    {
        public Point startPoint;
        public Point endPoint;

        public ImageBlock(int x0,int y0,int x1,int y1)
        {
            this.startPoint = new Point();
            this.endPoint = new Point();

            this.startPoint.x = x0;
            this.startPoint.y = y0;

            this.endPoint.x = x1;
            this.endPoint.y = y1;
        }
    }
}
