using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Infrastructure.Data.Seed
{
    public static class DefaultUsers
    {
        public static List<User> DefaultUserList = new List<User>
        {
            new User {UserName = "vtnghia22", FullName = "Võ Thành Nghĩa", Email = "vtnghia22@gmail.com"},
            new User {UserName = "nhquan22", FullName = "Nguyễn Hồng Quân", Email = "nhquan22@gmail.com"},
            new User {UserName = "pnk22", FullName = "Phạm Nguyên Khánh", Email = "pnk22@gmail.com"},
            new User {UserName = "tghuy22", FullName = "Triệu Gia Huy", Email = "tghuy22@gmail.com"}
        };

        public static string DefaultPassword = "Admin@123456";
    }
}
