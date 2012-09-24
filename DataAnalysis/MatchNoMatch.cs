using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAnalysis {
	/// <summary>
	/// Basically an alternative to the tuple structure
	/// </summary>
	public class MatchNoMatch {
		public MatchNoMatch() {
			Matches = 0;
			NonMatches = 0;
		}
		public int Matches { get; set; }
		public int NonMatches { get; set; }

		public void Match() {
			Matches++;
		}

		public void NonMatch() {
			NonMatches++;
		}
	}
}
