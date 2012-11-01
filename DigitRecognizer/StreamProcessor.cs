using DataAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitRecognizer {
    class StreamProcessor {
        public StreamProcessor(int width, int height) {
            this.Width = width;
            this.Height = height;
            this.Features = new List<Feature>();
        }
        public int Width { get; set; }
        public int Height { get; set; }
        int[][] featureContext;
        public Label DataLabel;
        public void SetNextFeautreContext(int[][] features, Label label) {
            this.featureContext = features;
            this.DataLabel = label;
        }

        /// <summary>
        /// Returns the number of features added.
        /// </summary>
        public int AddContextFeautres() {
            var a = ContextFeature.AllContextFeatures(this.Width, this.Height);
            Features.AddRange(a);
            return a.Count();
        }

        public void GenerateRandomFeatures(int featuresToGenerate) {
            for (int i = 0; i < featuresToGenerate; i++) {
                Features.Add(ComputeFeature.RandomFeature(500, this.Width, this.Height));
            }
        }

        List<Feature> Features;

        double utilThreshold = 0;
        /// <summary>
        /// At this point we pretend that we don't know the correct output label for the most recent piece of data
        /// </summary>
        public TestResult Predict() {
            TestResult result = new TestResult();
            List<int> indicesToRegenerate = new List<int>();
            int i = 0; 
            foreach (var a in Features) {
                double eval = a.Evaluate(featureContext);
                var output = a.Apply(eval, utilThreshold);
                //if (output.Count() == 0 && a.stats.Count() > 3) {
                //    indicesToRegenerate.Add(i);
                //}
                i++;
                result.Add(output);
            }

            for (int j = 0; j < indicesToRegenerate.Count(); j++) {
                Features[indicesToRegenerate[j]].Regenerate(30, this.Width, this.Height);
            }
            return result;
        }

        internal void Train() {
            foreach (var f in Features) {
                f.Train(f.Eval, DataLabel);
            }
        }

        internal int PurgeFeautres(double threshold) {
            int preserveCounter = 0;
            int provisional = 0;
            int regenerateCounter = 0;
            List<double> utilVals = new List<double>();
            foreach (var f in Features) {
                double util = f.TestUtility();
                if (util != -1 && util < threshold) {
                    Debug.Print("Feature regenerated. Old util: " + util.ToString());
                    f.Regenerate(500, this.Width, this.Height);
                    regenerateCounter++;
                    //f = ComputeFeature.RandomFeature(10, this.Width, this.Height);
                } else if(util == -1) {
                     provisional++;
                    //if (util != -1) Debug.Print("Feature presevred.");
                } else if (util >= threshold) {
                    preserveCounter++;
                }
            }
            //if (provisional > Features.Count() - 10) {
            //    threshold -= .1;
            //    PurgeFeautres(threshold);
            //}
            Debug.Print(preserveCounter.ToString() + " preserved, " + provisional.ToString() + " provisional, " + regenerateCounter.ToString() + " regenerated.");
            return preserveCounter;
        }

        internal void PrintUtil(int thresholdIdx) {
            List<double> utils = new List<double>();
            foreach (var a in Features) {
                double eval = a.TestUtility();
                utils.Add(eval);
                //Debug.Print("Util: " + eval.ToString());
            }
            var ordered = utils.OrderByDescending(i => i);
            double thresholdVal = ordered.ElementAt(thresholdIdx);
            Debug.Print("Purge cuttoff: " + thresholdVal.ToString() + " threshold Idx: " + thresholdIdx.ToString() + " average: " + utils.Average().ToString() + " max: " + ordered.First().ToString());
            foreach (var a in Features) {
                double eval = a.TestUtility();
                if (eval < thresholdVal) {
                    a.Regenerate(500, this.Width, this.Height);
                }
                //Todo: compare the stats of the features we keep vs. the features we reject
                //Treat this as a pattern recognition problem
            }

            //Debug.Print("Util: " + utils.Average().ToString());
        }
    }
}
