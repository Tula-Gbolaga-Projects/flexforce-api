namespace agency_portal_api.Configurations
{
    public static class Automapper
    {
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services) => services.AddAutoMapper(typeof(Program));
    }
}
