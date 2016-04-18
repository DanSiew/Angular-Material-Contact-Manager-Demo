using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Cors;
using ContactManager.Services.ViewModel;
using System.Net;
using AutoMapper;
using ContactManager.Services.Models;
using ContactManager.Services.Infrastructure.Repositories;
using ContactManager.Services.Infrastructure;
using Microsoft.Data.Entity;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactManager.Services.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Route("api/[controller]")]
    public class ContactNoteController : Controller
    {
        IContactNoteRepository ContactNoteRepository;
        public ContactNoteController(IContactNoteRepository contactNoteRepository)
        {
            ContactNoteRepository = contactNoteRepository;

        }

        // GET: api/values
        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                var contactNotes = ContactNoteRepository.GetAll();
                IEnumerable<ContactNoteModel> contactNotesVM = Mapper.Map<IEnumerable<ContactNote>, 
                    IEnumerable<ContactNoteModel>>(contactNotes);

                return Json(contactNotesVM);
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
        public JsonResult Get(int id)
        {
            try
            {
                ContactNote contactNote = ContactNoteRepository.GetSingle(n => n.Id == id);
                ContactNoteModel contactNotesVM = Mapper.Map<ContactNote, ContactNoteModel>(contactNote);

                return Json(contactNotesVM);
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

        // POST api/values
        [HttpPost]
        public JsonResult Post([FromBody] ContactNoteModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactNote newNote = Mapper.Map<ContactNote>(vm);
                    //_logger.LogInformation("Attempting to save into database", newNote);
                    //Save to the database
                    ContactNoteRepository.Add(newNote);
                    ContactNoteRepository.Commit();
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(Mapper.Map<ContactNoteModel>(newNote));
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
                    ContactNote note = ContactNoteRepository.GetSingle(n => n.Id == id);
                    //_logger.LogInformation("Attempting to save into database", newNote);
                    //Save to the database
                    ContactNoteRepository.Delete(note);
                    ContactNoteRepository.Commit();
                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Ok(note);
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
