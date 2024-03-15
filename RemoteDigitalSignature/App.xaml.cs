using Common.Mvvm.DI;
using Common.Mvvm.Dialog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RemoteDigitalSignature.DI;
using RemoteDigitalSignature.Hubs;
//using RemoteDigitalSignature.Hubs;
using RemoteDigitalSignature.Service.Abstractions;
using RemoteDigitalSignature.Service.Models;
using RemoteDigitalSignature.ViewModels;
using RemoteDigitalSignature.ViewModels.DI;
using RemoteDigitalSignature.Views.DI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RemoteDigitalSignature;

//public static class TestLib
//{
//    [DllImport("kernel32.dll")]
//    public static extern IntPtr LoadLibrary(string dllToLoad);

//    [DllImport("kernel32.dll")]
//    public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

//    [DllImport("kernel32.dll")]
//    public static extern bool FreeLibrary(IntPtr hModule);


//    // Delegate with function signature for the GetVersion function
//    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
//    [return: MarshalAs(UnmanagedType.U4)]
//    public delegate uint AvCmInit([OutAttribute][InAttribute] uint flags);
//}

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private Mutex? _instanceMutex;

    private WebApplication WebApp { get; set; } = null!;

    //private readonly ConcurrentDictionary<int,Task> _currentRunningTasks = new();

    private IDialogService? _dialogService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    protected override void OnExit(ExitEventArgs e)
    {
        _instanceMutex?.ReleaseMutex();
        base.OnExit(e);
    }

    private async void App_OnStartup(object sender, StartupEventArgs e)
    {
        // Init web server



        try
        {
            //TestLib.AvCmInit? _avCmInit = null;

            //var pLib = TestLib.LoadLibrary("C:\\Program Files (x86)\\Avest\\AvPCM_nces____333\\AvCryptMail.dll");
            //IntPtr p_avCmInit_nandle = TestLib.GetProcAddress(pLib, "AvCmInit");

            //// If successful, load function pointer
            //if (p_avCmInit_nandle != IntPtr.Zero)
            //{
            //    _avCmInit = (TestLib.AvCmInit)Marshal.GetDelegateForFunctionPointer(
            //        p_avCmInit_nandle,
            //        typeof(TestLib.AvCmInit));
            //}


            //var result = _avCmInit(0x1);

            //TestLib.FreeLibrary(pLib);
            ////IntPtr iplmc1_Close = TestLib.GetProcAddress(pLib, "lmc1_Close");


            var useSwagger = false;

            var fi = new FileInfo("store.bin");

            if (fi.Exists)
            {
                await using FileStream fs = new("store.bin", FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
                var store = await JsonSerializer.DeserializeAsync<CertificateRevocationListStore>(fs, new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                });

                if (store != null)
                {
                    useSwagger = store.UseSwagger;
                }
            }

            var builder = WebApplication.CreateBuilder();

            //builder.WebHost.UseUrls("http://172.20.40.40:15300");
            builder.WebHost.UseUrls("http://localhost:15300");

            builder.Services.AddCors();
            builder.Services.AddHealthChecks();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = 6291456;
                options.EnableDetailedErrors = true;
            });

            if (useSwagger)
            {
                builder.Services.AddEndpointsApiExplorer();

                builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Some API v1", Version = "v1" });
                    // Set the comments path for the Swagger JSON and UI.
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath, true);
                    c.AddSignalRSwaggerGen();
                });
            }

            SimpleIoC.Services = builder.Services;

            builder.Services
                .AddLogger()
                .AddServices()
                .AddViewModels()
                .AddViews();

            WebApp = builder.Build();

            var logger = WebApp.Services.GetRequiredService<ILogger<Application>>();

            var mainSigningService = WebApp.Services.GetRequiredService<IMainSignService>();

            WebApp.Use(async (context, next) =>
            {
                try
                {
                    var task = next();

                    while (mainSigningService.GetState())
                    {
                    }

                    context.Response.OnCompleted(async () =>
                    {
                        await Task.CompletedTask;
                    });

                    await Task.Delay(1);

                    await task;
                }
                catch (OperationCanceledException ex)
                {
                    Debug.WriteLine(ex);

                    mainSigningService.Shutdown();
                    mainSigningService.SetState(false);
                }
            });

            WebApp.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            if (useSwagger)
            {
                WebApp.UseSwagger();
                WebApp.UseSwaggerUI();
            }


            //WebApp.MapHealthChecks("/health");

            //WebApp.MapGet("api/avest/certs", async (IMainSignService mainSigningService) =>
            //{
            //    try
            //    {
            //        var result = await GetResult(mainSigningService.GetCerts());

            //        return Results.Ok(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.WriteLine($"{ex.Message} - IsBusy - {mainSigningService.GetState()}");

            //        mainSigningService.SetState(false);

            //        var result = new List<CertificateResultWebModel>
            //        {
            //            new CertificateResultWebModel
            //            {
            //                IsValid = false,
            //                Error = new ErrorWebModel
            //                {
            //                    Code = "0x0",
            //                    Message = ex.Message
            //                }
            //            }
            //        };

            //        return Results.BadRequest(result);
            //    }
            //});

            //WebApp.MapGet("api/avest/certs/{publicKeyId}", async (string publicKeyId, IMainSignService mainSigningService) =>
            //{
            //    try
            //    {
            //        var result = await GetResult(mainSigningService.GetCertAsync(publicKeyId));

            //        return Results.Ok(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        var result = new CertificateResultWebModel
            //        {
            //            IsValid = false,
            //            Error = new ErrorWebModel
            //            {
            //                Code = "0x0",
            //                Message = ex.Message
            //            }

            //        };

            //        return Results.BadRequest(result);
            //    }
            //});

            //WebApp.MapGet("api/avest/revoсationListCerts", async(IMainSignService mainSigningService) =>
            //{
            //    try
            //    {
            //        var result = await GetResult(mainSigningService.GetRevocationListCerts());
            //        return Results.Ok(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        return Results.BadRequest(ex.Message);
            //    }
            //});

            //WebApp.MapGet("api/avest/check", async (IMainSignService mainSigningService) =>
            //{
            //    try
            //    {
            //        var result = await GetResult(mainSigningService.TryCheckAvestAsync());
            //        return Results.Ok(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        var result = new CheckResultWebModel
            //        {
            //            IsValid = false,
            //            Errors = new List<ErrorWebModel>{
            //        new ErrorWebModel
            //        {
            //            Code = "0x0",
            //            Message = ex.Message
            //        }
            //    }
            //        };

            //        return Results.BadRequest(result);
            //    }
            //});

            //WebApp.MapGet("api/avest/importRevocationListCerts", async (IMainSignService mainSigningService, ICertificateRevocationListStoreService certificateRevocationListStoreService) =>
            //{
            //    try
            //    {
            //        var result = await GetResult(mainSigningService.DownloadImportRevocationListCertificates());

            //        if(!result.IsValid)
            //        {
            //            return Results.BadRequest(result);
            //        }

            //        return Results.Ok(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        var result = new CheckResultWebModel
            //        {
            //            IsValid = false,
            //            Errors = new List<ErrorWebModel>{
            //        new ErrorWebModel
            //        {
            //            Code = "0x0",
            //            Message = ex.Message
            //        }
            //    }
            //        };

            //        return Results.BadRequest(result);
            //    }
            //});

            //WebApp.MapPost("api/avest/signStringData", (SignDataStringRequestWebModel sign, IMainSignService mainSigningService) =>
            //{
            //    try
            //    {
            //        var result = mainSigningService.SignDataAsync(sign).GetAwaiter().GetResult();
            //        return Results.Ok(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        var result = new SignResultWebModel
            //        {
            //            IsValid = false,
            //            Error = new ErrorWebModel
            //            {
            //                Code = "0x0",
            //                Message = ex.Message
            //            }
            //        };

            //        return Results.BadRequest(result);
            //    }
            //});

            //WebApp.MapPost("api/avest/signBytesData", (SignDataByteRequestWebModel sign, IMainSignService mainSigningService) =>
            //{
            //    try
            //    {
            //        var result = mainSigningService.SignDataAsync(sign).GetAwaiter().GetResult();
            //        return Results.Ok(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        var result = new SignResultWebModel
            //        {
            //            IsValid = false,
            //            Error = new ErrorWebModel
            //            {
            //                Code = "0x0",
            //                Message = ex.Message
            //            }
            //        };

            //        return Results.BadRequest(result);
            //    }
            //});

            //WebApp.MapPost("api/avest/hashBytesData", (SignDataByteRequestWebModel sign, IMainSignService mainSigningService) =>
            //{
            //    try
            //    {
            //        var result = mainSigningService.GetDataHashAsync(sign).GetAwaiter().GetResult();
            //        return Results.Ok(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        var result = new SignResultWebModel
            //        {
            //            IsValid = false,
            //            Error = new ErrorWebModel
            //            {
            //                Code = "0x0",
            //                Message = ex.Message
            //            }
            //        };

            //        return Results.BadRequest(result);
            //    }
            //});

            //WebApp.MapPost("api/avest/signFile", (SignFileRequestWebModel sign, IMainSignService mainSigningService) =>
            //{
            //    try
            //    {
            //        var result = mainSigningService.SignFileAsync(sign).GetAwaiter().GetResult();
            //        return Results.Ok(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        var result = new SignResultWebModel
            //        {
            //            IsValid = false,
            //            Error = new ErrorWebModel
            //            {
            //                Code = "0x0",
            //                Message = ex.Message
            //            }
            //        };

            //        return Results.BadRequest(result);
            //    }
            //});

            //WebApp.MapPost("api/avest/hashFile", (SignFileRequestWebModel sign, IMainSignService mainSigningService) =>
            //{
            //    try
            //    {
            //        var result = mainSigningService.GetFileHashAsync(sign).GetAwaiter().GetResult();
            //        return Results.Ok(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        var result = new SignResultWebModel
            //        {
            //            IsValid = false,
            //            Error = new ErrorWebModel
            //            {
            //                Code = "0x0",
            //                Message = ex.Message
            //            }
            //        };

            //        return Results.BadRequest(result);
            //    }
            //});

            //WebApp.MapPost("api/avest/signFiles", () =>
            //{
            //    return new List<AvNative.Common> { AvNative.GetHashFileAndSignUn("") };
            //});

            //// Meta Resource
            //WebApp.MapGet("api/meta/version", () =>
            //{
            //    try
            //    {
            //        var flv = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            //        return Results.Ok(flv.ProductVersion);
            //    }
            //    catch (Exception ex)
            //    {
            //        var result = new List<CertificateResultWebModel>
            //        {
            //            new CertificateResultWebModel
            //            {
            //                IsValid = false,
            //                Error = new ErrorWebModel
            //                {
            //                    Code = "0x0",
            //                    Message = ex.Message
            //                }
            //            }
            //        };

            //        return Results.BadRequest(result);
            //    }
            //});

            WebApp.MapHub<AvestHub>("/avesthub");

            SimpleIoC.Current = WebApp.Services;

            var containerRegistry = SimpleIoC.Current.GetRequiredService<IContainerRegistry>();
            containerRegistry.RegisterDialogs();

            _dialogService = SimpleIoC.Current.GetRequiredService<IDialogService>();

            // check that there is only one instance of the control panel running...
            _instanceMutex = new Mutex(true, @"Global\ControlPanel", out var createdNew);
            if (!createdNew)
            {
                _instanceMutex = null;

                _dialogService.Warning("Другая копия программы уже запущена. Программа будет закрыта", "Завершение работы");

                Current.Shutdown();
                return;
            }

            _dialogService.Create<MainViewModel>(0.3f, 0.3f);
            _dialogService.ShowViewModel<MainViewModel>(false);

            await WebApp.RunAsync();
        }
        catch
        {
            // ignore
            //(ex.StackTrace, ex.Message, "Ошибка");
        }
    }
}