using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdolgozat.Specials
{
    public static class DataStatistics
    {

        //Átlag
        public static double GetAvarage(List<int> values)
        {
            double osszeg = values.Sum(x => x);
            return osszeg / values.Count;
        }

        //Átlag
        public static double GetAvarage(List<double> values)
        {
            double osszeg = values.Sum(x => x);
            return osszeg / values.Count;
        }

        //Mértani közepet adja vissza
        public static double GetMedian(List<int> values)
        {
            List<int> orderByASC = (List<int>)values.OrderBy(x => x);
            double median = 0;
            if (orderByASC.Count % 2 == 0)
            {
                int i = values.Count / 2;
                median = orderByASC[i];
                median += orderByASC[i + 1];
                median /= 2;
            }
            else
            {
                int i = (values.Count - 1) / 2;
                median = orderByASC[i + 1];
            }
            return median;
        }

        //Lista legkisebb értéke
        public static int GetMinimumValue(List<int> values)
        {
            return values.Min();
        }

        //Lista legnagyobb értéke
        public static int GetMaximumValue(List<int> values)
        {
            return values.Max();
        }


        //Mennyire oszlanak el az értékek, átlagos eltérés mértéke az értékek átlagától
        public static double GetStandardDeviation(List<int> values)
        {
            double avarage = GetAvarage(values);
            List<double> difference = new List<double>();
            foreach (var v in values)
            {
                difference.Add(v - avarage);
            }
            List<double> squares = new List<double>();
            foreach (var v in difference)
            {
                squares.Add(v * v);
            }
            double differenceSquaresAVG = GetAvarage(squares);
            return Math.Sqrt(differenceSquaresAVG);
        }

        //Lista össz értéke
        public static int GetSum(List<int> values)
        {
            return values.Sum();
        }
    }
}
