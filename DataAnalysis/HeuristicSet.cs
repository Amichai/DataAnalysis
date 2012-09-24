using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAnalysis {
	public class HeuristicSet<T> {
		//This object can be used for keeping a master store of all heuristic generation mechanisms	
		public HeuristicSet(string labelName, Func<T, IEnumerable<int>> heuristicGenerator, T input) {
			this.Label = new Label(labelName);
			this.Heuristics = heuristicGenerator(input);
		}
		public Label Label;
		public IEnumerable<int> Heuristics;
	}
}
