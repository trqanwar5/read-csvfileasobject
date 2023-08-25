using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CSVApplication
{
    public class Linq2CSVInput 
    {
        # region Construction

        /// <summary>
        /// Construction, place holder for initializing values
        /// </summary>
        public Linq2CSVInput()
        {
        }
        #endregion // Construction

        /// <summary>
        /// Reads the file content and add that to dictionary object
        /// </summary>
        /// <param name="filePathParam">CSV file path</param>
        /// <returns></returns>
        private List<Dictionary<string, string>> GetFileContentAsDictionary(string filePathParam)
        {
            List<Dictionary<string, string>> listDyanamicProperties = new List<Dictionary<string, string>>();
            DelimitedFileReader fileReader = new DelimitedFileReader(filePathParam);
            string[] headersReadFromFile = fileReader.GetLineAsFields(0);
            for (int lineNumber = 1; lineNumber < fileReader.Length; lineNumber++)
            {
                Dictionary<string, string> rowItem = new Dictionary<string, string>();
                var splitArray = fileReader.GetLineAsFields(lineNumber);
                for (int index = 0; index < splitArray.Length; index++)
                {
                    rowItem.Add(headersReadFromFile[index], splitArray[index]);
                }
                listDyanamicProperties.Add(rowItem);
            }

            return listDyanamicProperties;
        }

        /// <summary>
        /// Reads file contents from CSV as per the columns with values matched in CSV
        /// </summary>
        /// <param name="filePathParam">CSV file path</param>
        /// <param name="keyValuePairs">CSV column name with values</param>
        /// <param name="operationType">operation type</param>
        /// <returns></returns>
        public List<dynamic> ReadFileAsObject(string filePathParam, Dictionary<string, string> keyValuePairs, OperationType operationType = OperationType.AND)
        {
            List<dynamic> listDynamicData = new List<dynamic>();

            List<Dictionary<string, string>> listDyanamicProperties = GetFileContentAsDictionary(filePathParam);
            
            var predicate = ExpressionBuilder.ConvertDictionaryToExpression(keyValuePairs);

            Expression<Func<Dictionary<string, string>, bool>> combinedPredicate = null;

            switch (operationType)
            {
                case OperationType.AND:
                    combinedPredicate = ExpressionBuilder.CombinePredicate(predicate, Expression.AndAlso);
                    break;
                case OperationType.OR:
                    combinedPredicate = ExpressionBuilder.CombinePredicate(predicate, Expression.OrElse);
                    break;
                default:
                    break;
            }

            var resultAsDictionary = listDyanamicProperties.AsQueryable().Where(combinedPredicate);


            foreach (var item in resultAsDictionary)
            {
                listDynamicData.Add(GetDynamicData(item));
            }

            return listDynamicData;
        }

        private static DynamicData GetDynamicData(Dictionary<string, string> result)
        {
            DynamicData dynamicData = new DynamicData();
            foreach (var item in result)
            {
                dynamicData.AddProperty(item.Key, item.Value);
            }
            return dynamicData;
        }
    }
}
