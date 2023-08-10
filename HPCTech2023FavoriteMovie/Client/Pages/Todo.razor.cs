using HPCTech2023FavoriteMovie.Shared;

namespace HPCTech2023FavoriteMovie.Client.Pages;

public partial class Todo
{
    private List<TodoItem> todos = new List<TodoItem>();
    private string? newTodo = String.Empty;

    private void AddTodo()
    {
        if (!String.IsNullOrEmpty(newTodo))
        {
            todos.Add(new TodoItem { Title = newTodo, IsDone = false });
            newTodo = String.Empty;
        }
    }
}
