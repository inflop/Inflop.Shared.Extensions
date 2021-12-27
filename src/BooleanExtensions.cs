using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inflop.Shared.Extensions
{
	/// <summary>
	/// Extension methods for the <see cref="System.Boolean"/> data type.
	/// </summary>
	public static class BooleanExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <example>
		/// <code>
		///     { "1",          true  },
		///     { "T",          true  },
		///     { "TAK",        true  },
		///     { "TRUE",       true  },
		///     { "Y",          true  },
		///     { "YES",        true  },
		///     { "PRAWDA",     true  },
		///     { "P",          true  },
		///     { "0",          false },
		///     { "N",          false },
		///     { "NIE",        false },
		///     { "FALSE",      false },
		///     { "NO",         false },
		///     { "FAŁSZ",      false },
		///     { "FALSZ",      false },
		///		{ "F",          false }
		/// </code>
		/// </example>
		public static readonly Dictionary<string, bool> BOOLEAN_MAPPING = new Dictionary<string, bool>()
		{
				{ "1",          true  },
				{ "T",          true  },
				{ "TAK",        true  },
				{ "TRUE",       true  },
				{ "Y",          true  },
				{ "YES",        true  },
				{ "PRAWDA",     true  },
				{ "P",          true  },
				{ "0",          false },
				{ "N",          false },
				{ "NIE",        false },
				{ "FALSE",      false },
				{ "NO",         false },
				{ "FAŁSZ",      false },
				{ "FALSZ",      false },
				{ "F",          false }
		};
	}
}
