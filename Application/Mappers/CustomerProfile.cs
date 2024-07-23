using AutoMapper;
using BookManagementSystem.Application.Dtos.Customer;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<UpdateCustomerDto, Customer>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}