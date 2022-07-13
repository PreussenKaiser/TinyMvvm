using System.ComponentModel;

namespace TinyMvvm.Maui.Extensions;

/// <summary>
/// Contains extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class IServiceProviderExtensions
{
    /// <summary>
    /// Adds a View and associated ViewModel to a <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TView">The View of type <see cref="BindableObject"/>.</typeparam>
    /// <typeparam name="TViewModel">The ViewModel which implements <see cref="INotifyPropertyChanged"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the View and ViewModel to.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the View and ViewModel.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddViewAndViewModel<TView, TViewModel>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TView : BindableObject
        where TViewModel : class, INotifyPropertyChanged
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton<TView>()
                        .AddSingleton<TViewModel>();

                break;

            case ServiceLifetime.Scoped:
                services.AddScoped<TView>()
                        .AddScoped<TViewModel>();

                break;

            case ServiceLifetime.Transient:
                services.AddTransient<TView>()
                        .AddTransient<TViewModel>();

                break;

            default:
                throw !Enum.IsDefined<ServiceLifetime>(lifetime)
                    ? new InvalidEnumArgumentException(nameof(lifetime), (int)lifetime, typeof(ServiceLifetime))
                    : new NotSupportedException($"{lifetime} not supported");
        }

        return services;
    }

    /// <summary>
    /// Adds a View and ViewModel to <see cref="IServiceCollection"/> and registers it's route.
    /// </summary>
    /// <typeparam name="TView">The View of type <see cref="BindableObject"/>.</typeparam>
    /// <typeparam name="TViewModel">The ViewModel which implements <see cref="INotifyPropertyChanged"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the View and ViewModel to.</param>
    /// <param name="route">The View's route in <see cref="Shell"/>.</param>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the View and ViewModel.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    public static IServiceCollection AddViewAndViewModelWithRoute<TView, TViewModel>(this IServiceCollection services, string route, ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TView : BindableObject
        where TViewModel : class, INotifyPropertyChanged
    {
        Routing.RegisterRoute(route, typeof(TView));

        services.AddViewAndViewModel<TView, TViewModel>(lifetime);

        return services;
    }
}
