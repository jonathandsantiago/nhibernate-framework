using System;

namespace Framework.Domain.Base.Interfaces
{
    public interface IEntityAudit<TId>: IEntity<TId>
    {
        string UserInsertion { get; set; }
        string UserEdition { get; set; }
        DateTime DateInsertion { get; set; }
        DateTime? DateEdition { get; set; }
        int Revision { get; set; }
    }
}