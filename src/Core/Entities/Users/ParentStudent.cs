namespace Core.Entities.Users
{
    public class ParentStudent : BaseEntity
    {
        public Guid ParentsId { get; set; }
        public Guid StudentsId { get; set; }
    }
}
