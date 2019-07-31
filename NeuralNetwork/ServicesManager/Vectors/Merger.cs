﻿using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.Extensions;

namespace NeuralNetwork.ServicesManager.Vectors
{
    public class Merger
    {
        public List<List<double[]>> MergeItems(List<double[]> inputDataSets, List<double[]> outputDataSets)
        {
            List<double[]> newInputDataSets = new List<double[]>();
            List<double[]> newOutputDataSets = new List<double[]>();

            // Все индексы слов, которые повторяются:
            List<int> repeatedWordsIndexes = new List<int>();
            List<double[]> indexesList = new List<double[]>();
            int index;

            for(int i = 0; i < inputDataSets.Count; i++)
            {
                index = 0;
                // Поиск индексов всех повторяющихся элементов:
                while (index != -1)
                {
                   
                    //index = FindLastIndex(inputDataSets, inputDataSets[i], i);
           
                     index = inputDataSets.FindIndex(inputDataSets[i],i);

                    if (index != -1 && index != i)  // Если что-то найдено и найденный элемент не является обрабатываемым
                    {
                        repeatedWordsIndexes.Add(index);
                        inputDataSets.RemoveAt(index);
                    }
                    else
                    {
                        break;
                    }
                }

                // Слияние выходных векторов всех найденых элементов:
                double[] outputVector = new Double[outputDataSets[0].Length];
                if (repeatedWordsIndexes.Count != 0)
                {
                    outputVector = repeatedWordsIndexes.Aggregate(outputVector,
                        (current, t) => VectorSum(current, outputDataSets[t]));
                }

                try
                {
                    outputVector = VectorSum(outputVector, outputDataSets[i]);

                    if (repeatedWordsIndexes.Count == 0)
                    {
                        Console.Write("");
                    }
                }
                catch
                {
                    return new List<List<double[]>>() {newInputDataSets, newOutputDataSets};
                }

                // Добавление уже уникального элемента в общии коллекции:
                newInputDataSets.Add(inputDataSets[i]);
                if (repeatedWordsIndexes.Count != 0)
                {
                    newOutputDataSets.Add(outputVector);
                }
                else
                {
                    newOutputDataSets.Add(outputDataSets[i]);
                }

                // Удаление повторяющихся элементов:
                for (int c = 0; c < repeatedWordsIndexes.Count; c++)
                {
                    //inputDataSets.RemoveAt(repeatedWordsIndexes[c]);
                    outputDataSets.RemoveAt(repeatedWordsIndexes[c]);
                }
            }

            return new List<List<double[]>>() { newInputDataSets, newOutputDataSets };
        }

        private int FindLastIndex(List<double[]> inputDataSets, double[] item, int itemIndex)
        {
            int index = -1;
            
            for (int i = 0; i < inputDataSets.Count; i++)
            {
                bool s = false;

                if (itemIndex != i)
                {
                    for (int k = 0; k < item.Length; k++)
                    {
                        if (inputDataSets[i][k].GetHashCode() == item[k].GetHashCode())
                        {
                            s = true;
                        }
                        else
                        {
                            s = false;
                            break;
                        }
                    }
                }

                if (s)
                {
                    return i;
                }
            }

            return index;
        }

        private double[] VectorSum(double[] outputVector, double[] outputDataSet)
        {
            for (int i = 0; i < outputVector.Length; i++)
            {
                outputVector[i] += outputDataSet[i];
            }

            return outputVector;
        }
    }
}
