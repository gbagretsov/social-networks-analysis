using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SocialNetworksAnalysis.Vk
{
    internal partial class VKAuthorize : Form
    {

        private int appId = 5677623;
        private int scope = 0;

        public VKAuthorize()
        {
            InitializeComponent();
            vkAuthBrowser.Navigate(
                string.Format("https://api.vk.com/oauth/authorize?client_id={0}&scope={1}&display=popup&response_type=token",
                appId, scope));
        }

        private void vkAuthBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.ToString().IndexOf("access_token") != -1)
            {
                string token = "";

                Regex myReg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m in myReg.Matches(e.Url.ToString()))
                {
                    if (m.Groups["name"].Value == "access_token")
                    {
                        token = m.Groups["value"].Value;
                    }
                }

                VkApiHolder.Api.Authorize(new VkNet.ApiAuthParams()
                {
                    AccessToken = token
                });

                this.Close();
            }
        }

    }
}
