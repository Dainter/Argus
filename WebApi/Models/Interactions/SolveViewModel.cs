using Argus.Backend.Model.Nodes.Interactions;

namespace WebApi.Models.Interactions
{
    /// <summary/>
    public class SolveViewModel : AbstractInteractionViewModel
    {
        /// <summary/>
        public string RootCause { get; }
        /// <summary/>
        public string Implementation { get; }
        /// <summary/>
        public string TestSuggestion { get; }

        /// <summary/>
        public SolveViewModel( SolveInteraction solve )
            : base(solve.CreateTime, solve.CurrentStep, solve.Handler)
        {
            RootCause = solve.RootCause;
            Implementation = solve.Implementation;
            TestSuggestion = solve.TestSuggestion;
        }
    }
}