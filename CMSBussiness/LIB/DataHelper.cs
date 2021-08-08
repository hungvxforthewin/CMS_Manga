using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using Dapper;
using CRMModel.Models.Data;

namespace CRMBussiness.LIB
{
    public class DataHelper
    {
        public static List<string> GetPropertyOrColumnsAccess<T>(T dataObject, CrudFieldType usedFor)
        {
            Type type = typeof(T);
            PropertyInfo[] props = GetCachedProperties<T>();
            List<string> columns = new List<string>();

            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].PropertyType.Namespace.Equals("System.Collections.Generic"))
                    continue;
                if (!props[i].PropertyType.Namespace.Equals("System") && props[i].PropertyType.IsClass)
                    continue; //cancel class
                try
                {
                    var check = typeof(T).GetTypeInfo()
                                        .GetProperty(props[i].Name).GetCustomAttribute<CrudField>();
                    var tele = Attribute.IsDefined(props[i], typeof(CrudField));
                }
                catch (Exception ex)
                {

                    throw;
                }
                CrudField usedForAttr = Attribute.GetCustomAttribute(props[i], typeof(CrudField)) as CrudField;
                
                if (usedForAttr != null && ((usedForAttr.UsedFor & usedFor) == usedFor || usedForAttr.UsedFor == CrudFieldType.All))
                {
                    columns.Add(props[i].Name);
                }
                else if (usedForAttr == null) // Include the property is it has no CRUD Attribute.
                {
                    columns.Add(props[i].Name);
                }
            }
            return columns;
        }
        public static DynamicParameters GetSQLParametersFromPublicProperties<T>(T dataObject, CrudFieldType usedFor)
        {
            Type type = typeof(T);
            PropertyInfo[] props = GetCachedProperties<T>();
            DynamicParameters sqlParameter = new DynamicParameters();

            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].PropertyType.Namespace.Equals("System.Collections.Generic"))
                    continue;
                if (!props[i].PropertyType.Namespace.Equals("System") && props[i].PropertyType.IsClass)
                    continue; //cancel class

                CrudField usedForAttr = Attribute.GetCustomAttribute(props[i], typeof(CrudField)) as CrudField;
                if (usedForAttr != null && ((usedForAttr.UsedFor & usedFor) == usedFor || usedForAttr.UsedFor == CrudFieldType.All))
                {

                    var val = type.InvokeMember(props[i].Name, BindingFlags.GetProperty, null, dataObject, null);
                    sqlParameter.Add("@" + props[i].Name, val);
                }
                else if (usedForAttr == null) // Include the property is it has no CRUD Attribute.
                {
                    var val = type.InvokeMember(props[i].Name, BindingFlags.GetProperty, null, dataObject, null);
                    sqlParameter.Add("@" + props[i].Name, val);
                }

            }
            return sqlParameter;
        }
        private static ReaderWriterLockSlim propertiesCacheLock = new ReaderWriterLockSlim();
        private static IDictionary<string, PropertyInfo[]> propertiesCache = new Dictionary<string, PropertyInfo[]>();
        public static PropertyInfo[] GetCachedProperties<T>()
        {
            PropertyInfo[] props;
            if (propertiesCacheLock.TryEnterUpgradeableReadLock(100))
            {
                try
                {
                    if (!propertiesCache.TryGetValue(typeof(T).FullName, out props))
                    {
                        props = typeof(T).GetProperties();
                        if (propertiesCacheLock.TryEnterWriteLock(100))
                        {
                            try
                            {
                                propertiesCache.Add(typeof(T).FullName, props);
                            }
                            finally
                            {
                                propertiesCacheLock.ExitWriteLock();
                            }
                        }
                    }
                }
                finally
                {
                    propertiesCacheLock.ExitUpgradeableReadLock();
                }
                return props;
            }
            else
            {
                return typeof(T).GetProperties();
            }
        }
    }
}
//namespace ProFramework.Data.Attributes
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class FieldMapAttribute : System.Attribute
//    {
//        public string Column { get; set; }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="FieldMapAttribute"/> class.
//        /// </summary>
//        public FieldMapAttribute()
//        {
//        }

//        /// <summary>
//        /// Returns a <see cref="System.String" /> that represents this instance.
//        /// </summary>
//        /// <returns>
//        /// A <see cref="System.String" /> that represents this instance.
//        /// </returns>
//        public override string ToString()
//        {
//            return String.Format("");
//        }
//    }
//    [Flags]
//    public enum CrudFieldType
//    {
//        //Create = 1,
//        //Read = 2,
//        //Update = 4,
//        //Delete = 8,
//        //DontUse = 16,
//        //All = 32
//        Create = 0x1,
//        Read = 0x2,
//        Update = 0x4,
//        Delete = 0x8,
//        DontUse = 0x10,
//        All = 0x20
//    }
//    public class CrudFieldAttribute : Attribute
//    {
//        /// <summary>
//        /// Initializes a new instance of the <see cref="CrudField"/> class.
//        /// CrudField(false) : Create only
//        /// CrudField : Create and Update
//        /// </summary>
//        /// <param name="update">if set to <c>true</c> [update].</param>
//        /// <param name="create">if set to <c>true</c> [create].</param>
//        public CrudFieldAttribute(bool update = true, bool create = true)
//        {
//            if (create)
//                UsedFor = UsedFor | CrudFieldType.Create;
//            if (update)
//                UsedFor = UsedFor | CrudFieldType.Update;
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="CrudField"/> class.
//        /// CrudField(CrudFieldType.Create|CrudFieldType.Update)
//        /// </summary>
//        /// <param name="userFor">The user for.</param>
//        public CrudFieldAttribute(CrudFieldType userFor)
//        {
//            UsedFor = userFor;
//        }
//        public CrudFieldType UsedFor { get; set; }
//    }
//}

