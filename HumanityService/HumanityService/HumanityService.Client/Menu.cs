using HumanityService.DataContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace HumanityService.Client
{
    
    public partial class Menu : Form
    {
        private readonly HumanityServiceClient client;
        private readonly List<Panel> panels = new List<Panel>();
        private readonly Dictionary<string, string> transportationType = new Dictionary<string, string>();

        private Campaign matchedCampaign;
        private DeliveryDemand matchedDeliveryDemand;
        private double ETA;
        private string volunteeringTag;
        private string contributionIdTag;
        private Location donorLocation = new Location();
        private Location ngoLocation = new Location();

        public Menu()
        {
            client = new HumanityServiceClient();
            InitializeComponent();
            transportationType.Add("Car", "driving-car");
            transportationType.Add("Walking", "cycling-regular");
            transportationType.Add("Cycling", "foot-walking");
        }

        private void Navigate(Panel destinationPanel)
        {
            foreach(var panel in panels)
            {
                if (panel == destinationPanel)
                {
                    panel.Show();
                    panel.Focus();
                }
                else panel.Hide();
            }
        }

        private void SignoutButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["Username"] = "";
            Properties.Settings.Default.Save();
            WelcomeScreen welcomeScreen = new WelcomeScreen();
            this.Hide();
            welcomeScreen.Show();
        }

        private async void FindCampaignMatchAsync(object sender, EventArgs e)
        {
            var tag = ((Button)sender).Tag.ToString();
            var matchCampaignRequest = new MatchCampaignRequest
            {
                Type = "Donation",
                Category = tag
            };
            matchedCampaign = await client.MatchCampaign(matchCampaignRequest);
            Navigate(DonationCampaignMatchPanel);
        }

        private void DonationCampaignMatchPanel_Paint(object sender, PaintEventArgs e)
        {
            DonationCampaignMatchPanel_CampaignNameLabel.Text = matchedCampaign.Name;
            DonationCampaignMatchPanel_NgoLabel.Text = matchedCampaign.NgoName;
            DonationCampaignMatchPanel_DescriptionLabel.Text = matchedCampaign.Description;
        }

        private async void AnswerDonationCampaignAsync(object sender, EventArgs e)
        {
            long timeWindowStart;
            long timeWindowEnd;
            if (DonationInfoPanel_Checkbox.Checked)
            {
                timeWindowStart = 0L;
                timeWindowEnd = 9999999999;
            }
            else
            {
                timeWindowStart = ToUnixTime(DonationInfoPanel_DatePicker.Value, DonationInfoPanel_TimePicker1.Value);
                timeWindowEnd = ToUnixTime(DonationInfoPanel_DatePicker.Value, DonationInfoPanel_TimePicker2.Value);
            }
            var answerCampaignRequest = new AnswerCampaignRequest
            {
                Username = Properties.Settings.Default["Username"].ToString(),
                TimeWindowStart = timeWindowStart,
                TimeWindowEnd = timeWindowEnd,
                OtherInfo = DonationInfoPanel_OtherInfo.Text
            };
            await client.AnswerCampaign(matchedCampaign.Id, answerCampaignRequest);
            MessageBox.Show("Thank you for your donation! Please wait until a volunteer picks it up. Meanwhile, you can check the status of your contribution in \"My Contributions\".");
            Navigate(MenuPanel);
        }

        private async void DeliveryInfoPanel_FindDeliveryDemandMatch_Click(object sender, EventArgs e)
        {
            var timeWindowStart = ToUnixTime(DeliveryInfoPanel_DatePicker.Value, DeliveryInfoPanel_TimePicker1.Value);
            var timeWindowEnd = ToUnixTime(DeliveryInfoPanel_DatePicker.Value, DeliveryInfoPanel_TimePicker2.Value);
            var location = new Location
            {
                Longitude = Convert.ToDouble(DeliveryInfoPanel_Longitude.Text),
                Latitude = Convert.ToDouble(DeliveryInfoPanel_Latitude.Text)
            };
            var matchDeliveryDemandRequest = new MatchDeliveryDemandRequest
            {
                TimeWindowStart = timeWindowStart,
                TimeWindowEnd = timeWindowEnd,
                DelivererLocation = location,
                TransportationType = transportationType[DeliveryInfoPanel_TransportationType.Text]
            };
            var matchDeliveryDemandResult = await client.MatchDeliveryDemand(matchDeliveryDemandRequest);
            matchedDeliveryDemand = matchDeliveryDemandResult.DeliveryDemand;
            ETA = matchDeliveryDemandResult.ETA;
            Navigate(DeliveryDemandMatchPanel);
        }

        private async void DeliveryDemandMatchPanel_Paint(object sender, PaintEventArgs e)
        {
            var donor = await client.GetUserInfo(matchedDeliveryDemand.PickupUsername);
            var ngo = await client.GetNgoInfo(matchedDeliveryDemand.DestinationUsername);
            donorLocation = donor.Location;
            ngoLocation = ngo.Location;

            var etaMinutes = Math.Ceiling(ETA / 60);
            var etaString = etaMinutes.ToString();
            DeliveryDemandMatchPanel_CampaignNameLabel.Text = matchedDeliveryDemand.CampaignName;
            DeliveryDemandMatchPanel_ETAValue.Text = etaString + " minutes";
            DeliveryDemandMatchPanel_InstructionsValue.Text = matchedDeliveryDemand.OtherInfo;


        }

        private async void DeliveryDemandMatchPanel_AnswerCampaignButton_Click(object sender, EventArgs e)
        {
            var timeWindowStart = ToUnixTime(DeliveryInfoPanel_DatePicker.Value, DeliveryInfoPanel_TimePicker1.Value);
            var timeWindowEnd = ToUnixTime(DeliveryInfoPanel_DatePicker.Value, DeliveryInfoPanel_TimePicker2.Value);
            var answerDeliveryDemandRequest = new AnswerDeliveryDemandRequest
            {
                Username = Properties.Settings.Default["Username"].ToString(),
                TimeWindowStart = timeWindowStart,
                TimeWindowEnd = timeWindowEnd
            };
            await client.AnswerDeliveryDemand(matchedDeliveryDemand.Id, answerDeliveryDemandRequest);
            MessageBox.Show("Thank you for your intiative! You can access more details on the pickup and delivery locations in \"My Contributions\".");
            Navigate(MenuPanel);
        }

        private async void VolunteeringInfoPanel_FindMatch_Click(object sender, EventArgs e)
        {
            var location = new Location
            {
                Longitude = Convert.ToDouble(VolunteeringInfoPanel_Longitude.Text),
                Latitude = Convert.ToDouble(VolunteeringInfoPanel_Latitude.Text)
            };
            var matchCampaignRequest = new MatchCampaignRequest
            {
                Type = "Volunteering",
                Category = volunteeringTag,
                TransportationType = transportationType[VolunteeringInfoPanel_TransportationType.Text],
                Location = location
            };
            matchedCampaign = await client.MatchCampaign(matchCampaignRequest);
            Navigate(VolunteeringCampaignMatchPanel);
        }

        private void VolunteeringCampaignMatchPanel_Paint(object sender, PaintEventArgs e)
        {
            VolunteeringCampaignMatchPanel_CampaignNameLabel.Text = matchedCampaign.Name;
            VolunteeringCampaignMatchPanel_NgoLabel.Text = matchedCampaign.NgoName;
            VolunteeringCampaignMatchPanel_DescriptionLabel.Text = matchedCampaign.Description;
        }

        private async void VolunteeringCampaignMatchPanel_ContributeButton_Click(object sender, EventArgs e)
        {
            var answerCampaignRequest = new AnswerCampaignRequest
            {
                Username = Properties.Settings.Default["Username"].ToString(),
                OtherInfo = VolunteeringInfoPanel_OtherInfo.Text
            };
            await client.AnswerCampaign(matchedCampaign.Id, answerCampaignRequest);
            MessageBox.Show("Thank you for your initiative! Please wait until the NGOs accepts your volunteering request. Meanwhile, you can check the status of your contribution in \"My Contributions\". ");
            Navigate(MenuPanel);
        }


        private async void MyContributionsPanel_Enter(object sender, EventArgs e)
        {
            MyContributionsPanel_ListView.Columns[0].Width = 140;
            MyContributionsPanel_ListView.Columns[1].Width = 140;
            MyContributionsPanel_ListView.Items.Clear();
            MyContributionsPanel.Refresh();
            var getContributionsResult = await client.GetContributions(Properties.Settings.Default["Username"].ToString());
            foreach (var contribution in getContributionsResult.Contributions)
            {
                var item = new ListViewItem(new[] { contribution.Type, contribution.Status })
                {
                    Tag = contribution.Id
                };
                MyContributionsPanel_ListView.Items.Add(item);
            }
            MyContributionsPanel.Refresh();
        }

        private async void ContributionPanel_Enter(object sender, EventArgs e)
        {
            ContributionPanel.Refresh();
            var contribution = await client.GetContribution(contributionIdTag);
            var process = await client.GetProcess(contribution.ProcessId);
            var campaign = await client.GetCampaign(process.CampaignId);

            ContributionPanel_NgoLabel.Text = campaign.NgoName;
            ContributionPanel_DescriptionLabel.Text = campaign.Description;
            ContributionPanel_ContributionTitle.Text = campaign.Name;
            ContributionPanel_StatusLabel.Text = contribution.Status;
            ContributionPanel_TimeCreatedLabel.Text = UnixTimeToDateTime(contribution.TimeCreated).ToString();
            ContributionPanel_TimeCompletedLabel.Text = contribution.TimeCompleted == 0 ? "Ongoing" : UnixTimeToDateTime(contribution.TimeCompleted).ToString();

            ContributionPanel_DeliveryCodeLabel.Hide();
            ContributionPanel_DeliveryCodeTextBox.Hide();
            ContributionPanel_DeliveryCodeValue.Hide();
            ContributionPanel_ValidatePickupButton.Hide();
            ContributionPanel_ViewDonorLocation.Hide();
            ContributionPanel_ViewNgoLocation.Hide();
            ContributionPanel_InstructionsLabel.Hide();
            ContributionPanel_InstructionsValue.Hide();
            if (contribution.Type == "Donation" && contribution.Status == "InProgress")
            {
                ContributionPanel_ValidatePickupButton.Show();
                ContributionPanel_DeliveryCodeLabel.Show();
                ContributionPanel_DeliveryCodeTextBox.Show();
            }
            else if (contribution.Type == "Delivery" && contribution.Status != "Completed")
            {
                var deliveryDemand = await client.GetDeliveryDemand(contribution.DeliveryDemandId);
                var donor = await client.GetUserInfo(deliveryDemand.PickupUsername);
                var ngo = await client.GetNgoInfo(deliveryDemand.DestinationUsername);
                donorLocation = donor.Location;
                ngoLocation = ngo.Location;

                ContributionPanel_DeliveryCodeValue.Text = contribution.DeliveryCode;
                ContributionPanel_InstructionsValue.Text = deliveryDemand.OtherInfo;

                ContributionPanel_DeliveryCodeLabel.Show();
                ContributionPanel_DeliveryCodeValue.Show();
                ContributionPanel_ViewDonorLocation.Show();
                ContributionPanel_ViewNgoLocation.Show();
                ContributionPanel_InstructionsLabel.Show();
                ContributionPanel_InstructionsValue.Show();
            }
        }

        private void ContributionPanel_ViewLocation_Click(object sender, EventArgs e)
        {
            string tag = ((Button)sender).Tag.ToString();
            string url;
            if(tag == "donor")
            {
                url = CreateMapUrl(donorLocation.Latitude, donorLocation.Longitude);
            }
            else
            {
                url = CreateMapUrl(ngoLocation.Latitude, ngoLocation.Longitude);
            }

            ProcessStartInfo sInfo = new ProcessStartInfo(url);
            System.Diagnostics.Process.Start(sInfo);
        }

        private async void ContributionPanel_ValidatePickupButton_Click(object sender, EventArgs e)
        {
            var validateDeliveryRequest = new ValidateDeliveryRequest
            {
                ValidationType = "Pickup",
                ContributionId = contributionIdTag,
                DeliveryCode = ContributionPanel_DeliveryCodeTextBox.Text
            };

            var result = await client.ValidateDelivery(validateDeliveryRequest);
            string message;
            if (result.IsValid)
            {
                message = "Delivery Validated!";
                ContributionPanel.Focus();
            }
            else
            {
                message = "Wrong Delivery Code.";
            }
            MessageBox.Show(message);
        }


        private void MyContributionsPanel_ListView_ItemActivate(object sender, EventArgs e)
        {
            contributionIdTag = MyContributionsPanel_ListView.SelectedItems[0].Tag.ToString();
            Navigate(ContributionPanel);
        }

        private void BackToMenu_Click(object sender, EventArgs e)
        {
            Navigate(MenuPanel);
        }

        private void BackToCampaignMatch_Click(object sender, EventArgs e)
        {
            Navigate(DonationCampaignMatchPanel);
        }

        private void BackToDonations_Click(object sender, EventArgs e)
        {
            Navigate(DonationsPanel);
        }

        private void DonateButton_Click(object sender, EventArgs e)
        {
            Navigate(DonationsPanel);
        }

        private void VolunteerButton_Click(object sender, EventArgs e)
        {
            Navigate(VolunteeringPanel);
        }

        private void MyContributionsButton_Click(object sender, EventArgs e)
        {
            Navigate(MyContributionsPanel);
        }

        private void VolunteeringChoiceButton(object sender, EventArgs e)
        {
            volunteeringTag = ((Button)sender).Tag.ToString();
            VolunteeringInfoPanel_Title.Text = "Help us find a good " + volunteeringTag + " volunteering job for you.";
            Navigate(VolunteeringInfoPanel);
        }

        private void DonationCampaignMatchPanel_ContributeButton_Click(object sender, EventArgs e)
        {
            Navigate(DonationInfoPanel);
        }

        private void FindDeliveryDemandMatch_Click(object sender, EventArgs e)
        {
            Navigate(DeliveryInfoPanel);
        }

        private void BackToVolunteering_Click(object sender, EventArgs e)
        {
            Navigate(VolunteeringPanel);
        }

        private void VolunteeringInfoPanel_BackButton_Click(object sender, EventArgs e)
        {
            Navigate(VolunteeringPanel);
        }

        private void BackToVolunteeringInfo_Click(object sender, EventArgs e)
        {
            Navigate(VolunteeringInfoPanel);
        }

        private void ContributionPanel_BackButton_Click(object sender, EventArgs e)
        {
            Navigate(MyContributionsPanel);
        }

        private void ContributionPanel_Refresh_Click(object sender, EventArgs e)
        {
            Navigate(MenuPanel);
            Navigate(ContributionPanel);
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            panels.Add(DonationCampaignMatchPanel);
            panels.Add(DeliveryDemandMatchPanel);
            panels.Add(DeliveryInfoPanel);
            panels.Add(DonationInfoPanel);
            panels.Add(DonationsPanel);
            panels.Add(MenuPanel);
            panels.Add(VolunteeringPanel);
            panels.Add(MyContributionsPanel);
            panels.Add(VolunteeringCampaignMatchPanel);
            panels.Add(VolunteeringInfoPanel);
            panels.Add(ContributionPanel);
            Navigate(MenuPanel);
        }

        private void DonationInfoPanel_Paint(object sender, PaintEventArgs e)
        {
            DonationInfoPanel_TimePicker1.Format = DateTimePickerFormat.Custom;
            DonationInfoPanel_TimePicker1.CustomFormat = "HH : mm";
            DonationInfoPanel_TimePicker2.Format = DateTimePickerFormat.Custom;
            DonationInfoPanel_TimePicker2.CustomFormat = "HH : mm";
        }

        private void DeliveryInfoPanel_Paint(object sender, PaintEventArgs e)
        {
            DeliveryInfoPanel_TimePicker1.Format = DateTimePickerFormat.Custom;
            DeliveryInfoPanel_TimePicker1.CustomFormat = "HH : mm";
            DeliveryInfoPanel_TimePicker2.Format = DateTimePickerFormat.Custom;
            DeliveryInfoPanel_TimePicker2.CustomFormat = "HH : mm";
        }

        private long ToUnixTime(DateTime YYYYMMDD, DateTime HHmm)
        {
            var dateTime = new DateTime(YYYYMMDD.Year, YYYYMMDD.Month, YYYYMMDD.Day, HHmm.Hour, HHmm.Minute, 0);
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }

        public static DateTime UnixTimeToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private void DonationInfoPanel_Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (DonationInfoPanel_Checkbox.Checked)
            {
                DonationInfoPanel_Subpanel.Enabled = false;
            }
            else
            {
                DonationInfoPanel_Subpanel.Enabled = true;
            }
        }

        private string CreateMapUrl(double lat, double lon)
        {
            string latString = lat.ToString();
            string lonString = lon.ToString();
            return $"https://www.google.com/maps/search/?api=1&query={latString},{lonString}";
        }
    }
}
