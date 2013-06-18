using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


class L1S02
{
    public static void MainX()
    {
        var form = new Form();

        var label = new Label();
        label.Top = 10;
        label.Left = 10;
        label.Width = 200;
        label.Height = 20;
        label.Text = "Введите текст:";
        form.Controls.Add(label);

        var textBox = new TextBox();
        textBox.Top = label.Bottom+5;
        textBox.Left = label.Left;
        textBox.Size = label.Size;
        form.Controls.Add(textBox);

        var button = new Button();
        button.Top = textBox.Bottom + 15;
        button.Left = textBox.Left;
        button.Width = label.Width;
        button.Height = 30;
        button.Text = "OK";
        button.Click += (sender, args) =>
            {
                MessageBox.Show("Вы ввели: " + textBox.Text);
            };
        form.Controls.Add(button);

        form.ClientSize = new System.Drawing.Size(button.Right + 10, button.Bottom + 10);
        Application.Run(form);
    }
}
