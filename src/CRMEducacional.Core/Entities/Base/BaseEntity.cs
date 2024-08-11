namespace CRMEducacional.Core.Entities.Base;

public abstract class BaseEntity
{
    protected BaseEntity()
    { }

    protected BaseEntity(int id)
    {
        Id = id;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; protected set; }
}