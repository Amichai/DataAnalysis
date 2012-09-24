using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace DataAnalysis {
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class UI : UserControl, INotifyPropertyChanged {
		public UI() {
			InitializeComponent();
			this.root.DataContext = this;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		void OnPropertyChanged(string p) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(p));
			}
		}

		private string _library;
		public string Library {
			get { 
				return _library; 
			}
			set { 
				_library = value;
				OnPropertyChanged("Library");
			}
		}
		

		public void Add(HeurisiticArray<int[][]> heuristicArray) {
			Library += heuristicArray.ToString() + "\n";
		}

		public event EventHandler LoadNext;

		private void BtnNext_Click(object sender, RoutedEventArgs e) {
			LoadNext(this, e);
		}
	}
}
