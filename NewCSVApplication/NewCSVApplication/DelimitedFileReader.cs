using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CSVApplication
{
    internal class DelimitedFileReader
    {
        string[] fileLines;
        string fileDelimiter;

        /// <summary>
        /// Constructs reading a file content
        /// </summary>
        /// <param name="filePath">Path and name of file</param>
        internal DelimitedFileReader(string filePath)
        {
            try
            {
                fileDelimiter = "\\s*,\\s*";
                fileLines = File.ReadAllLines(filePath, Encoding.Default);
            }
            catch (Exception ex)
            {
                string exceptionDetail = "Exception occured on input File: " + filePath;
            }
        }

        /// <summary>
        /// This method gets Line as Fields
        /// </summary>
        /// <param name="lineNumber">Line number as input</param>
        /// <returns></returns>
        internal string[] GetLineAsFields(int lineNumber)
        {
            return Regex.Split(fileLines[lineNumber], fileDelimiter);
        }

        /// <summary>
        /// Length of total lines read from a file
        /// </summary>
        internal int Length
        {
            get
            {
                return fileLines.Length;
            }
        }
    }
}
