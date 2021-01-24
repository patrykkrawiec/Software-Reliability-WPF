using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Software_Reliability;
namespace Software_Reliability_WPF
{

    class JelinskiMorandaSpectrumSolver
    {
        public List<ChartPoint> fiPoints;
        public List<ChartPoint> nPoints;
        public List<ChartPoint> etPoints;

        public List<int> N;
        public List<double> FI;
        public List<double> ET;
        public List<double> stepsLabels;
        private double startValue;
        private double stopValue;
        private double stepValue;
        private ModelInput input;

        JelinskiMorandaModel jelinskiMorandaModel;
        public JelinskiMorandaSpectrumSolver(double startValue, double stopValue, double stepValue, ModelInput input)
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

            stepsLabels = new List<double>();
            this.input = input; 
            solveJelinskiMorandaSpectrum();



        }
        private void solveJelinskiMorandaSpectrum()
        {
            double procededValue = startValue;
            input.epsilon = procededValue;
            while(procededValue <= stopValue)
            {

                jelinskiMorandaModel = new JelinskiMorandaModel(input);

                fiPoints.Add(new ChartPoint() { yvalue = jelinskiMorandaModel.fi, xvalue = procededValue });
                nPoints.Add(new ChartPoint() { yvalue = jelinskiMorandaModel.N, xvalue = procededValue });
                etPoints.Add(new ChartPoint() { yvalue = jelinskiMorandaModel.ET, xvalue = procededValue });

                N.Add(jelinskiMorandaModel.N);
                ET.Add(jelinskiMorandaModel.ET);
                FI.Add(jelinskiMorandaModel.fi);
                stepsLabels.Add(procededValue);
                procededValue += stepValue;
                input.epsilon = procededValue;             


            }
        }

    }
}
