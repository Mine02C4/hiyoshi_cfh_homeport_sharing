using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HiyoshiCfhWeb.Models
{
    public class Permission
    {
        public int PermissionId { get; set; }

        /// <summary>
        /// 許可するメールアドレス。
        /// </summary>
        [Required]
        public string Email { get; set; }
    }
}