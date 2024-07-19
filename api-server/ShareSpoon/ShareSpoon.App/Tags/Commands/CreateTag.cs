using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ShareSpoon.App.Abstractions;
using ShareSpoon.App.ResponseModels;
using ShareSpoon.Domain.Enums;
using ShareSpoon.Domain.Models.Recipes;

namespace ShareSpoon.App.Tags.Commands
{
    public record CreateTag(string Name, TagType Type) : IRequest<TagResponseDto>;

    public class CreateTagHandler : IRequestHandler<CreateTag, TagResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTagHandler> _logger;

        public CreateTagHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateTagHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TagResponseDto> Handle(CreateTag request, CancellationToken ct)
        {
            var tag = new Tag()
            {
                Name = request.Name,
                Type = request.Type
            };
            var createdTag = await _unitOfWork.TagRepository.CreateTag(tag, ct);

            _logger.LogInformation("Created new tag");
            return _mapper.Map<TagResponseDto>(createdTag);
        }
    }
}
