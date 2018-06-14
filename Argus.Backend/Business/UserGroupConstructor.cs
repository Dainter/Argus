using System.Collections.Generic;
using Argus.Backend.Model.Edges;
using Argus.Backend.Model.Nodes;
using GraphDB.Core;

namespace Argus.Backend.Business
{
    public class UserGroupConstructor
    {
        public static void ConstructUserGroup(Graph graph, UserGroup group, User leader, 
            List<User> members, List<UserGroup> subGroups = null)
        {
            //Add edges between Group and Leader
            Edge newEdge = new Lead();
            graph.AddEdge(leader, group, newEdge);
            newEdge = new LeadBy();
            graph.AddEdge(group, leader, newEdge);

            //Add edges between Group and User
            foreach (var curItem in members)
            {
                newEdge = new Include();
                graph.AddEdge(group, curItem, newEdge);
                newEdge = new BelongTo();
                graph.AddEdge(curItem, group, newEdge);
            }

            if (subGroups == null)
            {
                return;
            }

            //Add edges between Group and Sub-Group
            foreach (var curItem in subGroups)
            {
                newEdge = new Include();
                graph.AddEdge(group, curItem, newEdge);
                newEdge = new BelongTo();
                graph.AddEdge(curItem, group, newEdge);
            }


        }
    }
}