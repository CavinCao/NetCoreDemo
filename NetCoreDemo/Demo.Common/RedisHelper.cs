using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace Demo.Common
{
    public class RedisHelper
    {
        private const int DB_INDEX = 0;

        private static readonly object objLock = new object();
        private static RedisHelper instance = null;

        private static ConnectionMultiplexer connection = null;

        private RedisHelper()
        {
            connection = ConnectionMultiplexer.Connect(AppSetting.GetConfig("Redis:ConnectionString"));
        }

        public static RedisHelper GetInstance()
        {
            if (instance == null)
            {
                lock (objLock)
                {
                    if (instance == null)
                    {
                        instance = new RedisHelper();
                    }
                }
            }

            return instance;
        }

        public async Task<T> Get<T>(string key)
        {
            IDatabaseAsync db = connection.GetDatabase(DB_INDEX);
            var result = await db.StringGetAsync(key);
            if (!result.HasValue)
                return default(T);

            string val = result.ToString();

            return JsonConvert.DeserializeObject<T>(val);
        }

        /// <summary>
        /// 向缓存存入string类型数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存的键</param>
        /// <param name="obj">待存入的数据对象</param>
        /// <param name="timeExpire">过期时长，单位秒，默认24小时</param>
        /// <returns></returns>
        public async Task Set<T>(string key, T obj, int timeExpire = 24 * 60 * 60)
        {
            IDatabaseAsync db = connection.GetDatabase(DB_INDEX);

            await db.StringSetAsync(key, JsonConvert.SerializeObject(obj), TimeSpan.FromSeconds(timeExpire));
        }

        /// <summary>
        /// 更新键的失效时间
        /// </summary>
        /// <param name="key">缓存的键</param>
        /// <param name="timeExpire">过期时长，单位秒</param>
        /// <returns></returns>
        public async Task UpdateTTL(string key, int timeExpire)
        {
            IDatabaseAsync db = connection.GetDatabase(DB_INDEX);

            await db.KeyExpireAsync(key, TimeSpan.FromSeconds(timeExpire));
        }

        public async Task<long> IncAsync(string key, long step = 1)
        {
            IDatabaseAsync db = connection.GetDatabase(DB_INDEX);

            return await db.StringIncrementAsync(key, step);
        }

        public async Task RemoveKey(string key)
        {
            IDatabaseAsync db = connection.GetDatabase(DB_INDEX);

            await db.KeyDeleteAsync(key);
        }
    }
}
