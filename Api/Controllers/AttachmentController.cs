using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Upload;
using BookManagementSystem.Application.Services;
using BookManagementSystem.Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/attachment")]
    public class AttachmentController : ControllerBase
    {
        AzureBlobService _service;
        public AttachmentController(AzureBlobService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlobs(List<IFormFile> files)
        {
            var response = await _service.UploadFiles(files);
            return Ok(new Response<UploadResultDto>(new UploadResultDto{
                 FileNames = response}));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlobs()
        {
            var response = await _service.GetUploadedBlobs();
            return Ok(response);
        }

    }
}