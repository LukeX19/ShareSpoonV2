using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Tags.Queries
{
    public record GetFilterTags() : IRequest<List<TagResponseDto>>;

    public class GetFilterTagsHandler : IRequestHandler<GetFilterTags, List<TagResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetFilterTagsHandler> _logger;

        public GetFilterTagsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetFilterTagsHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TagResponseDto>> Handle(GetFilterTags request, CancellationToken ct)
        {
            var tags = await _unitOfWork.TagRepository.GetFilterTags(ct);

            _logger.LogInformation("Retrieved all tags used in recipe filtering");
            return _mapper.Map<List<TagResponseDto>>(tags);
        }
    }
}
