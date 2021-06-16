using System;
using System.Configuration;

namespace GenericDefs.DotNet
{
    public class ConfigManager
    {
        private static readonly int _defaultMaxDegreeOfParalellism = 3;
        public static int GetMaxDegreeOfParalellism()
        {
            string s = ConfigurationManager.AppSettings[ConfigKeys.MaxDegreeOfParallelism];
            if (!string.IsNullOrEmpty(s)) { return Convert.ToInt32(s); }
            else { return _defaultMaxDegreeOfParalellism; }
        }

        public static string GetValue(string key)
        {
            string s = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(s)) { throw new ArgumentException("Given key was not present in app.config. key : " + key); }
            return s;
        }

        public static Tuple<TaskType, string> GetCurrentTask()
        {
            var configuration = ConfigurationManager.GetSection("classNameConfigSection") as ClassNameConfigSection;
            TaskType t = EnumExtensions.ToEnum(configuration.Element.TaskType, TaskType.Brilliant);
            string className = string.Empty;
            if (t == TaskType.Brilliant) {
                BrilliantProblems p = EnumExtensions.ToEnum(configuration.Element.Problem, BrilliantProblems.Brilliant_QuickCalculation);
                className = p.ToClassName();
            } else if (t == TaskType.ProjectEuler) {
                ProjectEulerProblems p = EnumExtensions.ToEnum(configuration.Element.Problem, ProjectEulerProblems.ProjectEuler_QuickCalculation);
                className = p.ToClassName();
            }

            return new Tuple<TaskType, string>(t, className);
        }
    }

    public class ConfigKeys
    {
        public const string TaskType = "TaskType";
        public const string ClassName = "ClassName";
        public const string MaxDegreeOfParallelism = "MaxDegreeOfParallelism";
    }

    public sealed class ClassNameConfigSection : ConfigurationSection
    {
        private const string XmlNamespaceConfigurationPropertyName = "xmlns:xs";
        [ConfigurationProperty(XmlNamespaceConfigurationPropertyName, IsRequired = false)]
        public string XmlNamespace
        {
            get
            {
                return (string)this[XmlNamespaceConfigurationPropertyName];
            }
            set
            {
                this[XmlNamespaceConfigurationPropertyName] = value;
            }
        }

        [ConfigurationProperty("xmlns", IsRequired = false)]
        public string Xmlns
        {
            get
            {
                return this["xmlns"] != null ? this["xmlns"].ToString() : string.Empty;
            }
        }

        [ConfigurationProperty("taskTypeConfigElement", IsRequired = false)]
        public TaskTypeConfigElement Element
        {
            get
            {
                return (TaskTypeConfigElement)this["taskTypeConfigElement"];
            }
        }
    }

    public sealed class TaskTypeConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("taskType", DefaultValue = "Brilliant", IsRequired = true, IsKey = true)]
        public string TaskType
        {
            get
            {
                return (string)this["taskType"];
            }
        }

        [ConfigurationProperty("problem", DefaultValue = "Brilliant_QuickCalculation", IsRequired = true, IsKey = true)]
        public string Problem
        {
            get
            {
                return (string)this["problem"];
            }
        }
    }

    public enum TaskType
    {
        Brilliant,
        ProjectEuler
    }

    public enum SchemaEnumerationType
    {
        Brilliant,
        ProjectEuler,
        TaskType
    }
}