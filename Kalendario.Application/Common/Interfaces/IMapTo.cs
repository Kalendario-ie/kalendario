using AutoMapper;

namespace Kalendario.Application.Common.Interfaces
{
    public interface IMapTo<T>
    {   
        void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T));
    }
}