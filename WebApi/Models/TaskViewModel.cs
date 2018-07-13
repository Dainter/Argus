using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Argus.Backend.Model.Nodes;
using Argus.Backend.Model.Nodes.Interactions;
using WebApi.Models.Interactions;

namespace WebApi.Models
{
    /// <summary/>
    public class TaskViewModel
    {
        private List<AbstractInteractionViewModel> myInteractions;

        /// <summary/>
        public string ID { get; }
        /// <summary/>
        public string Title { get; }
        /// <summary/>
        public string Description { get; }
        /// <summary/>
        public int Priority { get; }
        /// <summary/>
        public string CreateOn { get; }
        /// <summary/>
        public string CreateBy { get; }
        /// <summary/>
        public string AssignTo { get; }
        /// <summary/>
        public string DeviceId { get; }
        /// <summary/>
        public string Version { get; }
        /// <summary/>
        public string StartTime { get; }
        /// <summary/>
        public string EndTime { get; }
        /// <summary/>
        public IEnumerable<AbstractInteractionViewModel> Interactions => myInteractions;

        /// <summary/>
        public TaskViewModel(Task curTask)
        {
            ID = curTask.ID;
            Title = curTask.Title;
            Description = curTask.Description;
            Priority = curTask.Priority;
            Version = curTask.Version;
            CreateOn = curTask.CreateOn;
            CreateBy = curTask.Submitter.Name;
            AssignTo = curTask.Handler.Name;
            DeviceId = curTask.DeviceId;
            StartTime = curTask.StartTime;
            EndTime = curTask.EndTime;
            myInteractions = new List<AbstractInteractionViewModel>();
            foreach (var curItem in curTask.Interactions)
            {
                AbstractInteractionViewModel interactionViewModel;
                switch (curItem.GetType().Name)
                {
                    case "TicketCheckInteraction":
                        interactionViewModel = new TicketCheckViewModel( curItem as TicketCheckInteraction);
                        break;
                    case "PreAnalysisInteraction":
                        interactionViewModel = new PreAnalysisViewModel(curItem as PreAnalysisInteraction);
                        break;
                    case "SolveInteraction":
                        interactionViewModel = new SolveViewModel(curItem as SolveInteraction);
                        break;
                    case "EvaluateInteraction":
                        interactionViewModel = new EvaluateViewModel(curItem as EvaluateInteraction);
                        break;
                    case "RegressionInteraction":
                        interactionViewModel = new RegressionViewModel(curItem as RegressionInteraction);
                        break;
                    case "FeedbackInteraction":
                        interactionViewModel = new FeedbackViewModel(curItem as FeedbackInteraction);
                        break;
                    default:
                        continue;
                }
                myInteractions.Add( interactionViewModel );
            }
        }

    }
}