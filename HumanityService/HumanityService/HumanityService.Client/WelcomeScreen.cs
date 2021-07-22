using HumanityService.DataContracts;
using System;
using System.Windows.Forms;

namespace HumanityService.Client
{
    public partial class WelcomeScreen : Form
    {
        private string UserType;
        public WelcomeScreen()
        {
            InitializeComponent();
            WelcomePanel.Show();
            SigninPanel.Hide();
        }

        private void ContributorButton_Click(object sender, EventArgs e)
        {
            WelcomePanel.Hide();
            SigninPanel.Show();
            UserType = "Contributor";
        }

        private void NgoButton_Click(object sender, EventArgs e)
        {
            SigninPanel.Show();
            WelcomePanel.Hide();
            UserType = "Ngo";
        }

        private async void SigninButton_ClickAsync(object sender, EventArgs e)
        {
            var loginRequest = new LoginRequest
            {
                Username = UsernameTextBox.Text,
                Password = PasswordTextBox.Text
            };
            HumanityServiceClient client = new HumanityServiceClient();
            string message;
            if (UserType == "Contributor")
            {
                var authenticationResult = await client.LoginUser(loginRequest);
                if (authenticationResult.LoginValidated)
                {
                    Properties.Settings.Default["Username"] = UsernameTextBox.Text;
                    Properties.Settings.Default.Save();
                    Menu menu = new Menu();
                    this.Hide();
                    menu.Show();
                }
                else
                {
                    message = "Wrong username or password. Please try again.";
                    MessageBox.Show(message);
                }
            }
            else
            {
                var authenticationResult = await client.LoginNgo(loginRequest);
                if (authenticationResult.LoginValidated)
                {
                    Properties.Settings.Default["Username"] = UsernameTextBox.Text;
                    Properties.Settings.Default.Save();
                    Dashboard dashboard = new Dashboard();
                    this.Hide();
                    dashboard.Show();
                }
                else
                {
                    message = "Wrong username or password. Please try again.";
                    MessageBox.Show(message);
                }
            }
            
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            SigninPanel.Hide();
            WelcomePanel.Show();
        }

        private void WelcomeScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
