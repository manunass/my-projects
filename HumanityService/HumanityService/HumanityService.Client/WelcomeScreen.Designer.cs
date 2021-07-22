namespace HumanityService.Client
{
    partial class WelcomeScreen
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
            this.NgoButton = new System.Windows.Forms.Button();
            this.ContributorButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.WelcomePanel = new System.Windows.Forms.Panel();
            this.SigninPanel = new System.Windows.Forms.Panel();
            this.BackButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SigninButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.WelcomePanel.SuspendLayout();
            this.SigninPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // NgoButton
            // 
            this.NgoButton.Location = new System.Drawing.Point(70, 370);
            this.NgoButton.Name = "NgoButton";
            this.NgoButton.Size = new System.Drawing.Size(261, 57);
            this.NgoButton.TabIndex = 9;
            this.NgoButton.Text = "NGO";
            this.NgoButton.UseVisualStyleBackColor = true;
            this.NgoButton.Click += new System.EventHandler(this.NgoButton_Click);
            // 
            // ContributorButton
            // 
            this.ContributorButton.Location = new System.Drawing.Point(70, 238);
            this.ContributorButton.Name = "ContributorButton";
            this.ContributorButton.Size = new System.Drawing.Size(261, 61);
            this.ContributorButton.TabIndex = 8;
            this.ContributorButton.Text = "Contributor";
            this.ContributorButton.UseVisualStyleBackColor = true;
            this.ContributorButton.Click += new System.EventHandler(this.ContributorButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(169, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "You are:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label1.Location = new System.Drawing.Point(16, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(348, 29);
            this.label1.TabIndex = 10;
            this.label1.Text = "Welcome to HumanityService";
            // 
            // WelcomePanel
            // 
            this.WelcomePanel.Controls.Add(this.label1);
            this.WelcomePanel.Controls.Add(this.label2);
            this.WelcomePanel.Controls.Add(this.ContributorButton);
            this.WelcomePanel.Controls.Add(this.NgoButton);
            this.WelcomePanel.Location = new System.Drawing.Point(22, 24);
            this.WelcomePanel.Name = "WelcomePanel";
            this.WelcomePanel.Size = new System.Drawing.Size(386, 530);
            this.WelcomePanel.TabIndex = 12;
            // 
            // SigninPanel
            // 
            this.SigninPanel.Controls.Add(this.BackButton);
            this.SigninPanel.Controls.Add(this.button2);
            this.SigninPanel.Controls.Add(this.SigninButton);
            this.SigninPanel.Controls.Add(this.label3);
            this.SigninPanel.Controls.Add(this.PasswordTextBox);
            this.SigninPanel.Controls.Add(this.label4);
            this.SigninPanel.Controls.Add(this.UsernameTextBox);
            this.SigninPanel.Location = new System.Drawing.Point(12, 9);
            this.SigninPanel.Name = "SigninPanel";
            this.SigninPanel.Size = new System.Drawing.Size(407, 567);
            this.SigninPanel.TabIndex = 13;
            // 
            // BackButton
            // 
            this.BackButton.Location = new System.Drawing.Point(14, 17);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(66, 29);
            this.BackButton.TabIndex = 18;
            this.BackButton.Text = "Back";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(80, 378);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(245, 45);
            this.button2.TabIndex = 17;
            this.button2.Text = "Sign Up";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // SigninButton
            // 
            this.SigninButton.Location = new System.Drawing.Point(80, 304);
            this.SigninButton.Name = "SigninButton";
            this.SigninButton.Size = new System.Drawing.Size(245, 43);
            this.SigninButton.TabIndex = 16;
            this.SigninButton.Text = "Sign In";
            this.SigninButton.UseVisualStyleBackColor = true;
            this.SigninButton.Click += new System.EventHandler(this.SigninButton_ClickAsync);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(75, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 25);
            this.label3.TabIndex = 15;
            this.label3.Text = "Password";
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.PasswordTextBox.Location = new System.Drawing.Point(80, 203);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(245, 30);
            this.PasswordTextBox.TabIndex = 14;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(75, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 25);
            this.label4.TabIndex = 13;
            this.label4.Text = "Username";
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.UsernameTextBox.Location = new System.Drawing.Point(80, 106);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(245, 30);
            this.UsernameTextBox.TabIndex = 12;
            // 
            // WelcomeScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(431, 588);
            this.Controls.Add(this.SigninPanel);
            this.Controls.Add(this.WelcomePanel);
            this.Name = "WelcomeScreen";
            this.Text = "WelcomeScreen";
            this.Load += new System.EventHandler(this.WelcomeScreen_Load);
            this.WelcomePanel.ResumeLayout(false);
            this.WelcomePanel.PerformLayout();
            this.SigninPanel.ResumeLayout(false);
            this.SigninPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button NgoButton;
        private System.Windows.Forms.Button ContributorButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel WelcomePanel;
        private System.Windows.Forms.Panel SigninPanel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button SigninButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.Button BackButton;
    }
}