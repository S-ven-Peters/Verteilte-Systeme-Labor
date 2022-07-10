using TodoList;
using MySql.Data.MySqlClient;

MySqlConnection conn = null;
MySqlCommand selectOne = new MySqlCommand();
MySqlCommand deleteOne = new MySqlCommand();
MySqlCommand postOne = new MySqlCommand();
MySqlCommand putOne = new MySqlCommand();
try
{
    conn = new MySqlConnection("server=127.0.0.1;port=3306;uid=root;pwd=root;database=todos;CharSet=utf8");
    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
    conn.Open();

    selectOne.Connection = conn;
    selectOne.CommandText = "SELECT * FROM todos WHERE todo=@name";
    selectOne.Parameters.AddWithValue("@name", "todo1");
    selectOne.Prepare();

    deleteOne.Connection = conn;
    deleteOne.CommandText = "DELETE FROM todos WHERE todo=@name";
    deleteOne.Parameters.AddWithValue("@name", "todo1");
    deleteOne.Prepare();

    postOne.Connection = conn;
    postOne.CommandText = "INSERT INTO todos(todo, priority) VALUES(@name, @priority)";
    postOne.Parameters.AddWithValue("@name", "todo1");
    postOne.Parameters.AddWithValue("@priority", 0);
    postOne.Prepare();

    putOne.Connection = conn;
    putOne.CommandText = "UPDATE todos SET priority=@priority WHERE todo=@name";
    putOne.Parameters.AddWithValue("@name", "todo1");
    putOne.Parameters.AddWithValue("@priority", 0);
    putOne.Prepare();
}
catch (MySqlException ex)
{
    System.Diagnostics.Debug.WriteLine(ex);
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/todos/", () =>
{
    List<Dictionary<string, dynamic>> data = new List<Dictionary<string, dynamic>>();
    try
    {
        MySqlCommand selectAll = new MySqlCommand("SELECT * FROM todos", conn);

        MySqlDataReader reader = selectAll.ExecuteReader();
        while (reader.Read())
        {
            data.Add(new Dictionary<string, dynamic>() {
            { "todo", (string) reader["todo"] },
            { "priority", (int) reader["priority"] }
        });
        }
        reader.Close();
    }
    catch (MySqlException ex)
    {
        System.Diagnostics.Debug.WriteLine(ex);
        return null;
    }
    return data;
});

app.MapGet("/todos/{name}", (string name) =>
{
    try
    {
        selectOne.Parameters["@name"].Value = name;
        MySqlDataReader reader = selectOne.ExecuteReader();
        if (reader.Read())
        {
            Dictionary<string, dynamic> retVal = new Dictionary<string, dynamic>() {
                { "todo", (string) reader["todo"] },
                { "priority", (int) reader["priority"] }
            };
            reader.Close();
            return retVal;
        }
        else
        {
            reader.Close();
            return null;
        }
    }
    catch (MySqlException ex)
    {
        System.Diagnostics.Debug.WriteLine(ex);
        return null;
    }
});

app.MapPost("/todos/", (TodoItem todoItem) =>
{
    try
    {
        postOne.Parameters["@name"].Value = todoItem.Todo;
        postOne.Parameters["@priority"].Value = todoItem.Priority;
        postOne.ExecuteNonQuery();
    }
    catch (MySqlException ex)
    {
        System.Diagnostics.Debug.WriteLine(ex);
        return null;
    }
    return todoItem;
});

app.MapDelete("/todos/{name}", (string name) =>
{
    try
    {
        selectOne.Parameters["@name"].Value = name;
        MySqlDataReader reader = selectOne.ExecuteReader();
        if (reader.Read())
        {
            reader.Close();
            deleteOne.Parameters["@name"].Value = name;
            deleteOne.ExecuteNonQuery();
            return true;
        }
        else
        {
            reader.Close();
            return false;
        }
    }
    catch (MySqlException ex)
    {
        System.Diagnostics.Debug.WriteLine(ex);
        return false;
    }
});

app.MapPut("/todos/", (TodoItem todoItem) =>
{
    try
    {
        selectOne.Parameters["@name"].Value = todoItem.Todo;
        MySqlDataReader reader = selectOne.ExecuteReader();
        if (reader.Read())
        {
            reader.Close();
            putOne.Parameters["@name"].Value = todoItem.Todo;
            putOne.Parameters["@priority"].Value = todoItem.Priority;
            putOne.ExecuteNonQuery();
            return todoItem;
        }
        else
        {
            reader.Close();
            return null;
        }
    }
    catch (MySqlException ex)
    {
        System.Diagnostics.Debug.WriteLine(ex);
        return null;
    }
});

app.Run();
