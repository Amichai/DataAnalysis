using Accord.Statistics.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAnalysis {
    public abstract class Feature {
        public abstract double Evaluate(int[][] context);
        public double Eval { get; set; }
        public Dictionary<Label, RunningStats> stats = new Dictionary<Label, RunningStats>();
        public abstract void Regenerate(int maxNumOfOperations, int width, int height);

        public Dictionary<Label, double> Apply(double val, double utilityThreshold) {
            double normalizationVal = 0;
            Dictionary<Label, double> evals = new Dictionary<Label, double>();
            foreach (var a in stats) {
                double util = a.Value.ProbabilisticUtility();
                if (util < utilityThreshold) {
                    continue;
                }
                double eval = a.Value.Apply(val);
                if (double.IsNaN(eval)) {
                    evals[a.Key] = 0;
                } else {
                    evals[a.Key] = eval;
                    normalizationVal += eval;
                }
            }
            if (normalizationVal == 0) return evals;
            foreach (var a in evals.Keys.ToList()) {
                evals[a] /= normalizationVal;
            }
            return evals;
        }

        public Dictionary<Label, double> Apply(double val) {
            double normalizationVal = 0 ;
            Dictionary<Label, double> evals = new Dictionary<Label, double>();
            foreach (var a in stats) {                
                double eval = a.Value.Apply(val);
                if (double.IsNaN(eval)) {
                    evals[a.Key] = 0;
                } else {
                    evals[a.Key] = eval;
                    normalizationVal += eval;
                }
            }
            if (normalizationVal == 0) return evals;
            foreach (var a in evals.Keys.ToList()) {
                evals[a] /= normalizationVal;
            }
            return evals;
        }

        public void Train(double val, Label label) {
            if (!stats.ContainsKey(label)) {
                stats[label] = new RunningStats();
            }
            stats[label].Add(val, stats[label].Apply(val));
            if (stats[label].ElementsSeen < 10) return;
        }

        public double TestUtility() {
            RunningNormalStatistics utilityAve = new RunningNormalStatistics();
            int counter = 0;
            foreach (var a in stats) {
                if (a.Value.ElementsSeen > 20) {
                    utilityAve.Push(a.Value.ProbabilisticUtility());
                    counter++;
                }
            }
            if (counter > 8) {
                return utilityAve.Mean;
            } else {
                //Debug.Print(counter.ToString());
                return -1;
            }
        }

    }

    public class ContextFeature : Feature {



        private int i;
        private int j;
        public ContextFeature(int i, int j) {
            this.i = i;
            this.j = j;
        }

        public override void Regenerate(int maxNumOfOperations, int width, int height) {
            throw new NotImplementedException();
        }

        public override double Evaluate(int[][] context) {
            this.Eval = (double)context[i][j];
            return this.Eval;
        }

        public static List<ContextFeature> AllContextFeatures(int width, int height) {
            List<ContextFeature> features = new List<ContextFeature>(width* height);
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    features.Add(new ContextFeature(i, j));
                }
            }
            return features;
        }
    }

    public class ComputeFeature : Feature {
        public static ComputeFeature RandomFeature(int maxNumOfOperations, int width, int height) {
            int numOfOperations = rand.Next(1, maxNumOfOperations);
            int operationLib = operations.Count();
            ComputeFeature f = new ComputeFeature();
            for (int i = 0; i < numOfOperations; i++) {
                f.operationStack.Add(new AtomicOperation(rand.Next(0, width), rand.Next(0, height), rand.Next(0, operationLib)));
            }
            return f;
        }

        public override void Regenerate(int maxNumOfOperations, int width, int height) {
            stats.Clear();
            operationStack.Clear();
            int numOfOperations = rand.Next(1, maxNumOfOperations);
            int operationLib = operations.Count();
            for (int i = 0; i < numOfOperations; i++) {
                operationStack.Add(new AtomicOperation(rand.Next(0, width), rand.Next(0, height), rand.Next(0, operationLib)));
            }
        }

        public override double Evaluate(int[][] context) {
            double eval = context[operationStack.First().i][operationStack.First().j] + .1;
            for (int i = 1; i < operationStack.Count(); i++) {
                var a = operationStack[i];
                double val1 = context[a.i][a.j] + .1;
                eval = operations[a.operationIndex](eval, val1);
                if (double.IsNaN(eval) || double.IsInfinity(eval)) {
                }
            }
            this.Eval = eval;

            return eval;
        }

        List<AtomicOperation> operationStack = new List<AtomicOperation>();
        static Random rand = new Random();
        static List<Func<double, double, double>> operations = new List<Func<double, double, double>>() {
            (i, j) => i + j, //0
            //(i, j) => i * j,
            (i, j) => i - j, //2 
            //(i, j) => { if(j < .001) return 1000; else return i / j; },
            //(i, j) => { if(Math.Abs(j) < Math.Abs(i)) return j / i; else return i / j; },
           // (i, j) => Math.Max(i,j),
            //(i, j) => Math.Min(i,j),
        };

        class AtomicOperation {
            public AtomicOperation(int i, int j, int operation) {
                this.i = i;
                this.j = j;
                this.operationIndex = operation;
            }
            public int i, j;
            public int operationIndex;
        }
    }
}
