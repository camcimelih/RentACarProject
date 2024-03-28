using Application.Features.Brands.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Brands.Commands.Create;
                //add deki request       add deki response
public class CreateBrandCommand : IRequest<CreatedBrandResponse>
{
    public string Name { get; set; }

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, CreatedBrandResponse>
    {
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepository;
        private readonly BrandBusinessRules _brandBusinessRules;

        public CreateBrandCommandHandler(IMapper mapper, IBrandRepository brandRepository,
                                         BrandBusinessRules brandBusinessRules)
        {
            _mapper = mapper;
            _brandRepository = brandRepository;
            _brandBusinessRules = brandBusinessRules;
        }

        public async Task<CreatedBrandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            _brandBusinessRules.BrandNameCannotBeDuplicatedWhenInserted(request.Name);
            //baþka kurallarý da buraya aktif ediyoruz.
            //
            //

            Brand brand = _mapper.Map<Brand>(request);

            await _brandRepository.AddAsync(brand);

            CreatedBrandResponse response = _mapper.Map<CreatedBrandResponse>(brand);
            return response;
        }
    }
}