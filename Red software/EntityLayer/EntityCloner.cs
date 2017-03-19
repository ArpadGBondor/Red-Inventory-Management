using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class EntityCloner
    {
        public static void CloneProperties<Entity>( Entity to, Entity from)
        {
            PropertyInfo[] properties = typeof(Entity).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(to, property.GetValue(from));
            }
        }
    }
}
