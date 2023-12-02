namespace Access.Core.Entities
{
    public interface IBaseEntity : ICloneable
    {
        public bool IsDeleted { get; set; }
    }
}