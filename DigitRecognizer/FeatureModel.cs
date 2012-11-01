using DataAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitRecognizer {
    public class FeatureModel : IRecognize<int[][]> {
        public FeatureModel() {
            this.EvalSet = false;
            this.ContextSet = false;
            this.LabelSet = false;
            this.TrainCount = 0;
            this.stats = new Dictionary<Label, RunningStats>();
            this.sw = new Stopwatch();
        }

        Stopwatch sw;

        public void SetContext(int[][] context) {
            this.Context = context;
            this.ContextSet = true;
        }

        public int[][] Context { get; set; }

        public bool ContextSet { get; set; }

        public void SetLabel(Label l) {
            this.Label = l;
            this.LabelSet = true;
        }

        public Label Label { get; set; }

        public bool LabelSet { get; set; }

        public int TrainCount { get; set; }

        public TestResult Test() {
            sw.Reset();
            double val = this.Context[operationStack.First().i][operationStack.First().j] + .1;
            for (int i = 1; i < operationStack.Count(); i++) {
                var a = operationStack[i];
                double val1 = this.Context[a.i][a.j] + .1;
                val = operations[a.operationIndex](val, val1);
                if (double.IsNaN(val) || double.IsInfinity(val)) {
                }
            }
            this.Eval = val;
            this.EvalSet = true;
            TestResult result = new TestResult();
            foreach (var a in stats) {
                double prob = a.Value.Apply(val);
                result.Push(a.Key, prob);
            }
            //TestComplete(this, new EventArgs());
            this.TimeToCompleteLastTest = sw.Elapsed;
            this.ResultSet = true;
            this.LastResult = result;
            return result;
        }

        public event EventHandler TestComplete;

        public TestResult LastResult { get; set; }

        public bool ResultSet { get; set; }

        public TimeSpan TimeToCompleteLastTest { get; set; }

        public void Train() {
            if (!LabelSet || !ResultSet || !EvalSet) throw new Exception();
            IndicationStrength.Push(this.LastResult, this.Label);

            if (!stats.ContainsKey(this.Label)) {
                stats[this.Label] = new RunningStats();
            }
            stats[this.Label].Add(this.Eval, stats[this.Label].Apply(this.Eval));

            this.ContextSet = false;
            this.LabelSet = false;
            this.TrainCount++;
            TrainComplete(this, new EventArgs());
            throw new NotImplementedException();
        }

        public IndicationStrength IndicationStrength { get; set; }

        public event EventHandler TrainComplete;

        public List<IRecognize<int[][]>> InternalModels { get; set; }

        public Dictionary<Label, IndicationStrength> IndicationStrengthPerLabel { get; set; }

        public bool EvalSet { get; set; }
        public double Eval { get; set; }

        public void ResetModels(Dictionary<string, double> parameters) {
            int maxNumberOfOperations = (int)Math.Round(parameters["maxNumberOfParameters"]);
            int width = (int)Math.Round(parameters["width"]);
            int height = (int)Math.Round(parameters["height"]);
            stats.Clear();
            operationStack.Clear();
            int operationLib = operations.Count();
            for (int i = 0; i < maxNumberOfOperations; i++) {
                operationStack.Add(new AtomicOperation(rand.Next(0, width), rand.Next(0, height), rand.Next(0, operationLib)));
            }
        }

        public Dictionary<Label, RunningStats> stats;

        public void PurgeBadModels(Dictionary<string, double> parmaeters) {
            throw new NotImplementedException();
        }

        static Random rand = new Random();
        static List<Func<double, double, double>> operations = new List<Func<double, double, double>>() {
            (i, j) => i + j, //0
            (i, j) => i - j, //2 
        };
        List<AtomicOperation> operationStack = new List<AtomicOperation>();

        public static FeatureModel New(Dictionary<string, double> parameters){
            int maxNumberOfOperations = (int)Math.Round(parameters["MaxNumberOfOperations"]);
            int width = (int)Math.Round(parameters["Width"]);
            int height = (int)Math.Round(parameters["Height"]);

            int numOfOperations = rand.Next(1, maxNumberOfOperations);
            int operationLib = operations.Count();
            FeatureModel f = new FeatureModel();
            for (int i = 0; i < numOfOperations; i++) {
                f.operationStack.Add(new AtomicOperation(rand.Next(0, width), rand.Next(0, height), rand.Next(0, operationLib)));
            }
            return f;

            throw new NotImplementedException();
        }

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
