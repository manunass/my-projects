using HumanityService.DataContracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HumanityService.Client
{
    public partial class Dashboard : Form
    {
        private readonly List<Panel> panels = new List<Panel>();
        private readonly HumanityServiceClient client;

        private Campaign SelectedCampaign { get; set; }
        private Contribution SelectedContribution { get; set; }
        public Dashboard()
        {
            InitializeComponent();
            client = new HumanityServiceClient();
            DashboardPanel_TreeView.Focus();
            HideAll();
        }

        private void Navigate(Panel destinationPanel)
        {
            foreach (var panel in panels)
            {
                if (panel == destinationPanel)
                {
                    panel.Show();
                    panel.Focus();
                }
                else panel.Hide();
            }
        }


        private async void TreeView_Enter(object sender, EventArgs e)
        {
            HideAll();
            DashboardPanel_TreeView.Nodes.Clear();
            var username = Properties.Settings.Default["Username"].ToString();
            var getCampaignsResult = await client.GetCampaigns(username);
            foreach (var campaign in getCampaignsResult.Campaigns)
            {
                TreeNode nodeLevel1 = DashboardPanel_TreeView.Nodes.Add(campaign.Name + " (" + campaign.Status + ")");
                nodeLevel1.Tag = campaign;

                var getProcessesResult = await client.GetProcesses(campaign.Id);
                foreach (var process in getProcessesResult.Processes)
                {
                    TreeNode nodeLevel2 = nodeLevel1.Nodes.Add("Process (" + process.Status + ")");
                    nodeLevel2.Tag = process;
                    if(process.Status == "Pending")
                    {
                        Color color = Color.FromArgb(0, 240, 240);
                        nodeLevel2.BackColor = color;
                    }
                    var getDeliveryDemandsResult = await client.GetDeliveryDemands(process.Id);
                    if (getDeliveryDemandsResult.DeliveryDemands.Count != 0)
                    {
                        var deliveryDemand = getDeliveryDemandsResult.DeliveryDemands[0];
                        TreeNode node = nodeLevel2.Nodes.Add("Delivery Demand (" + deliveryDemand.Status + ")");
                        node.Tag = deliveryDemand;
                    }
                    var getContributionsResult = await client.GetContributions(processId: process.Id);
                    foreach (var contribution in getContributionsResult.Contributions)
                    {
                        if (contribution.Type != "Delivery")
                        {
                            TreeNode node = nodeLevel2.Nodes.Add("Contribution (" + contribution.Status + ")");
                            node.Tag = contribution;
                        }
                        else
                        {
                            TreeNode node = nodeLevel2.Nodes[0].Nodes.Add("Contribution (" + contribution.Status + ")");
                            node.Tag = contribution;
                        }
                    }
                }
            }
            for(var i=0; i<DashboardPanel_TreeView.Nodes.Count; i++)
            {
                DashboardPanel_TreeView.Nodes[i].Expand();
            }
        }

        private void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            HideAll();
            if(DashboardPanel_TreeView.SelectedNode != null)
            {
                IComponent component = (IComponent)DashboardPanel_TreeView.SelectedNode.Tag;
                if (component is Campaign)
                {
                    SelectedCampaign = ((Campaign)DashboardPanel_TreeView.SelectedNode.Tag);
                    if (SelectedCampaign.Type == "Donation")
                    {
                        DashboardPanel_DeliveryCodeLabel.Show();
                        DashboardPanel_DeliveryCodeTextBox.Show();
                        DashboardPanel_ValidateDeliveryButton.Show();
                    }
                }
                else if (component is Contribution)
                {
                    SelectedContribution = (Contribution)DashboardPanel_TreeView.SelectedNode.Tag;
                    InfoPanel_Username.Text = SelectedContribution.Username;
                    InfoPanel_OtherInfo.Text = SelectedContribution.OtherInfo;
                    InfoPanel.Show();
                    if (SelectedContribution.Type == "Volunteering" && SelectedContribution.Status == "Pending")
                    {
                        DashboardPanel_ApproveVolunteerButton.Show();
                    }
                    else if (SelectedContribution.Type == "Volunteering" && SelectedContribution.Status == "InProgress")
                    {
                        DashboardPanel_ValidateContributionButton.Show();
                    }
                }
            }
        }

        private async void ValidateDeliveryButton_Click(object sender, EventArgs e)
        {
            var validateDeliveryRequest = new ValidateDeliveryRequest
            {
                ValidationType = "Destination",
                CampaignId = SelectedCampaign.Id,
                DeliveryCode = DashboardPanel_DeliveryCodeTextBox.Text
            };

            var result = await client.ValidateDelivery(validateDeliveryRequest);
            string message;
            if (result.IsValid)
            {
                message = "Delivery Validated!";
                DashboardPanel_TreeView.Focus();
            }
            else
            {
                message = "Wrong Delivery Code.";
            }
            MessageBox.Show(message);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            DashboardPanel_TreeView.Focus();
        }

        private void Signout_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["Username"] = "";
            Properties.Settings.Default.Save();
            WelcomeScreen welcomeScreen = new WelcomeScreen();
            this.Hide();
            welcomeScreen.Show();
        }

        private async void ValidateContributionButton_Click(object sender, EventArgs e)
        {
            await client.ValidateContribution(SelectedContribution.Id);
            DashboardPanel_TreeView.Focus();
        }

        private async void ApproveVolunteerButton_Click(object sender, EventArgs e)
        {
            await client.ApproveContribution(SelectedContribution.Id);
            DashboardPanel_TreeView.Focus();
        }

        private void HideAll()
        {
            InfoPanel.Hide();
            DashboardPanel_DeliveryCodeLabel.Hide();
            DashboardPanel_DeliveryCodeTextBox.Hide();
            DashboardPanel_ValidateDeliveryButton.Hide();
            DashboardPanel_ValidateContributionButton.Hide();
            DashboardPanel_ApproveVolunteerButton.Hide();
        }

        private void CreateCampaignPanel_BackButton_Click(object sender, EventArgs e)
        {
            Navigate(DashboardPanel);
        }

        private void DashboardPanel_CreateCampaignButton_Click(object sender, EventArgs e)
        {
            Navigate(CreateCampaignPanel);
        }

        private void UserInfoPanel_BackButton_Click(object sender, EventArgs e)
        {
            Navigate(DashboardPanel);
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            panels.Add(DashboardPanel);
            panels.Add(CreateCampaignPanel);
            panels.Add(UserInfoPanel);
            Navigate(DashboardPanel);
        }

        private async void InfoPanel_Username_Click(object sender, EventArgs e)
        {
            var username = InfoPanel_Username.Text.ToString();
            var userInfo = await client.GetUserInfo(username);
            UserInfoPanel_Username.Text = userInfo.Username;
            UserInfoPanel_FirstName.Text = userInfo.FirstName;
            UserInfoPanel_LastName.Text = userInfo.LastName;
            UserInfoPanel_PhoneNumber.Text = userInfo.PhoneNumber;
            UserInfoPanel_Email.Text = userInfo.Email;
            Navigate(UserInfoPanel);
        }
    }
}
