using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetCoreProject.Controllers.infrastructure
{
    public static class SessionExtension
    {

        
        public static void setJson(this ISession session , string key , object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T getJson<T>( this ISession session , string key)
        {
            var JsonContent = session.GetString(key);

            return JsonContent == null ? default(T) : JsonConvert.DeserializeObject<T>(JsonContent);

        }



    }
}
