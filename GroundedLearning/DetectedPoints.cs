using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundedLearning {

    /// <summary>
    /// This is our version of a feature vector
    /// </summary>
    /// 
    ///first key is the heuristic index, second key is the heursitic eval value
    public class DetectedPoints : Dictionary<int, Dictionary<int, List<Pixel>>> {
        ///We are interested in the spatial relationship between detected points across different
        ///pieces of input contrlling for their labels
        public double Compare(DetectedPoints pts) {
            double comparison = 0;
            foreach (var heur in pts.Keys) {
                foreach (var eval in pts[heur].Keys) {
                    if (!this[heur].ContainsKey(eval)) {
                        continue;
                    }
                    //We want to compare two geometrical pixel spaces:
                    comparison += comparePixelSpaces(pts[heur][eval], this[heur][eval]);
                    //int union = pts[heur][eval].Union(this[heur][eval]).Count();
                    //double spaceSize = (pts[heur][eval].Count() + this[heur][eval].Count() - union) + .0001;
                    //comparison += ((double)union) / spaceSize;
                }
            }
            return comparison;
        }

        private double comparePixelSpaces(List<Pixel> p1, List<Pixel> p2) {
            //Each pixel space is turned into a feature vector of distances between points 
            ///and slopes between points
            ///these feature vectors are compared with the assumption that points may be missing from one
            ///or the other
            
        }

        private List<double> getDistanceVector(List<Pixel> px) {
            List<double> distances = new List<double>();
            for (int i = 0; i < px.Count(); i++) {
                for (int j = i + 1; j < px.Count(); j++) {
                    distances.Add(px[i].Distance(px[j]));
                }
            }
            return distances;
        }


        /// <summary>
        /// Accounting for the inverse of each slope complicates things quite a bit.
        /// For now we will ignore the slope vector...
        /// </summary>
        /// <param name="px"></param>
        /// <returns></returns>
        private List<double> getSlopeVector(List<Pixel> px) {
            List<double> slopes = new List<double>();
            for (int i = 0; i < px.Count(); i++) {
                for (int j = i + 1; j < px.Count(); j++) {
                    slopes.Add(px[i].Slope(px[j]));
                }
            }
            return slopes;
        }
    }

    public class DetectedPoint : Dictionary<int, List<Pixel>> {
        //Point type index 
        //List of pixels
    }
}
