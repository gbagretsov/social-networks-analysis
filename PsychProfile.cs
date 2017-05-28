using System.Collections.Generic;

using SocialNetworksAnalysis.ClassifierModel;

namespace SocialNetworksAnalysis
{
    /// <summary>
    /// API для построения психологического профиля
    /// </summary>
    public static class PsychProfile
    {
        /// <summary>
        /// Уровень выраженности психологической характеристики
        /// </summary>
        public enum TraitDegree
        {
            /// <summary>
            /// Низкий уровень
            /// </summary>
            LOW,
            /// <summary>
            /// Средний уровень
            /// </summary>
            MEDIUM,
            /// <summary>
            /// Высокий уровень
            /// </summary>
            HIGH
        }

        /// <summary>
        /// Возвращает значения выраженности видов психологической защиты пользователя
        /// (или null, если на стене пользователя отсутствует размещённая им текстовая информация)
        /// </summary>
        /// <param name="network">сеть, из которой собираются данные для анализа.
        /// Варианты:
        /// <see cref="SocialNetwork.VK"/></param>
        /// <param name="id">идентификатор пользователя в выбранной сети</param>
        /// <returns>Словарь с парами "название характеристики" - "степень выраженности" (или null, если на стене пользователя отсутствует размещённая им текстовая информация)</returns>
        /// <seealso cref="PsychProfile.TraitDegree"/>
        public static Dictionary<string, TraitDegree> GetUserPsychProfile(string network, long id)
        {
            Dictionary <string, TraitDegree> result = new Dictionary<string, TraitDegree>();
            List<string> data = SocialNetwork.GetPosts(network, id);

            if (data.Count == 0)
                return null;

            var svmResult = new SVM().GetPsychProfile(data.ToArray());

            foreach (var item in svmResult)
            {
                result.Add(item.Key, item.Value == 0 ? TraitDegree.LOW :
                                     item.Value == 1 ? TraitDegree.MEDIUM : 
                                                       TraitDegree.HIGH);
            }
            
            return result;
        }

    }
}
