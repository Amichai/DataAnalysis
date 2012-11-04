using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundedLearning {
    public class PointsOfInterest {
        public int NumberOfOperations { get; set; }
        public int DetectionRadius { get; set; }
        public int Quantity { get; set; }
        public int IndexVal { get; set; }
        public Dictionary<int,List<Pixel>> EvalOfInterestCount { get; set; }
        List<Operation> operations = new List<Operation>();

        public PointsOfInterest(int numOfOperations, int detectRadius, int quantity, int idxVal) {
            this.NumberOfOperations = numOfOperations;
            this.DetectionRadius = detectRadius;
            this.Quantity = quantity;
            this.EvalOfInterestCount = new Dictionary<int, List<Pixel>>();
            this.IndexVal = idxVal;
            for (int i = 0; i < numOfOperations; i++) {
                operations.Add(new Operation(detectRadius));
            }
        }

        public Dictionary<int, List<Pixel>> Locate(int[][] input) {
            Dictionary<int, List<Pixel>> valueCount = new Dictionary<int,List<Pixel>>();
            int currentVal;
            //Offset for detection radius
            for (int i = DetectionRadius; i < input.Length - DetectionRadius; i++) {
                for (int j = DetectionRadius; j < input[0].Length - DetectionRadius; j++) {
                    currentVal = 0;
                    foreach (var a in operations) {
                        currentVal = a.operation(currentVal,
                            input[i + a.xVal]
                            [j + a.yVal]);
                    }
                    if (valueCount.ContainsKey(currentVal)) {
                        valueCount[currentVal].Add(new Pixel() { xVal = i, yVal = j });
                    } else {
                        valueCount[currentVal] = new List<Pixel>();
                    }
                }
            }
            EvalOfInterestCount = valueCount.Where(i => i.Value.Count() < this.Quantity).ToDictionary(i => i.Key, i => i.Value);
            return EvalOfInterestCount;
        }
    }

    public class Operation {
        static Random rand = new Random();
        public int xVal, yVal;
        public Func<int, int, int> operation;
        private int detectRadius;
        public Operation(int detectRadius) {
            // TODO: Complete member initialization
            this.detectRadius = detectRadius;
            this.xVal = rand.Next(0, detectRadius * 2) - detectRadius;
            this.yVal = rand.Next(0, detectRadius * 2) - detectRadius;
            if (rand.Next(0, 2) == 0) {
                operation = (i, j) => i + j;
            } else {
                operation = (i, j) => i - j;
            }
        }
    }
}
