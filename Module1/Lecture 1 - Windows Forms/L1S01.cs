using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


class L1S01
{
    public static void MainX()
    {
        var form = new Form();

        var label = new Label();
        label.Top = 10;
        label.Left = 10;
        label.Width = 200;
        label.Height = 20;
        label.Text = "Hello, Windows Forms!";
        form.Controls.Add(label);

        var button = new Button();
        button.Top = label.Bottom + 15;
        button.Left = label.Left;
        button.Width = label.Width;
        button.Height = 30;
        button.Text = "OK";
        button.Click += (sender, args) =>
            {
                form.Close();
            };
        form.Controls.Add(button);

        form.ClientSize = new System.Drawing.Size(button.Right + 10, button.Bottom + 10);
        Application.Run(form);
    }
}
