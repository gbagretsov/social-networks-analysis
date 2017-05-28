using Accord.Statistics.Kernels;
using Accord.MachineLearning.VectorMachines;
using Accord.IO;
using System.Collections.Generic;
using System.Linq;

using SocialNetworksAnalysis.TextPreprocessing;
using System;

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
           
            TFIDF.TryLoadVocabulary();
        }

        internal Dictionary<string, int> GetPsychProfile(string[] documents)
        {
            string[] d = new string[] { documents.Aggregate((aggr, cur) => aggr + " " + cur) };
            double[][] inputs = TFIDF.Normalize(TFIDF.Transform(d));
            int[] output = new int[9];

            Dictionary<string, int> result = new Dictionary<string, int>(9);

            output[0] = modelDenial.Decide(inputs[0]);
            output[1] = modelRepression.Decide(inputs[0]);
            output[2] = modelRegression.Decide(inputs[0]);
            output[3] = modelCompensation.Decide(inputs[0]);
            output[4] = modelProjection.Decide(inputs[0]);
            output[5] = modelDisplacement.Decide(inputs[0]);
            output[6] = modelRationalization.Decide(inputs[0]);
            output[7] = modelReactionFormation.Decide(inputs[0]);

            output[8] = output[0] + output[1] + output[2] + output[3] + output[4] + output[5] + output[6] + output[7];
            output[8] = Convert.ToInt32(Math.Round((double)output[8] / 8));

            result.Add("Отрицание",        output[0]);
            result.Add("Вытеснение",       output[1]);
            result.Add("Регрессия",        output[2]);
            result.Add("Компенсация",      output[3]);
            result.Add("Проекция",         output[4]);
            result.Add("Замещение",        output[5]);
            result.Add("Рационализация",   output[6]);
            result.Add("Гиперкомпенсация", output[7]);
            result.Add("Общий уровень",    output[8]); 

            return result;
        }

    }
}
