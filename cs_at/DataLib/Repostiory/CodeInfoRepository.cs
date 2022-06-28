using DownByWind.DbSet;
using DownByWind.Entity;
using ServiceStack.OrmLite;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLib.Repostiory
{

    public class CodeInfoRepository
    {
        /// <summary>
        /// 获得当前还在交易中的代码
        /// </summary>
        /// <returns></returns>
        public List<CodeInfo> GetRunCodeInfos()
        {
            using(var con = QuantDB.GetCon())
            {
                var now = DateTime.Now;

                return con.Select<CodeInfo>(o => o.LastTradeDate > now);
            }
        }

        /// <summary>
        /// 只按照单一代码来获取
        /// </summary>
        /// <param name="codeId"></param>
        /// <returns></returns>
        public CodeInfo GetByCodeId(string codeId)
        {
            using(var con = QuantDB.GetCon())
            {
                return con.Single<CodeInfo>(o=>o.Code == codeId || o.WindCode == codeId);
            }
        }
    }
}
