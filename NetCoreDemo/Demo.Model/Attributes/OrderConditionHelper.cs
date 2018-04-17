using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo.Model.Attributes
{
    public class OrderConditionHelper
    {
        /// <summary>
        /// 获取排序字段仓储单例
        /// </summary>
        private static readonly OrderConditionStorage BaseModelFilds = OrderConditionStorage.GetInstance();

        /// <summary>
        /// 转换排序条件
        /// </summary>
        /// <typeparam name="T">检查字段是否符合条件</typeparam>
        /// <param name="sortby">排序字段</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="prefix">针对SortBy列出来字段的字段前缀，自动会补上点</param>
        /// <param name="special">特殊情况</param>
        /// <param name="defaultOrderby">默认排序方式</param>
        /// <returns></returns>
        public static string TransformOrderCondition<T>(string sortby, string orderby, string prefix = null, Dictionary<string, string> special = null, Orderby defaultOrderby = Orderby.Desc)
        {
            List<string> orderBySql = new List<string>();
            string[] fields;
            string[] orderModes = null;

            #region  + 预处理

            if (!string.IsNullOrWhiteSpace(sortby))
            {
                fields = sortby.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                //不需要排序 Or ID DESC
                return string.Empty;
            }
            if (!string.IsNullOrWhiteSpace(orderby))
            {
                orderModes = orderby.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix = $"{prefix.TrimEnd('.')}.";
            }

            #endregion
            //如果排序字段不合法，不拼接排序查询sql返回空字符
            if (!CheckPropertiesCorrect<T>(special, fields))
                return string.Empty;

            //Sql 拼装
            for (var i = 0; i < fields.Length; i++)
            {
                Orderby orderMode;
                if (orderModes == null || orderModes.Length <= i || !Enum.TryParse(orderModes[i], true, out orderMode) || !Enum.IsDefined(typeof(Orderby), orderMode))
                {
                    orderMode = defaultOrderby;
                }

                if (special != null && special.ContainsKey(fields[i]))
                {
                    //Special
                    orderBySql.Add(special[fields[i]]);
                }
                else
                {
                    //Properties
                    orderBySql.Add($"{prefix}{fields[i]} {orderMode}");
                }
            }

            return $" ORDER BY {string.Join(",", orderBySql.ToArray())} ";
        }

        /// <summary>
        /// 获取验证字段集合
        /// </summary>
        /// <typeparam name="T">返回结果Model</typeparam>
        /// <param name="special">特殊字段集</param>
        /// <param name="fields">字段集</param>
        /// <returns>排序字段是否合法</returns>
        public static bool CheckPropertiesCorrect<T>(Dictionary<string, string> special, string[] fields)
        {
            List<string> checkProperties;
            var modelType = typeof(T);
            if (BaseModelFilds.ContainsKey(modelType.FullName))
            {
                checkProperties = BaseModelFilds[modelType.FullName];
            }
            else
            {
                //将类下所有字段加入集合中
                checkProperties = modelType.GetProperties().Select(modelField => modelField.Name.ToUpper()).ToList();
                BaseModelFilds.Add(modelType.Name, checkProperties);
            }

            if (special != null && special.Count != 0)
            {
                special.Keys.ToList().ForEach(m => checkProperties.Add(m.ToUpper()));
            }

            foreach (var item in fields)
            {
                if (!checkProperties.Contains(item.ToUpper()))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 排序条件（字段）仓储（内部类）
        /// </summary>
        private class OrderConditionStorage
        {
            private static OrderConditionStorage _instance;
            private static readonly object ObjLock = new object();
            private readonly ConcurrentDictionary<string, List<string>> _cache;

            private OrderConditionStorage()
            {
                //初始化仓库
                _cache = new ConcurrentDictionary<string, List<string>>();
            }

            /// <summary>
            /// 获取单例
            /// </summary>
            /// <returns></returns>
            public static OrderConditionStorage GetInstance()
            {
                if (_instance == null)
                {
                    lock (ObjLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new OrderConditionStorage();
                        }
                    }
                }
                return _instance;
            }

            /// <summary>
            /// 创建索引
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public List<string> this[string key]
            {
                get
                {
                    List<string> val = null;
                    _cache.TryGetValue(key, out val);

                    return val;
                }
                set
                {
                    _cache.AddOrUpdate(key, value, (k, v) =>
                    {
                        v = value;
                        return value;
                    });
                }
            }

            /// <summary>
            /// 验证Key是否存在
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public bool ContainsKey(string key)
            {
                return _cache.ContainsKey(key);
            }

            /// <summary>
            /// 添加一个指定的键值对
            /// </summary>
            /// <param name="key"></param>
            /// <param name="val"></param>
            public void Add(string key, List<string> val)
            {
                _cache.AddOrUpdate(key, val, (k, v) =>
                {
                    v = val;
                    return val;
                });
            }
        }
    }
}
