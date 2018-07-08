using Argus.Backend.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary/>
    public class SubmitTaskInfoController : ApiController
    {
        // GET api/<controller>
        //public IHttpActionResult Get()
        //{
        //    IEnumerable<TaskInfoViewModel> tasks = GraphDatabase.GetDatabase().GetAll<Task>().Select(x => new TaskInfoViewModel(x));

        //    return Json(tasks);
        //}

        // GET api/<controller>/name
        /// <summary/>
        public IHttpActionResult Get(string name)
        {
            var curUser = GraphDatabase.GetDatabase().GetAll<User>().Where(x => x.Name == name);
            if( curUser .Count() <= 0 )
            {
                return Json( new List<TaskInfoViewModel>());
            }
            IEnumerable<TaskInfoViewModel> tasks = curUser.First().SubmitTasks.Select(x => new TaskInfoViewModel(x));

            return Json(tasks);
        }

        // POST api/<controller>
        /// <summary/>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        /// <summary/>
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        /// <summary/>
        public void Delete(int id)
        {
        }
    }
}