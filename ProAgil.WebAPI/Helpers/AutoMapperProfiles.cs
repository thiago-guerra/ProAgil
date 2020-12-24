using System.Linq;
using AutoMapper;
using ProAgil.Domain;
using ProAgil.Domain.Identity;
using ProAgil.WebAPI.Dtos;

namespace ProAgil.WebAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Evento, EventoDto>().
                                        ForMember(x => x.Palestrantes, y => { y.MapFrom(z => z.PalestranteEventos.Select(a => a.Palestrante).ToList());}).ReverseMap();

            CreateMap<Lote, LoteDto>().ReverseMap();

            CreateMap<Palestrante, PalestranteDto>().
                                                ForMember(x => x.Eventos, y => {
                                                    y.MapFrom(z => z.PalestranteEventos.Select(a => a.Evento).ToList());
                                                    }).ReverseMap();
                                                    
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();

        }
    }
}