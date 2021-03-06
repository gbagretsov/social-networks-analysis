﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace SocialNetworksAnalysis.TextPreprocessing
{
    /// <summary>
    /// Copyright (c) 2013 Kory Becker http://www.primaryobjects.com/kory-becker.aspx
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining
    /// a copy of this software and associated documentation files (the
    /// "Software"), to deal in the Software without restriction, including
    /// without limitation the rights to use, copy, modify, merge, publish,
    /// distribute, sublicense, and/or sell copies of the Software, and to
    /// permit persons to whom the Software is furnished to do so, subject to
    /// the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be
    /// included in all copies or substantial portions of the Software.
    /// 
    /// Description:
    /// Performs a TF*IDF (Term Frequency * Inverse Document Frequency) transformation on an array of documents.
    /// Each document string is transformed into an array of doubles, cooresponding to their associated TF*IDF values.
    /// 
    /// Usage:
    /// string[] documents = LoadYourDocuments();
    ///
    /// double[][] inputs = TFIDF.Transform(documents);
    /// inputs = TFIDF.Normalize(inputs);
    /// 
    /// </summary>
    internal static class TFIDF
    {
        /// <summary>
        /// Document vocabulary, containing each word's IDF value.
        /// </summary>
        private static Dictionary<string, double> _vocabularyIDF = new Dictionary<string, double>();
        private static Dictionary<string, double> _filteredVocabularyIDF = new Dictionary<string, double>();

        /// <summary>
        /// Transforms a list of documents into their associated TF*IDF values.
        /// If a vocabulary does not yet exist, one will be created, based upon the documents' words.
        /// </summary>
        /// <param name="documents">string[]</param>
        /// <returns>double[][]</returns>
        public static double[][] Transform(string[] documents)
        {
            List<List<string>> stemmedDocs;
            List<string> vocabulary;

            int vocabularyThreshold = 2;
            int featuresAmount = 9000;

            // Get the vocabulary and stem the documents at the same time.
            vocabulary = GetVocabulary(documents, out stemmedDocs, vocabularyThreshold);

            TryLoadVocabulary();

            _filteredVocabularyIDF = _vocabularyIDF.OrderByDescending(t => t.Value).Take(featuresAmount).ToDictionary(x => x.Key, x => x.Value);

            // Transform each document into a vector of tfidf values.
            return TransformToTFIDFVectors(stemmedDocs, _filteredVocabularyIDF);
        }

        /// <summary>
        /// Converts a list of stemmed documents (lists of stemmed words) and their associated vocabulary + idf values, into an array of TF*IDF values.
        /// </summary>
        /// <param name="stemmedDocs">List of List of string</param>
        /// <param name="vocabularyIDF">Dictionary of string, double (term, IDF)</param>
        /// <returns>double[][]</returns>
        private static double[][] TransformToTFIDFVectors(List<List<string>> stemmedDocs, Dictionary<string, double> vocabularyIDF)
        {
            // Transform each document into a vector of tfidf values.
            List<List<double>> vectors = new List<List<double>>();
            foreach (var doc in stemmedDocs)
            {
                List<double> vector = new List<double>();

                foreach (var vocab in vocabularyIDF)
                {
                    // Term frequency = count how many times the term appears in this document.
                    double tf = doc.Where(d => d == vocab.Key).Count();
                    double tfidf = tf * vocab.Value;

                    vector.Add(tfidf);
                }

                vectors.Add(vector);
            }

            return vectors.Select(v => v.ToArray()).ToArray();
        }

        /// <summary>
        /// Normalizes a TF*IDF array of vectors using L2-Norm.
        /// Xi = Xi / Sqrt(X0^2 + X1^2 + .. + Xn^2)
        /// </summary>
        /// <param name="vectors">double[][]</param>
        /// <returns>double[][]</returns>
        internal static double[][] Normalize(double[][] vectors)
        {
            // Normalize the vectors using L2-Norm.
            List<double[]> normalizedVectors = new List<double[]>();
            foreach (var vector in vectors)
            {
                var normalized = Normalize(vector);
                normalizedVectors.Add(normalized);
            }

            return normalizedVectors.ToArray();
        }

        /// <summary>
        /// Normalizes a TF*IDF vector using L2-Norm.
        /// Xi = Xi / Sqrt(X0^2 + X1^2 + .. + Xn^2)
        /// </summary>
        /// <param name="vectors">double[][]</param>
        /// <returns>double[][]</returns>
        internal static double[] Normalize(double[] vector)
        {
            List<double> result = new List<double>();

            double sumSquared = 0;
            foreach (var value in vector)
            {
                sumSquared += value * value;
            }

            double SqrtSumSquared = Math.Sqrt(sumSquared);

            foreach (var value in vector)
            {
                if (sumSquared == 0)
                {
                    result.Add(0);
                }
                else
                {
                    // L2-norm: Xi = Xi / Sqrt(X0^2 + X1^2 + .. + Xn^2)
                    result.Add(value / SqrtSumSquared);
                }
            }

            return result.ToArray();
        }
        
        /// <summary>
        /// Loads the TFIDF vocabulary from disk.
        /// </summary>
        /// <param name="filePath">File path</param>
        internal static void TryLoadVocabulary(string filePath = "vocabulary.dat")
        {
            // Load from disk.
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    _vocabularyIDF = (Dictionary<string, double>)formatter.Deserialize(fs);
                }
            }
            catch (FileNotFoundException e)
            {
                throw (new FileNotFoundException("Словарь не найден", e.FileName));
            }
        }

        #region Private Helpers

        /// <summary>
        /// Parses and tokenizes a list of documents, returning a vocabulary of words.
        /// </summary>
        /// <param name="docs">string[]</param>
        /// <param name="stemmedDocs">List of List of string</param>
        /// <returns>Vocabulary (list of strings)</returns>
        private static List<string> GetVocabulary(string[] docs, out List<List<string>> stemmedDocs, int vocabularyThreshold)
        {
            // TODO: N-grams
            List<string> vocabulary = new List<string>();
            Dictionary<string, int> wordCountList = new Dictionary<string, int>();
            stemmedDocs = new List<List<string>>();

            foreach (var doc in docs)
            {
                List<string> stemmedDoc = new List<string>();
                
                string[] parts2 = Tokenize(doc);

                List<string> words = new List<string>();
                foreach (string part in parts2)
                {
                    // Strip non-alphanumeric characters.
                    string stripped = Regex.Replace(part, "[^a-zA-Zа-яА-Я0-9]", "");

                    if (!StopWords.stopWordsList.Contains(stripped.ToLower()))
                    {
                        try
                        {
                            string stem = stripped.ToLower();
                            words.Add(stem);

                            if (stem.Length > 0)
                            {
                                // Build the word count list.
                                if (wordCountList.ContainsKey(stem))
                                {
                                    wordCountList[stem]++;
                                }
                                else
                                {
                                    wordCountList.Add(stem, 0);
                                }

                                stemmedDoc.Add(stem);
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                stemmedDocs.Add(stemmedDoc);
            }

            // Get the top words.
            var vocabList = wordCountList.Where(w => w.Value >= vocabularyThreshold);
            foreach (var item in vocabList)
            {
                vocabulary.Add(item.Key);
            }

            return vocabulary;
        }

        /// <summary>
        /// Tokenizes a string, returning its list of words.
        /// </summary>
        /// <param name="text">string</param>
        /// <returns>string[]</returns>
        private static string[] Tokenize(string text)
        {
            // Strip all HTML.
            text = Regex.Replace(text, "<[^<>]+>", "");

            // Strip numbers.
            text = Regex.Replace(text, "[0-9]+", "");

            // Strip urls.
            text = Regex.Replace(text, @"(http|https)://[^\s]*", "");

            // Strip email addresses.
            text = Regex.Replace(text, @"[^\s]+@[^\s]+", "");

            // Strip dollar sign.
            text = Regex.Replace(text, "[$]+", "");

            // Strip usernames.
            text = Regex.Replace(text, @"@[^\s]+", "usename");

            // Tokenize and also get rid of any punctuation
            return text.Split(" @$/#.-:&*+=[]?!(){},''\">_<;%\\".ToCharArray());
        }

        #endregion
    }
}
