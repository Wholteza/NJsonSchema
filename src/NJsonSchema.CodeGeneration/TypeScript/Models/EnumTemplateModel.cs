//-----------------------------------------------------------------------
// <copyright file="EnumTemplateModel.cs" company="NJsonSchema">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/rsuter/NJsonSchema/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace NJsonSchema.CodeGeneration.TypeScript.Models
{
    /// <summary>The TypeScript enum template model.</summary>
    public class EnumTemplateModel
    {
        private readonly JsonSchema4 _schema;

        /// <summary>Initializes a new instance of the <see cref="EnumTemplateModel"/> class.</summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="schema">The schema.</param>
        public EnumTemplateModel(string typeName, JsonSchema4 schema)
        {
            _schema = schema; 
            Name = typeName;
        }

        /// <summary>Gets the name of the enum.</summary>
        public string Name { get; }

        /// <summary>Gets the enum values.</summary>
        public List<EnumerationEntry> Enums => GetEnumeration(_schema);

        /// <summary>Gets a value indicating whether the enum has description.</summary>
        public bool HasDescription => !(_schema is JsonProperty) && !string.IsNullOrEmpty(_schema.Description);

        /// <summary>Gets the description.</summary>
        public string Description => ConversionUtilities.RemoveLineBreaks(_schema.Description);

        private List<EnumerationEntry> GetEnumeration(JsonSchema4 schema)
        {
            var entries = new List<EnumerationEntry>();
            for (int i = 0; i < schema.Enumeration.Count; i++)
            {
                var value = schema.Enumeration.ElementAt(i);
                var name = schema.EnumerationNames.Count > i ?
                    schema.EnumerationNames.ElementAt(i) :
                    schema.Type == JsonObjectType.Integer ? "_" + value : value.ToString();

                entries.Add(new EnumerationEntry
                {
                    Value = schema.Type == JsonObjectType.Integer ? value.ToString() : "<any>\"" + value + "\"",
                    Name = ConversionUtilities.ConvertToUpperCamelCase(name, true)
                });
            }
            return entries;
        }
    }
}