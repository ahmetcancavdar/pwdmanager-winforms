using System;
using System.Collections.Generic;

namespace PwdManager.Infrastructure.Persistence.Entities;

public partial class AppMetum
{
    public string MetaKey { get; set; } = null!;

    public byte[] MetaValue { get; set; } = null!;
}
