using System.Linq.Expressions;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using SharedLibrary.Dtos;

namespace Services.Services;

public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<TEntity> _genericRepository;

    public GenericService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> repository)
    {
        _unitOfWork = unitOfWork;
        _genericRepository = repository;
    }

    public async Task<Response<TDto>> GetByIdAsync(Guid id)
    {
        var entity = await _genericRepository.GetByIdAsync(id);
        if (entity == null)
        {
            return Response<TDto>.Fail("Id not found", 404, true);
        }

        return Response<TDto>.Success(ObjectMapper.Mapper.Map<TDto>(entity), 200);
    }

    public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
    {
        var entities = ObjectMapper.Mapper.Map<List<TDto>>(await _genericRepository.GetAllAsync());
        return Response<IEnumerable<TDto>>.Success(entities, 200);
    }

    public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
    {
        var list = _genericRepository.Where(predicate);
        return Response<IEnumerable<TDto>>.Success(ObjectMapper.Mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()), 200);
    }

    public async Task<Response<TDto>> AddAsync(TDto dto)
    {
        var newEntity = ObjectMapper.Mapper.Map<TEntity>(dto);
        await _genericRepository.AddAsync(newEntity);
        await _unitOfWork.CommitAsync();
        var newDto = ObjectMapper.Mapper.Map<TDto>(newEntity);
        return Response<TDto>.Success(newDto, 200);
    }

    public async Task<Response<NoDataDto>> Remove(Guid id)
    {
        var isExistEntity = await _genericRepository.GetByIdAsync(id);
        if (isExistEntity == null)
        {
            return Response<NoDataDto>.Fail("Id not found", 404, true);
        }

        _genericRepository.Remove(isExistEntity);
        await _unitOfWork.CommitAsync();
        return Response<NoDataDto>.Success(204);
    }

    public async Task<Response<NoDataDto>> Update(TDto dto, Guid id)
    {
        var isExistEntity = await _genericRepository.GetByIdAsync(id);
        if (isExistEntity == null)
        {
            return Response<NoDataDto>.Fail("Id not found", 404, true);
        }

        var updateEntity = ObjectMapper.Mapper.Map<TEntity>(dto);
        _genericRepository.Update(updateEntity);
        await _unitOfWork.CommitAsync();
        return Response<NoDataDto>.Success(204);
    }
}