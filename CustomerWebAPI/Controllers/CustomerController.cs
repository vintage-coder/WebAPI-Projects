using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerWebAPI.Models;
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData;



namespace CustomerWebAPI.Controllers
{
    public class CustomerController : ApiController
    {

        [EnableQuery]
        [HttpGet]
        public HttpResponseMessage LoadAllEmployee()
        {
            using (CustomerDBEntities dbContext=new CustomerDBEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, dbContext.CustomerTbls.ToList());
                
            }
        }

        [HttpGet]
        public HttpResponseMessage LoadCustomerById(int id)
        {
            using (CustomerDBEntities dbContext = new CustomerDBEntities())
            {
                var Entity = dbContext.CustomerTbls.FirstOrDefault(e => e.CustId == id);

                if (Entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Entity);
                }
                else
                {
                    var message = Request.CreateErrorResponse(HttpStatusCode.NotFound, " Customer with id= " + id.ToString() + " Not found");
                    message.Headers.Location = new Uri(Request.RequestUri.ToString());

                    return message;
                }
            }
        }

        public HttpResponseMessage Put(int id, [FromBody] CustomerTbl custTbl)
        {

            try
            {

                using (CustomerDBEntities dbContext = new CustomerDBEntities())
                {
                    var entity = dbContext.CustomerTbls.FirstOrDefault(e => e.CustId == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Customer with ID= " + id.ToString() + " Not found to update..");
                    }
                    else
                    {
                        
                        entity.FirstName = custTbl.FirstName;
                        entity.LastName = custTbl.LastName;
                        entity.City = custTbl.City;

                        dbContext.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }
        }




        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (CustomerDBEntities dbContext = new CustomerDBEntities())
                {
                    var entity = dbContext.CustomerTbls.FirstOrDefault(e => e.CustId == id);

                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, " Customer with ID= " + id.ToString() + " Not found to delete..");
                    }
                    else
                    {
                        dbContext.CustomerTbls.Remove(entity);
                        dbContext.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        public HttpResponseMessage PostEmployee([FromBody] CustomerTbl custTbl)
        {
            try
            {
                using (CustomerDBEntities dbContext = new CustomerDBEntities())
                {
                    dbContext.CustomerTbls.Add(custTbl);
                    dbContext.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, custTbl);
                    message.Headers.Location = new Uri(Request.RequestUri + "/" + custTbl.ToString());

                    return message;

                }

            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

    }
}
