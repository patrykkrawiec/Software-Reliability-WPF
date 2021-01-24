using System;
using System.Collections.Generic;
using System.Linq;

namespace Software_Reliability
{
    class JelinskiMorandaModel
    {

        public List<int> times;
        public int SIZE;
        public double epsilon;
        public int N;
        public double fi;
        public double ET;

        public JelinskiMorandaModel(ModelInput input)
        {
            epsilon = input.epsilon;
            times = input.times;
            SIZE = times.Count;
            calculate();
        }


        protected void calculate()
        {
            N = calculateN();
            fi = calculateFi();
            ET = 1d / (fi * (N - SIZE));
        }
        private int calculateN()
        {
            int N = SIZE;
            double diff;
            do
            {
                N += 1;
                double left = calculateLeft(N);
                double right = calculateRight(N);
                diff = Math.Abs(left - right);
            } while (diff > epsilon);
            return N;
        }

        private double calculateFi()
        {
            int n = times.Count;
            int firstValue = times.Sum();
            int secondValue = 0;
            for (int i = 0; i < n; i++)
            {
                secondValue += i * times[i];

            }
            return n / (double)((firstValue * N) - secondValue);

        }

        private double calculateLeft(int N)
        {
            double value = 0;
            for (int i = 1; i <= SIZE; i++)
            {
                value += 1d / (N - (i - 1));
            }
            return value;
        }

        private double calculateRight(int N)
        {
            return calculateUpperSum() / calculateLowerPart(N);
        }

        private double calculateUpperSum()
        {
            return sumTimes() * SIZE;
        }

        private double calculateLowerPart(int N)
        {
            double firstValue = times.Sum();
            double secondValue = 0;
            for (int i = 0; i < SIZE; i++)
            {
                secondValue += i * times[i];

            }
            return (firstValue * N) - secondValue;
        }

        private double sumTimes()
        {
            double sum = 0;
            for (int i = 1; i <= SIZE; i++)
            {
                sum += times[i - 1];
            }
            return sum;
        }


    }
}
