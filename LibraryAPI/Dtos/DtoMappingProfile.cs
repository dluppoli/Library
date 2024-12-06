using AutoMapper;
using LibraryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryAPI.Dtos
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Book, BookDto>();
            CreateMap<BookDto, Book>()
                .ForMember(d=> d.CategoryId, m=> m.MapFrom( s=>s.Category.Id) )
                .ForMember(d => d.Category, m => m.Ignore());

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
        }
    }
}