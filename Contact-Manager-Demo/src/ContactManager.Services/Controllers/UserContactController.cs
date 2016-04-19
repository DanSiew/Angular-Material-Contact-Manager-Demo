using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ContactManager.Services.Infrastructure.Repositories;
using ContactManager.Services.Models;
using ContactManager.Services.ViewModel;
using AutoMapper;
using Microsoft.AspNet.Cors;
using System.Net;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactManager.Services.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Route("api/[controller]")]
    public class UserContactController : Controller
    {
        IUserContactRepository UserContactRepository;
        public UserContactController(IUserContactRepository userContactRepository)
        {
            UserContactRepository = userContactRepository;
        }

        // GET: api/values
        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                var userContacts = UserContactRepository.AllIncluding(u => u.ContactNotes).Where(u => u.Active);
                IEnumerable<UserContactModel> userContactsVM = Mapper.Map<IEnumerable<UserContact>, IEnumerable<UserContactModel>>(userContacts);
                
                return Json(userContactsVM);
            }
            catch (BadRequestException bex)
            {
                var message = string.Format("Please try again due to input. {0}", bex.Message);
                return Json(message);
            }
            catch (Exception ex)
            {
                var message = string.Format("Please try again due to error. {0}", ex.Message);
                Exception err = new Exception(message);
                return Json(err);
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public JsonResult Post([FromBody]UserContactModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserContact newContact = Mapper.Map<UserContact>(vm);
                    //_logger.LogInformation("Attempting to save into database", newNote);
                    //Save to the database
                    newContact.Active = true;
                    UserContactRepository.Add(newContact);
                    UserContactRepository.Commit();
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(Mapper.Map<UserContactModel>(newContact));
                }
            }
            catch (Exception ex)
            {
                // _logger.LogError("Failed to save to the database", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = "Failed to Added", ErrorMessage = ex.ToString(), StackTrace = ex.StackTrace, ModelState = ModelState });

            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserContact userContact = UserContactRepository.GetSingle(n => n.Id == id);
                    //_logger.LogInformation("Attempting to save into database", newNote);
                    //Save to the database
                    userContact.Active = false;
                    UserContactRepository.Commit();
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Ok(userContact);
                }
            }
            catch (Exception ex)
            {
                // _logger.LogError("Failed to save to the database", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = "Failed to Added", ErrorMessage = ex.ToString(), StackTrace = ex.StackTrace, ModelState = ModelState });

            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }
    }
}
