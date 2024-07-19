using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public enum Roles
    {
        QuanLy,     // Quản lý
        ThuNgan,   // Nhân viên
        KhachHang,     // Kế toán
        ThuKho       // Bảo vệ
    }
    public class Users : Base
    {
        [StringLength(100)]
        public required string fullName { get; set; }

        [StringLength(100)]
        public string? email { get; set; }

        [StringLength(10)]
        public string? phoneNumber { get; set; }

        public required Roles Role { get; set; }

        [StringLength(100)]
        public required string userlogin { get; set; }
        [StringLength(100)]
        public required string password { get; set; }

    }


}