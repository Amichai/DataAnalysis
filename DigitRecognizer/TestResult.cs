using DataAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitRecognizer {
    public class TestResult {
        public TestResult() {
            this.outputLabels = new Dictionary<Label, double>();
            this.Normalized = false;
        }
        Dictionary<Label, double> outputLabels;

        double normalizationVal = 0;

        public void Add(Dictionary<Label, double> output) {
            foreach (var a in output) {
                normalizationVal += a.Value;
                if (double.IsNaN(normalizationVal) || double.IsInfinity(normalizationVal)) {
                }
                if (!outputLabels.ContainsKey(a.Key)) {
                    outputLabels[a.Key] = 0;
                }
                outputLabels[a.Key] += a.Value;
            }
        }
        public bool Normalized { get; set; }

        public Dictionary<Label, double> Result() {
            if (normalizationVal == 0) throw new Exception();
            foreach (var a in outputLabels.Keys.ToList()) {
                outputLabels[a] /= normalizationVal;
            }
            this.Normalized = true;
            return outputLabels;
        }

        public Tuple<Label, double> BestResult() {
            if (outputLabels.Count() == 0 || normalizationVal == 0) return null;
            var a = outputLabels.OrderByDescending(i => i.Value).First();
            return new Tuple<Label, double>(a.Key, a.Value / normalizationVal);
        }

        internal void Add(TestResult output) {
            throw new NotImplementedException();
        }

        internal void Push(Label label, double eval) {
            throw new NotImplementedException();
        }
    }
}
