using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiword.IOC2
{
    /// <summary>
    /// 定义一个服务工厂,Singleton注入
    /// 注意这个不是真正意义上的工厂，只是提供服务的存取
    /// </summary>
    public class SingletonFactory
    {
        // 定义一个字典,存储我们的接口服务和别名
        Dictionary<Type, Dictionary<string, object>> serviceDict;
        public SingletonFactory()
        {
            serviceDict = new Dictionary<Type, Dictionary<string, object>>();
        }

        /// <summary>
        /// 根据别名,获取接口实例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public TService GetService<TService>(string id) where TService : class
        {
            // 获取接口的类型
            var serviceType = typeof(TService);
            // out 方法,先获取某一个接口下的,<别名,实例>字典
            if (serviceDict.TryGetValue(serviceType, out Dictionary<string, object> implDict))
            {
                // 根据别名,获取接口的实例对象
                if (implDict.TryGetValue(id, out object service))
                {
                    // 强类型转换
                    return service as TService;
                }
            }
            return null;
        }

        /// <summary>
        /// 将实例和别名 匹配存储
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="service"></param>
        /// <param name="id"></param>
        public void AddService<TService>(TService service, string id) where TService : class
        {
            var serviceType = typeof(TService);
            // 对象实例判空
            if (service != null)
            {
                // 如果不存在,则填充
                if (serviceDict.TryGetValue(serviceType, out Dictionary<string, object> implDict))
                {
                    implDict[id] = service;
                }
                else
                {
                    implDict = new Dictionary<string, object>();
                    implDict[id] = service;
                    serviceDict[serviceType] = implDict;
                }
            }
        }
    }
}
