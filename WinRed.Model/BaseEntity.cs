using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRed.Model
{
    //基础实体
    public class BaseEntity
    {
        [Key,MaxLength(32)]
        [Required]
        public string ID { get; set; }        

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required]
        public System.DateTime CreatedTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required]
        public System.DateTime UpdatedTime { get; set; }

        /// <summary>
        /// 状态枚举
        /// </summary>
        public bool IsDelete { get; set; } = false;
    }
}
