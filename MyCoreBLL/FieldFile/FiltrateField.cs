using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreBLL.FieldFile
{
    public class FiltrateField
    {
        /// <summary>
        /// 查询页数
        /// </summary>
        public int P { get; set; } = 0;

        /// <summary>
        /// 查询数量
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort { get; set; } = "AdditionDate";

        /// <summary>
        /// 条件查询
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// Like查询
        /// </summary>
        public string Contain { get; set; }

        /// <summary>
        /// 需要返回的字段
        /// </summary>
        public string Fields { get; set; }
    }
}
