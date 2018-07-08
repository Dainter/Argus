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
    public class UsersController : ApiController
    {
        // GET api/users
        /// <summary/>
        public IHttpActionResult Get()
        {
            IEnumerable<UserViewModel> users = GraphDatabase.GetDatabase().GetAll<User>().Select(x => new UserViewModel(x));

            return Json(users);
        }

        // GET api/<controller>/5
        /// <summary/>
        public string Get(int id)
        {
            return "value";
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