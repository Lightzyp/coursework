namespace Project3War
{
    partial class Form1
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
            this.comboBoxPlayers = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listBoxRounds = new System.Windows.Forms.ListBox();
            this.lblWinner = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblScores = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxPlayers
            // 
            this.comboBoxPlayers.FormattingEnabled = true;
            this.comboBoxPlayers.Location = new System.Drawing.Point(506, 178);
            this.comboBoxPlayers.Name = "comboBoxPlayers";
            this.comboBoxPlayers.Size = new System.Drawing.Size(188, 28);
            this.comboBoxPlayers.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(892, 178);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(324, 245);
            this.button1.TabIndex = 1;
            this.button1.Text = "btnStartGame";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnStartGame_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 178);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(297, 245);
            this.button2.TabIndex = 2;
            this.button2.Text = "btnPlayRound";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnPlayRound_Click);
            // 
            // listBoxRounds
            // 
            this.listBoxRounds.FormattingEnabled = true;
            this.listBoxRounds.ItemHeight = 20;
            this.listBoxRounds.Location = new System.Drawing.Point(452, 236);
            this.listBoxRounds.Name = "listBoxRounds";
            this.listBoxRounds.Size = new System.Drawing.Size(297, 164);
            this.listBoxRounds.TabIndex = 3;
            this.listBoxRounds.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // lblWinner
            // 
            this.lblWinner.AutoSize = true;
            this.lblWinner.Location = new System.Drawing.Point(510, 155);
            this.lblWinner.Name = "lblWinner";
            this.lblWinner.Size = new System.Drawing.Size(184, 20);
            this.lblWinner.TabIndex = 4;
            this.lblWinner.Text = "Chose how many players";
            this.lblWinner.Click += new System.EventHandler(this.lblWinner_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(587, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "WAR";
            // 
            // lblScores
            // 
            this.lblScores.AutoSize = true;
            this.lblScores.Location = new System.Drawing.Point(448, 488);
            this.lblScores.Name = "lblScores";
            this.lblScores.Size = new System.Drawing.Size(0, 20);
            this.lblScores.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1218, 703);
            this.Controls.Add(this.lblScores);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblWinner);
            this.Controls.Add(this.listBoxRounds);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBoxPlayers);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxPlayers;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox listBoxRounds;
        private System.Windows.Forms.Label lblWinner;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblScores;
    }
}

