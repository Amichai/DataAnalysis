using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DataAnalysis {
	public class InputLoader {
		private string filename;

		public string Filename {
			get { return filename; }
			set { filename = value; }
		}

		private string[] lines;

		public string[] Lines {
			get { return lines; }
			set { lines = value; }
		}
		
		public void LoadFile(string filename) {
			this.Filename = filename;
			this.Lines = File.ReadAllLines(filename);

			/*
			var values = lines.Skip(1).Select(l => 
				new { FirstColumn = l.Split(',').First(), Values = l.Split(',').Skip(1).Select(v => int.Parse(v)) });
			var library = new TrainingDataLibrary<int[][]>();
			foreach (var a in values) {
				int[][] inputData = new int[28][];
				for (int i = 0; i < 28; i++) {
					inputData[i] = new int[28];
				}

				for (int i = 0; i < a.Values.Count(); i++) {
					inputData[i / 28][i % 28] = a.Values.ElementAt(i);
				}
				var set = new HeuristicSet<int[][]>("pixels", pixel => a.Values, inputData);
				var b = new HeurisiticArray<int[][]>(inputData);
				b.AddLabel(new Label(a.FirstColumn));
				b.AddHeuristics(set);
				ui.Add(b);
				break;
			}
			 */
		}

        public int[][] AccessElement(int at, out Label label) {
            var a = lines.ElementAt(at);
            label = new Label(a.Split(',').First());
            var data = a.Split(',').Skip(1).Select(v => int.Parse(v));
            int[][] inputData = new int[28][];
            for (int i = 0; i < 28; i++) {
                inputData[i] = new int[28];
            }

            for (int i = 0; i < data.Count(); i++) {
                inputData[i / 28][i % 28] = data.ElementAt(i);
            }
            return inputData;
        }

        public HeurisiticArray<int[][]> AccessElement(int at) {
            var a = lines.ElementAt(at);
            var label = a.Split(',').First();
            var data = a.Split(',').Skip(1).Select(v => int.Parse(v));
            int[][] inputData = new int[28][];
            for (int i = 0; i < 28; i++) {
                inputData[i] = new int[28];
            }

            for (int i = 0; i < data.Count(); i++) {
                inputData[i / 28][i % 28] = data.ElementAt(i);
            }
            var set = new HeuristicSet<int[][]>("pixels", pixel => data, inputData);
            var b = new HeurisiticArray<int[][]>(inputData);
            b.AddLabel(new Label(label));
            b.AddHeuristics(set);
            return b;
        }
	}
}
