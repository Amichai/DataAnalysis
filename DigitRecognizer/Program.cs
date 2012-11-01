using DataAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitRecognizer {
    class Program {
        static void Main(string[] args) {
            InputLoader loader = new InputLoader();
            loader.LoadFile("digits.csv");
            Label l;
            int i = 0;
            DigitRecognizer recognizer = new DigitRecognizer();
            Dictionary<string, double> parameters = new Dictionary<string, double>() {
                {"NumberOfModels", 100},
                {"MaxNumberOfOperations", 100},
                {"Width", 28},
                {"Height", 28},
            };
            recognizer.ResetModels(parameters);
            while (true) {
                i = i % 25000;
                if (i == 0) i++;
                var a = loader.AccessElement(i, out l);
                recognizer.SetContext(a);
                recognizer.SetLabel(l);
                var output = recognizer.Test();
                recognizer.Train();
                i++;
            }

        }

        static void Main2(string[] args){ 
            double purgeThreshold = .7;
            InputLoader loader = new InputLoader();
            loader.LoadFile("digits.csv");
            StreamProcessor processor = new StreamProcessor(28,28);
            //var count = processor.AddContextFeautres();
            //Debug.Print(count.ToString() + " context features added.");
            processor.GenerateRandomFeatures(1150);
            LinkedList<bool> rollingRightWrong = new LinkedList<bool>();
            int thresholdIdx = 2;
            int correct = 0;
            int i = 1; 
            //for (int i = 1; i < 25000; i++) {
            while(true){
                i = i % 25000;

                //Debug.Print(i.ToString());
                Label l;
                var a = loader.AccessElement(i, out l);
                processor.SetNextFeautreContext(a, l);
                var output = processor.Predict();
                processor.Train();
                var best = output.BestResult();
                if (best != null && best.Item2 != 0) {
                    //Debug.Print(i.ToString() + "  " +
                    //    best.Item1.TextRepresentation + " "
                    //    + best.Item2.ToString());
                    //Debug.Print("Desired: " + processor.DataLabel.TextRepresentation);
                    bool guessedRight = processor.DataLabel.TextRepresentation == best.Item1.TextRepresentation;
                    rollingRightWrong.AddLast(guessedRight);
                    if (guessedRight) {
                        correct++;
                    }
                    if (rollingRightWrong.Count() > 100) {
                        if (rollingRightWrong.First()) {
                            correct--;
                        } rollingRightWrong.RemoveFirst();
                    }

                }

               
                //if(processor.PurgeFeautres(purgeThreshold) > 1000) purgeThreshold+= .01; 
                if (i % 400 == 0) {

                    Debug.Print("Idx: " + i.ToString() + " " + ((double)correct / 100).ToString());
                    processor.PrintUtil(thresholdIdx);
                    thresholdIdx += 2;
                    //string output2 = processor.DescribeAllFeatures();
                    //Debug.Print(output2);
                }
                i++;
            }
            //Get the ability to quickly serialize good heuristics for the future
        }
     
    }
}
