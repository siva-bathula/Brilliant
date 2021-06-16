using System;

namespace GenericDefs.DotNet
{
    public class ClassnameAttribute : Attribute
    {
        private string _className;

        /// <summary>Gets the description stored in this attribute.</summary>
        /// <returns>The description stored in this attribute.</returns>
        public virtual string ClassName { get { return ClassNameValue; } }

        /// <summary>Gets or sets the string stored as the description.</summary>
        /// <returns>The string stored as the description. The default value is an empty string ("").</returns>
        protected string ClassNameValue { get { return _className; } set { _className = value; } }
        public ClassnameAttribute() { }
        public ClassnameAttribute(string className) { _className = className; }
    }

    public class SearchKeyword : Attribute
    {
        private string _searchKeyword;        
        public SearchKeyword(string searchKeyword) { _searchKeyword = searchKeyword; }

        //////////////////////////////////////
        // Use this comments section to quickly check for existing attribute keywords.
        // Incomplete Solution to problem - ToDoIncomplete
        // Wrong solution - InCorrect
        //////////////////////////////////////
    }
}