using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.NoSql.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionAttribute : Attribute
    {
        public string Name { get; private set; }

        public CollectionAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("blank collection name is not allowed", value);

            this.Name = value;
        }


        public static string GetCollectionName<T>()
        {
            var attribute = GetCustomAttribute<T>(typeof(CollectionAttribute));
            if (attribute == null)
                throw new NullReferenceException("CollectionName Attribute not specified.");
            return ((CollectionAttribute)attribute).Name;
        }

        public static string GetCollectionName(MemberInfo memberInfo)
        {
            var attribute = GetCustomAttribute(typeof(CollectionAttribute), memberInfo);
            if (attribute == null)
                throw new NullReferenceException("MongoCollectionName Attribute not specified");
            return ((CollectionAttribute)attribute).Name;
        }

        private static Attribute GetCustomAttribute<T>(Type attribute)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));
            return attribute.GetTypeInfo().GetCustomAttribute(typeof(T));
            //return Attribute.GetCustomAttribute(typeof(T), attribute);

        }

        private static Attribute GetCustomAttribute(Type attribute, MemberInfo memberInfo)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));
            return memberInfo.GetCustomAttribute(attribute);
            //return GetCustomAttribute(memberInfo, attribute);
        }

    }
}
