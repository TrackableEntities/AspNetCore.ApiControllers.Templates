using Microsoft.EntityFrameworkCore;

namespace TemplatesSample.Models
{
    public partial class NorthwindSlimContext
    {
        public NorthwindSlimContext(DbContextOptions<NorthwindSlimContext> options) : base(options) { }
    }
}