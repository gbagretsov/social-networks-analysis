using System.Collections.Generic;

namespace SocialNetworksAnalysis
{
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
        /// </summary>
        /// <param name="network">сеть, из которой собираются данные для анализа.
        /// Варианты:
        /// <see cref="SocialNetwork.VK"/></param>
        /// <param name="id">идентификатор пользователя в выбранной сети</param>
        /// <returns>Словарь с парами "название характеристики" - "степень выраженности"</returns>
        /// <seealso cref="PsychProfile.TraitDegree"/>
        public static Dictionary<string, TraitDegree> GetUserPsychProfile(string network, long id)
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
