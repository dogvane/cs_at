using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MysteelSpider.Entitys
{
    /// <summary>
    /// 消息快讯的消息实体类
    /// </summary>
    /// <remarks>
    public class FastcommentEntity
    {
        /// 消息id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        public int categoryId { get; set; }
        /// <summary>
        /// 栏目id
        /// </summary>
        public int sectionId { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 关联品种id
        /// </summary>
        public string relationBreedId { get; set; }
        /// <summary>
        /// 关联品种
        /// </summary>
        public List<RelationBreed> relationBreed { get; set; }
        /// <summary>
        /// 关联地区id
        /// </summary>
        public string relationCityId { get; set; }
        /// <summary>
        /// 关联地区
        /// </summary>
        public List<RelationCity> relationCity { get; set; }
        /// <summary>
        /// 关联工厂id
        /// </summary>
        public string relationFactoryId { get; set; }
        /// <summary>
        /// 关联工厂
        /// </summary>
        public List<RelationFactory> relationFactory { get; set; }
        /// <summary>
        /// 关联港口id
        /// </summary>
        public string relationPortId { get; set; }
        /// <summary>
        /// 关联港口
        /// </summary>
        public List<RelationPort> relationPort { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string inArticleTitle { get; set; }
        /// <summary>
        /// 文章链接
        /// </summary>
        public string inArticleUrl { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string outArticleTitle { get; set; }
        /// <summary>
        /// 文章链接
        /// </summary>
        public string outArticleUrl { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string source
        {
            get; set;
        }

        DateTime? _publisherTime;

        /// <summary>
        /// mysteel 网站爬取到的时间是 long 类型，对应到前端js 是需要 new Date(value) 的方式进行转换的
        /// 同时，本地数据使用时，还是基于 DateTime模式来，所以当前数据在返回时，只有 DateTime 格式的数据
        /// 但是为了方便序列化，这里会同时支持3种类型的变量作为数据，请注意处理
        /// </summary>
        public object publisherTime
        {
            get
            {
                if (_publisherTime == null)
                    _publisherTime = DateTime.Now;

                return _publisherTime;
            }
            set
            {
                if (value is long)
                {
                    _publisherTime = ConvertJavaScriptToDateTime((long)value);
                }
                else if (value is string)
                {
                    var v = (string)value;
                    if (v.Contains("date", StringComparison.OrdinalIgnoreCase))
                    {
                        // 通过正则表达拿到v里的数字数字
                        var reg = new System.Text.RegularExpressions.Regex(@"\d+");
                        var match = reg.Match(v).Value;
                        var lv = long.Parse(match);
                        _publisherTime = ConvertJavaScriptToDateTime(lv);
                    }
                    else
                    {
                        _publisherTime = DateTime.Parse(v);
                    }
                }
                else if (value is DateTime)
                {
                    _publisherTime = (DateTime)value;
                }
                else
                {
                    _publisherTime = null;
                }
            }
        }

        /// <summary>
        /// 将javascript 的 date() 转换为 DateTime
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal static DateTime ConvertJavaScriptToDateTime(long date){
            var start = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var ret = start.AddMilliseconds(date).DateTime;
            // 还需要将时间时区转为北京时间
            ret = ret.AddHours(8);  // 代码帮我生成的，暂时也就这样吧，理论上是要读取当前系统时区的
            return ret;
        }

        /// <summary>
        /// 发布时间戳
        /// </summary>
        public long publisherTimeLong { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public List<string> imageUrl { get; set; }
        /// <summary>
        /// 分享图片地址
        /// </summary>
        public string shareImageUrl { get; set; }
        /// <summary>
        /// 语音地址
        /// </summary>
        public string voiceUrl { get; set; }
        /// <summary>
        /// 是否为广告
        /// </summary>
        public int advertisementFlag { get; set; }
        /// <summary>
        /// 品种标签
        /// </summary>
        public List<string> breedTags { get; set; }

        /// <summary>
        /// 数据来源
        /// </summary>
        public int dataSource { get; set; }
        /// <summary>
        /// 关联id
        /// </summary>
        public int relationId { get; set; }
        /// <summary>
        /// 文章id
        /// </summary>
        public int inArticleAid { get; set; }
        /// <summary>
        /// 文章id
        /// </summary>
        public int outArticleAid { get; set; }
        /// <summary>
        /// 是否限制wap
        /// </summary>
        public bool wapRestrict { get; set; }
        /// <summary>
        /// wap剩余字数
        /// </summary>
        public string wapResidualWords { get; set; }
        /// <summary>
        /// 栏目名称
        /// </summary>
        public string sectionName { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string categoryName { get; set; }
        /// <summary>
        /// 阅读量
        /// </summary>
        public int? readingCount { get; set; }
    }
}
