using AutoMapper;
using Simple_Quotes_API.Dtos;
using Simple_Quotes_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Profiles
{
    public class QuotesProfile : Profile
    {
        public QuotesProfile()
        {
            CreateMap<QuoteUpdateDto, Quote>();
            CreateMap<Quote, QuoteUpdateDto>();
        }
    }
}
