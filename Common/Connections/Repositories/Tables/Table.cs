using System;

namespace CommonLibs.Connections.Repositories.Tables
{
    /// <summary>
    /// Represent table of data base
    /// </summary>
    public class Table : BaseTable
    {

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields">Enum of all fields in the table</param>
        /// <param name="table">Name of Table</param>
        public Table(Enum fields, string table)
        {
            Table = table;
            Fields = fields;
        }

        #endregion

        #region Main Methods

        /// <summary>
        /// Return names of all fields
        /// </summary>
        /// <returns></returns>
        public override string GetFields()
        {
            string names = "";
            foreach (var item in Enum.GetNames(Fields.GetType()))
            {
                names += item + ", ";
            }
            return names.Remove(names.Length - 2, 2); ;
        }

        public override string GetFieldsToUpdate()
        {
            string names = "";
            foreach (var item in Enum.GetNames(Fields.GetType()))
            {
                names += item + "=@" + item + ", ";
            }
            return names.Remove(names.Length - 2, 2); ;
        }

        /// <summary>
        /// Returns names of all fields with @ before them
        /// </summary>
        /// <returns></returns>
        public override string GetFieldsForQuery()
        {
            string names = "";
            foreach (var item in Enum.GetNames(Fields.GetType()))
            {
                names += "@" + item + ", ";
            }
            return names.Remove(names.Length - 2, 2);
        }

        /// <summary>
        /// Return name of table
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Table;
        }

        #endregion


    }
}
