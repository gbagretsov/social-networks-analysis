using System.Collections.Generic;

using SocialNetworksAnalysis.Vk;

namespace SocialNetworksAnalysis
{
    /// <summary>
    /// API для работы с социальными сетями
    /// </summary>
    public static class SocialNetwork
    {
        /// <summary>
        /// ВКонтакте
        /// </summary>
        public const string VK = "vk";

        /// <summary>
        /// Вызывает окно авторизации в социальной сети
        /// </summary>
        /// <param name="network">сеть, в которой происходит авторизация.
        /// Варианты:
        /// <see cref="SocialNetwork.VK"/></param>
        public static void Authorize(string network)
        {
            switch (network)
            {
                case VK:
                    VkApiWrapper.VkAutorize();
                    return;
            }
        }
        
        internal static List<string> GetPosts(string network, long id)
        {
            switch (network)
            {
                case VK:
                    return VkApiWrapper.GetPostsFromVk(id);
            }
            return null;
        }
        
    }
}
