using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;

namespace ShareSpoon.App.Tags.Queries
{
    public record SearchTagsByName(string Name) : IRequest<List<TagResponseDto>>;

    public class SearchTagsByNameHandler : IRequestHandler<SearchTagsByName, List<TagResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchTagsByNameHandler> _logger;

        public SearchTagsByNameHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SearchTagsByNameHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TagResponseDto>> Handle(SearchTagsByName request, CancellationToken ct)
        {
            var tags = await _unitOfWork.TagRepository.SearchTagsByName(request.Name, ct);

            _logger.LogInformation($"Retrieved all tags containing {request.Name}");
            return _mapper.Map<List<TagResponseDto>>(tags);
        }
    }
}
