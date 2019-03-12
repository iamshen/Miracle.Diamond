using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Map
{
    public abstract class Mapper<FObject, TObject> where FObject : class where TObject : class
    {
        private static IDictionary<Type, Type[]> g_relational = null;

        private static IDictionary<Type, PropertyInfo[]> g_persistent = null;

        public Mapper()
        {
            g_relational = new Dictionary<Type, Type[]>();
            g_persistent = new Dictionary<Type, PropertyInfo[]>();
        }

        public static void CreateMap()
        {
            Type[] types;
            Type f_type = typeof(FObject);
            Type t_type = typeof(TObject);

            if (g_relational.TryGetValue(f_type, out types))
            {
                PropertyInfo[] properties = null;
                foreach (Type type in types)
                {
                    if (g_persistent.TryGetValue(type, out properties) && type == t_type)
                        break;
                }

                if (properties == null)
                {
                    List<PropertyInfo> l_properties = new List<PropertyInfo>();
                    PropertyInfo[] f_properties = f_type.GetProperties();
                    PropertyInfo[] t_properties = t_type.GetProperties();

                    TObject obj = default(TObject);
                    foreach (PropertyInfo info in f_properties)
                    {

                    }
                }
            }
        }
    }
}
