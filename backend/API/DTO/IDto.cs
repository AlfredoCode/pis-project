using PRegSys.DAL.Entities;

namespace PRegSys.API.DTO;

public interface IWriteDto<TDto, T>
    where TDto : IWriteDto<TDto, T>
{
    // `T ToEntity(...)` but each DTO type requires a different set of parameters
}

public interface IReadDto<TDto, T>
    where TDto : IReadDto<TDto, T>
{
    public static abstract TDto FromEntity(T entity);
}

public static class DtoExtensions
{
    public static TDto ToDto<TEntity, TDto>(this TEntity entity)
        where TEntity : IEntity
        where TDto : IReadDto<TDto, TEntity>
    {
        return TDto.FromEntity(entity);
    }

    //public static TEntity ToEntity<TDto, TEntity>(this TDto dto, int id)
    //    where TDto : IWriteDto<TDto, TEntity>
    //    where TEntity : IEntity
    //{
    //    var entity = dto.ToEntity();
    //    entity.Id = id;
    //    return entity;
    //}
}
