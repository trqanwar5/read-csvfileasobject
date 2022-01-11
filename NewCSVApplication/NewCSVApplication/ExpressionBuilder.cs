using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CSVApplication
{
    public static class ExpressionBuilder
    {
        /// <summary>
        /// Converts the dictionary to expresion
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static List<Expression<Func<Dictionary<string, string>, bool>>> ConvertDictionaryToExpression(Dictionary<string, string> keys)
        {
            List<Expression<Func<Dictionary<string, string>, bool>>> lambda = new List<Expression<Func<Dictionary<string, string>, bool>>>();
            foreach (var item in keys)
            {
                Expression<Func<Dictionary<string, string>, bool>> containsExpression = ConvertStringToExpression(item.Key, item.Value);
                lambda.Add(containsExpression);
            }
            return lambda;
        }

        /// <summary>
        /// Converts string to expression
        /// </summary>
        /// <param name="propName">Column name in CSV</param>
        /// <param name="propValue">value in CSV column</param>
        /// <returns></returns>
        public static Expression<Func<Dictionary<string, string>, bool>> ConvertStringToExpression(string propName, string propValue)
        {
            Expression<Func<Dictionary<string, string>, bool>> lambdaExpression;
            var parameter = Expression.Parameter(typeof(Dictionary<string, string>), "x");

            var property= typeof(Dictionary<string, string>).GetProperty("Item");

            var propertyValue = Expression.Constant(propValue);

            var arguments = new List<Expression> { Expression.Constant(propName) };
            var indexExpression = Expression.MakeIndex(parameter, property, arguments);
            var expression = Expression.MakeBinary(ExpressionType.Equal, indexExpression, propertyValue);
            lambdaExpression = Expression.Lambda<Func<Dictionary<string, string>, bool>>(expression, parameter);
            
            return lambdaExpression;
        }

        /// <summary>
        /// Combine the expression with logical operator 
        /// </summary>
        /// <param name="predicateExpressions"></param>
        /// <param name="logicalFunction"></param>
        /// <returns></returns>
        public static Expression<Func<Dictionary<string, string>, bool>> CombinePredicate(IList<Expression<Func<Dictionary<string, string>, bool>>> predicateExpressions, Func<Expression, Expression, BinaryExpression> logicalFunction)
        {
            Expression<Func<Dictionary<string, string>, bool>> filter = null;
            if (predicateExpressions.Count > 0)
            {
                var firstPredicate = predicateExpressions[0];
                Expression body = firstPredicate.Body;

                for (int i = 1; i < predicateExpressions.Count; i++)
                {
                    var ie = Expression.Invoke(predicateExpressions[i], firstPredicate.Parameters);
                    body = logicalFunction(body, ie);
                }
                filter = Expression.Lambda<Func<Dictionary<string, string>, bool>>(body, firstPredicate.Parameters);
            }
            return filter;
        }
    }
}
