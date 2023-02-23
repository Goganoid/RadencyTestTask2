using Application.Books.Commands;
using Application.Books.Commands.Core.Models;
using Application.Books.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Common;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // Output DTOs
        CreateMap<Book, ListedBookResponseDTO>()
            .ForMember(dto => dto.Rating,
                opt => opt.MapFrom(b => b.AverageScore()))
            .ForMember(dto => dto.Reviews,
                opt => opt.MapFrom(b => b.Reviews.Count));
        CreateMap<Book, BookDetailsResponseDTO>()
            .ForMember(dto => dto.Rating,
                opt => opt.MapFrom(b => b.AverageScore()));
        CreateMap<Review, ReviewResponseDTO>();
        CreateMap<Review, IdResponseDTO>().ForMember(dto => dto.Id,
            opt=>opt.MapFrom(r=>r.ReviewId));
        CreateMap<Book, IdResponseDTO>().ForMember(dto => dto.Id,
            opt=>opt.MapFrom(b=>b.BookId));

        // Input DTOs
        CreateMap<SaveBookRequestDTO, Book>();
        CreateMap<SaveReviewRequestDTO, Review>();
        CreateMap<RateBookRequestDTO, Rating>();
    }
}