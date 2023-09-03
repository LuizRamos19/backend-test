using System.Data;

namespace teste.Models
{
    public class DataBaseEntity
    {
        public bool ColumnExists(DataTable table, string columnName) => !string.IsNullOrEmpty(columnName) && table.Columns.Contains(columnName.ToLower());
    }
}
