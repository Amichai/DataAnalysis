using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAnalysis {
	public class TrainingDataLibrary<T> {
		Dictionary<Label, List<double>> probabilityArrays;
		//Dictionary<List<Label>, HeurisiticArray<T>> allTrainingData;
		Dictionary<Label, List<HeurisiticArray<T>>> allTrainingData;

		public void Add(HeurisiticArray<T> trainingData) {
			foreach(var a in trainingData.HeuristicMethods()){
				//If contains, add. Otherwise create new
				if (allTrainingData.ContainsKey(a)) {
					allTrainingData[a].Add(trainingData);
				} else {
					allTrainingData[a] = new List<HeurisiticArray<T>>() { trainingData};
				}
			}
		}

		/// <summary>
		/// Returns certainty associated with all possible input labels
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public Dictionary<Label, LabelLookupResult> Resolve(T input) {
			HeurisiticArray<T> ha = new HeurisiticArray<T>(input);
			//Add a bunch of heuristics:
			//ha.AddHeuristics()
			return Resolve(ha);
		}

		//To resolve probabilistic information:
		//Given a particular unlabeled heuristic value:
		//For each possible label, how well does this value correlate (and anti-correlate) 
		//with a particular piece of input data? 
		//Dictionary<training data element, correlationVal>
		public Dictionary<Label, LabelLookupResult> Resolve(HeurisiticArray<T> ha) {
			//Iterate over every possible output label
			//For each output label iterate over every piece of training data 
			//For each piece of training data iterate over every heuristic
			//Construct: Dictionary<Label, Dictionary<heurisitcMethod,List<Tuple<match, noMatch>>>
			Dictionary<Label, HeuristicIndicationArray> indication
				 = new Dictionary<Label, HeuristicIndicationArray>();
			foreach (var td in allTrainingData) {
				foreach (var heuristicArray in td.Value) {
					foreach(var method in ha.GetHeuristicMethods()){
						var heur1 = heuristicArray.GetHeuristics(method);
						var heur2 = heuristicArray.GetHeuristics(method);
						//Make sure that heur2 actually exists
						indication[td.Key].Add(method, heur1, heur2);

					}
				}
			}
			throw new NotImplementedException();
		}
	}
}
