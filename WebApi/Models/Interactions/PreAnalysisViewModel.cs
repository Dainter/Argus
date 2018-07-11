using Argus.Backend.Model.Nodes.Interactions;

namespace WebApi.Models.Interactions
{
    /// <summary/>
    public class PreAnalysisViewModel : AbstractInteractionViewModel
    {
        /// <summary/>
        public string AnalysisResult { get; }
        /// <summary/>
        public string AnalysisSuggestion { get; }

        /// <summary/>
        public PreAnalysisViewModel( PreAnalysisInteraction preAnalysis ) 
            : base(preAnalysis.CreateTime, preAnalysis.CurrentStep, preAnalysis.Handler)
        {
            AnalysisResult = preAnalysis.AnalysisResult;
            AnalysisSuggestion = preAnalysis.Suggestion;
        }
    }
}