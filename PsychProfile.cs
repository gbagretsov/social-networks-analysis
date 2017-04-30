using System.Collections.Generic;

namespace SocialNetworksAnalysis
{
    public static class PsychProfile
    {
        public enum TraitDegree
        {
            LOW,
            MEDIUM,
            HIGH
        }

        public static Dictionary<string, TraitDegree> GetUserPsychDefense(string network, long id)
        {
            Dictionary <string, TraitDegree> result = new Dictionary<string, TraitDegree>();
            List<string> data = SocialNetwork.GetPosts(network, id);

            // TODO: подключение к Azure

            result.Add("Отрицание",        TraitDegree.LOW);
            result.Add("Вытеснение",       TraitDegree.LOW);
            result.Add("Регрессия",        TraitDegree.LOW);
            result.Add("Компенсация",      TraitDegree.LOW);
            result.Add("Проекция",         TraitDegree.LOW);
            result.Add("Замещение",        TraitDegree.LOW);
            result.Add("Рационализация",   TraitDegree.LOW);
            result.Add("Гиперкомпенсация", TraitDegree.LOW);
            result.Add("Общий уровень",    TraitDegree.LOW);
            
            return result;
        }

    }
}
