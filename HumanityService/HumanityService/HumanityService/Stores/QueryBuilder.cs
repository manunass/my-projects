using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HumanityService.Stores
{
    public class QueryBuilder
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public QueryBuilder SelectColumns(string tableName, params string[] columns)
        {
            _stringBuilder.Append($"SELECT {CommaSeparated(columns)} FROM {tableName}");
            return this;
        }

        private static string CommaSeparated<T>(IEnumerable<T> columns)
        {
            return string.Join(',', columns);
        }

        public QueryBuilder Where(string condition)
        {
            _stringBuilder.Append($" WHERE {condition}");
            return this;
        }

        public QueryBuilder Where()
        {
            _stringBuilder.Append($" WHERE ");
            return this;
        }

        public QueryBuilder OrderBy(string columnName, string direction)
        {
            _stringBuilder.Append($" ORDER BY {columnName} {direction}");
            return this;
        }

        public QueryBuilder Offset(int offset)
        {
            _stringBuilder.Append($" OFFSET {offset} ROWS");
            return this;
        }

        public QueryBuilder FetchNext(int limit)
        {
            _stringBuilder.Append($" FETCH NEXT {limit} ROWS ONLY");
            return this;
        }

        public QueryBuilder Limit(long offset, int limit)
        {
            _stringBuilder.Append($" LIMIT {offset},{limit}");
            return this;
        }

        public QueryBuilder Limit(int limit)
        {
            _stringBuilder.Append($" LIMIT {limit}");
            return this;
        }

        public QueryBuilder DeleteFrom(string tableName)
        {
            _stringBuilder.Append($"DELETE FROM {tableName}");
            return this;
        }

        public QueryBuilder InsertInto(string tableName, params string[] columns)
        {
            _stringBuilder.Append(
                $@"INSERT INTO {tableName} ({CommaSeparated(columns)}) VALUES ({CommaSeparated(columns.Select(c => $"@{c}"))}); 
SELECT SCOPE_IDENTITY()");
            return this;
        }

        public QueryBuilder Update(string tableName, params string[] columns)
        {
            _stringBuilder.Append($"UPDATE {tableName} SET {CommaSeparated(columns.Select(column => $"{column} = @{column}"))}");
            return this;
        }

        public QueryBuilder Sum(string tableName, string columnName)
        {
            _stringBuilder.Append($"SELECT SUM({columnName}) FROM {tableName}");
            return this;
        }

        public QueryBuilder And(string condition = "")
        {
            _stringBuilder.Append($" AND {condition}");
            return this;
        }

        public QueryBuilder ColumnValueIn<T>(string columnName, IEnumerable<T> values)
        {
            _stringBuilder.Append(InStatement(columnName, values));
            return this;
        }

        public QueryBuilder Count(string tableName)
        {
            _stringBuilder.Append($"SELECT COUNT(*) FROM {tableName}");
            return this;
        }

        public static string InStatement<T>(string columnName, IEnumerable<T> values)
        {
            return $"{columnName} IN({CommaSeparated(values)})";
        }

        public string Build()
        {
            return _stringBuilder.ToString();
        }
    }
}
