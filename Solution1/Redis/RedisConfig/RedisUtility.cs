using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Redis;
using ServiceStack.Redis.Support;
using Newtonsoft.Json;

namespace Redis.RedisConfig
{
    public class RedisUtility
    {
        /// <summary>
        /// Redis服务地址
        /// </summary>
        private static string _RedisServerIP = "127.0.0.1:6379";

        /// <summary>
        /// 服务IP
        /// </summary>
        public static string RedisServerIP
        {
            set
            {
                _RedisServerIP = value;
                basicRedisClientManager = new BasicRedisClientManager(_RedisServerIP);
            }
        }
        /// <summary>
        /// Redis客户端管理
        /// </summary>
        private static BasicRedisClientManager basicRedisClientManager = null;
        /// <summary>
        /// 应用程序ID
        /// </summary>
        protected static string _APPID="test";
        /// <summary>
        /// 应用程序ID
        /// </summary>
        public static string APPID
        {
            set
            {
                _APPID = value;
            }
        }

        /// <summary>
        /// 打开服务
        /// </summary>
        private static IRedisClient OpenServer()
        {
            if (string.IsNullOrWhiteSpace(_APPID))
            {
                throw new Exception("无应用程序ID");
            }
            if (string.IsNullOrWhiteSpace(_RedisServerIP))
            {
                throw new Exception("无Redis服务地址");
            }
            if (basicRedisClientManager == null)
            {
                //basicRedisClientManager = new BasicRedisClientManager(_RedisServerIP);
            }

            //var redisClient = basicRedisClientManager.GetClient();
            var redisClient = new RedisClient("127.0.0.1", 6379, "123456");
            redisClient.ConnectTimeout = 500;
            return redisClient;
        }

        /// <summary>
        /// 信息存入集中式缓存(基本类型)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存标识</param>
        /// <param name="value">信息</param>
        /// <returns>是否存入成功</returns>
        public static void SetCatch<T>(string key, T value, TimeSpan time)
        {
            try
            {
                using (var redisClient = OpenServer())
                {
                    redisClient.Set<T>(_APPID + "_" + key, value, time);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 信息存入集中式缓存(基本类型)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="seconds"></param>
        public static void SetCatch<T>(string key, T value, int seconds)
        {
            try
            {
                using (var redisClient = OpenServer())
                {
                    if (redisClient == null)
                    {
                        return;
                    }
                    redisClient.Set<T>(_APPID + "_" + key, value, new TimeSpan(0, 0, seconds));
                    redisClient.Dispose();
                    GC.SuppressFinalize(redisClient);
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message + ex.StackTrace;
            }
        }

        public static void DelRedis(string key)
        {
            try
            {
                using (var redisClient = OpenServer())
                {
                    if (redisClient == null)
                    {
                        return;
                    }
                    redisClient.Remove(_APPID + "_" + key);
                    redisClient.Dispose();
                    GC.SuppressFinalize(redisClient);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 从集中式缓存获取信息(基本类型)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存标识</param>
        /// <returns></returns>
        public static T GetCatch<T>(string key)
        {
            try
            {
                using (var redisClient = OpenServer())
                {
                    var obj = redisClient.Get<T>(_APPID + "_" + key);
                    redisClient.Dispose();
                    GC.SuppressFinalize(redisClient);
                    return obj;
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 信息存入列表集中式缓存(基本类型)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetListCatch(string key, string value)
        {
            try
            {
                using (var redisClient = OpenServer())
                {
                    redisClient.AddItemToList(_APPID + "_" + key, value);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 设置列表缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public static void SetSortedListValue<TK, TV>(string key, SortedList<TK, TV> values, TimeSpan time)
        {
            try
            {
                //object序列化方式存储
                var ser = new ObjectSerializer();    //位于namespace ServiceStack.Redis.Support;
                using (var redisClient = OpenServer())
                {
                    redisClient.Set<byte[]>(_APPID + "_" + key, ser.Serialize(values), time);
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 获取列表缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static SortedList<TK, TV> GetSortedListValue<TK, TV>(string key)
        {
            try
            {
                //object序列化方式存储
                var ser = new ObjectSerializer();    //位于namespace ServiceStack.Redis.Support;
                using (var redisClient = OpenServer())
                {
                    return ser.Deserialize(redisClient.Get<byte[]>(_APPID + "_" + key)) as SortedList<TK, TV>;
                }
            }
            catch (Exception ex)
            {
                return new SortedList<TK, TV>();
            }
        }

        #region -- Queue --
        /// <summary>
        /// 将消息插入到队列中
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="message"></param>
        public static void Push(string queueName, string message)
        {
            try
            {
                using (IRedisClient redis = OpenServer())
                {
                    redis.PushItemToList(queueName, message);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 从队列中取出一条消息
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public static string Pop(string queueName)
        {
            try
            {
                using (IRedisClient redis = OpenServer())
                {
                    return redis.PopItemFromList(queueName);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 阻塞队列直到取出一条消息
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public static string PopBlocking(string queueName)
        {
            try
            {
                using (IRedisClient redis = OpenServer())
                {
                    return redis.BlockingPopItemFromList(queueName, TimeSpan.FromDays(1));
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}