using DataAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitRecognizer {
    public class DigitRecognizer : IRecognize<int[][]> {
        public DigitRecognizer() {
            this.TrainCount = 0;
            this.ContextSet = false;
            this.LabelSet = false;
            this.InternalModels = new List<IRecognize<int[][]>>();
            this.sw = new Stopwatch();
        }

        Stopwatch sw;

        public int TrainCount { get; set; }
        public int[][] Context { get; set; }
        public void SetContext(int[][] context) {
            this.ResultSet = false;
            this.Context = context;
            this.ContextSet = true;
        }

        public bool ContextSet { get; set; }

        public Label Label { get; set; }
        public void SetLabel(Label l) {
            if (!ContextSet) throw new Exception("Set the context first.");
            this.Label = l;
            LabelSet = true;
        }

        public bool LabelSet { get; set; }

        public TestResult Test() {
            sw.Reset();
            TestResult result = new TestResult();
            foreach (var a in InternalModels) {
                a.SetContext(Context);
                TestResult output = a.Test();
                result.Add(output);
            }
            TestComplete(this, new EventArgs());
            this.TimeToCompleteLastTest = sw.Elapsed;
            this.ResultSet = true;
            this.LastResult = result;
            return result;
        }

        public TestResult LastResult { get; set; }
        public bool ResultSet { get; set; }

        public void Train() {
            if (!LabelSet || !ResultSet) throw new Exception();
            IndicationStrength.Push(LastResult, Label);
            foreach (var f in InternalModels) {
                f.Train();
            }

            this.ContextSet = false;
            this.LabelSet = false;
            this.TrainCount++;
            TrainComplete(this, new EventArgs());
        }

        public IndicationStrength IndicationStrength { get; set; }

        public event EventHandler TestComplete;

        public TimeSpan TimeToCompleteLastTest { get; set; }

        public event EventHandler TrainComplete;

        public List<IRecognize<int[][]>> InternalModels { get; set; }

        public Dictionary<Label, IndicationStrength> IndicationStrengthPerLabel { get; set; }

        public void ResetModels(Dictionary<string, double> parameters) {
            InternalModels.Clear();
            this.TrainCount = 0;
            this.ContextSet = false;
            this.LabelSet = false;
            this.sw = new Stopwatch();
            this.IndicationStrength = new IndicationStrength();
            this.IndicationStrengthPerLabel = new Dictionary<Label, IndicationStrength>();


            int numberOfModels = (int)Math.Round(parameters["NumberOfModels"]);
            int maxNumberOfOperations = (int)Math.Round(parameters["MaxNumberOfOperations"]);
            int width = (int)Math.Round(parameters["Width"]);
            int height = (int)Math.Round(parameters["Height"]);
            for (int i = 0; i < numberOfModels; i++) {
                InternalModels.Add(FeatureModel.New(parameters));

            }
        }

        public void PurgeBadModels(Dictionary<string, double> parmaeters) {
            throw new NotImplementedException();
        }
    }
}
