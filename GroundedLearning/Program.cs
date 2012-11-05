using DataAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitRecognizer;
using System.Diagnostics;

namespace GroundedLearning {
    class Program {
        static Random rand = new Random();
        static void Main(string[] args) {
            InputLoader loader = new InputLoader();
            loader.LoadFile("digits.csv");
            Stopwatch sw = new Stopwatch();

            var heursiticDetection = new HeuristicDetection(10, 5, quantity:50, numberOfPoints:500);
            var hypothesis = new CurrentHypothesis();
            foreach (var input in loader.AllElements()) {
                ///For every new input we extract n points of interest
                ///And create a feature vector which characterizes the spatial relationship between these features
                ///For every heuristic we get a dictionary of points of interest
                DetectedPoints v = heursiticDetection.getFeatureVector(input.Item1);

                ///Compare this feature vector agaist each of the other feature vectors we know about
                sw.Reset();
                sw.Start();
                TestResult r = hypothesis.Predict(v);
                Debug.Print("Prediction: " + sw.Elapsed.Milliseconds.ToString());
                var best= r.BestResult();
                if(best != null && best.Item2 != 0){
                    LogProgress(best.Item1, input.Item2);
                }

                sw.Reset();
                sw.Start();
                hypothesis.Train(v, input.Item2, r);
                Debug.Print("Training: " + sw.Elapsed.Milliseconds.ToString());
                //heursiticDetection.pointsOfInterest.Add(HeuristicDetection.Generate(10, 5, 10));
            }
        }

        static LinkedList<bool> rollingRightWrong = new LinkedList<bool>();
        static int correct = 0;
        static void LogProgress(Label guessed, Label target) {
                bool guessedRight = guessed.TextRepresentation == target.TextRepresentation;
                rollingRightWrong.AddLast(guessedRight);
                if (guessedRight) {
                    correct++;
                }
                if (rollingRightWrong.Count() > 100) {
                    if (rollingRightWrong.First()) {
                        correct--;
                    } rollingRightWrong.RemoveFirst();
                }
                Debug.Print(((double)correct / rollingRightWrong.Count()).ToString());

            }
        }
    }
