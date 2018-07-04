using System.Collections.Generic;
using Argus.Backend.Model.Edges;
using Argus.Backend.Model.Nodes;
using Argus.Backend.Model.Nodes.Interactions;
using GraphDB.Core;

namespace Argus.Backend.Business
{
    public class TaskBuilder
    {
        public static AbstractInteraction GetInteraction( ProcedureStepEunm step, string user )
        {
            switch (step)
            {
                case ProcedureStepEunm.TicketCheck:
                    return new TicketCheckInteraction( user );
            }

            return null;
        }

        public static void BuildTask(Graph graph, Task newTask, string creator, string handler, string currentStep)
        {
            Edge newEdge;
            graph.AddNode(newTask);
            User cse = graph.GetNodeByName(creator) as User;

            newEdge = new Create();
            graph.AddEdge(cse, newTask, newEdge);
            newEdge = new CreateBy();
            graph.AddEdge(newTask, cse, newEdge);

            User inter = graph.GetNodeByName(handler) as User;
            newEdge = new Assigned();
            graph.AddEdge(inter, newTask, newEdge);
            newEdge = new AssignTo();
            graph.AddEdge(newTask, inter, newEdge);

            ProcedureStep curStep = graph.GetNodeByName(currentStep) as ProcedureStep;
            newEdge = new CurrentStep();
            graph.AddEdge(newTask, curStep, newEdge);

        }
    }
}