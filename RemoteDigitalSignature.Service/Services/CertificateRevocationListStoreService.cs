using RemoteDigitalSignature.Service.Abstractions;
using RemoteDigitalSignature.Service.Models;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace RemoteDigitalSignature.Service.Services;

public class CertificateRevocationListStoreService : ICertificateRevocationListStoreService
{
    public CertificateRevocationListStore Store { get; set; } = new CertificateRevocationListStore();

    private readonly string _storeName = "store.bin"; 

    public List<DownaloadedCert> DownloadedCertificates { get; set; } = new();

    private readonly IHttpClientFactory _httpClientFactory;

    public CertificateRevocationListStoreService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _ = InitAsync();
    }

    public async Task DownloadAsync()
    {
        using var client = _httpClientFactory.CreateClient("netClient");

        // after download

        foreach (var cert in Store.Certificates)
        {

            Stream? stream = null;
            var filename = cert.Path;

            if (!Store.IsLocal)
            {
                var uri = new Uri(cert.Path);
                if (!uri.IsFile)
                {
                    var response = await client.GetAsync(cert.Path);

                    if (response.IsSuccessStatusCode)
                    {
                        stream = await response.Content.ReadAsStreamAsync();
                    }
                }
            }
            else
            {
                stream = new FileStream(Path.Combine(filename), FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            }

            var dirCerts = "certs";

            if (!Directory.Exists(dirCerts))
            {
                Directory.CreateDirectory(dirCerts);
            }

            if (stream != null)
            {
                var onlyfileName = Path.GetFileName(filename);

                await using FileStream fs = new(Path.Combine(dirCerts, onlyfileName), FileMode.Create, FileAccess.Write, FileShare.Write, 4096, true);

                await stream.CopyToAsync(fs);

                DownloadedCertificates.Add(new DownaloadedCert
                {
                    Name = cert.Name,
                    Path = Path.Combine(dirCerts, onlyfileName),
                });
            }
        }
    }

    public async Task InitAsync()
    {
        var fi = new FileInfo(_storeName);

        if (fi.Exists)
        {
            await using FileStream fs = new(_storeName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            var store = await JsonSerializer.DeserializeAsync<CertificateRevocationListStore>(fs, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            });

            if (store != null)
            {
                Store = store;

                Console.WriteLine("Объект десериализован");
                Console.WriteLine($"Имя: {Store.Name}");
            }
        }

        InitCryptoLibrary();

        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        // получаем поток, куда будем записывать сериализованный объект
        await using FileStream fs = new(_storeName, FileMode.Create, FileAccess.Write, FileShare.Write, 4096, true);
        await JsonSerializer.SerializeAsync(fs, Store, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        });

        Console.WriteLine("Объект сериализован");
    }

    public async Task ClearCertFolderAsync()
    {
        await Task.Run(() =>
        {
            var dirCerts = "certs";
            if (Directory.Exists(dirCerts))
            {
                Directory.Delete(dirCerts, true);
            }

            DownloadedCertificates.Clear();
        });
    }

    public void InitCryptoLibrary()
    {
        if (Store != null && string.IsNullOrEmpty(Store.CryptLibraryPath))
        {
            var DefaultProgramFiles = Environment.Is64BitOperatingSystem ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            var filename = "AvCryptMail.dll";

            var avestPath = Path.Combine(DefaultProgramFiles, "Avest");

            var di = new DirectoryInfo(avestPath);

            string? libraryPath = Store.CryptLibraryPath;

            if (di.Exists)
            {
                var dirs = di.GetDirectories().ToList();

                // find nces dirs (AvPCM_nces)

                var ncesDir = dirs.FirstOrDefault(x => x.Name == "AvPCM_nces");

                if (ncesDir != null)
                {
                    if (ncesDir.Exists)
                    {
                        if (File.Exists(Path.Combine(ncesDir.FullName, filename)))
                        {
                            libraryPath = ncesDir.FullName;
                        }
                    }

                    // found 
                }

                if (libraryPath == null)
                {
                    var foundAny = dirs.FirstOrDefault(x => x.GetFiles().Any(x => x.Name == filename));

                    if (foundAny != null)
                    {
                        if (foundAny.Exists)
                        {
                            if (File.Exists(Path.Combine(foundAny.FullName, filename)))
                            {
                                libraryPath = foundAny.FullName;
                            }
                        }
                    }
                }

                // 
            }

            if (libraryPath != null)
            {
                Store.CryptLibraryPath = libraryPath;
            }
        }
    }
}
