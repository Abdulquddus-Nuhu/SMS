using Access.Core.Entities.Users;
using Sharedd.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Core.Entities
{
    public class QrCode : BaseEntity
    {
        /// <summary>
        /// Parent-Id or BusDriver-Id or Staff-Id
        /// </summary>
        public Guid UserId { get; set; }
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }
        public bool InSchool { get; set; } = true;
        public DateTime PickUpTime { get; set; }
        public DateTime DropOffTime { get; set; }
        public AuthorizedUserType AuthorizedUser { get; set; } = AuthorizedUserType.Self;
        public string? AuthorizedUserRelationship { get; set; }
        public string? AuthorizedUserFirstName { get; set; }
        public string? AuthorizedUserLastName { get; set; }
    }
}
