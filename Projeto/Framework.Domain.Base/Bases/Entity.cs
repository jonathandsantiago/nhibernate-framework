using Framework.Domain.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.Domain.Base
{
    public abstract class Entity : IEntity
    {
        protected Guid guid;

        public virtual Guid Guid
        {
            get
            {
                if (guid == null || guid == Guid.Empty)
                {
                    guid = Guid.NewGuid();
                }

                return guid;
            }
        }

        public abstract object GetId();
    }

    public abstract class Entity<TId> : Entity, IEntity<TId>
    {
        [Key]
        public virtual TId Id { get; set; }

        public override object GetId()
        {
            return Id;
        }

        public virtual void Prepare()
        { }
    }
}