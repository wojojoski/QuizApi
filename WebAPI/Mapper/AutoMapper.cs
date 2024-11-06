using AutoMapper;
using BackendLab01;
using Infrastructure.EF.Entities;
using System.Net;
using WebAPI.DTO;

namespace WebAPI.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<NewQuizDto, Quiz>();

            CreateMap<QuizItem, QuizItemDto>()
                .ForMember(
                q => q.Options,
                op => op.MapFrom(i => new List<string>(i.IncorrectAnswers) { i.CorrectAnswer }));

            CreateMap<Quiz, QuizDto>().
                ForMember(
                q => q.Items,
                op => op.MapFrom<List<QuizItem>>(i => i.Items)
                );

            CreateMap<QuizItemUserAnswer, AnswerDto>()
                .ForMember(
                dest => dest.Questions,
                opt => opt.MapFrom(src => src.QuizItem.Question)
                )
                .ForMember(
                dest => dest.IsCorrect,
                opt => opt.MapFrom(src => src.IsCorrect())
                );

            CreateMap<QuizItemUserAnswer, FeedbackDto>()
                .ForMember(
                dest => dest.TotalQuestion,
                opt => opt.MapFrom(src => src.Answer.Count())
                )
                .ForMember(
                dest => dest.Answers,
                opt => opt.MapFrom(src => src.Answer)
                );
            CreateMap<QuizDto, Quiz>();

        }
    }
}
