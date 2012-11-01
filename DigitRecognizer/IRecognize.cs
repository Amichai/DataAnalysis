using DataAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitRecognizer {
    public interface IRecognize<T> {
        void SetContext(T context);
        T Context { get; set; }
        bool ContextSet { get; set; }
        
        void SetLabel(Label l);
        Label Label { get; set; }
        bool LabelSet { get; set; }

        int TrainCount { get; set; } 
        TestResult Test();
        event EventHandler TestComplete;

        TestResult LastResult { get; set; }
        bool ResultSet { get; set; }

        TimeSpan TimeToCompleteLastTest { get; set; }
        //Guid guid;

        void Train();

        IndicationStrength IndicationStrength { get; set; }
        event EventHandler TrainComplete;

        List<IRecognize<T>> InternalModels { get; set; }

        Dictionary<Label, IndicationStrength> IndicationStrengthPerLabel { get; set; }

        void ResetModels(Dictionary<string, double> parameters);

        void PurgeBadModels(Dictionary<string, double> parameters);
    }
}
