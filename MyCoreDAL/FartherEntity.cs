using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyCoreDAL
{
    public class FartherEntity
    {
        public FartherEntity()
        {
            AdditionDate = DateTime.Now;
            AdditionUnix = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
        }

        /// <summary>
        /// 数据添加时间
        /// </summary>
        public DateTime? AdditionDate { get; set; }

        /// <summary>
        /// 数据添加时间(时间戳格式)
        /// </summary>
        [StringLength(50)]
        public string AdditionUnix { get; set; }
    }
}
