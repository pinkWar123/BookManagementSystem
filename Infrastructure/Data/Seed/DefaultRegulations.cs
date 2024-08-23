using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Infrastructure.Data.Seed
{
    public static class DefaultRegulations
    {
        public static List<Regulation> DefaultRegulationList = new List<Regulation>
        {
            new Regulation
            {
                Code = "QD1.1",
                Content = "Số lượng nhập ít nhất là",
                Value = 150,
                Status = true
            },
            new Regulation
            {
                Code = "QD1.2",
                Content = "Chỉ nhập các đầu sách có số lượng nhập ít hơn",
                Value = 300,
                Status = true
            },
            new Regulation
            {
                Code = "QD2.1",
                Content = "Chỉ bán cho các khách hàng nợ không quá",
                Value = 20000,
                Status = true
            },
            new Regulation
            {
                Code = "QD2.2",
                Content = "Chỉ bán các đầu sách có số lượng tồn sau khi bán ít nhất là",
                Value = 20,
                Status = true
            },
            new Regulation
            {
                Code = "QD4.0",
                Content = "Số tiền thu không vượt quá số tiền khách hàng đang nợ",
                Value = 1,
                Status = true
            }
        };
    }
}