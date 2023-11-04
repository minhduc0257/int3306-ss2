namespace int3306.Entities.Shared
{
    public interface IBaseEntity
    {
        public int Id { get; set; }
        public int Status { get; set; }
    }
}