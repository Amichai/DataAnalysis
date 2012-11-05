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

        internal void Train(DetectedPoints pts, Label label, TestResult result) {
            if (!library.ContainsKey(label)) {
                library[label] = pts;
            } else {
                var baseVal = library[label].Compare(pts);
                foreach (var a in library) {
                    if (a.Key == label) continue;
                    var a1 = a.Value.Compare(pts);
                    if (a1 > baseVal) {
                        a.Value.Sanitize(pts);
                    }
                }
            }
        }
    }
}
