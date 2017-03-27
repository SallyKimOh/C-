using System;
using System.Collections.Generic;
using System.Web.Http;

namespace OAuthApi.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }



        /************************************************************************************
          *     Token value parsing for getting user info
          *     param: bearer {token}
          * 
          *     return userName == email.
          * 
          * *********************************************************************************/
        // GET api/values
        [Route("GetValue")]
        public string GetValue()
        {
            var email = this.RequestContext.Principal.Identity.Name;
            //           GetMasterpieceList(1); 
            return String.Format("{0}", email);
        }


        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
