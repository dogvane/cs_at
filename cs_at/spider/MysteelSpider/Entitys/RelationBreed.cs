namespace MysteelSpider.Entitys
{
    /// <summary>
    /// 关联品种
    /// </summary>
    [Serializable]
    public class RelationBreed
    {
        /// <summary>
        /// 关联品种id
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 关联品种
        /// </summary>
        public string name { get; set; }
    }
}