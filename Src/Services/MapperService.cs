using AutoMapper;
using Google.Protobuf.Collections;
using user_ms.Src.Services.Interfaces;

namespace user_ms.Src.Services
{
    public class MapperService : IMapperService
    {
        private readonly IMapper _mapper;

        public MapperService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public List<TDestination> MapList<TSource, TDestination>(List<TSource> sourceItems)
        {
            var mappedObjects = sourceItems.Select(x => _mapper.Map<TDestination>(x)).ToList();
            return mappedObjects;
        }

        public RepeatedField<TDestination> MapRepeatedField<TSource, TDestination>(List<TSource> sourceItems)
        {
            var repeatedField = new RepeatedField<TDestination>();
            repeatedField.AddRange(sourceItems.Select(x => _mapper.Map<TDestination>(x)));
            return repeatedField;
        }
    }
}