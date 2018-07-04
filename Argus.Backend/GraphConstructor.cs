using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using Argus.Backend.Business;
using Argus.Backend.Model.Edges;
using Argus.Backend.Model.Nodes;
using Argus.Backend.Model.Nodes.Interactions;
using Argus.Backend.Utility;
using GraphDB.Core;

namespace Argus.Backend
{
    public class GraphConstructor
    {
        private readonly Graph myWorkflowGraph;
        private Role admin, leader, employee;

        public Graph Database => myWorkflowGraph;

        public GraphConstructor(string dbName, string assemblyPath)
        {
            myWorkflowGraph = new Graph("Workflow", dbName, assemblyPath);
        }

        public void CreateGraph()
        {
            //BuildRole();
            //BuildProcedure();
            //BuildUserGroup();
            //BindingProcedureAndUserGroup();
            BuildTask();
            myWorkflowGraph.SaveDataBase();

        }

        private void BuildTask()
        {
            //FaultInfo faultInfo = new FaultInfo( "111210", "VC50", DateTime.Now, DateTime.Now );
            //Task newTask = new Task("100101", "Moodlight can not work when first start it", "Moodlight can not work when first start it", faultInfo, 3);

            //AbstractInteraction newInteraction = TaskBuilder.GetInteraction(ProcedureStepEunm.TicketCheck, "Dave");
            //newTask.AddInteraction(newInteraction);
            //TaskBuilder.BuildTask(myWorkflowGraph, newTask, "Bob", "Dave", ProcedureStepEunm.TicketCheck.ToString());

            FaultInfo faultInfo = new FaultInfo("131456", "VC40", DateTime.Now, DateTime.Now);
            Task newTask = new Task("100102", "Moodlight can not work when first start it", "Moodlight can not work when first start it", faultInfo, 3);

            AbstractInteraction newInteraction = TaskBuilder.GetInteraction(ProcedureStepEunm.TicketCheck, "Dave");
            newTask.AddInteraction(newInteraction);
            newInteraction = TaskBuilder.GetInteraction(ProcedureStepEunm.PreAnalysis, "Haden");
            newTask.AddInteraction(newInteraction);
            newInteraction = TaskBuilder.GetInteraction(ProcedureStepEunm.Solve, "Isaac");
            newTask.AddInteraction(newInteraction);
            TaskBuilder.BuildTask(myWorkflowGraph, newTask, "Clare", "Isaac", ProcedureStepEunm.Solve.ToString());

            return;
        }

        private void BuildRole()
        {
            admin = new Role(RoleEnum.Administrator.ToString(), 0, "Administrator privilege.");
            myWorkflowGraph.AddNode(admin);
            leader = new Role(RoleEnum.Leader.ToString(), 10, "Leader privilege.");
            myWorkflowGraph.AddNode(leader);
            employee = new Role(RoleEnum.Empolyee.ToString(), 100, "Employee privilege.");
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

            ProcedureStep submit = new ProcedureStep( ProcedureStepEunm.CreateStep.ToString(), "The CSE submit a maintain ticket.");
            ProcedureStep ticketCheck = new ProcedureStep( ProcedureStepEunm.TicketCheck.ToString(), "The Interface check the ticket.");
            ProcedureStep preAnalysis = new ProcedureStep( ProcedureStepEunm.PreAnalysis.ToString(), "The SE pre-analysis the issue and assign to RP.");
            ProcedureStep solve = new ProcedureStep( ProcedureStepEunm.Solve.ToString(), "The RP solve the issue.");
            ProcedureStep evaluate = new ProcedureStep( ProcedureStepEunm.Evaluate.ToString(), "The SE evaluate the solution.");
            ProcedureStep regression = new ProcedureStep( ProcedureStepEunm.Regression.ToString(), "The test center regression the soultion.");
            ProcedureStep feedback = new ProcedureStep( ProcedureStepEunm.Feedback.ToString(), "The CSE give a Feedback to the customer.");

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

            User gmLeader = new User("Sanders", "Siemens Healthineer");
            myWorkflowGraph.AddNode(gmLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(gmLeader, leader, newEdge);

            UserGroup group = new UserGroup("Healthineer", GroupTypeEnum.Executive.ToString(), "Siemens Healthineer");
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

            Node ticketCheck = myWorkflowGraph.GetNodeByName(ProcedureStepEunm.TicketCheck.ToString());
            Node interGroup = myWorkflowGraph.GetNodeByName("Interface");
            Edge newEdge = new HandleBy();
            myWorkflowGraph.AddEdge(ticketCheck, interGroup, newEdge);
            newEdge = new Incharge();
            myWorkflowGraph.AddEdge(interGroup, ticketCheck, newEdge);


            Node preAnalysis = myWorkflowGraph.GetNodeByName(ProcedureStepEunm.PreAnalysis.ToString());
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

            Node regression = myWorkflowGraph.GetNodeByName(ProcedureStepEunm.Regression.ToString());
            Node testGroup = myWorkflowGraph.GetNodeByName("Test Center");
            newEdge = new HandleBy();
            myWorkflowGraph.AddEdge(regression, testGroup, newEdge);
            newEdge = new Incharge();
            myWorkflowGraph.AddEdge(testGroup, regression, newEdge);

            Node feedback = myWorkflowGraph.GetNodeByName(ProcedureStepEunm.Feedback.ToString());
            Node cseGroup = myWorkflowGraph.GetNodeByName("CSE");
            newEdge = new HandleBy();
            myWorkflowGraph.AddEdge(feedback, cseGroup, newEdge);
            newEdge = new Incharge();
            myWorkflowGraph.AddEdge(cseGroup, feedback, newEdge);
        }

        private UserGroup BuildCSEGroup()
        {
            List<User> users = new List<User>();
            
            User cseLeader = new User("Alice", "CSE Department");
            myWorkflowGraph.AddNode(cseLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(cseLeader, leader, newEdge);

            users.Add(new User("Bob", "CSE Department"));
            users.Add(new User("Clare", "CSE Department"));

            foreach (var curItem in users)
            {
                myWorkflowGraph.AddNode(curItem);
                newEdge = new As();
                myWorkflowGraph.AddEdge(curItem, employee, newEdge);
            }

            UserGroup cseGroup = new UserGroup("CSE", GroupTypeEnum.Executive.ToString(), "CSE Team" );
            myWorkflowGraph.AddNode(cseGroup);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, cseGroup, cseLeader, users);
            return cseGroup;
        }

        private UserGroup BuildInterfaceGroup()
        {
            List<User> users = new List<User>();

            User interLeader = new User("Dave", "Interface");
            myWorkflowGraph.AddNode(interLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(interLeader, leader, newEdge);

            UserGroup interGroup = new UserGroup("Interface", GroupTypeEnum.Executive.ToString(), "Interface Team");
            myWorkflowGraph.AddNode(interGroup);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, interGroup, interLeader, users);
            return interGroup;
        }

        private UserGroup BuildRDGroup()
        {
            List<UserGroup> userGroups = new List<UserGroup> {BuildUIGroup(), BuildExamGroup(), BuildImageGroup()};

            User rdLeader = new User("Rafael", "RD Department");
            myWorkflowGraph.AddNode(rdLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(rdLeader, leader, newEdge);

            UserGroup group = new UserGroup("RD", GroupTypeEnum.Executive.ToString(), "RD Department");
            myWorkflowGraph.AddNode(group);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, group, rdLeader, new List<User>(), userGroups);
            return group;
        }

        private UserGroup BuildUIGroup()
        {
            List<User> users = new List<User>();

            User uiLeader = new User("Emilia", "UI Team");
            myWorkflowGraph.AddNode(uiLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(uiLeader, leader, newEdge);

            users.Add(new User("Florence", "UI Team"));
            users.Add(new User("Grant", "UI Team"));

            foreach (var curItem in users)
            {
                myWorkflowGraph.AddNode(curItem);
                newEdge = new As();
                myWorkflowGraph.AddEdge(curItem, employee, newEdge);
            }

            UserGroup group = new UserGroup("UI", GroupTypeEnum.Executive.ToString(), "UI Team");
            myWorkflowGraph.AddNode(group);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, group, uiLeader, users);
            return group;
        }

        private UserGroup BuildExamGroup()
        {
            List<User> users = new List<User>();

            User examLeader = new User("Haden", "Exam Team");
            myWorkflowGraph.AddNode(examLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(examLeader, leader, newEdge);

            users.Add(new User("Isaac", "Exam Team"));
            users.Add(new User("Jackson", "Exam Team"));

            foreach (var curItem in users)
            {
                myWorkflowGraph.AddNode(curItem);
                newEdge = new As();
                myWorkflowGraph.AddEdge(curItem, employee, newEdge);
            }

            UserGroup group = new UserGroup("Exam", GroupTypeEnum.Executive.ToString(), "Exam Team");
            myWorkflowGraph.AddNode(group);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, group, examLeader, users);
            return group;
        }

        private UserGroup BuildImageGroup()
        {
            List<User> users = new List<User>();

            User imageLeader = new User("Kimi", "Imaging Team");
            myWorkflowGraph.AddNode(imageLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(imageLeader, leader, newEdge);

            users.Add(new User("Lacy", "Imaging Team"));
            users.Add(new User("Michael", "Imaging Team"));

            foreach (var curItem in users)
            {
                myWorkflowGraph.AddNode(curItem);
                newEdge = new As();
                myWorkflowGraph.AddEdge(curItem, employee, newEdge);
            }

            UserGroup group = new UserGroup("Imaging", GroupTypeEnum.Executive.ToString(), "Imaging Team");
            myWorkflowGraph.AddNode(group);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, group, imageLeader, users);
            return group;
        }

        private UserGroup BuildTestGroup()
        {
            List<User> users = new List<User>();

            User testLeader = new User("Nina", "Test Center");
            myWorkflowGraph.AddNode(testLeader);
            Edge newEdge = new As();
            myWorkflowGraph.AddEdge(testLeader, leader, newEdge);

            users.Add(new User("Oakley", "Test Center"));
            users.Add(new User("Pol", "Test Center"));
            users.Add(new User("Queen", "Test Center"));

            foreach (var curItem in users)
            {
                myWorkflowGraph.AddNode(curItem);
                newEdge = new As();
                myWorkflowGraph.AddEdge(curItem, employee, newEdge);
            }

            UserGroup group = new UserGroup("Test Center", GroupTypeEnum.Executive.ToString(), "Test Center");
            myWorkflowGraph.AddNode(group);

            UserGroupBuilder.BuildUserGroup(myWorkflowGraph, group, testLeader, users);
            return group;
        }
    }
}
