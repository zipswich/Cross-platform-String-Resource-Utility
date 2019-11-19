using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Data;

namespace XResourceUtils
{

    public static class XRUtils
    {
        //Default string dictionary
        static Dictionary<string, string> dictStringsDefault = new Dictionary<string, string>();

        /// Dictionary for the selected language
        static Dictionary<string, string> dictStringsLanguage = new Dictionary<string, string>();

        /// Dictionary for the selected culture
        static Dictionary<string, string> dictStringsCulture = new Dictionary<string, string>();

        //Default vaule
        static string sDefaultValue = "N/A";

        /// <summary>
        /// Specify a language in the format of "xx" (e.g. "en") or xx-XX (e.g. "en-US") to override OS language. 
        /// </summary>
        public static string sLanguageCode = null;

        static XRUtils()
        {
            //Unfortunately, we have to use async methods
            //Assume reading two xml files is fairly quick
            //This hangs at  await sfolderInstallation.GetFileAsyn();
            //Therefore, InitializeAsync() has to be called by an app 
            //await InitializeAsync(); 
        }

        /// <summary>
        /// This method would hang if it were awaited 
        /// </summary>
        /// <returns></returns>


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="typeCalling"></param>
        /// <param name="bReset"></param>
        /// <param name="sLanguageCodeParam"></param>
        public static void Initialize(
            Type typeCalling,
            bool bReset = true,
            string sLanguageCodeParam = null)
        {
            try
            {
                sLanguageCode = sLanguageCodeParam;
                if (bReset)
                {
                    dictStringsDefault.Clear();
                    dictStringsCulture.Clear();
                    dictStringsLanguage.Clear();
                }
                else
                {
                    //do nothing.
                    //Keep strings already in the dictionaries
                }

                LoadStringResource("", dictStringsDefault, typeCalling);
                if (string.IsNullOrEmpty(sLanguageCode) || sLanguageCode == "default")
                {
                    string sCulture = Thread.CurrentThread.CurrentCulture.Name;
                    LoadStringResource(sCulture, dictStringsCulture, typeCalling);

                    string sLanguage = sCulture.Substring(0, 2);
                    LoadStringResource(sLanguage, dictStringsLanguage, typeCalling);
                }
                else
                {
                    LoadStringResource(sLanguageCode, dictStringsCulture, typeCalling);
                }
            }
            catch (Exception ex)
            {
                sDefaultValue = ex.Message;
            }
        }




        /// <summary>
        /// Load strings from string.xml corresponding to the specified language code
        /// </summary>
        /// <param name="sFolderName">Folder of string files</param>
        /// <param name="dictStrings">String dictionary</param>
        /// <returns></returns>
        private static void LoadStringResource(
            string sLanguageCulture,
            Dictionary<string, string> dictStrings,
            Type typeCalling)
        {
            try
            {
                string sFolderName = "values";
                if (string.IsNullOrEmpty(sLanguageCulture))
                {
                    //do nothing
                }
                else
                {
                    sFolderName += "-" + sLanguageCulture;
                }
                sDefaultValue = string.Empty;
                //The following initial code for Silverlight WP cannot be used for Uno X-platform projects
                //StorageFolder sfolderInstallation = Package.Current.InstalledLocation;
                //StorageFile sfStrings = await sfolderInstallation.GetFileAsync(@"Strings\" + sFolderName + @"\strings.xml");
                //using (Stream stream = File.OpenRead(sfStrings.Path))

                //Please note that "." instead of "\"
                string sPath = @"res." + sFolderName + @".strings.xml";
                using (Stream stream = GetStreamFromEmbeddedResourceFile(
                    sPath,
                    typeCalling))
                {
                    XDocument xd = XDocument.Load(stream);
                    foreach (XElement xe in xd.Root.Elements())
                    {
                        if (dictStrings.ContainsKey(xe.Attribute("name").Value))
                        {
                            //This is allowed.  For example, an app project's string can overwrite a library's strings
                            Debug.WriteLine("Duplicate: " + xe.Attribute("name").Value + " Path: " + sPath);
                        }
                        else
                        {
                            //do nothing
                        }
                        string sValue = xe.Value;
                        //Remove double-quotes
                        Match m = Regex.Match(sValue, "\"(?<value>.*)\"", RegexOptions.Singleline);
                        if (m.Success)
                        {
                            sValue = m.Groups["value"].Value;
                        }
                        dictStrings.Add(xe.Attribute("name").Value, sValue);
                    }

                    //Obsolete
                    //if (sDefaultValue.Length > 3)
                    //{
                    //    dictStrings.Clear();
                    //    sDefaultValue = "Please remove duplicate keys: " + sDefaultValue +
                    //        Environment.NewLine + "File: " + sPath;
                    //    Debug.WriteLine(sDefaultValue);
                    //}
                }
            }
            catch (FileNotFoundException)
            {
                //do nothing
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName">Resource file path</param>
        /// <param name="typeCalling"></param>
        /// <returns></returns>
        public static Stream GetStreamFromEmbeddedResourceFile(string resourceName, Type typeCalling)
        {
            try
            {
                Assembly assy = typeCalling?.Assembly;

                resourceName = resourceName.Replace("_", "-"); //Most likely redundant

                foreach (string sResourceName in assy?.GetManifestResourceNames())
                {
                    Console.WriteLine(sResourceName);  //for debugging
                    //At least UWP changes "-" to "_"
                    if (sResourceName.Replace("_", "-").ToUpperInvariant().Contains(resourceName.ToUpperInvariant()))
                    {
                        // resource found
                        return assy.GetManifestResourceStream(sResourceName);
                    }
                }
            }
            catch
            {
                //Do nothing
            }
            throw new Exception("Unable to find resource file:" + resourceName);
        }

        /// <summary>
        /// Retrieve a string
        /// </summary>
        /// <param name="sID">String ID</param>
        /// <returns></returns>
        public static string GetString(string sID)
        {
            string sReturn = sID;
            try
            {
                if (dictStringsCulture.ContainsKey(sID))
                {
                    sReturn = dictStringsCulture[sID];
                }
                else
                {
                    if (dictStringsLanguage.ContainsKey(sID))
                    {
                        sReturn = dictStringsLanguage[sID];
                    }
                    else
                    {
                        if (dictStringsDefault.ContainsKey(sID))
                        {
                            sReturn = dictStringsDefault[sID];
                        }
                        else
                        {
                            //do nothing;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sReturn = ex.Message;
            }
            return sReturn;
        }


        /// <summary>
        /// Obsolete method for Silverlight based apps
        /// Deal with that ApplicationBarMenuItem and ApplicationBarIconButton are unable to bind text
        /// </summary>
        /// <param name="ab">Action bar</param>
        //public static void InitializeApplicationBar(IApplicationBar ab)
        //{
        //    if (ab == null)
        //    {

        //    }
        //    else
        //    {
        //        foreach (ApplicationBarMenuItem mi in ab.MenuItems)
        //        {
        //            if (mi.Text == null)
        //            {
        //                //do nothing
        //            }
        //            else
        //            {
        //                mi.Text = XResourceUtils.GetString(mi.Text);
        //            }
        //        }

        //        foreach (ApplicationBarIconButton ib in ab.Buttons)
        //        {
        //            if (ib.Text == null)
        //            {
        //                //do nothing
        //            }
        //            else
        //            {
        //                ib.Text = XResourceUtils.GetString(ib.Text);
        //            }
        //        }
        //    }
        //}



    }

    public sealed class XRStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return XRUtils.GetString(parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
