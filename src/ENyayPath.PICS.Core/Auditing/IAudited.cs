using ENyayPath.PICS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Auditing
{
    #region Delete

    public interface ISoftDelete
    {
        //
        // Summary:
        //     Used to mark an Entity as 'Deleted'.
        bool IsDeleted { get; set; }
    }

    public interface IHasDeletionTime : ISoftDelete
    {
        //
        // Summary:
        //     Deletion time of this entity.
        DateTime? DeletionTime { get; set; }
    }

    public interface IDeletionAudited : IHasDeletionTime, ISoftDelete
    {
        //
        // Summary:
        //     Which user deleted this entity?
        long? DeleterUserId { get; set; }
    }
    public interface IDeletionAudited<TUser> : IDeletionAudited, IHasDeletionTime, ISoftDelete where TUser : IEntity<long>
    {
        //
        // Summary:
        //     Reference to the deleter user of this entity.
        TUser DeleterUser { get; set; }
    }
    #endregion Delete

    #region Create
    public interface IHasCreationTime
    {
        //
        // Summary:
        //     Creation time of this entity.
        DateTime CreationTime { get; set; }
    }

    public interface ICreationAudited : IHasCreationTime
    {
        //
        // Summary:
        //     Id of the creator user of this entity.
        long? CreatorUserId { get; set; }
    }

    public interface ICreationAudited<TUser> : ICreationAudited, IHasCreationTime where TUser : IEntity<long>
    {
        //
        // Summary:
        //     Reference to the creator user of this entity.
        TUser CreatorUser { get; set; }
    }
    #endregion Create

    #region Modify
    public interface IHasModificationTime
    {
        //
        // Summary:
        //     The last modified time for this entity.
        DateTime? LastModificationTime { get; set; }
    }

    public interface IModificationAudited : IHasModificationTime
    {
        //
        // Summary:
        //     Last modifier user for this entity.
        long? LastModifierUserId { get; set; }
    }

    public interface IModificationAudited<TUser> : IModificationAudited, IHasModificationTime where TUser : IEntity<long>
    {
        //
        // Summary:
        //     Reference to the last modifier user of this entity.
        TUser LastModifierUser { get; set; }
    }
    #endregion Modify


    public interface IAudited : ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime
    {
    }

    public interface IAudited<TUser> : IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime, ICreationAudited<TUser>, IModificationAudited<TUser> where TUser : IEntity<long>
    {
    }

    public interface IFullAudited : IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime, IDeletionAudited, IHasDeletionTime, ISoftDelete
    {
    }

    public interface IFullAudited<TUser> : IAudited<TUser>, IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime, ICreationAudited<TUser>, IModificationAudited<TUser>, IFullAudited, IDeletionAudited, IHasDeletionTime, ISoftDelete, IDeletionAudited<TUser> where TUser : IEntity<long>
    {
    }
}
