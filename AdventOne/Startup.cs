using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Owin;

namespace AdventOne {
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        public void ConfigureServices(IServiceCollection services) {
            // Adds a default in-memory implementation of IDistributedCache  
            //services.AddCaching();
            services.AddSession();
            //// This Method may contain other code as well  
        }
        public void Configure(IApplicationBuilder app) {
            app.UseSession();
            //// This Method may contain other code as well  
        }
    }
}
