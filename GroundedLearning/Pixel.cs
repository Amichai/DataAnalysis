using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroundedLearning {
    public class Pixel {
        public int xVal, yVal;

        public double Distance(Pixel p) {
            return Math.Sqrt((p.xVal - xVal).Sqrd() + (p.yVal - yVal).Sqrd());
        }

        public double Slope(Pixel p) {
            return (p.yVal - yVal) / (p.xVal - xVal + .0001);
        }
    }

    public static class ext {
        public static double Sqrd(this double p) {
            return Math.Pow(p, 2);
        }

        public static double Sqrd(this int p) {
            return Math.Pow(p, 2);
        }
    }
}
