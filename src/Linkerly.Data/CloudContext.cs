#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Linkerly.Data;

public class CloudContext : DbContext
{
	private readonly ISaveChangesInterceptor _auditor;

	public CloudContext(DbContextOptions<CloudContext> options) : base(options)
	{
	}
}