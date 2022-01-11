using System.Collections.Generic;
using System.Dynamic;

namespace CSVApplication
{
    public class DynamicData : DynamicObject
    {
        /// <summary>
        /// Holds list of Property and it's value
        /// </summary>
        public Dictionary<string, object> dynamicProperties;

        /// <summary>
        /// Construction
        /// </summary>
        public DynamicData()
        {
            dynamicProperties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Overrides this member for runtime binding of member property
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string memberName = binder.Name.ToLower();
            return dynamicProperties.TryGetValue(memberName, out result);
        }

        /// <summary>
        /// Overrides this memver for runtime assignment of member Property
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            string memberName = binder.Name.ToLower();
            dynamicProperties[memberName] = value;
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return dynamicProperties.Keys;
        }

        /// <summary>
        /// Add Property to object.  i.e., Read from CSV File, to add Header and value
        /// as property and value of object
        /// </summary>
        /// <param name="propName">Property Name</param>
        /// <param name="propValue">Property Value</param>
        public void AddProperty(string propName, object propValue)
        {
            dynamicProperties[propName.ToLower()] = propValue;
        }

        /// <summary>
        /// Gets Property value
        /// </summary>
        /// <param name="propName">Name of Property</param>
        /// <returns></returns>
        public object GetPropertyValue(string propName)
        {
            return dynamicProperties[propName.ToLower()];
        }
    }
}
