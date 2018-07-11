using Argus.Backend.Model.Nodes.Interactions;

namespace WebApi.Models.Interactions
{
    /// <summary/>
    public class TicketCheckViewModel : AbstractInteractionViewModel
    {
        /// <summary/>
        public string CheckResult { get; }
        /// <summary/>
        public string CheckSuggestion { get; }

        /// <summary/>
        public TicketCheckViewModel(TicketCheckInteraction ticketCheck) : base(ticketCheck.CreateTime, ticketCheck.CurrentStep, ticketCheck.Handler)
        {
            CheckResult = ticketCheck.AnalysisResult;
            CheckSuggestion = ticketCheck.Suggestion;
        }
    }
}