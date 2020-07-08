using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Models;
using MongoDB.Driver.Linq;
using Services;

namespace LabSlotBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabController : ControllerBase
    {
        private ILabServices _service;

        public LabController(ILabServices services)
        {
            _service = services;
        }

        // GET: api/Lab
        [HttpGet(nameof(GetLabList))]
        public IActionResult GetLabList()
        {
            return Ok(_service.GetLabList());
        }

        
        [HttpPut(nameof(ApproveLabSlot))]
        public string ApproveLabSlot(int Bookingid, bool Approved)
        {
            return _service.ApproveLabSlot(Bookingid, Approved);
        }

        [HttpGet(nameof(ApprovalSlots))]
        public List<ApprovalList> ApprovalSlots()
        {
            return _service.ApprovalSlots();
        }

        // POST: api/Lab
        [HttpPost(nameof(InsertLabSLot))]
        public string InsertLabSLot([FromBody] LabModel labModel)
        {
            return _service.InsertLabSLot(labModel);
        }

        [HttpGet(nameof(GetLabSlots))]
        public bool GetLabSlots([FromQuery] LabModel labModel)
        {
            return _service.GetLabSlots(labModel);
        }

        //// PUT: api/Lab/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
