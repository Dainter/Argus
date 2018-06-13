using Argus.Backend.Model.Edges;
using Argus.Backend.Model.Nodes;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend
{
    public class GraphConstructor
    {
        private readonly Graph myWorkflowGraph;

        public Graph Database => myWorkflowGraph;

        public GraphConstructor(string dbName)
        {
            myWorkflowGraph = new Graph("Workflow", dbName);
        }

        public void CreateGraph()
        {

            //BuildProcedure();

            myWorkflowGraph.SaveDataBase();

        }

        private void BuildProcedure()
        {
            Node procedure = new Procedure("Maintainance", "Maintainance procedure of the Siemens CT.");
            myWorkflowGraph.AddNode(procedure);

            Node step1 = new ProcedureStep("Submit", "The CSE submit a maintain ticket.");
            myWorkflowGraph.AddNode(step1);

            Node step2 = new ProcedureStep("Ticket Check", "The Interface check the ticket.");
            myWorkflowGraph.AddNode(step2);

            Node step3 = new ProcedureStep("Pre-Analysis", "The SE pre-analysis the issue and assign to RP.");
            myWorkflowGraph.AddNode(step3);

            Node step4 = new ProcedureStep("Solve", "The RP solve the issue.");
            myWorkflowGraph.AddNode(step4);

            Node step5 = new ProcedureStep("Evaluate", "The SE evaluate the solution.");
            myWorkflowGraph.AddNode(step5);

            Node step6 = new ProcedureStep("Regression", "The test center regression the soultion.");
            myWorkflowGraph.AddNode(step6);

            Node step7 = new ProcedureStep("Update", "The CSE provide a patch to fix the issue.");
            myWorkflowGraph.AddNode(step7);

            Edge newEdge = new Include();
            myWorkflowGraph.AddEdge(procedure, step1, newEdge);
            newEdge = new BelongTo();
            myWorkflowGraph.AddEdge(step1, procedure, newEdge);

            newEdge = new Include();
            myWorkflowGraph.AddEdge(procedure, step2, newEdge);
            newEdge = new BelongTo();
            myWorkflowGraph.AddEdge(step2, procedure, newEdge);

            newEdge = new Include();
            myWorkflowGraph.AddEdge(procedure, step3, newEdge);
            newEdge = new BelongTo();
            myWorkflowGraph.AddEdge(step3, procedure, newEdge);

            newEdge = new Include();
            myWorkflowGraph.AddEdge(procedure, step4, newEdge);
            newEdge = new BelongTo();
            myWorkflowGraph.AddEdge(step4, procedure, newEdge);

            newEdge = new Include();
            myWorkflowGraph.AddEdge(procedure, step5, newEdge);
            newEdge = new BelongTo();
            myWorkflowGraph.AddEdge(step5, procedure, newEdge);

            newEdge = new Include();
            myWorkflowGraph.AddEdge(procedure, step6, newEdge);
            newEdge = new BelongTo();
            myWorkflowGraph.AddEdge(step6, procedure, newEdge);

            newEdge = new Include();
            myWorkflowGraph.AddEdge(procedure, step7, newEdge);
            newEdge = new BelongTo();
            myWorkflowGraph.AddEdge(step7, procedure, newEdge);

            newEdge = new Next();
            myWorkflowGraph.AddEdge(step1, step2, newEdge);
            newEdge = new Previous();
            myWorkflowGraph.AddEdge(step2, step1, newEdge);

            newEdge = new Next();
            myWorkflowGraph.AddEdge(step2, step3, newEdge);
            newEdge = new Previous();
            myWorkflowGraph.AddEdge(step3, step2, newEdge);

            newEdge = new Next();
            myWorkflowGraph.AddEdge(step3, step4, newEdge);
            newEdge = new Previous();
            myWorkflowGraph.AddEdge(step4, step3, newEdge);

            newEdge = new Next();
            myWorkflowGraph.AddEdge(step4, step5, newEdge);
            newEdge = new Previous();
            myWorkflowGraph.AddEdge(step5, step4, newEdge);

            newEdge = new Next();
            myWorkflowGraph.AddEdge(step5, step6, newEdge);
            newEdge = new Previous();
            myWorkflowGraph.AddEdge(step6, step5, newEdge);

            newEdge = new Next();
            myWorkflowGraph.AddEdge(step6, step7, newEdge);
            newEdge = new Previous();
            myWorkflowGraph.AddEdge(step7, step6, newEdge);
        }

        private void BuildUser()
        {
            string info = "Dai, Xiao Gang (CT DD DS AA CN DI NJ)";
            StringHelper.ExtractUserInfo(info, out var name, out var department, out _);
            Node newNode = new User(name, department);

            myWorkflowGraph.AddNode(newNode);
        }
    }
}
