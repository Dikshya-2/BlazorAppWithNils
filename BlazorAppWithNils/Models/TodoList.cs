using System;
using System.Collections.Generic;

namespace BlazorAppWithNils.Models;

public partial class TodoList
{
    public int TodoId { get; set; }

    public int? UserId { get; set; }

    public string? Item { get; set; }

    public virtual Cpr? User { get; set; }
}
