﻿using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace AniWPF.StartupHelper
{
    public static class ServiceExtensions
    {
        public static void AddFormFactory<TForm>(this IServiceCollection services)
            where TForm : class
        {
            services.AddTransient<TForm>();
            services.AddSingleton<Func<Window, TForm>>(x => (parentWindow) =>
            {
                var formService = x.GetService<TForm>();
                if (formService == null)
                {
                    throw new InvalidOperationException($"Could not resolve service for {typeof(TForm)}");
                }

                if (formService is IWindowAware windowAware)
                {
                    windowAware.ParentWindow = parentWindow;
                }

                return formService;
            });
            services.AddSingleton<IAbstractFactory<TForm>, AbstractFactory<TForm>>();
        }
    }
    public interface IWindowAware
    {
        Window ParentWindow { get; set; }
    }
}
 