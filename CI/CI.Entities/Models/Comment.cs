using System;
using System.Collections.Generic;

namespace CI.Entities.Models;

public  class Comment
{
    //public int rating;

    public long CommentId { get; set; }

    public long? UserId { get; set; }

    public long MissionId { get; set; }

    public string Comments { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Mission Mission { get; set; } = null!;

    public virtual User? User { get; set; }
    //public int Rating { get; set; }
}
