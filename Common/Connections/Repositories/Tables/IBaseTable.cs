
using System;

namespace CommonLibs.Connections.Repositories.Tables
{
    /// <summary>
    /// Base class for DB Table 
    /// </summary>
    public abstract class BaseTable
    {

        public Enum Fields { get; protected set; }

        public string Table { get; protected set; }

        /// <summary>
        /// Rerutns names of all fields
        /// </summary>
        /// <returns></returns>
        public abstract string GetFields();

        /// <summary>
        /// Returns names of all fields with @ before them
        /// </summary>
        /// <returns></returns>
        public abstract string GetFieldsForQuery();
    }
}
