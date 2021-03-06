using System;
using System.Linq.Expressions;

namespace Folke.Elm.Mapping
{
    /// <summary>
    /// Allows to manipulate a TypeMapping
    /// </summary>
    /// <typeparam name="T">The type to map</typeparam>
    public partial class FluentTypeMapping<T>
    {
        private readonly TypeMapping typeMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentTypeMapping{T}"/> class.
        /// </summary>
        /// <param name="typeMapping">The <see cref="TypeMapping"/> to configure.</param>
        public FluentTypeMapping(TypeMapping typeMapping)
        {
            this.typeMapping = typeMapping;
        }

        /// <summary>Chooses the table to map to.</summary>
        /// <param name="name">The table name</param>
        /// <param name="schema">The schema</param>
        public void ToTable(string name, string schema = null)
        {
            typeMapping.TableName = name;
            typeMapping.TableSchema = schema;
        }

        /// <summary>
        /// Defines the primary key
        /// </summary>
        /// <param name="expression">An expression that selects the primary key</param>
        public void HasKey(Expression<Func<T, object>> expression)
        {
            var propertyInfo = TableHelpers.GetExpressionPropertyInfo(expression);
            typeMapping.Key = typeMapping.Columns[propertyInfo.Name];
            typeMapping.Key.IsKey = true;
            if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType == typeof(long))
                typeMapping.Key.IsAutomatic = true;
        }

        /// <summary>
        /// Maps a property
        /// </summary>
        /// <param name="expression">The expression that returns the property to map</param>
        /// <returns>An helper to define the property mapping</returns>
        public FluentPropertyMapping<T> Property(Expression<Func<T, object>> expression)
        {
            var property = TableHelpers.GetExpressionPropertyInfo(expression);
            var propertyMapping = typeMapping.Columns[property.Name];
            return new FluentPropertyMapping<T>(propertyMapping);
        }
    }
}