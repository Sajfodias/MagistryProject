using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wyszukiwarka_publikacji_v0._2.Logic.ClusteringAlgorithms;
using Wyszukiwarka_publikacji_v0._2.Logic.eBase;
using Wyszukiwarka_publikacji_v0._2.Logic.TextProcessing;
using Wyszukiwarka_publikacji_v0._2.Tests;

namespace Wyszukiwarka_publikacji_v0._2.Tests
{
    class NormilizedMutualInformation
    {
        public static double NMI_Calculating(List<Centroid> clusteringResult, List<List<string>> classList, List<DocumentVector> vSphere)
        {
            double NMI = 0.0F;

            int number_Of_Couple_Elements_in_k = 0;
            int[,] Couple_element_matrix = new int[clusteringResult.Count, classList.Count];

            for (int ki = 0; ki < clusteringResult.Count; ki++)
            {
                for (int i = 0; i < clusteringResult[ki].GroupedDocument.Count; i++)
                {
                    for (int Li = 0; Li < classList.Count; Li++)
                    {
                        for (int l = 0; l < classList[Li].Count; l++)
                        {
                            if (clusteringResult[ki].GroupedDocument[i].Content == classList[Li][l] || clusteringResult[ki].GroupedDocument[i].Content.Contains(classList[Li][l]))
                                number_Of_Couple_Elements_in_k++;
                        }
                        Couple_element_matrix[ki, Li] = number_Of_Couple_Elements_in_k;
                        number_Of_Couple_Elements_in_k = 0;
                    }
                }
            }

            double sum1 = 0.0F;
           
            
                
            for (int C=0; C<clusteringResult.Count; C++)
            {
                for (int L = 0; L < classList.Count; L++)
                {
                    /*var licznik = sum1 + Couple_element_matrix[L_i, C_j] *
                        Math.Log((vSphere.Count * Couple_element_matrix[L_i, C_j]) /
                        (classList[L_i].Count * clusteringResult[C_j].GroupedDocument.Count));
                        */
                    double licznik6 = 0;
                    var licznik2 = vSphere.Count * Couple_element_matrix[C,L];
                    var licznik3 = classList[L].Count * clusteringResult[C].GroupedDocument.Count;
                    var licznik4 = licznik2 / licznik3;
                    var licznik5 = Math.Log(licznik4);
                    if (double.IsInfinity(licznik5) || double.IsNaN(licznik5))
                        licznik6 = 0;
                    else
                        licznik6 = licznik5;
                    sum1 += Couple_element_matrix[C,L] * licznik6;
                }
            }

            double sum2 = 0;
            for(int L_i =0; L_i<classList.Count; L_i++)
            {
                var element = classList[L_i].Count * Math.Log(classList[L_i].Count / vSphere.Count);
                if (double.IsNaN(element) || double.IsInfinity(element))
                    sum2 += 0;
                else
                    sum2 += element;
            }

            double sum3 = 0;
            for(int C_j=0; C_j<clusteringResult.Count; C_j++)
            {
                var element = clusteringResult[C_j].GroupedDocument.Count * Math.Log(clusteringResult[C_j].GroupedDocument.Count / vSphere.Count);
                if (double.IsInfinity(element) || double.IsNaN(element))
                    sum3 += 0;
                else
                    sum3 += element;
            }

            var licznik = sum1;
            var mianownik = Math.Sqrt(sum2*sum3);

            NMI = licznik / mianownik;

            if (double.IsNaN(NMI) || double.IsInfinity(NMI))
                return 0;
            else
                return NMI;
        }
    }
}
