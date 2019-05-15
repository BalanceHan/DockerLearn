using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyCoreDAL.FatherEntity
{
    public class FatherEntitys
    {
        public FatherEntitys()
        {
            AdditionDate = DateTime.Now;
            AdditionUnix = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
            IsDelete = false;
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AdditionDate { get; set; }

        /// <summary>
        /// 添加时间时间戳
        /// </summary>
        public long? AdditionUnix { get; set; }

        /// <summary>
        /// 数据是否删除
        /// </summary>
        public bool? IsDelete { get; set; } = false;

        /// <summary>
        /// 操作添加人
        /// </summary>
        [StringLength(50)]
        public string CreatePeople { get; set; }
    }
}
