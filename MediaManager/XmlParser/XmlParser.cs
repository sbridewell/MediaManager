#region Copyright (C) Simon Bridewell
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 3
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

// You can read the full text of the GNU General Public License at:
// http://www.gnu.org/licenses/gpl.html

// See also the Wikipedia entry on the GNU GPL at:
// http://en.wikipedia.org/wiki/GNU_General_Public_License
#endregion

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using SDE.Utility;

namespace SDE.MediaManager
{
    /// <summary>
    /// Base class for classes to parse XML documents.
    /// </summary>
    public abstract class XmlParser
    {
        #region constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlParser"/> class.
        /// </summary>
        /// <param name="xml">String of XML to parse.</param>
        protected XmlParser(string xml)
        {
            using (TextReader reader = new StringReader(xml))
            {
                XmlDocument document = new XmlDocument();
                document.Load(reader);
                NodeToParse = document.SelectSingleNode("/");
                NamespaceManager = new XmlNamespaceManager(document.NameTable);
                foreach (XmlAttribute att in document.DocumentElement.Attributes)
                {
                    if (att.Name.StartsWith("xmlns:", StringComparison.Ordinal))
                    {
                        NamespaceManager.AddNamespace(att.Name.Replace("xmlns:", string.Empty), att.Value);
                    }
                }
            }

            ParseErrors = new Collection<string>();
            UnparsedContent = new MemoryStream();
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlParser"/> class.
        /// </summary>
        /// <param name="node">The <see cref="XmlNode"/> to parse.</param>
        /// <param name="manager">Used to resolve namespaces.</param>
        protected XmlParser(IXPathNavigable node, XmlNamespaceManager manager)
        {
            NodeToParse = (XmlNode)node;
            NamespaceManager = manager;
            ParseErrors = new Collection<string>();
            UnparsedContent = new MemoryStream();
        }
        #endregion
        
        #region properties
        /// <summary>
        /// Gets or sets a value indicating whether content from the feed which has been 
        /// parsed should be removed from the document.
        /// This can be used in development scenarios to identify content which the 
        /// parser is not consuming, and therefore contains information not being made
        /// available to the user.
        /// </summary>
        public bool RemoveParsedContent { get; set; }

        /// <summary>
        /// Gets an object for managing XML namespaces.
        /// </summary>
        protected XmlNamespaceManager NamespaceManager { get; private set; }
        
        /// <summary>
        /// Gets the document being parsed.
        /// </summary>
        protected IXPathNavigable NodeToParse { get; private set; }
        
        /// <summary>
        /// Gets any errors encountered whilst parsing the XML.
        /// </summary>
        protected Collection<string> ParseErrors { get; private set; }
        
        /// <summary>
        /// Gets any RSS data which hasn't been consumed by the parser.
        /// Only populated if the RemoveParsedContent property is set to true.
        /// </summary>
        protected MemoryStream UnparsedContent { get; private set; }
        #endregion
        
        #region GetUriFromChildElement
        /// <summary>
        /// Gets a Uri from the named child element of the supplied parent element.
        /// </summary>
        /// <param name="childElementName">Name of the child element.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <returns>The Uri, or null if the named element is not found or contains an invalid value.</returns>
        protected Uri GetUriFromChildElement(string childElementName, IXPathNavigable parentElement)
        {
            string uriString = GetStringFromChildElement(childElementName, parentElement);
            if (string.IsNullOrEmpty(uriString))
            {
                return null;
            }
            
            try
            {
                return new Uri(uriString);
            }
            catch (FormatException)
            {
                LogInvalidElementValue(typeof(Uri).Name, childElementName, uriString);
                return null;
            }
        }
        #endregion
        
        #region GetUriFromChildElementAttribute
        /// <summary>
        /// Gets a Uri from the named attribute of the named child element of the supplied parent element.
        /// </summary>
        /// <param name="childElementName">Name of the child element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <returns>The Uri, or null if the named element is not found or contains an invalid value.</returns>
        protected Uri GetUriFromChildElementAttribute(
            string childElementName, 
            string attributeName, 
            IXPathNavigable parentElement)
        {
            string uriString = GetStringFromChildElementAttribute(childElementName, attributeName, parentElement);
            if (string.IsNullOrEmpty(uriString))
            {
                return null;
            }
            
            try
            {
                return new Uri(uriString);
            }
            catch (FormatException)
            {
                LogInvalidAttributeValue(typeof(Uri).Name, childElementName, attributeName, uriString);
                return null;
            }
        }
        #endregion
        
        #region GetDateTimeFromChildElement
        /// <summary>
        /// Gets a DateTime value from the named child element of the supplied parent element.
        /// </summary>
        /// <param name="childElementName">Name of the child element.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <returns>
        /// The DateTime value, or null if the element is missing or 
        /// contains an invalid value.
        /// </returns>
        protected DateTime? GetDateTimeFromChildElement(
            string childElementName, 
            IXPathNavigable parentElement)
        {
            DateTime returnValue;
            string dateTimeString = GetStringFromChildElement(childElementName, parentElement);
            if (string.IsNullOrEmpty(dateTimeString))
            {
                return null;
            }
            
            bool success = DateTime.TryParse(dateTimeString, out returnValue);
            if (success)
            {
                return returnValue;
            }
            else
            {
                LogInvalidElementValue(typeof(DateTime).Name, childElementName, dateTimeString);
                return null;
            }
        }
        #endregion
        
        #region GetTimeSpanFromChildElement
        /// <summary>
        /// Gets a TimeSpan value from the named child element of the supplied parent element.
        /// </summary>
        /// <param name="childElementName">Name of the child element.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <returns>
        /// The TimeSpan value, or <see cref="TimeSpan.MinValue"/> if the element is missing or 
        /// contains an invalid value.
        /// </returns>
        protected TimeSpan GetTimeSpanFromChildElement(string childElementName, IXPathNavigable parentElement)
        {
            TimeSpan returnValue;
            string timeSpanString = GetStringFromChildElement(childElementName, parentElement);
            if (string.IsNullOrEmpty(timeSpanString))
            {
                return TimeSpan.MinValue;
            }
            
            // If the string is in mm:ss format, prepend zero hours
            if (Regex.IsMatch(timeSpanString, "^[0-9][0-9]:[0-9][0-9]$"))
            {
                timeSpanString = "0:" + timeSpanString;
            }
            
            bool success = TimeSpan.TryParse(timeSpanString, out returnValue);
            if (success)
            {
                return returnValue;
            }
            else
            {
                LogInvalidElementValue(typeof(TimeSpan).Name, childElementName, timeSpanString);
                return TimeSpan.MinValue;
            }
        }
        #endregion
        
        #region GetBooleanFromChildElement
        /// <summary>
        /// Gets a boolean value from the named child element of the supplied parent element.
        /// </summary>
        /// <param name="childElementName">Name of the child element.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <returns>
        /// The boolean value, or false if the element is missing or contains a value other 
        /// than 'yes' or 'no'.
        /// </returns>
        protected bool GetBooleanFromChildElement(
            string childElementName, 
            IXPathNavigable parentElement)
        {
            bool returnValue;
            string booleanString = GetStringFromChildElement(childElementName, parentElement);
            if (string.IsNullOrEmpty(booleanString))
            {
                return false;
            }
            
            switch (booleanString)
            {
                case "yes":
                    returnValue = true;
                    break;
                    
                case "no":
                    returnValue = false;
                    break;
                    
                default:
                    LogInvalidElementValue(typeof(bool).Name, childElementName, booleanString);
                    return false;
            }
            
            return returnValue;
        }
        #endregion
        
        #region GetLongFromChildElementAttribute
        /// <summary>
        /// Gets a 64-bit integer from the named attribute of the named child element of 
        /// the supplied parent element.
        /// </summary>
        /// <param name="childElementName">Name of the child element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <returns>
        /// The integer value, or <see cref="long.MinValue"/> if the element or attribute 
        /// are missing or contain an invalid value.
        /// </returns>
        [SuppressMessage("Microsoft.Naming", "CA1720", Justification = "It returns a 64-bit integer")]
        protected long GetLongFromChildElementAttribute(
            string childElementName, 
            string attributeName, 
            IXPathNavigable parentElement)
        {
            long returnValue;
            string longString = GetStringFromChildElementAttribute(
                childElementName, 
                attributeName, 
                parentElement);
            if (string.IsNullOrEmpty(longString))
            {
                return long.MinValue;
            }
            
            bool success = long.TryParse(longString, out returnValue);
            if (success)
            {
                return returnValue;
            }
            else
            {
                LogInvalidAttributeValue(typeof(long).Name, childElementName, attributeName, longString);
                return long.MinValue;
            }
        }
        #endregion
        
        #region GetStringFromChildElement
        /// <summary>
        /// Gets a string value from the named child element of the supplied parent element.
        /// </summary>
        /// <param name="childElementName">Name of the child element.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <returns>
        /// The child element's inner text, or <see cref="string.Empty"/> if the child element is missing.
        /// </returns>
        protected string GetStringFromChildElement(string childElementName, IXPathNavigable parentElement)
        {
            Guard.NotNull("parentElement", parentElement);
            XmlNode parentNode = (XmlNode)parentElement;
            XmlNode childNode = parentNode.SelectSingleNode(childElementName, NamespaceManager);
            if (childNode == null)
            {
                return string.Empty;
            }
            
            string returnValue = childNode.InnerText;
            if (RemoveParsedContent)
            {
                parentNode.RemoveChild(childNode);
                SaveUnparsedContent();
            }
            
            return returnValue;
        }
        #endregion
        
        #region GetStringFromChildElementAttribute
        /// <summary>
        /// Gets a string value from the named attribute of the named child element of the 
        /// supplied parent element.
        /// </summary>
        /// <param name="childElementName">Name of the child element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="parentElement">The parent element.</param>
        /// <returns>
        /// The child element's inner text, or <see cref="string.Empty"/> if the child element is missing.
        /// </returns>
        protected string GetStringFromChildElementAttribute(
            string childElementName, 
            string attributeName, 
            IXPathNavigable parentElement)
        {
            Guard.NotNull("parentElement", parentElement);
            XmlNode parentNode = (XmlNode)parentElement;
            string attributeXPath = childElementName + "/@" + attributeName;
            XmlNode attribute = parentNode.SelectSingleNode(attributeXPath, NamespaceManager);
            if (attribute == null)
            {
                return string.Empty;
            }
            
            string returnValue = attribute.InnerText;
            if (RemoveParsedContent)
            {
                XmlNode childNode = parentNode.SelectSingleNode(childElementName, NamespaceManager);
                childNode.Attributes.RemoveNamedItem(attributeName);
                if (childNode.Attributes.Count == 0)
                {
                    parentNode.RemoveChild(childNode);
                }
                
                SaveUnparsedContent();
            }
            
            return returnValue;
        }
        #endregion
        
        #region protected SaveUnparsedContent method
        /// <summary>
        /// Saves any content not yet consumed by the parser to a memory stream.
        /// If we don't do this then consumed content isn't removed from the document.
        /// </summary>
        protected void SaveUnparsedContent()
        {
            UnparsedContent = new MemoryStream();
            XmlDocument xDoc = NodeToParse as XmlDocument;
            if (xDoc == null)
            {
                xDoc = new XmlDocument();
                xDoc.LoadXml(((XmlNode)NodeToParse).OuterXml);
                xDoc.Save(UnparsedContent);
            }
            else
            {
                xDoc.Save(UnparsedContent);
            }
        }
        #endregion
        
        #region private methods
        /// <summary>
        /// Records that the value of the element with the supplied name could not be
        /// converted to the supplied data type.
        /// </summary>
        /// <param name="dataType">Name of the data type.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="elementValue">Value of the element.</param>
        private void LogInvalidElementValue(string dataType, string elementName, string elementValue)
        {
            string error = "Invalid value for data type '" + dataType 
                + "'. Element name '" + elementName + "'. Element value '" + elementValue + "'.";
            ParseErrors.Add(error);
        }
        
        /// <summary>
        /// Records that the value of the attribute of the element with the supplied
        /// name could not be converted to the supplied data type.
        /// </summary>
        /// <param name="dataType">Name of the data type.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="elementValue">Value of the element.</param>
        private void LogInvalidAttributeValue(
            string dataType, 
            string elementName, 
            string attributeName, 
            string elementValue)
        {
            string error = "Invalid value for data type '" + dataType 
                + "'. Element name '" + elementName + "'. Attribute name '" + attributeName 
                + "'. Element value '" + elementValue + "'.";
            ParseErrors.Add(error);
        }
        #endregion
    }
}
