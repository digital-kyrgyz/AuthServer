using System.Linq.Expressions;
using SharedLibrary.Dtos;

namespace Core.Services;

public interface IGenericService<TEntity, TDto> where TEntity : class where TDto : class
{
    Task<Response<TDto>> GetByIdAsync(Guid id);

    Task<Response<IEnumerable<TDto>>> GetAllAsync();

    Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate);

    Task<Response<TDto>> AddAsync(TDto dto);

    Task<Response<NoDataDto>> Remove(Guid id);

    Task<Response<NoDataDto>> Update(TDto dto, Guid id);
}