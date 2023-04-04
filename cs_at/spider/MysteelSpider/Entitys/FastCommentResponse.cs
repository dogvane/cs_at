using MysteelSpider.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteelSpider.Entitys
{

    internal class FastCommentResponse
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int pageNo { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int totalPage { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public List<FastcommentEntity> list { get; set; }
    }
}
