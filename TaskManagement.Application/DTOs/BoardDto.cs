using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement.Application.DTOs
{
    public class BoardDto
    {
        public Guid Id {  get; set; }
        public string Name { get; set; } = default!;
        public Guid ProjectId { get; set; }
    }
}
