using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Models.Queries.GetById;

public class GetByIdModelQuery : IRequest<GetByIdModelResponse>
{
    public Guid Id { get; set; }

    public class GetByIdModelQueryHandler : IRequestHandler<GetByIdModelQuery, GetByIdModelResponse>
    {
        private readonly IMapper _mapper;
        private readonly IModelRepository _modelRepository;

        public GetByIdModelQueryHandler(IModelRepository modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        public async Task<GetByIdModelResponse> Handle(GetByIdModelQuery request, CancellationToken cancellationToken)
        {
            Model? model = await _modelRepository.GetAsync(
         predicate: b => b.Id == request.Id,
         include: m => m.Include(m => m.Brand).Include(m => m.Fuel).Include(m => m.Transmission),
         withDeleted: true,
         cancellationToken: cancellationToken
     );

            GetByIdModelResponse response = _mapper.Map<GetByIdModelResponse>(model);
            return response;

        }
    }
}
