using System;
using System.Collections.Generic;
using System.Text;

namespace Software_Reliability
{
    class SchickWolvertonModel
    {

        public List<int> times;
        public int SIZE;
        public double epsilon;
        public int N;
        public double fi;
        public double ET;
        protected void calculate()
        {
            calculateParameters();
            ET = Math.Sqrt(Math.PI / (2 * fi * (N - SIZE)));
        }

        public  SchickWolvertonModel(ModelInput input)
        {
            epsilon = input.epsilon;
            times = input.times;
            SIZE = times.Count;

            calculate();
        }

        private void calculateParameters()
        {
            int N = SIZE;
            double T = calculateT();
            double diff;
            int a = 0;
            do
            {
                a++;
                N += 1;
                double left = calculateFirst(N, T);
                double right = calculateSecond(N, T);
                diff = Math.Abs(left - right);
            } while (diff > epsilon);

            this.N = N;
            fi = calculateFirst(N, T);
        }

        private double calculateFirst(int N, double T)
        {
            double sum = 0;
            for (int i = 1; i <= SIZE; i++)
            {
                sum += 1d / ((N - (i - 1)) * T);
            }
            return 2 * sum;
        }

        private double calculateSecond(int N, double T)
        {
            double sum = 0;
            for (int i = 1; i <= SIZE; i++)
            {
                sum += (i - 1) * Math.Pow(times[i - 1], 2);
            }
            return (2 * SIZE) / (N * T - sum);
        }

        private double calculateT()
        {
            double sum = 0;
            
            for(int i =0;i<SIZE;i++)
            {
                sum += Math.Pow(times[i], 2);
            }
            return sum;
       

        }
    }
}
