using System;
using System.Collections.Generic;
using System.Linq;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace SocialNetworksAnalysis.Vk
{
    internal class VkApiWrapper
    {
        internal static void VkAutorize()
        {
            new VKAuthorize().ShowDialog();
        }

        internal static List<string> GetPostsFromVk(long id)
        {
            List<string> data = new List<string>();

            List<Post> posts = VkApiHolder.Api.Wall.Get(new WallGetParams()
            {
                OwnerId = id,
                Filter = WallFilter.Owner,
                Count = 100
            }).WallPosts.ToList();

            foreach (Post post in posts)
            {
                string s = post.Text + " ";
                if (post.CopyHistory.Count != 0)
                {
                    s += post.CopyHistory.First().Text;
                }
                s = s.Replace(Environment.NewLine, " ")
                     .Replace("\n", " ")
                     .Replace("\r", " ")
                     .Replace("\"", "")
                     .Replace(";", " ")
                     .Replace(",", " ")
                     .Replace("'", "")
                     .Trim();
                if (s == string.Empty) continue;
                data.Add(s);
            }

            return data;
        }

    }
}
