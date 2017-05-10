using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines;
using Accord.IO;
using System.Collections.Generic;
using System.Linq;

using SocialNetworksAnalysis.TextPreprocessing;

namespace SocialNetworksAnalysis.ClassifierModel
{
    internal class SVM
    {
        private MulticlassSupportVectorMachine<Linear> modelDenial;
        private MulticlassSupportVectorMachine<Linear> modelRepression;
        private MulticlassSupportVectorMachine<Linear> modelRegression;
        private MulticlassSupportVectorMachine<Linear> modelCompensation;
        private MulticlassSupportVectorMachine<Linear> modelProjection;
        private MulticlassSupportVectorMachine<Linear> modelDisplacement;
        private MulticlassSupportVectorMachine<Linear> modelRationalization;
        private MulticlassSupportVectorMachine<Linear> modelReactionFormation;
        private MulticlassSupportVectorMachine<Linear> modelOverallLevel;

        internal SVM()
        {            
            modelDenial            = Serializer.Load<MulticlassSupportVectorMachine<Linear>>(@"Models\Denial.dat");
            modelRepression        = Serializer.Load<MulticlassSupportVectorMachine<Linear>>(@"Models\Repression.dat");
            modelRegression        = Serializer.Load<MulticlassSupportVectorMachine<Linear>>(@"Models\Regression.dat");
            modelCompensation      = Serializer.Load<MulticlassSupportVectorMachine<Linear>>(@"Models\Compensation.dat");
            modelProjection        = Serializer.Load<MulticlassSupportVectorMachine<Linear>>(@"Models\Projection.dat");
            modelDisplacement      = Serializer.Load<MulticlassSupportVectorMachine<Linear>>(@"Models\Displacement.dat");
            modelRationalization   = Serializer.Load<MulticlassSupportVectorMachine<Linear>>(@"Models\Rationalization.dat");
            modelReactionFormation = Serializer.Load<MulticlassSupportVectorMachine<Linear>>(@"Models\Reaction Formation.dat");
            modelOverallLevel      = Serializer.Load<MulticlassSupportVectorMachine<Linear>>(@"Models\Overall Level.dat");

            TFIDF.TryLoadVocabulary();
        }

        internal Dictionary<string, int> GetPsychProfile(string[] documents)
        {
            string[] d = new string[] { documents.Aggregate((aggr, cur) => aggr + " " + cur) };
            double[][] inputs = TFIDF.Normalize(TFIDF.Transform(d));

            Dictionary<string, int> result = new Dictionary<string, int>(9);

            modelDenial.Decide(inputs[0]);

            result.Add("Отрицание",        modelDenial.Decide(inputs[0]));
            result.Add("Вытеснение",       modelRepression.Decide(inputs[0]));
            result.Add("Регрессия",        modelRegression.Decide(inputs[0]));
            result.Add("Компенсация",      modelCompensation.Decide(inputs[0]));
            result.Add("Проекция",         modelProjection.Decide(inputs[0]));
            result.Add("Замещение",        modelDisplacement.Decide(inputs[0]));
            result.Add("Рационализация",   modelRationalization.Decide(inputs[0]));
            result.Add("Гиперкомпенсация", modelReactionFormation.Decide(inputs[0]));
            result.Add("Общий уровень",    modelOverallLevel.Decide(inputs[0]));

            return result;
        }

    }
}
