using Core.Entities.Users;
using Shared.Enums;

namespace Core.Entities
{
    public class QrCode : BaseEntity
    {
        /// <summary>
        /// Parent-Id or BusDriver-Id or Staff-Id
        /// </summary>
        //public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }
        public bool InSchool { get; set; } = true;
        public DateTime PickUpTime { get; set; }
        public DateTime DropOffTime { get; set; }
        public AuthorizedUserType AuthorizedUser { get; set; } = AuthorizedUserType.Self;
        public string? AuthorizedUserRelationship { get; set; }
        public string? AuthorizedUserFullName { get; set; }
        public string? AuthorizedUserPhoneNumber { get; set; }

        public string? ScannedBy { get; set; }
        public DateTime? ScannedTime { get; set; }
    }
}
