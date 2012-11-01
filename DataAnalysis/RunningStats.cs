using Accord.Statistics.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis {
    /// <summary>
    /// Every feature heuristics keeps a running stat object for each known output label
    /// We assume a normal distribution of 
    /// </summary>
    public class RunningStats {
        public RunningStats() {
            this.ProbabilityUtilData = false;
        }
        //TODO: we should be tracking more than the mean and variance - we should be tracking all moments
        ///TODO: use a highly optimized normality test
        RunningNormalStatistics stat = new RunningNormalStatistics();
        RunningNormalStatistics utilStats = new RunningNormalStatistics();
        RunningNormalStatistics probReturnedWhenCorrect = new RunningNormalStatistics();
        RunningNormalStatistics probReturnedInGeneral = new RunningNormalStatistics();

        /// <summary>
        /// DEBUG only:
        /// </summary>
        double maxVal = int.MinValue;
        double minVal = int.MaxValue;
        public int ElementsSeen = 0;
        public double Apply(double a) {
            if (maxVal == double.MinValue || minVal == double.MaxValue) return 0;
            if (a > maxVal || a < minVal) return 0;
            double range = maxVal - minVal;
            if (range == 0) range = .0001;
            //double distanceFromMean = Math.Abs(stat.Mean - a) / (maxVal - minVal);
            double distanceFromMean = Math.Abs(minVal + range / 2.0 - a) / (range);
            double prob = Math.Abs(2 - distanceFromMean * 2) / range; 

            probReturnedInGeneral.Push(prob);
            //Debug.Print(prob.ToString());
            return prob;
        }

        public void Add(double a) {
            if (a < minVal) minVal = a;
            if (a > maxVal) maxVal = a;
            stat.Push(a);
            ElementsSeen++;
        }

        public bool ProbabilityUtilData { get; set; }

        public void Add(double eval, double probabilityReturned) {
            if (eval < minVal) minVal = eval;
            if (eval > maxVal) maxVal = eval;
            stat.Push(eval);
            probReturnedWhenCorrect.Push(probabilityReturned);
            ElementsSeen++;
            ProbabilityUtilData = true;
        }

        public double ProbabilisticUtility() {
            if (!ProbabilityUtilData) throw new Exception();
            //What was the average probability returned when you gave the right answer?
            if (probReturnedInGeneral.Mean == 0) {
                return (probReturnedWhenCorrect.Mean / .0001);
            }
            return (probReturnedWhenCorrect.Mean / probReturnedInGeneral.Mean);
        }

        public double VariancePerElement() {
            return stat.Variance / ElementsSeen;
        }
    }
}
