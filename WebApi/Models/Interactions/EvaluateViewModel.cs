using Argus.Backend.Model.Nodes.Interactions;

namespace WebApi.Models.Interactions
{
    /// <summary/>
    public class EvaluateViewModel : AbstractInteractionViewModel
    {
        /// <summary/>
        public string SolutionComments { get; set; }

        /// <summary/>
        public EvaluateViewModel(EvaluateInteraction evaluate)
            : base(evaluate.CreateTime, evaluate.CurrentStep, evaluate.Handler)
        {
            SolutionComments = evaluate.SolutionComments;
        }
    }
}