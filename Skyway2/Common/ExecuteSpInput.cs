using System.Data;

namespace Skyway2.Common;

public class ExecuteSpInput
{
    public ExecuteSpInput(string name, SqlDbType dbType, object value, int size = 0)
    {
        Name = name;
        DbType = dbType;
        Value = value;
        Size = size;
    }

    public string Name { get; set; }
    public SqlDbType DbType { get; set; }
    public int Size { get; set; }
    public object Value { get; set; }
}