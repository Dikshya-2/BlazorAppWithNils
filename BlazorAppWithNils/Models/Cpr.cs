using System;
using System.Collections.Generic;

namespace BlazorAppWithNils.Models;

public partial class Cpr
{
    public int Cprid { get; set; }

    public string? User { get; set; }

    public string? CprNr { get; set; }

    public virtual ICollection<TodoList> TodoLists { get; set; } = new List<TodoList>();
}
