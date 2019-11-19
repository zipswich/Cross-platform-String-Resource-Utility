using System;
using System.Diagnostics;
using Windows.Storage;
using System.Reflection;

namespace ClassLibraryDemo
{
    public static class Utility
    {
        /// <summary>
        /// Get a local setting value.  Set it to a default value if it does not exist
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="sKey">Setting key</param>
        /// <param name="DefaultValue">Setting default value</param>
        /// <returns></returns>
        public static T GetOrSetDefaultSettingValue<T>(string sKey, T DefaultValue)
        {
            T treturn = DefaultValue;
            try
            {
                ApplicationDataContainer localSettings = ApplicationData.Current?.LocalSettings;
                if (localSettings == null)
                {
                    Debug.WriteLine("ApplicationData.Current?.LocalSettings is null.");
                }
                else
                {
                    if (localSettings.Values.Keys.Contains(sKey))
                    {
                        try
                        {
                            if (typeof(T).GetTypeInfo().IsEnum)
                            {
                                string sValue = (string)localSettings.Values[sKey];
                                treturn = (T)Enum.Parse(typeof(T), sValue);
                            }
                            else if (typeof(T) == typeof(DateTime))
                            {
                                string sValue = (string)localSettings.Values[sKey];
                                treturn = (T)(object)DateTime.Parse(sValue);
                            }
                            else
                            {
                                treturn = (T)localSettings.Values[sKey];
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Exception in getting the value of setting "
                                + sKey + " Exception: " + ex.Message
                                + " Value type: " + localSettings.Values[sKey].GetType()
                                + " Return type: " + typeof(T));
                            treturn = DefaultValue;
                            if (typeof(T).GetTypeInfo().IsEnum)
                            {
                                localSettings.Values[sKey] = DefaultValue.ToString();
                            }
                            else
                            {
                                localSettings.Values[sKey] = DefaultValue;
                            }
                        }
                    }
                    else
                    {
                        treturn = DefaultValue;
                        if (typeof(T).GetTypeInfo().IsEnum)
                        {
                            localSettings.Values[sKey] = DefaultValue.ToString();
                        }
                        else
                        {
                            localSettings.Values[sKey] = DefaultValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
            return treturn;
        }


        /// <summary>
        /// Set a local setting 
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="sKey">Key that cannot contain "\"</param>
        /// <param name="Value">Value</param>
        public static void SetSettingValue<T>(string sKey, T Value)
        {
            try
            {
                var localSettings = ApplicationData.Current.LocalSettings;
                if (typeof(T).GetTypeInfo().IsEnum)
                {
                    localSettings.Values[sKey] = Value.ToString();
                }
                else if (typeof(T) == typeof(DateTime))
                {
                    localSettings.Values[sKey] = Value.ToString();
                }
                else
                {
                    localSettings.Values[sKey] = Value;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in SetSettingValue():" + ex.Message);
            }
        }


    }
}
