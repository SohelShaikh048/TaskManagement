using System;
using System.Collections.Generic;
using System.Text;
using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
    public class ProjectMember : BaseEntity
    {
        public Guid ProjectId { get; set; }
        public string UserId { get; set; } = default!;
        public Project Project { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
        public MemberRoleEnum Role { get; set; }
    }
}
