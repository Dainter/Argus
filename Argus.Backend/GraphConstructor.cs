using System.Collections.Generic;
using Argus.Backend.Business;
using Argus.Backend.Model.Edges;
using Argus.Backend.Model.Nodes;
using GraphDB.Core;

namespace Argus.Backend
{
    public class GraphConstructor
    {
        private readonly Graph myWorkflowGraph;
        private Role admin, leader, employee;

        public Graph Database => myWorkflowGraph;

        public GraphConstructor(string dbName)
        {
            //myWorkflowGraph = new Graph("Workflow", dbName);
        }

        public void CreateGraph()
        {
            //BuildRole();
            //BuildProcedure();
            //BuildUserGroup();
            //BindingProcedureAndUserGroup();
            //myWorkflowGraph.SaveDataBase();

        }

        private void BuildRole()
        {
            admin = new Role("Administrator", 0, "Administrator privilege.");
            myWorkflowGraph.AddNode(admin);
            leader = new Role("Leader", 10, "Leader privilege.");
            myWorkflowGraph.AddNode(leader);
            employee = new Role("Employee", 100, "Employee privilege.");
            myWorkflowGraph.AddNode(employee);

            User dainter = new User("Dainter ", "Siemens Ltd. China");
            myWorkflowGraph.AddNode(dainter);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(dainter, admin, newEdge);
        }

        private void BuildProcedure()
        {
            List<ProcedureStep> procedureSteps = new List<ProcedureStep>();

            Procedure procedure = new Procedure("Maintainance", "Maintainance procedure of the Siemens CT.");
            myWorkflowGraph.AddNode(procedure);

            ProcedureStep submit = new ProcedureStep("Submit", "The CSE submit a maintain ticket.");
            ProcedureStep ticketCheck = new ProcedureStep("Ticket Check", "The Interface check the ticket.");
            ProcedureStep preAnalysis = new ProcedureStep("Pre-Analysis", "The SE pre-analysis the issue and assign to RP.");
            ProcedureStep solve = new ProcedureStep("Solve", "The RP solve the issue.");
            ProcedureStep evaluate = new ProcedureStep("Evaluate", "The SE evaluate the solution.");
            ProcedureStep regression = new ProcedureStep("Regression", "The test center regression the soultion.");
            ProcedureStep feedback = new ProcedureStep("Feedback", "The CSE give a Feedback to the customer.");

            procedureSteps.Add(submit);
            procedureSteps.Add(ticketCheck);
            procedureSteps.Add(preAnalysis);
            procedureSteps.Add(solve);
            procedureSteps.Add(evaluate);
            procedureSteps.Add(regression);
            procedureSteps.Add(feedback);

            foreach (var curItem in procedureSteps)
            {
                myWorkflowGraph.AddNode(curItem);
            }

            ProcedureBuilder.BuildProcedure(myWorkflowGraph, procedure, procedureSteps);

            Edge newEdge = new Next("Submit");
            myWorkflowGraph.AddEdge(submit, ticketCheck, newEdge);
            newEdge = new Next("Assign");
            myWorkflowGraph.AddEdge(ticketCheck, preAnalysis, newEdge);
            newEdge = new Next("Assign");
            myWorkflowGraph.AddEdge(preAnalysis, solve, newEdge);
            newEdge = new Next("Submit");
            myWorkflowGraph.AddEdge(solve, evaluate, newEdge);
            newEdge = new Next("Assign");
            myWorkflowGraph.AddEdge(evaluate, regression, newEdge);
            newEdge = new Next("Submit");
            myWorkflowGraph.AddEdge(regression, feedback, newEdge);

            newEdge = new Next("Feedback");
            myWorkflowGraph.AddEdge(ticketCheck, feedback, newEdge);
            newEdge = new Next("Feedback");
            myWorkflowGraph.AddEdge(preAnalysis, feedback, newEdge);
            newEdge = new Next("Feedback");
            myWorkflowGraph.AddEdge(evaluate, feedback, newEdge);

            newEdge = new Next("Rework");
            myWorkflowGraph.AddEdge(evaluate, solve, newEdge);
            newEdge = new Next("Return");
            myWorkflowGraph.AddEdge(regression, evaluate, newEdge);
        }

        private void BuildUserGroup()
        {
            List<UserGroup> userGroups =
                new List<UserGroup> {BuildCSEGroup(), BuildInterfaceGroup(), BuildRDGroup(), BuildTestGroup()};

            User gmLeader = new User("Sanders ", "Siemens Healthineer");
            myWorkflowGraph.AddNode(gmLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(gmLeader, leader, newEdge);

            UserGroup group = new UserGroup("Healthineer", "Executive", "Siemens Healthineer");
            myWorkflowGraph.AddNode(group);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, group, gmLeader, new List<User>(), userGroups);
        }

        private void BindingProcedureAndUserGroup()
        {
            //Node submit = myWorkflowGraph.GetNodeByName("Submit");
            //Node cseGroup = myWorkflowGraph.GetNodeByName("CSE");
            //Edge newEdge = new HandleBy();
            //myWorkflowGraph.AddEdge(submit, cseGroup, newEdge);
            //newEdge = new Incharge();
            //myWorkflowGraph.AddEdge(cseGroup, submit, newEdge);

            Node ticketCheck = myWorkflowGraph.GetNodeByName("Ticket Check");
            Node interGroup = myWorkflowGraph.GetNodeByName("Interface");
            Edge newEdge = new HandleBy();
            myWorkflowGraph.AddEdge(ticketCheck, interGroup, newEdge);
            newEdge = new Incharge();
            myWorkflowGraph.AddEdge(interGroup, ticketCheck, newEdge);


            Node preAnalysis = myWorkflowGraph.GetNodeByName("Pre-Analysis");
            Node uiGroup = myWorkflowGraph.GetNodeByName("UI");
            newEdge = new HandleBy();
            myWorkflowGraph.AddEdge(preAnalysis, uiGroup, newEdge);
            newEdge = new Incharge();
            myWorkflowGraph.AddEdge(uiGroup, preAnalysis, newEdge);

            Node examGroup = myWorkflowGraph.GetNodeByName("Exam");
            newEdge = new HandleBy();
            myWorkflowGraph.AddEdge(preAnalysis, examGroup, newEdge);
            newEdge = new Incharge();
            myWorkflowGraph.AddEdge(examGroup, preAnalysis, newEdge);

            Node imageGroup = myWorkflowGraph.GetNodeByName("Imaging");
            newEdge = new HandleBy();
            myWorkflowGraph.AddEdge(preAnalysis, imageGroup, newEdge);
            newEdge = new Incharge();
            myWorkflowGraph.AddEdge(imageGroup, preAnalysis, newEdge);

            Node regression = myWorkflowGraph.GetNodeByName("Regression");
            Node testGroup = myWorkflowGraph.GetNodeByName("Test Center");
            newEdge = new HandleBy();
            myWorkflowGraph.AddEdge(regression, testGroup, newEdge);
            newEdge = new Incharge();
            myWorkflowGraph.AddEdge(testGroup, regression, newEdge);

            Node feedback = myWorkflowGraph.GetNodeByName("Feedback");
            Node cseGroup = myWorkflowGraph.GetNodeByName("CSE");
            newEdge = new HandleBy();
            myWorkflowGraph.AddEdge(feedback, cseGroup, newEdge);
            newEdge = new Incharge();
            myWorkflowGraph.AddEdge(cseGroup, feedback, newEdge);
        }

        private UserGroup BuildCSEGroup()
        {
            List<User> users = new List<User>();
            
            User cseLeader = new User("Alice ", "CSE Department");
            myWorkflowGraph.AddNode(cseLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(cseLeader, leader, newEdge);

            users.Add(new User("Bob ", "CSE Department"));
            users.Add(new User("Clare ", "CSE Department"));

            foreach (var curItem in users)
            {
                myWorkflowGraph.AddNode(curItem);
                newEdge = new As();
                myWorkflowGraph.AddEdge(curItem, employee, newEdge);
            }

            UserGroup cseGroup = new UserGroup("CSE", "Executive", "CSE Team" );
            myWorkflowGraph.AddNode(cseGroup);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, cseGroup, cseLeader, users);
            return cseGroup;
        }

        private UserGroup BuildInterfaceGroup()
        {
            List<User> users = new List<User>();

            User interLeader = new User("Dave ", "Interface");
            myWorkflowGraph.AddNode(interLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(interLeader, leader, newEdge);

            UserGroup interGroup = new UserGroup("Interface", "Executive", "Interface Team");
            myWorkflowGraph.AddNode(interGroup);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, interGroup, interLeader, users);
            return interGroup;
        }

        private UserGroup BuildRDGroup()
        {
            List<UserGroup> userGroups = new List<UserGroup> {BuildUIGroup(), BuildExamGroup(), BuildImageGroup()};

            User rdLeader = new User("Rafael ", "RD Department");
            myWorkflowGraph.AddNode(rdLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(rdLeader, leader, newEdge);

            UserGroup group = new UserGroup("RD", "Executive", "RD Department");
            myWorkflowGraph.AddNode(group);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, group, rdLeader, new List<User>(), userGroups);
            return group;
        }

        private UserGroup BuildUIGroup()
        {
            List<User> users = new List<User>();

            User uiLeader = new User("Emilia ", "UI Team");
            myWorkflowGraph.AddNode(uiLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(uiLeader, leader, newEdge);

            users.Add(new User("Florence ", "UI Team"));
            users.Add(new User("Grant ", "UI Team"));

            foreach (var curItem in users)
            {
                myWorkflowGraph.AddNode(curItem);
                newEdge = new As();
                myWorkflowGraph.AddEdge(curItem, employee, newEdge);
            }

            UserGroup group = new UserGroup("UI", "Executive", "UI Team");
            myWorkflowGraph.AddNode(group);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, group, uiLeader, users);
            return group;
        }

        private UserGroup BuildExamGroup()
        {
            List<User> users = new List<User>();

            User examLeader = new User("Haden ", "Exam Team");
            myWorkflowGraph.AddNode(examLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(examLeader, leader, newEdge);

            users.Add(new User("Isaac ", "Exam Team"));
            users.Add(new User("Jackson ", "Exam Team"));

            foreach (var curItem in users)
            {
                myWorkflowGraph.AddNode(curItem);
                newEdge = new As();
                myWorkflowGraph.AddEdge(curItem, employee, newEdge);
            }

            UserGroup group = new UserGroup("Exam", "Executive", "Exam Team");
            myWorkflowGraph.AddNode(group);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, group, examLeader, users);
            return group;
        }

        private UserGroup BuildImageGroup()
        {
            List<User> users = new List<User>();

            User imageLeader = new User("Kimi ", "Imaging Team");
            myWorkflowGraph.AddNode(imageLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(imageLeader, leader, newEdge);

            users.Add(new User("Lacy ", "Imaging Team"));
            users.Add(new User("Michael ", "Imaging Team"));

            foreach (var curItem in users)
            {
                myWorkflowGraph.AddNode(curItem);
                newEdge = new As();
                myWorkflowGraph.AddEdge(curItem, employee, newEdge);
            }

            UserGroup group = new UserGroup("Imaging", "Executive", "Imaging Team");
            myWorkflowGraph.AddNode(group);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, group, imageLeader, users);
            return group;
        }

        private UserGroup BuildTestGroup()
        {
            List<User> users = new List<User>();

            User testLeader = new User("Nina ", "Test Center");
            myWorkflowGraph.AddNode(testLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(testLeader, leader, newEdge);

            users.Add(new User("Oakley ", "Test Center"));
            users.Add(new User("Pol ", "Test Center"));
            users.Add(new User("Queen ", "Test Center"));

            foreach (var curItem in users)
            {
                myWorkflowGraph.AddNode(curItem);
                newEdge = new As();
                myWorkflowGraph.AddEdge(curItem, employee, newEdge);
            }

            UserGroup group = new UserGroup("Test Center", "Executive", "Test Center");
            myWorkflowGraph.AddNode(group);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, group, testLeader, users);
            return group;
        }
    }
}
