using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace DataAnalysis {
	class Program {
		[STAThread]
		static void Main(string[] args) {
			Window window = new Window();
			ui = new UI();

			ui.LoadNext += new EventHandler(ui_LoadNext);
			loader = new InputLoader();
			loader.LoadFile("digits.csv");
			
			window.Content = ui;
			window.ShowDialog();
		}
		static UI ui;
		static int loadIdx = 0;
		static InputLoader loader;
		static void ui_LoadNext(object sender, EventArgs e) {

			var a = loader.AccessElement(loadIdx + 1);
			ui.Add(a);
			loadIdx++;
		}
	}
}
