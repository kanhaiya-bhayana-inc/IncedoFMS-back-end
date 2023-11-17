using System.Collections.Specialized;

namespace FMS.Services.AzueFileUploadAPI.Helpers
{
    public class ExtractNameFromString
    {
        public static string GetNameAsync(string data, string key)
        {
                string result = "";
            try
            {
                int accountNameIndex = data.IndexOf($"{key}=");
                if (accountNameIndex != -1)
                {
                    // Find the starting position of the value
                    int valueStartIndex = accountNameIndex + "AccountName=".Length;

                    // Find the position of the next semicolon after the value
                    int valueEndIndex = data.IndexOf(';', valueStartIndex);

                    if (valueEndIndex == -1)
                    {
                        // If there's no semicolon, use the end of the string as the value's end
                        valueEndIndex = data.Length;
                    }

                    // Extract the value
                    string accountName = data.Substring(valueStartIndex, valueEndIndex - valueStartIndex);
                    result = accountName;

                }
            }
            catch (Exception ex)
            {
                result = ex.Message; 
                throw ex;
            }
            return result;



        }
    }
}
