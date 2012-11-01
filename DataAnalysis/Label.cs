using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAnalysis {
	/// <summary>
	/// This is to be used internally as an alternative to string labels
	/// Useful for logging, visualizing and preventing duplicate key errors, etc.
	/// Labels with the same id are identical irrespective of other properties
	/// </summary>
	public class Label {
		private int id;

		public int ID {
			get { return id; }
			set { id = value; }
		}

		private string textRep;

		public string TextRepresentation {
			get { return textRep; }
			set { textRep = value; }
		}

		private DateTime creationTime;

		public DateTime CreationTime {
			get { return creationTime; }
			set { creationTime = value; }
		}
		
		
		public static int LabelCounter = 0;
		//Amount of times each type of label was instantiated within this application instance
		public static Dictionary<Label, int> numberOfLabels;

		/// <summary>
		/// The id numbers associated with seen label strings.
		/// </summary>
		Dictionary<string, int> idNumbers;

		//Perhaps we want to add an optional "label type parameter?"
		//We may want to perform a case invariant check
		//We may want to facilitate a search of extant labels, etc.
		public Label(string labelName) {
			//For now we are going to make this case invariant.
			labelName = labelName.ToUpper();

			idNumbers = new Dictionary<string, int>();
			if (idNumbers.ContainsKey(labelName)) {
				this.ID = idNumbers[labelName];
			} else {
				this.ID = LabelCounter++;
				idNumbers[labelName] = this.ID;
			}

			this.TextRepresentation = labelName;
			this.CreationTime = DateTime.Now;
			
			numberOfLabels = new Dictionary<Label, int>();
			//We are looking up against id number
			if (numberOfLabels.ContainsKey(this)) {
				//this needs to be tested:
				numberOfLabels[this]++;
			} else {
				numberOfLabels[this] = 1;
			}
		}

		// override object.Equals
		public override bool Equals(object obj) {
			//       
			// See the full list of guidelines at
			//   http://go.microsoft.com/fwlink/?LinkID=85237  
			// and also the guidance for operator== at
			//   http://go.microsoft.com/fwlink/?LinkId=85238
			//

			if (obj == null || GetType() != obj.GetType()) {
				return false;
			}

            if (((Label)obj).TextRepresentation == this.TextRepresentation) {
				return true;
			} else return false;
		}

		// override object.GetHashCode
		public override int GetHashCode() {
			return this.TextRepresentation.GetHashCode();
		}
	}
}
