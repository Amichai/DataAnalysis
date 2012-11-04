using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundedLearning {
    public class HeuristicDetection {
        public int MaxNumberOfOperations { get; set; }
        public int MaxRadius { get; set; }
        public int Quantity { get; set; }
        public int NumberOfPoints { get; set; }
        Random rand;
        public List<PointsOfInterest> pointsOfInterest { get; set; }
        public HeuristicDetection(int maxOperations, int maxRadius, int quantity, int numberOfPoints) {
            this.rand = new Random();
            this.MaxNumberOfOperations = maxOperations;
            this.MaxRadius = maxRadius;
            this.Quantity = quantity;
            pointsOfInterest = new List<PointsOfInterest>(numberOfPoints);
            this.NumberOfPoints = numberOfPoints;
            for (int i = 0; i < this.NumberOfPoints; i++) {
                int numberOfOperations = rand.Next(1, this.MaxNumberOfOperations);
                int radius = rand.Next(1, this.MaxRadius);
                pointsOfInterest.Add(HeuristicDetection.Generate(numberOfOperations, radius, quantity));
            }
        }

        private static int counter = 0;

        public static PointsOfInterest Generate(int numOfOperations, int detectionRadius, int quantity) {
            PointsOfInterest p = new PointsOfInterest(numOfOperations, detectionRadius, quantity, counter++);
            return p;
        }

        

        public DetectedPoints getFeatureVector(int[][] p) {
            DetectedPoints output = new DetectedPoints();
            foreach (var a in pointsOfInterest) {
                output[a.IndexVal] = a.Locate(p);
            }
            return output;
        }

        ///How does a feature vector work?
        ///Given pairs of feature indices
        ///we return distance and slope
        ///Identity can be measured 
    }
}
