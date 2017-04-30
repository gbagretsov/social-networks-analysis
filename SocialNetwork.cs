using System;
using System.Collections.Generic;
using System.Linq;

using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace SocialNetworksAnalysis
{
    public static class SocialNetwork
    {
        public const string VK = "vk";

        internal class VkApiHolder
        {
            private static readonly Lazy<VkApi> vkApiHolder = new Lazy<VkApi>(() => new VkApi());

            private VkApiHolder() { }

            internal static VkApi Api
            {
                get { return vkApiHolder.Value; }
            }
        }

        #region autorize

        public static void Authorize(string network)
        {
            switch (network)
            {
                case VK:
                    VkAutorize();
                    return;
            }
        }

        private static void VkAutorize()
        {
            // TODO: вызвать форму для авторизации, передавать токен            
        }

        #endregion autorize

        #region posts

        internal static List<string> GetPosts(string network, long id)
        {
            switch (network)
            {
                case VK:
                    return GetPostsFromVk(id);
            }
            return null;
        }

        private static List<string> GetPostsFromVk(long id)
        {
            List<string> data = new List<string>();

            List<Post> posts = new List<Post>(); // TODO: wall.get

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

        #endregion posts
    }

}
