// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.OpenApi.Models;

// namespace GestFinancas_Api.Configurations
// {
//   public static class DependencyInjectionSwagger
//   {
//     public static IServiceCollection AddInfrastrutureSwagger(this IServiceCollection services)

//     {
//       services.AddSwaggerGen(options =>
//       {
//         options.SwaggerDoc("v1", new OpenApiInfo
//         {
//           Title = "GestFinancas API",
//           Version = "v1",
//           Description = "API para gerenciamento de usuários e finanças",
//           Contact = new OpenApiContact
//           {
//             Name = "Suporte",
//             Email = "suporte@exemplo.com"
//           }
//         });

//         // Adicionando a segurança ao Swagger com JWT
//         options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//         {
//           In = ParameterLocation.Header,
//           Description = "Por favor insira o token JWT",
//           Name = "Authorization",
//           Type = SecuritySchemeType.ApiKey,
//           BearerFormat = "JWT"
//         });

//         options.AddSecurityRequirement(new OpenApiSecurityRequirement
//           {
//                 {
//                     new OpenApiSecurityScheme
//                     {
//                         Reference = new OpenApiReference
//                         {
//                             Type = ReferenceType.SecurityScheme,
//                             Id = "Bearer"
//                         }
//                     },
//                     new string[] {}
//                 }
//           });
//       });
//       return services;
//     }
//   }
// }
