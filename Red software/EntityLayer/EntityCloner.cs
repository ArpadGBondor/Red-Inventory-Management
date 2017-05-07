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
        struct DictionaryEntity
        {
            public Type           entityType;
            public PropertyInfo[] entityProperties;
        }

        private static List<DictionaryEntity> propertyDictionary = new List<DictionaryEntity>();

        public static PropertyInfo[] GetProperties(Type entityType)
        {
            PropertyInfo[] result = null;
            foreach (var DE in propertyDictionary.Where(p => p.entityType == entityType)) 
                result = DE.entityProperties;
            if (result == null)
            {
                result = entityType.GetProperties();
                propertyDictionary.Add(
                    new DictionaryEntity()
                    {
                        entityType = entityType,
                        entityProperties = result
                    });
            }
            return result;
        }

        public static void CloneProperties<Entity>( Entity from, Entity to)
        {
            var typeofentity = typeof(Entity);
            PropertyInfo[] properties = EntityCloner.GetProperties(typeof(Entity));
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(to, property.GetValue(from));
            }
        }
    }
}
