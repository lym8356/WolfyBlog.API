using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace WolfyBlog.API.Helper
{
	public static class IEnumerableExtensions
	{
		public static IEnumerable<ExpandoObject> ShapeData<TSource>(
			this IEnumerable<TSource> source,
			string fields)
		{
			if (source == null)
			{
				throw new ArgumentException(nameof(source));
			}

			var expandoObjectList = new List<ExpandoObject>();

			var propertyInfoList = new List<PropertyInfo>();

			if (string.IsNullOrWhiteSpace(fields))
			{
				// no filter fields, return all properties
				var propertyInfos = typeof(TSource)
					.GetProperties(BindingFlags.IgnoreCase
					| BindingFlags.Public | BindingFlags.Instance);
				propertyInfoList.AddRange(propertyInfos);
			} else
			{
				// use comma to separate string
				var fieldsAfterSplit = fields.Split(',');

				foreach(var field in fieldsAfterSplit)
				{
					var propertyName = field.Trim();

					var propertyInfo = typeof(TSource)
						.GetProperty(propertyName, BindingFlags.IgnoreCase
						| BindingFlags.Public | BindingFlags.Instance);

					if (propertyInfo == null)
					{
						throw new Exception($"Property {propertyName} does not exist" + $"{typeof(TSource)}");
					}
					propertyInfoList.Add(propertyInfo);
				}
			}

			foreach(TSource sourceObject in source)
			{
				// create shapedObjectList
				var dataShapedObject = new ExpandoObject();

				foreach (var propertyInfo in propertyInfoList)
				{
					var propertyValue = propertyInfo.GetValue(sourceObject);

					((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
				}

				expandoObjectList.Add(dataShapedObject);
			}

			return expandoObjectList;
		}
	}
}

