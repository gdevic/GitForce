using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace GitForce
{
    /// <summary>
    /// Class describing a custom tool.
    /// </summary>
    public class ClassTool : ICloneable
    {
        public string Name;
        public string Cmd;
        public string Args;
        public string Dir;
        public string Desc;
        public readonly bool[] Checks = new bool[7];

        /// <summary>
        /// Implements the clonable interface.
        /// </summary>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    /// <summary>
    /// Class describing our set of custom tools
    /// </summary>
    public class ClassCustomTools
    {
        /// <summary>
        /// List of tools
        /// </summary>
        public readonly List<ClassTool> Tools = new List<ClassTool>();

        /// <summary>
        /// Load a set of tools from a given file into the current tool-set.
        /// Returns a new class structure containing all the tools.
        /// If error loading, ClassUtils.LastError will be set.
        /// </summary>
        public static ClassCustomTools Load(string name)
        {
            ClassUtils.ClearLastError();
            ClassCustomTools ct = new ClassCustomTools();
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(ClassCustomTools));
                using (TextReader textReader = new StreamReader(name))
                {
                    ct = (ClassCustomTools)deserializer.Deserialize(textReader);                    
                }
            }
            catch (Exception ex)
            {
                ClassUtils.LastError = ex.Message;
            }
            return ct;
        }

        /// <summary>
        /// Save current set of tools to a given file.
        /// Returns true if save successful.
        /// If save failed, the error will be set with ClassUtils.LastError
        /// </summary>
        public bool Save(string name)
        {
            ClassUtils.ClearLastError();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ClassCustomTools));
                using (TextWriter textWriter = new StreamWriter(name))
                {
                    serializer.Serialize(textWriter, this);
                }
            }
            catch (Exception ex)
            {
                ClassUtils.LastError = ex.Message;
            }
            return !ClassUtils.IsLastError();
        }

        /// <summary>
        /// Implements a deep copy of the whole class.
        /// </summary>
        public ClassCustomTools Copy()
        {
            ClassCustomTools ct = new ClassCustomTools();
            foreach (var classTool in Tools)
                ct.Tools.Add((ClassTool)classTool.Clone());
            return ct;
        }
    }
}
