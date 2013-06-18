using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

class L1S04 : Form
{
    protected override void OnPaint(PaintEventArgs e)
    {
        int w = ClientSize.Width / 10;
        int h = ClientSize.Height / 10;
        for (int x=0;x<10;x++)
            for (int y = 0; y < 10; y++)
            {
                e.Graphics.FillRectangle(
                    new SolidBrush((x + y) % 2 == 0 ? Color.White : Color.Black),
                    new Rectangle(x * w, y * h, w, h)
                    );

            }
    }

    public static void MainX()
    {
        Application.Run(new L1S04());
    }
}
