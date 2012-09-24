using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAnalysis {
	public class HeuristicIndicationArray {
		public HeuristicIndicationArray(Label method, IEnumerable<int> heur1, IEnumerable<int> heur2) {
			if (heur1.Count() != heur2.Count()) throw new Exception();
			for (int i = 0; i < heur1.Count(); i++) {
				//I'm pretty sure this is fine, but we may want to check that this actually enumerates the whole thing just once:
				if (heur1.ElementAt(i) == heur2.ElementAt(i)) { 
					indications[method][i].Match(); //match
				} else {
					indications[method][i].NonMatch(); //not a match
				}
			}
		}
		//List<Tuple<match, noMatch>
		Dictionary<Label, List<MatchNoMatch>> indications;
		public HeuristicIndicationArray() {
			this.indications = new Dictionary<Label, List<MatchNoMatch>>();
		}

		public void Add(Label method, IEnumerable<int> heur1, IEnumerable<int> heur2) {
			if (heur1.Count() != heur2.Count()) throw new Exception();
			for (int i = 0; i < heur1.Count(); i++) {
				//I'm pretty sure this is fine, but we may want to check that this actually enumerates the whole thing just once:
				if (heur1.ElementAt(i) == heur2.ElementAt(i)) {
					indications[method][i].Match(); //match
				} else {
					indications[method][i].NonMatch(); //not a match
				}
			}
		}
	}
}
