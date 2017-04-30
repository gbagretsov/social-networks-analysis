using System;
using VkNet;

namespace SocialNetworksAnalysis.Vk
{
    internal class VkApiHolder
    {
        private static readonly Lazy<VkApi> vkApiHolder = new Lazy<VkApi>(() => new VkApi());

        private VkApiHolder() { }

        internal static VkApi Api
        {
            get { return vkApiHolder.Value; }
        }
    }
}
