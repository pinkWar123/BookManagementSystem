using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.Upload
{
    public class UploadResultDto
    {
        public List<string> FileNames { get; set; } = new List<string>();
    }
}