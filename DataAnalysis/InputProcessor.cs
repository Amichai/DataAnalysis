using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DataAnalysis {
	/// <summary>
	/// Takes some labeled input data
	/// Process that input and returns a labeled heuristic array for probabilistic analysis
	/// </summary>
	public class InputProcessor {
		//Should handle images, text, etc,
		HeurisiticArray<List<int>> heurisiticArray;

		public bool PixelsAsHeuristics { get; set; }
		public InputProcessor(List<int> image) {
			heurisiticArray = new HeurisiticArray<List<int>>(new List<int>());

			//define the functions which generate these heuristics
			var pixelData = new HeuristicSet<List<int>>("Pixel Data", i => new List<int>(), image);
			heurisiticArray.AddHeuristics(pixelData);
			var colorData = new HeuristicSet<List<int>>("Color data", i => new List<int>(), image);
			heurisiticArray.AddHeuristics(colorData);
			
				
		}
	}
}
