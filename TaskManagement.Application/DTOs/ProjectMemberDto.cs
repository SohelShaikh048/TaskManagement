using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs
{
    public class ProjectMemberDto
    {
        public Guid ProjectId { get; set; }
        public string UserId { get; set; } = default!;
        public MemberRoleEnum Role { get; set; }
    }
}
