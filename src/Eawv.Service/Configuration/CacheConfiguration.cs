// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;

namespace Eawv.Service.Configuration;

[Serializable]
public class CacheConfiguration : Dictionary<string, TimeSpan>;
