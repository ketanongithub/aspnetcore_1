using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Entities
{
    public interface IEntity : IEntity<int>
    {
    }

    public interface IEntity<TPrimaryKey>
    {
        //
        // Summary:
        //     Unique identifier for this entity.
        TPrimaryKey Id { get; set; }

        //
        // Summary:
        //     Checks if this entity is transient (not persisted to database and it has not
        //     an App.Domain.Entities.IEntity`1.Id).
        //
        // Returns:
        //     True, if this entity is transient
        bool IsTransient();
    }
}
