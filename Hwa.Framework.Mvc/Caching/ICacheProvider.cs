using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace Hwa.Framework.Mvc.Caching
{
    /// <summary>
    /// 缓存接口 -- zhangh @ 2013/12/17
    /// </summary>
    public interface ICacheProvider
    {
        T Get<T>(string key);

        void Add<T>(string key, T data);

        void Add<T>(string key, T data, int cacheTime);

        void Set<T>(string key, T data);

        void Set<T>(string key, T data, int cacheTime);

        void Replace<T>(string key, T data);

        void Replace<T>(string key, T data, int cacheTime);

        IList<string> SearchKeys(string pattern);

        bool HasKey(string key);

        void Remove(string key);

        void RemoveAll();
    }
}
