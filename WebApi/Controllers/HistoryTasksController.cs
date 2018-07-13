using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Argus.Backend.Model.Nodes;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary/>
    public class HistoryTasksController : ApiController
    {
        // GET api/<controller>
        /// <summary/>
        public IHttpActionResult Get()
        {
            IEnumerable<TaskViewModel> tasks = GraphDatabase.GetDatabase().GetAll<Task>().Select(x => new TaskViewModel(x));
            return Json(tasks);
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