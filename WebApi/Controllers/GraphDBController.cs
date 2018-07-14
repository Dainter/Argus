using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GraphDB.Core;
using WebApi.Models;
using WebApi.Models.JSON;

namespace WebApi.Controllers
{
    public class GraphDBController : ApiController
    {
        // GET api/<controller>
        public IHttpActionResult Get()
        {
            Graph graph = GraphDatabase.GetDatabase().GetGraph(Properties.Settings.Default.WorkflowDBName);
            return Json(new GraphJson(graph));
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}