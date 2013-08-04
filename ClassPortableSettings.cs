using System;
using System.Collections;
using System.Configuration;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Xml;
using System.IO;

namespace GitForce
{
    /// <summary>
    /// This class implements portable settings as shared by the project at:
    /// https://github.com/crdx/PortableSettingsProvider
    /// 
    /// Changes have been made to make the location of setting file consistent
    /// and in user space so that we dont lose settings when the application
    /// executable is moved around.
    /// </summary>
    public sealed class PortableSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        private const string rootNodeName = "settings";
        private const string localSettingsNodeName = "localSettings";
        private const string globalSettingsNodeName = "globalSettings";
        private const string className = "PortableSettingsProvider";
        private XmlDocument xmlDocument;

        private string settingsFilePath
        {
            get
            {
                // TODO: Create option to override my changes below and make it truly portable by storing settings with the application
                // This is the original code - it creates portable settings
                //return Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), string.Format("{0}.settings", ApplicationName));

                // Create application settings in the user's home directory:
                // This is very much dependent on the platform; by default, use the application directory
                string portableSettings = Path.GetDirectoryName(Application.ExecutablePath);
                if (ClassUtils.IsMono())
                {
                    string home = Environment.GetEnvironmentVariable("HOME");
                    if (!string.IsNullOrEmpty(home))
                        portableSettings = Path.Combine(home, ".config/GitForce");
                }
                else
                    portableSettings = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GitForce");

                return Path.Combine(portableSettings, "GitForce.settings.xml");
            }
        }

        private XmlNode LocalSettingsNode
        {
            get
            {
                XmlNode settingsNode = GetSettingsNode(localSettingsNodeName);
                XmlNode machineNode = settingsNode.SelectSingleNode(Environment.MachineName.ToLowerInvariant());

                if (machineNode == null)
                {
                    machineNode = RootDocument.CreateElement(Environment.MachineName.ToLowerInvariant());
                    settingsNode.AppendChild(machineNode);
                }

                return machineNode;
            }
        }

        private XmlNode GlobalSettingsNode
        {
            get { return GetSettingsNode(globalSettingsNodeName); }
        }

        private XmlNode RootNode
        {
            get { return RootDocument.SelectSingleNode(rootNodeName); }
        }

        private XmlDocument RootDocument
        {
            get
            {
                if (xmlDocument == null)
                {
                    try
                    {
                        xmlDocument = new XmlDocument();
                        xmlDocument.Load(settingsFilePath);
                    }
                    catch (Exception)
                    {

                    }

                    if (xmlDocument.SelectSingleNode(rootNodeName) != null)
                        return xmlDocument;

                    xmlDocument = GetBlankXmlDocument();
                }

                return xmlDocument;
            }
        }

        public override string ApplicationName
        {
            get { return Path.GetFileNameWithoutExtension(Application.ExecutablePath); }
            set { }
        }

        public override string Name
        {
            get { return className; }
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(Name, config);
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            foreach (SettingsPropertyValue propertyValue in collection)
                SetValue(propertyValue);

            try
            {
                RootDocument.Save(settingsFilePath);
            }
            catch (Exception)
            {
                /* 
                 * If this is a portable application and the device has been 
                 * removed then this will fail, so don't do anything. It's 
                 * probably better for the application to stop saving settings 
                 * rather than just crashing outright. Probably.
                 */
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

            foreach (SettingsProperty property in collection)
            {
                values.Add(new SettingsPropertyValue(property)
                {
                    SerializedValue = GetValue(property)
                });
            }

            return values;
        }

        private void SetValue(SettingsPropertyValue propertyValue)
        {
            XmlNode targetNode = IsGlobal(propertyValue.Property)
               ? GlobalSettingsNode
               : LocalSettingsNode;

            XmlNode settingNode = targetNode.SelectSingleNode(string.Format("setting[@name='{0}']", propertyValue.Name));

            if (settingNode != null)
                settingNode.InnerText = propertyValue.SerializedValue.ToString();
            else
            {
                settingNode = RootDocument.CreateElement("setting");

                XmlAttribute nameAttribute = RootDocument.CreateAttribute("name");
                nameAttribute.Value = propertyValue.Name;

                settingNode.Attributes.Append(nameAttribute);
                settingNode.InnerText = propertyValue.SerializedValue.ToString();

                targetNode.AppendChild(settingNode);
            }
        }

        private string GetValue(SettingsProperty property)
        {
            XmlNode targetNode = IsGlobal(property) ? GlobalSettingsNode : LocalSettingsNode;
            XmlNode settingNode = targetNode.SelectSingleNode(string.Format("setting[@name='{0}']", property.Name));

            if (settingNode == null)
                return property.DefaultValue != null ? property.DefaultValue.ToString() : string.Empty;

            return settingNode.InnerText;
        }

        private bool IsGlobal(SettingsProperty property)
        {
            foreach (DictionaryEntry attribute in property.Attributes)
            {
                if ((Attribute)attribute.Value is SettingsManageabilityAttribute)
                    return true;
            }

            return false;
        }

        private XmlNode GetSettingsNode(string name)
        {
            XmlNode settingsNode = RootNode.SelectSingleNode(name);

            if (settingsNode == null)
            {
                settingsNode = RootDocument.CreateElement(name);
                RootNode.AppendChild(settingsNode);
            }

            return settingsNode;
        }

        public XmlDocument GetBlankXmlDocument()
        {
            XmlDocument blankXmlDocument = new XmlDocument();
            blankXmlDocument.AppendChild(blankXmlDocument.CreateXmlDeclaration("1.0", "utf-8", string.Empty));
            blankXmlDocument.AppendChild(blankXmlDocument.CreateElement(rootNodeName));

            return blankXmlDocument;
        }

        public void Reset(SettingsContext context)
        {
            LocalSettingsNode.RemoveAll();
            GlobalSettingsNode.RemoveAll();

            xmlDocument.Save(settingsFilePath);
        }

        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            // do nothing
            return new SettingsPropertyValue(property);
        }

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
        }
    }
}
