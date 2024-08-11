global using CRMEducacional.API.Controllers.Base;
global using CRMEducacional.API.Extensions;
global using CRMEducacional.API.Filters;
global using CRMEducacional.API.Ioc;
global using CRMEducacional.Application.Validation;
global using CRMEducacional.Core.Abstractions;
global using CRMEducacional.Core.Interfaces.Services;
global using CRMEducacional.Core.Interfaces.Validations;
global using CRMEducacional.Persistence.Contexts;
global using CRMEducacional.Persistence.Repositories;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ApiExplorer;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Logging.Abstractions;
global using Microsoft.Extensions.Logging.Console;
global using Microsoft.Extensions.Options;
global using Microsoft.OpenApi.Models;

global using Serilog;
global using Serilog.Events;

global using Swashbuckle.AspNetCore.SwaggerGen;
global using Swashbuckle.AspNetCore.SwaggerUI;

global using System;
global using System.Collections.Generic;
global using System.Diagnostics;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Net.Http;
global using System.Text.Json;
global using System.Threading.Tasks;

global using ApiVersionDescription = Asp.Versioning.ApiExplorer.ApiVersionDescription;
global using IApiVersionDescriptionProvider = Asp.Versioning.ApiExplorer.IApiVersionDescriptionProvider;
global using ApiVersion = Asp.Versioning.ApiVersion;
global using ApiVersionAttribute = Asp.Versioning.ApiVersionAttribute;
global using ILogger = Serilog.ILogger;