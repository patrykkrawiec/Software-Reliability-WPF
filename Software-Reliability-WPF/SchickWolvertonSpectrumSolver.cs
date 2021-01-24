using Newtonsoft.Json.Linq;
using Software_Reliability;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Software_Reliability_WPF
{
    class SchickWolvertonSpectrumSolver
    {
        public List<ChartPoint> fiPoints;
        public List<ChartPoint> nPoints;
        public List<ChartPoint> etPoints;
        public List<int> N;
        public List<double> FI;
        public List<double> ET;
        private double startValue;
        private double stopValue;
        private double stepValue;
        private ModelInput input;

        SchickWolvertonModel schickWolvertonModel;
        public SchickWolvertonSpectrumSolver(double startValue, double stopValue, double stepValue,ModelInput input)
        {
            this.startValue = startValue;
            this.stopValue = stopValue;
            this.stepValue = stepValue;
            N = new List<int>();
            FI = new List<double>();
            ET = new List<double>();
            fiPoints = new List<ChartPoint>();
            nPoints = new List<ChartPoint>();
            etPoints = new List<ChartPoint>();

            this.input = input;
            solveSchickWolvertonSpectrum();



        }
        private void solveSchickWolvertonSpectrum()
        {
            double procededValue = startValue;
            input.epsilon = procededValue;
            while (procededValue <= stopValue)
            {

                schickWolvertonModel = new SchickWolvertonModel(input);

                fiPoints.Add(new ChartPoint() { yvalue = schickWolvertonModel.fi, xvalue = procededValue });
                nPoints.Add(new ChartPoint() { yvalue = schickWolvertonModel.N, xvalue = procededValue });
                etPoints.Add(new ChartPoint() { yvalue = schickWolvertonModel.ET, xvalue = procededValue });

                N.Add(schickWolvertonModel.N);
                ET.Add(schickWolvertonModel.ET);
                FI.Add(schickWolvertonModel.fi);

                procededValue += stepValue;
                input.epsilon = procededValue;


            }
        }
    }
}
