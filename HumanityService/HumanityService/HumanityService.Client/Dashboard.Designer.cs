namespace HumanityService.Client
{
    partial class Dashboard
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
            this.DashboardPanel_TreeView = new System.Windows.Forms.TreeView();
            this.DashboardPanel_DeliveryCodeLabel = new System.Windows.Forms.Label();
            this.DashboardPanel_DeliveryCodeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.DashboardPanel_ValidateDeliveryButton = new System.Windows.Forms.Button();
            this.DashboardPanel_RefreshButton = new System.Windows.Forms.Button();
            this.DashboardPanel_Signout = new System.Windows.Forms.Button();
            this.DashboardPanel_ApproveVolunteerButton = new System.Windows.Forms.Button();
            this.DashboardPanel_ValidateContributionButton = new System.Windows.Forms.Button();
            this.DashboardPanel_CreateCampaignButton = new System.Windows.Forms.Button();
            this.DashboardPanel = new System.Windows.Forms.Panel();
            this.InfoPanel = new System.Windows.Forms.Panel();
            this.InfoPanel_OtherInfo = new System.Windows.Forms.Label();
            this.InfoPanel_Username = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.CreateCampaignPanel = new System.Windows.Forms.Panel();
            this.CreateCampaignPanel_CreateCampaignButton = new System.Windows.Forms.Button();
            this.CreateCampaignPanel_BackButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.CreateCampaignPanel_Description = new System.Windows.Forms.TextBox();
            this.CreateCampaignPanel_Target = new System.Windows.Forms.MaskedTextBox();
            this.CreateCampaignPanel_Category = new System.Windows.Forms.TextBox();
            this.CreateCampaignPanel_Type = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CreateCampaignPanel_CampaignName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.UserInfoPanel = new System.Windows.Forms.Panel();
            this.UserInfoPanel_PhoneNumber = new System.Windows.Forms.Label();
            this.UserInfoPanel_Email = new System.Windows.Forms.Label();
            this.UserInfoPanel_LastName = new System.Windows.Forms.Label();
            this.UserInfoPanel_FirstName = new System.Windows.Forms.Label();
            this.UserInfoPanel_Username = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.UserInfoPanel_BackButton = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.DashboardPanel.SuspendLayout();
            this.InfoPanel.SuspendLayout();
            this.CreateCampaignPanel.SuspendLayout();
            this.UserInfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DashboardPanel_TreeView
            // 
            this.DashboardPanel_TreeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.DashboardPanel_TreeView.Location = new System.Drawing.Point(313, 11);
            this.DashboardPanel_TreeView.Name = "DashboardPanel_TreeView";
            this.DashboardPanel_TreeView.Size = new System.Drawing.Size(812, 589);
            this.DashboardPanel_TreeView.TabIndex = 0;
            this.DashboardPanel_TreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_NodeMouseDoubleClick);
            this.DashboardPanel_TreeView.Enter += new System.EventHandler(this.TreeView_Enter);
            // 
            // DashboardPanel_DeliveryCodeLabel
            // 
            this.DashboardPanel_DeliveryCodeLabel.AutoSize = true;
            this.DashboardPanel_DeliveryCodeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.DashboardPanel_DeliveryCodeLabel.Location = new System.Drawing.Point(38, 520);
            this.DashboardPanel_DeliveryCodeLabel.Name = "DashboardPanel_DeliveryCodeLabel";
            this.DashboardPanel_DeliveryCodeLabel.Size = new System.Drawing.Size(141, 25);
            this.DashboardPanel_DeliveryCodeLabel.TabIndex = 1;
            this.DashboardPanel_DeliveryCodeLabel.Text = "Delivery Code:";
            // 
            // DashboardPanel_DeliveryCodeTextBox
            // 
            this.DashboardPanel_DeliveryCodeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.DashboardPanel_DeliveryCodeTextBox.Location = new System.Drawing.Point(198, 517);
            this.DashboardPanel_DeliveryCodeTextBox.Mask = "000000";
            this.DashboardPanel_DeliveryCodeTextBox.Name = "DashboardPanel_DeliveryCodeTextBox";
            this.DashboardPanel_DeliveryCodeTextBox.Size = new System.Drawing.Size(78, 30);
            this.DashboardPanel_DeliveryCodeTextBox.TabIndex = 2;
            // 
            // DashboardPanel_ValidateDeliveryButton
            // 
            this.DashboardPanel_ValidateDeliveryButton.Location = new System.Drawing.Point(21, 559);
            this.DashboardPanel_ValidateDeliveryButton.Name = "DashboardPanel_ValidateDeliveryButton";
            this.DashboardPanel_ValidateDeliveryButton.Size = new System.Drawing.Size(277, 40);
            this.DashboardPanel_ValidateDeliveryButton.TabIndex = 3;
            this.DashboardPanel_ValidateDeliveryButton.Text = "Validate Delivery";
            this.DashboardPanel_ValidateDeliveryButton.UseVisualStyleBackColor = true;
            this.DashboardPanel_ValidateDeliveryButton.Click += new System.EventHandler(this.ValidateDeliveryButton_Click);
            // 
            // DashboardPanel_RefreshButton
            // 
            this.DashboardPanel_RefreshButton.ForeColor = System.Drawing.Color.Green;
            this.DashboardPanel_RefreshButton.Location = new System.Drawing.Point(224, 12);
            this.DashboardPanel_RefreshButton.Name = "DashboardPanel_RefreshButton";
            this.DashboardPanel_RefreshButton.Size = new System.Drawing.Size(74, 33);
            this.DashboardPanel_RefreshButton.TabIndex = 4;
            this.DashboardPanel_RefreshButton.Text = "Refresh";
            this.DashboardPanel_RefreshButton.UseVisualStyleBackColor = true;
            this.DashboardPanel_RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // DashboardPanel_Signout
            // 
            this.DashboardPanel_Signout.ForeColor = System.Drawing.Color.Red;
            this.DashboardPanel_Signout.Location = new System.Drawing.Point(22, 12);
            this.DashboardPanel_Signout.Name = "DashboardPanel_Signout";
            this.DashboardPanel_Signout.Size = new System.Drawing.Size(80, 33);
            this.DashboardPanel_Signout.TabIndex = 5;
            this.DashboardPanel_Signout.Text = "Sign Out";
            this.DashboardPanel_Signout.UseVisualStyleBackColor = true;
            this.DashboardPanel_Signout.Click += new System.EventHandler(this.Signout_Click);
            // 
            // DashboardPanel_ApproveVolunteerButton
            // 
            this.DashboardPanel_ApproveVolunteerButton.Location = new System.Drawing.Point(21, 441);
            this.DashboardPanel_ApproveVolunteerButton.Name = "DashboardPanel_ApproveVolunteerButton";
            this.DashboardPanel_ApproveVolunteerButton.Size = new System.Drawing.Size(277, 48);
            this.DashboardPanel_ApproveVolunteerButton.TabIndex = 6;
            this.DashboardPanel_ApproveVolunteerButton.Text = "Approve Volunteer";
            this.DashboardPanel_ApproveVolunteerButton.UseVisualStyleBackColor = true;
            this.DashboardPanel_ApproveVolunteerButton.Click += new System.EventHandler(this.ApproveVolunteerButton_Click);
            // 
            // DashboardPanel_ValidateContributionButton
            // 
            this.DashboardPanel_ValidateContributionButton.Location = new System.Drawing.Point(21, 441);
            this.DashboardPanel_ValidateContributionButton.Name = "DashboardPanel_ValidateContributionButton";
            this.DashboardPanel_ValidateContributionButton.Size = new System.Drawing.Size(277, 48);
            this.DashboardPanel_ValidateContributionButton.TabIndex = 7;
            this.DashboardPanel_ValidateContributionButton.Text = "Validate Contribution";
            this.DashboardPanel_ValidateContributionButton.UseVisualStyleBackColor = true;
            this.DashboardPanel_ValidateContributionButton.Click += new System.EventHandler(this.ValidateContributionButton_Click);
            // 
            // DashboardPanel_CreateCampaignButton
            // 
            this.DashboardPanel_CreateCampaignButton.Location = new System.Drawing.Point(22, 72);
            this.DashboardPanel_CreateCampaignButton.Name = "DashboardPanel_CreateCampaignButton";
            this.DashboardPanel_CreateCampaignButton.Size = new System.Drawing.Size(276, 40);
            this.DashboardPanel_CreateCampaignButton.TabIndex = 8;
            this.DashboardPanel_CreateCampaignButton.Text = "Create Campaign";
            this.DashboardPanel_CreateCampaignButton.UseVisualStyleBackColor = true;
            this.DashboardPanel_CreateCampaignButton.Click += new System.EventHandler(this.DashboardPanel_CreateCampaignButton_Click);
            // 
            // DashboardPanel
            // 
            this.DashboardPanel.Controls.Add(this.InfoPanel);
            this.DashboardPanel.Controls.Add(this.DashboardPanel_CreateCampaignButton);
            this.DashboardPanel.Controls.Add(this.DashboardPanel_ValidateContributionButton);
            this.DashboardPanel.Controls.Add(this.DashboardPanel_ApproveVolunteerButton);
            this.DashboardPanel.Controls.Add(this.DashboardPanel_Signout);
            this.DashboardPanel.Controls.Add(this.DashboardPanel_RefreshButton);
            this.DashboardPanel.Controls.Add(this.DashboardPanel_ValidateDeliveryButton);
            this.DashboardPanel.Controls.Add(this.DashboardPanel_DeliveryCodeTextBox);
            this.DashboardPanel.Controls.Add(this.DashboardPanel_DeliveryCodeLabel);
            this.DashboardPanel.Controls.Add(this.DashboardPanel_TreeView);
            this.DashboardPanel.Location = new System.Drawing.Point(6, 7);
            this.DashboardPanel.Name = "DashboardPanel";
            this.DashboardPanel.Size = new System.Drawing.Size(1125, 612);
            this.DashboardPanel.TabIndex = 9;
            // 
            // InfoPanel
            // 
            this.InfoPanel.BackColor = System.Drawing.Color.DarkGray;
            this.InfoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoPanel.Controls.Add(this.InfoPanel_OtherInfo);
            this.InfoPanel.Controls.Add(this.InfoPanel_Username);
            this.InfoPanel.Controls.Add(this.label8);
            this.InfoPanel.Controls.Add(this.label7);
            this.InfoPanel.Location = new System.Drawing.Point(22, 142);
            this.InfoPanel.Name = "InfoPanel";
            this.InfoPanel.Size = new System.Drawing.Size(276, 285);
            this.InfoPanel.TabIndex = 9;
            // 
            // InfoPanel_OtherInfo
            // 
            this.InfoPanel_OtherInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.InfoPanel_OtherInfo.Location = new System.Drawing.Point(11, 100);
            this.InfoPanel_OtherInfo.Name = "InfoPanel_OtherInfo";
            this.InfoPanel_OtherInfo.Size = new System.Drawing.Size(242, 172);
            this.InfoPanel_OtherInfo.TabIndex = 3;
            this.InfoPanel_OtherInfo.Text = "label10";
            // 
            // InfoPanel_Username
            // 
            this.InfoPanel_Username.AutoSize = true;
            this.InfoPanel_Username.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.InfoPanel_Username.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.InfoPanel_Username.Location = new System.Drawing.Point(126, 16);
            this.InfoPanel_Username.Name = "InfoPanel_Username";
            this.InfoPanel_Username.Size = new System.Drawing.Size(60, 24);
            this.InfoPanel_Username.TabIndex = 2;
            this.InfoPanel_Username.Text = "label9";
            this.InfoPanel_Username.Click += new System.EventHandler(this.InfoPanel_Username_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(16, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(164, 24);
            this.label8.TabIndex = 1;
            this.label8.Text = "User\'s Message:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(16, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 24);
            this.label7.TabIndex = 0;
            this.label7.Text = "Username:";
            // 
            // CreateCampaignPanel
            // 
            this.CreateCampaignPanel.Controls.Add(this.CreateCampaignPanel_CreateCampaignButton);
            this.CreateCampaignPanel.Controls.Add(this.CreateCampaignPanel_BackButton);
            this.CreateCampaignPanel.Controls.Add(this.label6);
            this.CreateCampaignPanel.Controls.Add(this.CreateCampaignPanel_Description);
            this.CreateCampaignPanel.Controls.Add(this.CreateCampaignPanel_Target);
            this.CreateCampaignPanel.Controls.Add(this.CreateCampaignPanel_Category);
            this.CreateCampaignPanel.Controls.Add(this.CreateCampaignPanel_Type);
            this.CreateCampaignPanel.Controls.Add(this.label5);
            this.CreateCampaignPanel.Controls.Add(this.label4);
            this.CreateCampaignPanel.Controls.Add(this.label3);
            this.CreateCampaignPanel.Controls.Add(this.label2);
            this.CreateCampaignPanel.Controls.Add(this.CreateCampaignPanel_CampaignName);
            this.CreateCampaignPanel.Controls.Add(this.label1);
            this.CreateCampaignPanel.Location = new System.Drawing.Point(9, 0);
            this.CreateCampaignPanel.Name = "CreateCampaignPanel";
            this.CreateCampaignPanel.Size = new System.Drawing.Size(1122, 606);
            this.CreateCampaignPanel.TabIndex = 11;
            // 
            // CreateCampaignPanel_CreateCampaignButton
            // 
            this.CreateCampaignPanel_CreateCampaignButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.CreateCampaignPanel_CreateCampaignButton.Location = new System.Drawing.Point(437, 508);
            this.CreateCampaignPanel_CreateCampaignButton.Name = "CreateCampaignPanel_CreateCampaignButton";
            this.CreateCampaignPanel_CreateCampaignButton.Size = new System.Drawing.Size(211, 44);
            this.CreateCampaignPanel_CreateCampaignButton.TabIndex = 12;
            this.CreateCampaignPanel_CreateCampaignButton.Text = "Create Campaign";
            this.CreateCampaignPanel_CreateCampaignButton.UseVisualStyleBackColor = true;
            // 
            // CreateCampaignPanel_BackButton
            // 
            this.CreateCampaignPanel_BackButton.Location = new System.Drawing.Point(19, 18);
            this.CreateCampaignPanel_BackButton.Name = "CreateCampaignPanel_BackButton";
            this.CreateCampaignPanel_BackButton.Size = new System.Drawing.Size(71, 27);
            this.CreateCampaignPanel_BackButton.TabIndex = 11;
            this.CreateCampaignPanel_BackButton.Text = "Back";
            this.CreateCampaignPanel_BackButton.UseVisualStyleBackColor = true;
            this.CreateCampaignPanel_BackButton.Click += new System.EventHandler(this.CreateCampaignPanel_BackButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(432, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(216, 29);
            this.label6.TabIndex = 10;
            this.label6.Text = "Create Campaign";
            // 
            // CreateCampaignPanel_Description
            // 
            this.CreateCampaignPanel_Description.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.CreateCampaignPanel_Description.Location = new System.Drawing.Point(273, 393);
            this.CreateCampaignPanel_Description.Multiline = true;
            this.CreateCampaignPanel_Description.Name = "CreateCampaignPanel_Description";
            this.CreateCampaignPanel_Description.Size = new System.Drawing.Size(533, 94);
            this.CreateCampaignPanel_Description.TabIndex = 9;
            // 
            // CreateCampaignPanel_Target
            // 
            this.CreateCampaignPanel_Target.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.CreateCampaignPanel_Target.Location = new System.Drawing.Point(273, 307);
            this.CreateCampaignPanel_Target.Mask = "000000000";
            this.CreateCampaignPanel_Target.Name = "CreateCampaignPanel_Target";
            this.CreateCampaignPanel_Target.Size = new System.Drawing.Size(121, 30);
            this.CreateCampaignPanel_Target.TabIndex = 8;
            // 
            // CreateCampaignPanel_Category
            // 
            this.CreateCampaignPanel_Category.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.CreateCampaignPanel_Category.Location = new System.Drawing.Point(273, 231);
            this.CreateCampaignPanel_Category.Name = "CreateCampaignPanel_Category";
            this.CreateCampaignPanel_Category.Size = new System.Drawing.Size(194, 30);
            this.CreateCampaignPanel_Category.TabIndex = 7;
            // 
            // CreateCampaignPanel_Type
            // 
            this.CreateCampaignPanel_Type.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.CreateCampaignPanel_Type.FormattingEnabled = true;
            this.CreateCampaignPanel_Type.Items.AddRange(new object[] {
            "Donation",
            "Volunteering"});
            this.CreateCampaignPanel_Type.Location = new System.Drawing.Point(273, 157);
            this.CreateCampaignPanel_Type.Name = "CreateCampaignPanel_Type";
            this.CreateCampaignPanel_Type.Size = new System.Drawing.Size(121, 33);
            this.CreateCampaignPanel_Type.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(97, 160);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 25);
            this.label5.TabIndex = 5;
            this.label5.Text = "Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label4.Location = new System.Drawing.Point(97, 234);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "Category";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label3.Location = new System.Drawing.Point(97, 310);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "Target";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(97, 384);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Description";
            // 
            // CreateCampaignPanel_CampaignName
            // 
            this.CreateCampaignPanel_CampaignName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.CreateCampaignPanel_CampaignName.Location = new System.Drawing.Point(273, 87);
            this.CreateCampaignPanel_CampaignName.Name = "CreateCampaignPanel_CampaignName";
            this.CreateCampaignPanel_CampaignName.Size = new System.Drawing.Size(608, 30);
            this.CreateCampaignPanel_CampaignName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(97, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Campaign Name";
            // 
            // UserInfoPanel
            // 
            this.UserInfoPanel.Controls.Add(this.UserInfoPanel_PhoneNumber);
            this.UserInfoPanel.Controls.Add(this.UserInfoPanel_Email);
            this.UserInfoPanel.Controls.Add(this.UserInfoPanel_LastName);
            this.UserInfoPanel.Controls.Add(this.UserInfoPanel_FirstName);
            this.UserInfoPanel.Controls.Add(this.UserInfoPanel_Username);
            this.UserInfoPanel.Controls.Add(this.label9);
            this.UserInfoPanel.Controls.Add(this.label10);
            this.UserInfoPanel.Controls.Add(this.label11);
            this.UserInfoPanel.Controls.Add(this.label12);
            this.UserInfoPanel.Controls.Add(this.label13);
            this.UserInfoPanel.Controls.Add(this.UserInfoPanel_BackButton);
            this.UserInfoPanel.Controls.Add(this.label14);
            this.UserInfoPanel.Location = new System.Drawing.Point(6, 0);
            this.UserInfoPanel.Name = "UserInfoPanel";
            this.UserInfoPanel.Size = new System.Drawing.Size(1122, 616);
            this.UserInfoPanel.TabIndex = 11;
            // 
            // UserInfoPanel_PhoneNumber
            // 
            this.UserInfoPanel_PhoneNumber.AutoSize = true;
            this.UserInfoPanel_PhoneNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.UserInfoPanel_PhoneNumber.Location = new System.Drawing.Point(227, 327);
            this.UserInfoPanel_PhoneNumber.Name = "UserInfoPanel_PhoneNumber";
            this.UserInfoPanel_PhoneNumber.Size = new System.Drawing.Size(75, 25);
            this.UserInfoPanel_PhoneNumber.TabIndex = 21;
            this.UserInfoPanel_PhoneNumber.Text = "label11";
            // 
            // UserInfoPanel_Email
            // 
            this.UserInfoPanel_Email.AutoSize = true;
            this.UserInfoPanel_Email.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.UserInfoPanel_Email.Location = new System.Drawing.Point(227, 265);
            this.UserInfoPanel_Email.Name = "UserInfoPanel_Email";
            this.UserInfoPanel_Email.Size = new System.Drawing.Size(75, 25);
            this.UserInfoPanel_Email.TabIndex = 20;
            this.UserInfoPanel_Email.Text = "label10";
            // 
            // UserInfoPanel_LastName
            // 
            this.UserInfoPanel_LastName.AutoSize = true;
            this.UserInfoPanel_LastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.UserInfoPanel_LastName.Location = new System.Drawing.Point(227, 203);
            this.UserInfoPanel_LastName.Name = "UserInfoPanel_LastName";
            this.UserInfoPanel_LastName.Size = new System.Drawing.Size(64, 25);
            this.UserInfoPanel_LastName.TabIndex = 19;
            this.UserInfoPanel_LastName.Text = "label9";
            // 
            // UserInfoPanel_FirstName
            // 
            this.UserInfoPanel_FirstName.AutoSize = true;
            this.UserInfoPanel_FirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.UserInfoPanel_FirstName.Location = new System.Drawing.Point(227, 143);
            this.UserInfoPanel_FirstName.Name = "UserInfoPanel_FirstName";
            this.UserInfoPanel_FirstName.Size = new System.Drawing.Size(64, 25);
            this.UserInfoPanel_FirstName.TabIndex = 18;
            this.UserInfoPanel_FirstName.Text = "label8";
            // 
            // UserInfoPanel_Username
            // 
            this.UserInfoPanel_Username.AutoSize = true;
            this.UserInfoPanel_Username.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.UserInfoPanel_Username.Location = new System.Drawing.Point(227, 85);
            this.UserInfoPanel_Username.Name = "UserInfoPanel_Username";
            this.UserInfoPanel_Username.Size = new System.Drawing.Size(64, 25);
            this.UserInfoPanel_Username.TabIndex = 17;
            this.UserInfoPanel_Username.Text = "label7";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(43, 327);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(162, 25);
            this.label9.TabIndex = 16;
            this.label9.Text = "Phone Number:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(43, 265);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 25);
            this.label10.TabIndex = 15;
            this.label10.Text = "Email:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(43, 203);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(122, 25);
            this.label11.TabIndex = 14;
            this.label11.Text = "Last Name:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(43, 143);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(123, 25);
            this.label12.TabIndex = 13;
            this.label12.Text = "First Name:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(43, 85);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(117, 25);
            this.label13.TabIndex = 12;
            this.label13.Text = "Username:";
            // 
            // UserInfoPanel_BackButton
            // 
            this.UserInfoPanel_BackButton.Location = new System.Drawing.Point(19, 18);
            this.UserInfoPanel_BackButton.Name = "UserInfoPanel_BackButton";
            this.UserInfoPanel_BackButton.Size = new System.Drawing.Size(71, 27);
            this.UserInfoPanel_BackButton.TabIndex = 11;
            this.UserInfoPanel_BackButton.Text = "Back";
            this.UserInfoPanel_BackButton.UseVisualStyleBackColor = true;
            this.UserInfoPanel_BackButton.Click += new System.EventHandler(this.UserInfoPanel_BackButton_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(429, 18);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(205, 29);
            this.label14.TabIndex = 10;
            this.label14.Text = "User Information";
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 621);
            this.Controls.Add(this.DashboardPanel);
            this.Controls.Add(this.CreateCampaignPanel);
            this.Controls.Add(this.UserInfoPanel);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.DashboardPanel.ResumeLayout(false);
            this.DashboardPanel.PerformLayout();
            this.InfoPanel.ResumeLayout(false);
            this.InfoPanel.PerformLayout();
            this.CreateCampaignPanel.ResumeLayout(false);
            this.CreateCampaignPanel.PerformLayout();
            this.UserInfoPanel.ResumeLayout(false);
            this.UserInfoPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView DashboardPanel_TreeView;
        private System.Windows.Forms.Label DashboardPanel_DeliveryCodeLabel;
        private System.Windows.Forms.MaskedTextBox DashboardPanel_DeliveryCodeTextBox;
        private System.Windows.Forms.Button DashboardPanel_ValidateDeliveryButton;
        private System.Windows.Forms.Button DashboardPanel_RefreshButton;
        private System.Windows.Forms.Button DashboardPanel_Signout;
        private System.Windows.Forms.Button DashboardPanel_ApproveVolunteerButton;
        private System.Windows.Forms.Button DashboardPanel_ValidateContributionButton;
        private System.Windows.Forms.Button DashboardPanel_CreateCampaignButton;
        private System.Windows.Forms.Panel DashboardPanel;
        private System.Windows.Forms.Panel CreateCampaignPanel;
        private System.Windows.Forms.Button CreateCampaignPanel_CreateCampaignButton;
        private System.Windows.Forms.Button CreateCampaignPanel_BackButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox CreateCampaignPanel_Description;
        private System.Windows.Forms.MaskedTextBox CreateCampaignPanel_Target;
        private System.Windows.Forms.TextBox CreateCampaignPanel_Category;
        private System.Windows.Forms.ComboBox CreateCampaignPanel_Type;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CreateCampaignPanel_CampaignName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel InfoPanel;
        private System.Windows.Forms.Label InfoPanel_OtherInfo;
        private System.Windows.Forms.Label InfoPanel_Username;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel UserInfoPanel;
        private System.Windows.Forms.Label UserInfoPanel_PhoneNumber;
        private System.Windows.Forms.Label UserInfoPanel_Email;
        private System.Windows.Forms.Label UserInfoPanel_LastName;
        private System.Windows.Forms.Label UserInfoPanel_FirstName;
        private System.Windows.Forms.Label UserInfoPanel_Username;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button UserInfoPanel_BackButton;
        private System.Windows.Forms.Label label14;
    }
}