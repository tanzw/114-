using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryPlatform.Code.Common
{
    /// <summary>
    /// 缓存的键
    /// </summary>
    public enum CacheKey
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        UserName,
        RealName,
        Role
    }


    /// <summary>
    /// 全局缓存类
    /// </summary>
    public class CacheStrategy
    {
        private static volatile Dictionary<CacheKey, object> dic = new Dictionary<CacheKey, object>();
        public static readonly CacheStrategy Instance = new CacheStrategy();
        static CacheStrategy()
        {

        }


         
        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="o"></param>
        public void SetObj(CacheKey key, object o)
        {
            if (dic.ContainsKey(key))
            {
                dic.Remove(key);
            }
            dic.Add(key, o);
        }

        /// <summary>
        /// 删除缓存对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        public void RemoveObject(CacheKey objId)
        {
            if (!dic.ContainsKey(objId))
            {
                return;
            }
            dic.Remove(objId);
        }

        /// <summary>
        /// 返回一个指定的对象
        /// </summary>
        /// <param name="objId">对象的关键字</param>
        /// <returns>对象</returns>
        public object GetObject(CacheKey objId)
        {
            if (!dic.ContainsKey(objId))
            {
                return null;
            }
            return dic[objId];
        }
    }
}
