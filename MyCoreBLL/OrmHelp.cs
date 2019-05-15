using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using MyCoreBLL.FieldFile;
using MyCoreDAL;

namespace MyCoreBLL
{
    public class OrmHelp
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _http;

        public OrmHelp(DataContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        public IQueryable SelectDynamic<T>(IQueryable<T> queryable, FiltrateField value) where T : class
        {
            List<string> list = new List<string>();
            if (value.Fields != null)
            {
                var strArr = value.Fields.Split(',');
                for (int i = 0; i < strArr.Length; i++)
                {
                    list.Add(strArr[i]);
                }
            }
            else
            {
                foreach (var current in queryable)
                {
                    var properties = current.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        list.Add(property.Name);
                    }
                    break;
                }
            }
            //字典存储字段名称及属性
            Dictionary<string, PropertyInfo> sourceProperties = list.ToDictionary(name => name, name => queryable.ElementType.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance), StringComparer.OrdinalIgnoreCase);
            Type dynamicType = LinqRuntimeTypeBuilder.GetDynamicType(sourceProperties.Values);
            ParameterExpression sourceItem = Expression.Parameter(queryable.ElementType, "s");
            IEnumerable<MemberBinding> bindings = dynamicType.GetFields().Select(s => Expression.Bind(s, Expression.Property(sourceItem, sourceProperties[s.Name]))).OfType<MemberBinding>();

            Expression selector = Expression.Lambda(Expression.MemberInit(
                Expression.New(dynamicType.GetConstructor(Type.EmptyTypes)),
                bindings),
                sourceItem
                );

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string strSort = textInfo.ToTitleCase(value.Sort);

            Expression left = Expression.Property(sourceItem, queryable.ElementType.GetProperty("IsDelete"));
            Expression right = Expression.Constant(true, typeof(bool?));
            Expression expressionOne = Expression.NotEqual(left, right);
            Expression expression = expressionOne;

            if (value.Where != null)
            {
                List<Expression> pairs = new List<Expression>();
                Expression expressionTwo = expression;
                string strField = Regex.Replace(textInfo.ToTitleCase(value.Where), @"\(|\)", "");
                if (strField.ToLower().Contains("~or") || strField.ToLower().Contains("~and"))
                {
                    var strNotJoint = Regex.Replace(strField, @"~or|~and", "~", RegexOptions.IgnoreCase);
                    var strFieldArrs = strNotJoint.Split('~');
                    for (int i = 0; i < strFieldArrs.Length; i++)
                    {
                        var strFieldInfo = strFieldArrs[i].Split(',');
                        var typeFieldInfo = queryable.ElementType.GetProperty(strFieldInfo[0], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        left = Expression.Property(sourceItem, typeFieldInfo);
                        var strType = typeFieldInfo.PropertyType;
                        right = Expression.Constant(FieldRuntimeTypeBuilder.ChangeType(strFieldInfo[2].ToLower(), strType), strType);
                        switch (strFieldInfo[1].ToLower())
                        {
                            case "is":
                                pairs.Add(Expression.Equal(left, right));
                                break;
                            case "eq":
                                pairs.Add(Expression.Equal(left, right));
                                break;
                            case "ne":
                                pairs.Add(Expression.NotEqual(left, right));
                                break;
                            case "gt":
                                pairs.Add(Expression.GreaterThan(left, right));
                                break;
                            case "gte":
                                pairs.Add(Expression.GreaterThanOrEqual(left, right));
                                break;
                            case "lt":
                                pairs.Add(Expression.LessThan(left, right));
                                break;
                            case "lte":
                                pairs.Add(Expression.LessThanOrEqual(left, right));
                                break;
                            case "bw":
                                pairs.Add(Expression.Equal(left, right));
                                break;
                        }
                    }
                    var expressResult = pairs.Skip(1).Aggregate(pairs[0], Expression.And);

                    expression = Expression.And(expressionTwo, expressResult);
                }
                else
                {
                    var strFieldInfo = strField.Split(',');
                    var typeFieldInfo = queryable.ElementType.GetProperty(strFieldInfo[0], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    left = Expression.Property(sourceItem, typeFieldInfo);
                    var strType = typeFieldInfo.PropertyType;
                    right = Expression.Constant(FieldRuntimeTypeBuilder.ChangeType(strFieldInfo[2].ToLower(), strType), strType);
                    switch (strFieldInfo[1].ToLower())
                    {
                        case "is":
                            expression = Expression.Equal(left, right);
                            break;
                        case "eq":
                            expression = Expression.Equal(left, right);
                            break;
                        case "ne":
                            expression = Expression.NotEqual(left, right);
                            break;
                        case "gt":
                            expression = Expression.GreaterThan(left, right);
                            break;
                        case "gte":
                            expression = Expression.GreaterThanOrEqual(left, right);
                            break;
                        case "lt":
                            expression = Expression.LessThan(left, right);
                            break;
                        case "lte":
                            expression = Expression.LessThanOrEqual(left, right);
                            break;
                        case "in":
                            for (int i = 2; i < strFieldInfo.Length; i++)
                            {
                                pairs.Add(Expression.Equal(left, Expression.Constant(FieldRuntimeTypeBuilder.ChangeType(strFieldInfo[i].ToLower(), strType), strType)));
                            }
                            var expressResult = pairs.Skip(1).Aggregate(pairs[0], Expression.Or);
                            expression = Expression.And(expressionTwo, expressResult);
                            break;
                    }
                }
            }

            Dictionary<string, object> valuePairs = new Dictionary<string, object>();
            if (value.Contain != null)
            {
                List<Expression> pairs = new List<Expression>();
                string strField = Regex.Replace(textInfo.ToTitleCase(value.Contain), @"\(|\)", "");
                var strFieldArrs = strField.Split('~');
                for (int i = 0; i < strFieldArrs.Length; i++)
                {
                    var strFieldInfo = strFieldArrs[i].Split(',');
                    var typeFieldInfo = queryable.ElementType.GetProperty(strFieldInfo[0], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    valuePairs.Add(strFieldInfo[0], strFieldInfo[1].ToLower());
                }
                var expressionContains = valuePairs.Select(propName => Expression.Call(
                    Expression.Call(
                        Expression.Coalesce(Expression.Property(sourceItem, propName.Key), Expression.Constant("", typeof(string))),
                        "ToLower",
                        Type.EmptyTypes),
                    "Contains",
                    Type.EmptyTypes,
                    Expression.Constant(propName.Value))).Cast<Expression>().ToList();

                var expressResult = expressionContains.Skip(1).Aggregate(expressionContains[0], Expression.And);

                expression = Expression.And(expression, expressResult);
            }

            var typeSort = queryable.ElementType.GetProperty(strSort, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            Expression propertyExpr = Expression.Property(sourceItem, typeSort);

            MethodCallExpression WhereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { queryable.ElementType },
                queryable.Expression,
                Expression.Lambda(expression, new ParameterExpression[] { sourceItem }));

            MethodCallExpression OrderByCallExpression = Expression.Call(
                typeof(Queryable),
                "OrderByDescending",
                new Type[] { queryable.ElementType, typeSort.PropertyType },
                WhereCallExpression,
                Expression.Lambda(propertyExpr, sourceItem)
                );

            var SelectCallExpression = Expression.Call(
                typeof(Queryable),
                "select",
                new Type[] { queryable.ElementType, dynamicType },
                OrderByCallExpression,
                Expression.Quote(selector)
                );

            var results = queryable.Provider.CreateQuery<T>(SelectCallExpression);
            int intCount = results.Count();
            if (value.Size > 0)
            {
                results = queryable.Provider.CreateQuery<T>(SelectCallExpression).Skip(value.P * value.Size).Take(value.Size);
                int intPage = intCount / value.Size;
                _http.HttpContext.Response.Headers.Add("x-data-page", intPage.ToString());
                _http.HttpContext.Response.Headers.Add("x-data-page-size", value.Size.ToString());
            }
            else
            {
                _http.HttpContext.Response.Headers.Add("x-data-page", "0");
                _http.HttpContext.Response.Headers.Add("x-data-page-size", intCount.ToString());
            }
            _http.HttpContext.Response.Headers.Add("x-data-total-count", intCount.ToString());
            return results;
        }
    }
}
