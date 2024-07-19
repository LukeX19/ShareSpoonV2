using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Tags.Queries
{
    public record GetAllTags() : IRequest<List<TagResponseDto>>;

    public class GetAllTagsHandler : IRequestHandler<GetAllTags, List<TagResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTagsHandler> _logger;

        public GetAllTagsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllTagsHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TagResponseDto>> Handle(GetAllTags request, CancellationToken ct)
        {
            var tags = await _unitOfWork.TagRepository.GetAll(ct);

            _logger.LogInformation("Retrieved all tags");
            return _mapper.Map<List<TagResponseDto>>(tags);
        }
    }
}
