using Argus.Backend.Model.Nodes.Interactions;

namespace WebApi.Models.Interactions
{
    /// <summary/>
    public class RegressionViewModel : AbstractInteractionViewModel
    {
        /// <summary/>
        public string TestReport { get; }

        /// <summary/>
        public RegressionViewModel(RegressionInteraction regression)
            : base(regression.CreateTime, regression.CurrentStep, regression.Handler)
        {
            TestReport = regression.TestReport;
        }
    }
}