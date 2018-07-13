using Argus.Backend.Model.Nodes.Interactions;

namespace WebApi.Models.Interactions
{
    /// <summary/>
    public class FeedbackViewModel : AbstractInteractionViewModel
    {
        /// <summary/>
        public string FixPlan { get; }
        /// <summary/>
        public string Feedback { get; }

        /// <summary/>
        public FeedbackViewModel(FeedbackInteraction feedback) 
            : base(feedback.CreateTime, feedback.CurrentStep, feedback.Handler)
        {
            FixPlan = feedback.FixPlan;
            Feedback = feedback.Feedback;
        }
    }
}