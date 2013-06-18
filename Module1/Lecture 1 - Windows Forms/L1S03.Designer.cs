
partial class L1S03
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
        this.label1 = new System.Windows.Forms.Label();
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.checkBox1 = new System.Windows.Forms.CheckBox();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.radioButton1 = new System.Windows.Forms.RadioButton();
        this.radioButton2 = new System.Windows.Forms.RadioButton();
        this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
        this.label2 = new System.Windows.Forms.Label();
        this.button1 = new System.Windows.Forms.Button();
        this.groupBox1.SuspendLayout();
        this.SuspendLayout();
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(15, 15);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(133, 13);
        this.label1.TabIndex = 0;
        this.label1.Text = "Фамилия, имя, отчество";
        // 
        // textBox1
        // 
        this.textBox1.Location = new System.Drawing.Point(18, 31);
        this.textBox1.Name = "textBox1";
        this.textBox1.Size = new System.Drawing.Size(254, 20);
        this.textBox1.TabIndex = 1;
        // 
        // checkBox1
        // 
        this.checkBox1.AutoSize = true;
        this.checkBox1.Location = new System.Drawing.Point(190, 57);
        this.checkBox1.Name = "checkBox1";
        this.checkBox1.Size = new System.Drawing.Size(81, 17);
        this.checkBox1.TabIndex = 2;
        this.checkBox1.Text = "VIP-клиент";
        this.checkBox1.UseVisualStyleBackColor = true;
        // 
        // groupBox1
        // 
        this.groupBox1.Controls.Add(this.radioButton2);
        this.groupBox1.Controls.Add(this.radioButton1);
        this.groupBox1.Location = new System.Drawing.Point(18, 80);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(248, 70);
        this.groupBox1.TabIndex = 3;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "График";
        // 
        // radioButton1
        // 
        this.radioButton1.AutoSize = true;
        this.radioButton1.Location = new System.Drawing.Point(6, 19);
        this.radioButton1.Name = "radioButton1";
        this.radioButton1.Size = new System.Drawing.Size(70, 17);
        this.radioButton1.TabIndex = 0;
        this.radioButton1.TabStop = true;
        this.radioButton1.Text = "Дневной";
        this.radioButton1.UseVisualStyleBackColor = true;
        // 
        // radioButton2
        // 
        this.radioButton2.AutoSize = true;
        this.radioButton2.Location = new System.Drawing.Point(6, 42);
        this.radioButton2.Name = "radioButton2";
        this.radioButton2.Size = new System.Drawing.Size(92, 17);
        this.radioButton2.TabIndex = 1;
        this.radioButton2.TabStop = true;
        this.radioButton2.Text = "Полный день";
        this.radioButton2.UseVisualStyleBackColor = true;
        // 
        // checkedListBox1
        // 
        this.checkedListBox1.FormattingEnabled = true;
        this.checkedListBox1.Items.AddRange(new object[] {
            "Информационная рассылка",
            "Персональные тренировки",
            "Солярий",
            "Массаж"});
        this.checkedListBox1.Location = new System.Drawing.Point(18, 185);
        this.checkedListBox1.Name = "checkedListBox1";
        this.checkedListBox1.Size = new System.Drawing.Size(253, 64);
        this.checkedListBox1.TabIndex = 4;
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(15, 169);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(131, 13);
        this.label2.TabIndex = 5;
        this.label2.Text = "Дополнительные услуги";
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(156, 267);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(115, 29);
        this.button1.TabIndex = 6;
        this.button1.Text = "Зарегистрировать";
        this.button1.UseVisualStyleBackColor = true;
        // 
        // L2S03
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(284, 307);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.checkedListBox1);
        this.Controls.Add(this.groupBox1);
        this.Controls.Add(this.checkBox1);
        this.Controls.Add(this.textBox1);
        this.Controls.Add(this.label1);
        this.Name = "L2S03";
        this.Text = "Новый клиент";
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.RadioButton radioButton2;
    private System.Windows.Forms.RadioButton radioButton1;
    private System.Windows.Forms.CheckedListBox checkedListBox1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button button1;
}