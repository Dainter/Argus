using System.Collections.Generic;
using Argus.Backend.Model.Edges;
using Argus.Backend.Model.Nodes;
using GraphDB.Core;

namespace Argus.Backend.Business
{
    public static class ProcedureConstructor
    {
        public static void ConstructProcedure(Graph graph,Procedure procedure, List<ProcedureStep> steps)
        {
            //Add edges between Procedure and ProcedureStep
            foreach (var curItem in steps)
            {
                Edge newEdge = new Include();
                graph.AddEdge(procedure, curItem, newEdge);
                newEdge = new BelongTo();
                graph.AddEdge(curItem, procedure, newEdge);
            }

            //Add edges between ProcedureSteps
            ProcedureStep lastStep = null;
            foreach (var curItem in steps)
            {
                if (lastStep == null)
                {
                    lastStep = curItem;
                    continue;
                }
                Edge newEdge = new Next();
                graph.AddEdge(lastStep, curItem, newEdge);
                newEdge = new Previous();
                graph.AddEdge(curItem, lastStep, newEdge);
                lastStep = curItem;
            }
        }
    }
}