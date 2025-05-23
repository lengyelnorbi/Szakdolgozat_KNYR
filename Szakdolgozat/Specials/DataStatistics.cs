using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Szakdolgozat.Specials
{
    public static class DataStatistics
    {

        //Átlag
        public static double GetAvarage(List<int> values)
        {
            double osszeg = values.Sum(x => x);
            return Math.Round(osszeg / values.Count, 2);
        }

        //Átlag
        public static double GetAvarage(ChartValues<ObservableValue> values)
        {
            return values.Average(x => x.Value);
            //double osszeg = values.Sum(x => x.Value);
            //return osszeg / values.Count;
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
            if(values.Count == 0)
                return 0;
            List<int> orderByASC = values.OrderBy(x => x).ToList();
            double median = 0;
            if (orderByASC.Count % 2 == 0)
            {
                //indexek 0-tól kezdődnek, ezért -1
                int i = (values.Count / 2) - 1;
                median = orderByASC[i];
                median += orderByASC[i + 1];
                median /= 2;
            }
            else
            {
                int i = ((values.Count - 1) / 2);
                median = orderByASC[i];
            }
            return median;
        }

        //Mértani közepet adja vissza
        public static double GetMedian(List<double> values)
        {
            if (values.Count == 0)
                return 0;
            List<double> orderByASC = values.OrderBy(x => x).ToList();
            double median = 0;
            if (orderByASC.Count % 2 == 0)
            {
                //indexek 0-tól kezdődnek, ezért -1
                int i = (values.Count / 2) - 1;
                median = orderByASC[i];
                median += orderByASC[i + 1];
                median /= 2;
            }
            else
            {
                int i = ((values.Count - 1) / 2);
                median = orderByASC[i];
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

        //Lista legkisebb értéke
        public static double GetMinimumValue(List<double> values)
        {
            return values.Min();
        }

        //Lista legnagyobb értéke
        public static double GetMaximumValue(List<double> values)
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
            return Math.Round(Math.Sqrt(differenceSquaresAVG), 2);
        }

        //Mennyire oszlanak el az értékek, átlagos eltérés mértéke az értékek átlagától
        public static double GetStandardDeviation(List<double> values)
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
            return Math.Round(Math.Sqrt(differenceSquaresAVG), 2);
        }

        //Lista össz értéke
        public static int GetSum(List<int> values)
        {
            return values.Sum();
        }

        //Lista össz értéke
        public static double GetSum(List<double> values)
        {
            return values.Sum();
        }
    }
}
