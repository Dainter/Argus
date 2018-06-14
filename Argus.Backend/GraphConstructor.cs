using System.Collections.Generic;
using Argus.Backend.Business;
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
            BuildCSEGroup();
            //BuildUserGroup();
            //BuildProcedure();

            myWorkflowGraph.SaveDataBase();

        }

        private void BuildProcedure()
        {
            List<ProcedureStep> procedureSteps = new List<ProcedureStep>();

            Procedure procedure = new Procedure("Maintainance", "Maintainance procedure of the Siemens CT.");
            myWorkflowGraph.AddNode(procedure);

            procedureSteps.Add(new ProcedureStep("Submit", "The CSE submit a maintain ticket."));
            procedureSteps.Add(new ProcedureStep("Ticket Check", "The Interface check the ticket."));
            procedureSteps.Add(new ProcedureStep("Pre-Analysis", "The SE pre-analysis the issue and assign to RP."));
            procedureSteps.Add(new ProcedureStep("Solve", "The RP solve the issue."));
            procedureSteps.Add(new ProcedureStep("Evaluate", "The SE evaluate the solution."));
            procedureSteps.Add(new ProcedureStep("Regression", "The test center regression the soultion."));
            procedureSteps.Add(new ProcedureStep("Update", "The CSE provide a patch to fix the issue."));

            foreach (var curItem in procedureSteps)
            {
                myWorkflowGraph.AddNode(curItem);
            }

            ProcedureConstructor.ConstructProcedure(myWorkflowGraph, procedure, procedureSteps);
        }

        private void BuildUserGroup()
        {
            List<UserGroup> userGroups = new List<UserGroup>();
            userGroups.Add(BuildCSEGroup());


        }

        private UserGroup BuildCSEGroup()
        {
            List<User> users = new List<User>();
            
            User cseLeader = new User("Alice ", "CSE Department");
            myWorkflowGraph.AddNode(cseLeader);

            users.Add(new User("Bob ", "CSE Department"));
            users.Add(new User("Clare ", "CSE Department"));

            foreach (var curItem in users)
            {
                myWorkflowGraph.AddNode(curItem);
            }

            UserGroup cseGroup = new UserGroup("CSE", "Executive", "CSE Team" );
            myWorkflowGraph.AddNode(cseGroup);

            UserGroupConstructor.ConstructUserGroup(myWorkflowGraph, cseGroup, cseLeader, users);
            return cseGroup;
        }
    }
}
