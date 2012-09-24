using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DataAnalysis {
	/// <summary>
	/// All heuristic data (computed lazily) for a single input label
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class HeurisiticArray<T> {
		private Dictionary<Label, IEnumerable<int>> heuristics;
		private T inputData;

		public IEnumerable<Label> HeuristicMethods() {
			return heuristics.Keys;
		}

		public override string ToString() {
			string output = string.Empty;
			foreach (var a in InputLabels()) {
				output += a.TextRepresentation + " ";
			}
			output += "\n";
			foreach (var a in heuristics) {
				foreach (var b in a.Value) {
					output += b.ToString() + " ";
				}
			}
			return output;
		}

		public IEnumerable<int> GetAllHeuristics() {
			foreach (var a in heuristics.Keys) {
				foreach(var b in GetHeuristics(a)){
					yield return b;
				}
			}
		}

		public IEnumerable<int> GetHeuristics(Label heuristicMethod) {
			return heuristics[heuristicMethod];
		}

		public IEnumerable<Label> GetHeuristicMethods() {
			return heuristics.Keys.AsEnumerable<Label>();
		}

		public HeurisiticArray(T inputData) {
			this.inputLabels = new HashSet<Label>();
			this.heuristics = new Dictionary<Label, IEnumerable<int>>();
			this.inputData = inputData;
		}

		HashSet<Label> inputLabels;

		public void AddLabels(IEnumerable<Label> labels) {
			foreach (var l in labels) {
				inputLabels.Add(l);
			}
		}

		public IEnumerable<Label> InputLabels() {
			return inputLabels;
		}

		public void AddLabel(Label lbl) {
			inputLabels.Add(lbl);
		}

		public void AddHeuristics(HeuristicSet<T> set) {
			heuristics.Add(set.Label, set.Heuristics);
		}

		//Methods to lookup against the training library using a variety of methods
		//Methods of probabilistic analysis
	}
}
