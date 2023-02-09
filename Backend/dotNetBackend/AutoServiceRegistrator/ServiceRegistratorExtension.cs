using Microsoft.Extensions.DependencyInjection;

namespace AutoServiceRegistrator
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SingletonServiceAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TransientServiceAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ScopedServiceAttribute : Attribute
    {
    }

    public static class ServiceRegistratorExtension
    {
        public static IServiceCollection RegisterSingletons(
                     this IServiceCollection services)
        {
            var singletonClasses =
                (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                 from type in assembly.GetTypes()
                 let singletonAttribute = type.GetCustomAttributes(
                 typeof(SingletonServiceAttribute), false).FirstOrDefault()
                 where singletonAttribute != null
                 select type).ToList();

            //register all singleton classes
            foreach (var singletonClass in singletonClasses)
            {
                services.AddSingleton(singletonClass);
            }

            return services;
        }

        public static IServiceCollection RegisterTransient(
                     this IServiceCollection services)
        {
            var transientClasses =
                (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                 from type in assembly.GetTypes()
                 let transientAttribute = type.GetCustomAttributes(
                 typeof(TransientServiceAttribute), false).FirstOrDefault()
                 where transientAttribute != null
                 select type).ToList();

            //register all transient classes
            foreach (var transientClass in transientClasses)
            {
                services.AddTransient(transientClass);
            }

            return services;
        }

        public static IServiceCollection RegisterScoped(this IServiceCollection services)
        {
            var scopedClasses =
                (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                 from type in assembly.GetTypes()
                 let scopedAttribute = type.GetCustomAttributes(
                     typeof(ScopedServiceAttribute), false).FirstOrDefault()
                 where scopedAttribute != null
                 select type).ToList();

            // register all scoped classes
            foreach (var scopedClass in scopedClasses)
            {
                services.AddScoped(scopedClass);
            }

            return services;
        }

    }
}