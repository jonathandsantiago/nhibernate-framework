using Framework.Validator.Interfaces;
using System;

namespace Framework.Domain.Base.Interfaces
{
    public interface IEntity : IEntityValidation
    {
        Guid Guid { get; }
        object GetId();
    }

    public interface IEntity<TId> : IEntity
    {
        TId Id { get; set; }
        void Prepare();
    }
}