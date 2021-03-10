using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CustomApplication
{
    public partial class Form1 : Form
    {
        //PUBLIC VARIABLES
        public string UserId { get; set; }

        public string SearchText { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        public string GetUser()
        {
            try
            {
                string query = searchBox.Text;
                var client = new RestClient("https://dev-10864403-admin.okta.com/api/v1/users?q=" + query + "&limit=1");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "SSWS 00uofXk0ZmwBdmaNLohHAtdQx9p5zAhp6sVFEX28We");
                request.AddHeader("Cookie", "JSESSIONID=47ACCD23AF835C13A4EF68EA23159683");
                IRestResponse response = client.Execute(request);
                string responset1 = response.Content.TrimStart('[');
                string responset2 = responset1.TrimEnd(']');
                var oktaUser = JsonConvert.DeserializeObject<OktaUser.Root>(responset2);
                if (oktaUser == null)
                { 
                    richTextBox1.Text = "User not found.";
                    return null;
                }
                else
                {
                    UserId = oktaUser.id;
                    return UserId;
                }
            }
            catch
            {  
                richTextBox1.Text = "Please enter a username in search box.";
                return null;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //CREATE USER
           
            var client = new RestClient("https://dev-10864403-admin.okta.com/api/v1/users?activate=false");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "SSWS 00uofXk0ZmwBdmaNLohHAtdQx9p5zAhp6sVFEX28We");
            request.AddHeader("Cookie", "JSESSIONID=958E54031AF1D4E163B5786136211898");
           
            request.AddParameter("application/json", "{\n  \"profile\": {\n    \"firstName\": \"Test\",\n    \"lastName\": \"User\",\n    \"email\": \"tuser2@oktabank.com\",\n    \"login\": \"tuser2@oktabank.com\"\n  },\n  \"credentials\": {\n    \"password\" : { \"value\": \"VMware@123\" }\n  }\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            richTextBox1.Text = "USER CREATED: tuser2@oktabank.com";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //GET USER 
            if (GetUser() != null)
                richTextBox1.Text = "Found user with UserId = " + GetUser();
            else
                richTextBox1.Text = "User not found.";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // DEACTIVATE USER
            try
            {
                UserId = GetUser();
                var client2 = new RestClient("https://dev-10864403-admin.okta.com/api/v1/users/" + UserId + "/lifecycle/deactivate");
                client2.Timeout = -1;
                var request2 = new RestRequest(Method.POST);
                request2.AddHeader("Content-Type", "application/json");
                request2.AddHeader("Accept", "application/json");
                request2.AddHeader("Authorization", "SSWS 00uofXk0ZmwBdmaNLohHAtdQx9p5zAhp6sVFEX28We");
                request2.AddHeader("Cookie", "JSESSIONID=885DDD6FBC8568DCD0B1E69FBD66C027");
                request2.AddParameter("application/json", "", ParameterType.RequestBody);
                IRestResponse response2 = client2.Execute(request2);
                richTextBox1.Text = "User deactivated";
            }
            catch
            {
                richTextBox1.Text = "Please enter a username in search box.";
            }          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //DELETE USER

            try
            {
                string query = searchBox.Text;
                var client = new RestClient("https://dev-10864403-admin.okta.com/api/v1/users?q=" + query + "&limit=1");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "SSWS 00uofXk0ZmwBdmaNLohHAtdQx9p5zAhp6sVFEX28We");
                request.AddHeader("Cookie", "JSESSIONID=47ACCD23AF835C13A4EF68EA23159683");
                IRestResponse response = client.Execute(request);
                string responset1 = response.Content.TrimStart('[');
                string responset2 = responset1.TrimEnd(']');
                var oktaUser = JsonConvert.DeserializeObject<OktaUser.Root>(responset2);
                UserId = oktaUser.id;
                richTextBox1.Text = response.Content;
            }
            catch
            {
                richTextBox1.Text = "Please enter a username in search box.";
            }

            try
            {
                var client = new RestClient("https://dev-10864403-admin.okta.com/api/v1/users/"+UserId);
                client.Timeout = -1;
                var request = new RestRequest(Method.DELETE);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "SSWS 00uofXk0ZmwBdmaNLohHAtdQx9p5zAhp6sVFEX28We");
                request.AddHeader("Cookie", "JSESSIONID=2AA8E87C3F65D0C69D21AD1B5ADCFC4D");
                IRestResponse response = client.Execute(request);
                richTextBox1.Text = response.Content;
            }
            catch { richTextBox1.Text = "Please enter a username in search box."; }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // ACTIVATE USER
            try
            {
                var client = new RestClient("https://dev-10864403-admin.okta.com/api/v1/users/" + UserId + "/lifecycle/activate?sendEmail=false");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", "SSWS 00uofXk0ZmwBdmaNLohHAtdQx9p5zAhp6sVFEX28We");
                request.AddHeader("Cookie", "JSESSIONID=9696BF8793A369BD661FBA8F0A8751AD");
                request.AddParameter("application/json", "", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                richTextBox1.Text = response.Content;
            }
            catch
            {
                richTextBox1.Text = "Please enter a username in search box.";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //ADD USER TO ADMIN GROUP
            try
            {
                UserId = GetUser();
                var client = new RestClient("https://dev-10864403-admin.okta.com/api/v1/groups/00gbdx3h5BXpaArdL5d6/users/"+UserId);
                client.Timeout = -1;
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "SSWS 00uofXk0ZmwBdmaNLohHAtdQx9p5zAhp6sVFEX28We");
                request.AddHeader("Cookie", "JSESSIONID=9696BF8793A369BD661FBA8F0A8751AD");
                request.AddParameter("application/json", "", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                richTextBox1.Text = "User added to admin group.";
            }
            catch
            {
                richTextBox1.Text = "Cannot add user to Admin group";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
             System.Diagnostics.Process.Start("https://ubiquitous-summer-team.glitch.me");
        }
    }

    internal class OktaUser
    {
        public class Type
        {
            public string id { get; set; }
        }

        public class Profile
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public object mobilePhone { get; set; }
            public object secondEmail { get; set; }
            public string login { get; set; }
            public string email { get; set; }
        }

        public class Password
        {
        }

        public class Email
        {
            public string value { get; set; }
            public string status { get; set; }
            public string type { get; set; }
        }

        public class Provider
        {
            public string type { get; set; }
            public string name { get; set; }
        }

        public class Credentials
        {
            public Password password { get; set; }
            public List<Email> emails { get; set; }
            public Provider provider { get; set; }
        }

        public class Self
        {
            public string href { get; set; }
        }

        public class Links
        {
            public Self self { get; set; }
        }

        public class Root
        {
            public string id { get; set; }
            public string status { get; set; }
            public DateTime created { get; set; }
            public object activated { get; set; }
            public object statusChanged { get; set; }
            public object lastLogin { get; set; }
            public DateTime lastUpdated { get; set; }
            public DateTime? passwordChanged { get; set; }
            public Type type { get; set; }
            public Profile profile { get; set; }
            public Credentials credentials { get; set; }
            public Links _links { get; set; }
        }
    }
}
