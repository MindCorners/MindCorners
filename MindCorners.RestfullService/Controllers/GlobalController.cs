using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MindCorners.RestfullService.Models;
using MindCorners.RestfullService.Results;

namespace MindCorners.RestfullService.Controllers
{
    
    public class GlobalController : ApiController
    {
        // GET: Global
        public string Index()
        {
            return "test";
        }

        [System.Web.Mvc.Route("text_ascii")]
        public OkTextPlainResult GetTextAscii()
        {
            return this.Text("Hello, world!", Encoding.ASCII);
        }


        [System.Web.Http.HttpGet]
        public HttpResponseMessage GetStudent()
        {
            //HttpResponseMessage response = new HttpResponseMessage();  
            //response.Content = new StringContent("This is HttpResponse's Content");  
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "test");
            return response;
        }
    }
}