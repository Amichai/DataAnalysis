using DataAnalysis;
using DigitRecognizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundedLearning {
    public class CurrentHypothesis {
        Dictionary<Label, DetectedPoints> library = new Dictionary<Label, DetectedPoints>();

        public TestResult Predict(DetectedPoints pts) {
            TestResult result = new TestResult();
            Dictionary<Label, double> results = new Dictionary<Label,double>();
            foreach (var a in library) {
                double comparison = a.Value.Compare(pts);
                results[a.Key] = comparison;
            }
            result.Add(results);
            return result;
        }

        internal void Train(DetectedPoints r, Label label) {
            if (!library.ContainsKey(label)) {
                library[label] = r;
            } else {
                return;
            }
        }
    }
}
