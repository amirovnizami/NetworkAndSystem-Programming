﻿using System;
using System.Collections.Generic;

namespace HttpServer.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? Age { get; set; }

    public DateTime? RegistrationDate { get; set; }
}
