using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using TaskMasterDetails.DTO;
using TaskMasterDetails.services;
using TaskMasterDetails.services.contarcts;

namespace TaskMasterDetails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {

        private readonly IMasterService _masterService;

        public MasterController(IMasterService masterService)
        {

            _masterService = masterService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMasterDetail(CreateMasterDetailDTO dto)
        {
            try
            {
                var output = await _masterService.CreateMasterDetail(dto);

                return Ok(new { MasterId = output });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMasterDetail(int id, [FromBody] UpdateMasterDetailDTO dto)
        {
            try
            {
                dto.Id = id;
                var isUpdated = await _masterService.UpdateMasterDetail(dto);

                if (!isUpdated)
                    return NotFound("Record not found!");

                return Ok("Update successful!");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMasterDetail(int id)
        {
            try
            {
                var isDeleted = await _masterService.DeleteMasterDetail(id);

                if (!isDeleted)
                {
                    return NotFound("Master record not found or already deleted.");
                }

                return Ok("Master record and associated details have been deleted.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMasterDetails()
        {
            try
            {
                var masters = await _masterService.GetAllMasterDetails();
                return Ok(masters);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMasterDetailById(int id)
        {
            try
            {
                var master = await _masterService.GetMasterDetailById(id);

                if (master == null)
                    return NotFound("Master record not found.");

                return Ok(master);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


    }
}
