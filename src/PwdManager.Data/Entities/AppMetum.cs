using System;
using System.Collections.Generic;

namespace PwdManager.Data.Entities;

public partial class AppMetum
{
    public string MetaKey { get; set; } = null!;

    public byte[] MetaValue { get; set; } = null!;
}
