using DigitalSigning.Av;
using DigitalSigning.Crypt32;
using DigitalSigning.Logging;
using RemoteDigitalSignature.Service.Abstractions;
using RemoteDigitalSignature.Service.Models;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using static DigitalSigning.Av.AvNative;

namespace RemoteDigitalSignature.Service.Services;

/// <summary>
/// 
/// </summary>
public class MainSignService : IMainSignService
{
    private readonly IStateManager _stateManager;
    private readonly ICertificateRevocationListStoreService _certificateRevocationListStoreService;
    private readonly IXmlSignToXmlDsigService _xaDesService;

    public MainSignService(IStateManager stateManager, ICertificateRevocationListStoreService certificateRevocationListStoreService, IXmlSignToXmlDsigService xaDesService)
    {
        Logger.Init();
        _stateManager = stateManager;
        _certificateRevocationListStoreService = certificateRevocationListStoreService;
        _xaDesService = xaDesService;

        if (!string.IsNullOrEmpty(_certificateRevocationListStoreService.Store.CryptLibraryPath))
        {
            if (!_stateManager.IsReInitLibrary)
            {
                if (ReInitLibrary())
                {

                }
            }
        }
    }

    private IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName == "AvCryptMail")
        {
            // On systems with AVX2 support, load a different library.
            //if (System.Runtime.Intrinsics.X86.Avx2.IsSupported)
            //{
            return NativeLibrary.Load($"{_certificateRevocationListStoreService.Store.CryptLibraryPath}\\AvCryptMail.dll", assembly, searchPath);
           // }
        }

        // Otherwise, fallback to default import resolver.
        return IntPtr.Zero;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<CertificateResultWebModel>> GetCerts(bool showOnlyValid = true)
    {
        var res = await TryCheckAvestAsync();

        if (!res.IsValid && res.Errors != null)
        {
            throw new Exception(string.Join("\n", res.Errors.Select(x => x.Message)));
        }

        _stateManager.IsBusy = true;

        var crtsTask = await Task.Run(() =>
        {
            //await Task.Delay(TimeSpan.FromSeconds(10));

            var certs = GetCertificatesListAvCryptMailUn();

            if (!certs.State)
            {
                throw new Exception(certs.Error.Result);
            }

            if (showOnlyValid)
            {
                certs.Certs = certs.Certs.Where(cert =>
                cert.GetAttributeByOid("2.5.4.3") != null &&
                cert.GetAttributeByOid("2.5.4.4") != null &&
                cert.GetAttributeByOid("2.5.4.41") != null &&
                cert.GetAttributeByOid("1.2.112.1.2.1.1.1.1.2") != null)
                .ToList();
            }

            return Task.FromResult(certs.Certs.Select(cert => new CertificateResultWebModel
            {
                IsValid = cert.State,
                Name = cert.Name,
                PublicKeyId = cert.PublicKeyId,
                SerialNumber = cert.SerialNumber,
                ThumbPrint = cert.ThumbPrint,
                PublicKey = cert.PublicKey,
                NotBefore = cert.NotBefore,
                NotAfter = cert.NotAfter,
                Error = cert.Error.State ? new ErrorWebModel
                {
                    Code = cert.Error.Code,
                    Message = cert.Error.Result
                } : null,
                Attributes = cert.Attributes.Select(att => new CertificateAttributeResultWebModel
                {
                    IsValid = att.State,
                    Title = att.Title,
                    Oid = att.Oid,
                    Value = att.Value,
                    Error = att.Error.State ? new ErrorWebModel
                    {
                        Code = att.Error.Code,
                        Message = att.Error.Result
                    } : null,
                }).ToList()
            }));
        });

        _stateManager.IsBusy = false;
        return crtsTask;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<RevocationListCertificateResultWebModel>> GetRevocationListCerts()
    {
        var res = await TryCheckAvestAsync();

        if (!res.IsValid && res.Errors != null)
        {
            throw new Exception(string.Join("\n", res.Errors.Select(x => x.Message)));
        }

        return await Task.Run(() =>
        {

            List<RevocationListCertificate> certs;

            var crls = new List<SimpleCrl>
            {
                new SimpleCrl
                {
                    Subject = "O=\"Республиканское унитарное предприятие \"\"Национальный центр электронных услуг\"\"\", CN=Корневой удостоверяющий центр ГосСУОК, C=BY",
                    Name = "Корневой удостоверяющий центр ГосСУОК",
                },
                new SimpleCrl
                {
                    Subject = "O=\"Республиканское унитарное предприятие \"\"Национальный центр электронных услуг\"\"\", CN=Республиканский удостоверяющий центр ГосСУОК, C=BY, S=Минская, L=г. Минск, STREET=\"пр-т Машерова, 25\", E=rca@pki.gov.by",
                    Name = "Республиканский удостоверяющий центр ГосСУОК"
                }
            };

            certs = Crypt32Native.FindRevokedCertificates(crls.Select(x => x.Subject));

            var notfound = crls.FirstOrDefault(d => !certs.Any(x => x.Name == d.Subject));
            if (notfound != null)
            {
                throw new Exception($"COC {notfound.Name} не найден в хранилище сертификатов");
            }

            return certs.Select(cert => new RevocationListCertificateResultWebModel
            {
                IsValid = cert.State,
                Name = cert.Name,
                SimpleName = cert.SimpleName,
                ThisUpdate = cert.ThisUpdate,
                NextUpdate = cert.NextUpdate,
                Error = cert.Error.State ? new ErrorWebModel
                {
                    Code = cert.Error.Code,
                    Message = cert.Error.Result
                } : null
            });
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="publicKeyId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<CertificateResultWebModel?> GetCertAsync(string publicKeyId)
    {
        var res = await TryCheckAvestAsync();

        if (!res.IsValid && res.Errors != null)
        {
            throw new Exception(string.Join("\n", res.Errors.Select(x => x.Message)));
        }

        return await Task.Run(() =>
        {
            var certs = GetCertificatesListAvCryptMailUn();

            if (certs.Error.State)
            {
                throw new Exception(certs.Error.Result);
            }

            return certs.Certs.Where(c => c.PublicKeyId == publicKeyId).Select(cert => new CertificateResultWebModel
            {
                IsValid = cert.State,
                Name = cert.Name,
                PublicKeyId = cert.PublicKeyId,
                SerialNumber = cert.SerialNumber,
                ThumbPrint = cert.ThumbPrint,
                PublicKey = cert.PublicKey,
                NotBefore = cert.NotBefore,
                NotAfter = cert.NotAfter,
                Error = cert.Error.State ? new ErrorWebModel
                {
                    Code = cert.Error.Code,
                    Message = cert.Error.Result
                } : null,
                Attributes = cert.Attributes.Select(att => new CertificateAttributeResultWebModel
                {
                    IsValid = att.State,
                    Title = att.Title,
                    Oid = att.Oid,
                    Value = att.Value,
                    Error = att.Error.State ? new ErrorWebModel
                    {
                        Code = att.Error.Code,
                        Message = att.Error.Result
                    } : null,
                }).ToList()
            }).FirstOrDefault();
        });
    }

    public async Task<CheckResultWebModel> ImportCrl()
    {
        return await Task.Run(() =>
        {
            try
            {
                if (File.Exists("kuc.crl"))
                {
                    Crypt32Native.AddOrUpdateCRLToStore("kuc.crl");
                }

                if (File.Exists("ruc.crl"))
                {
                    Crypt32Native.AddOrUpdateCRLToStore("ruc.crl");
                }

                return new CheckResultWebModel
                {
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                return new CheckResultWebModel
                {
                    IsValid = false,
                    Errors = new List<ErrorWebModel>
                    {
                        new ErrorWebModel
                        {
                            Code = "0x0",
                            Message = ex.Message
                        }
                    }
                };
            }
        });
    }

    public async Task<CheckResultWebModel> TryCheckAvestAsync()
    {
        var result = new CheckResultWebModel();

        await Task.Run(() =>
        {
            //ImportCrl();

            result.IsValid = TryCheckAvest(out var errors, out _);

            result.Errors = errors.Select(error => new ErrorWebModel
            {
                Code = "0",
                Message = error
            }).ToList();
        });

        return result;
    }

    public async Task<HashFileResultWebModel> GetFileHashAsync(SignFileRequestWebModel sign)
    {
        var res = await TryCheckAvestAsync();

        if (!res.IsValid && res.Errors != null)
        {
            throw new Exception(string.Join("\n", res.Errors.Select(x => x.Message)));
        }

        var common = await Task.Run(() => GetFileHashUn(sign.Path, sign.PublicKeyId, sign.Password));

        var fi = new FileInfo(sign.Path);

        var result = new HashFileResultWebModel
        {
            InitialPath = fi.Exists ? sign.Path : null,
            IsValid = fi.Exists
        };

        if (common.State)
        {
            result.HashData = new HashWebModel
            {
                Hash = common.Value,
                IsValid = common.State,
            };

            if (sign.IsReturnData)
            {
                result.HashData.Data = common.Data;
            }
        }
        else
        {
            if (!common.Error.State)
            {
                result.Error = new ErrorWebModel
                {
                    Code = common.Error.Code,
                    Message = common.Error.Result
                };
            }
        }

        return result;
    }

    public async Task<HashResultWebModel> GetDataHashAsync(SignDataByteRequestWebModel sign)
    {
        var res = await TryCheckAvestAsync();

        if (!res.IsValid && res.Errors != null)
        {
            throw new Exception(string.Join("\n", res.Errors.Select(x => x.Message)));
        }

        var common = await Task.Run(() => GetHashUn(sign.Data, sign.PublicKeyId, sign.Password));

        var result = new HashResultWebModel
        {
            IsValid = true
        };

        if (common.State)
        {
            result.HashData = new HashWebModel
            {
                Data = sign.IsReturnData ? common.Data : null,
                Hash = common.Value,
                IsValid = common.State
            };

            if (sign.IsReturnData)
            {
                result.HashData.Data = common.Data;
            }
        }
        else
        {
            if (!common.Error.State)
            {
                result.Error = new ErrorWebModel
                {
                    Code = common.Error.Code,
                    Message = common.Error.Result
                };
            }
        }

        result.IsValid = result.Error == null;

        return result;
    }

    public async Task<SignFileResultWebModel> SignFileAsync(SignFileRequestWebModel sign)
    {
        var res = await TryCheckAvestAsync();

        if (!res.IsValid && res.Errors != null)
        {
            throw new Exception(string.Join("\n", res.Errors.Select(x => x.Message)));
        }

        var common = await Task.Run(() => GetHashFileAndSignUn(sign.Path, sign.PublicKeyId, sign.Password));

        var fi = new FileInfo(sign.Path);

        var result = new SignFileResultWebModel
        {
            InitialPath = fi.Exists ? sign.Path : null,
            IsValid = fi.Exists
        };

        if (common.State)
        {
            result.SignData = new SignWebModel
            {
                Hash = common.Value,
                IsValid = common.State,
                Signature = common.ExtraValueInBytes
            };

            if (sign.IsReturnData)
            {
                result.SignData.Data = common.Data;
            }
        }
        else
        {
            if (!common.Error.State)
            {
                result.Error = new ErrorWebModel
                {
                    Code = common.Error.Code,
                    Message = common.Error.Result
                };
            }
        }

        return result;
    }


    public async Task<XmlSignToXaDesWebModel> SignXmlFileInXmlDsigAsync(SignFileRequestWebModel sign)
    {
        var res = await TryCheckAvestAsync();
        if (res is { IsValid: false, Errors: { } })
        {
            throw new Exception(string.Join("\n", res.Errors.Select(x => x.Message)));
        }
        
        
        // Проверка, что файл имеется на диске
        if (!File.Exists(sign.Path))
        {
            throw new Exception($"Файл {sign.Path} не найден");
        }
        var fileToByte = await File.ReadAllBytesAsync(sign.Path);

        // Проверка, что файл является XML
        var xmlFile = Encoding.UTF8.GetString(fileToByte);
        using var xr = XmlReader.Create(new StringReader(xmlFile), new XmlReaderSettings { Async = true });
        try
        {
            while (await xr.ReadAsync()) { }
        }
        catch
        {
            throw new Exception($"Содержимое файла {sign.Path} не является XML-структурой");
        }
        
        var certificate = await GetCertAsync(sign.PublicKeyId);
        if (certificate == null) { throw new Exception($"Отсутствует сертификат соответствующий ключу с Id = {sign.PublicKeyId}"); }

        // Подписание
        var result = new XmlSignToXaDesWebModel();

        try
        {
             var signDocument = await _xaDesService.SignToXmlDsigAsync(certificate.SerialNumber, fileToByte, sign.PublicKeyId, sign.Password);
             result.SignXml = signDocument.OuterXml;
             result.IsValid = true;

             // TODO Временно сохраняю элемент в файл
             var path = "d:\\outXML";
             if (!Directory.Exists(path))
             {
                 Directory.CreateDirectory(path);
             }
             signDocument.Save(Path.Combine(path,$"{Guid.NewGuid()}.xml"));
        }
        catch (Exception e)
        {
            result.Error = new ErrorWebModel
            {
                Code = "Ошибка формирования подписанной XML",
                Message = e.GetBaseException().Message
            };
        }
        
        return result;
    }

    public async Task<XmlSignToXaDesWebModel> VerifySignXmlFileInXmlDsig(SignFileRequestWebModel verifyData)
    {
        var res = await TryCheckAvestAsync();
        if (res is { IsValid: false, Errors: { } })
        {
            throw new Exception(string.Join("\n", res.Errors.Select(x => x.Message)));
        }


        // Проверка, что файл имеется на диске
        if (!File.Exists(verifyData.Path))
        {
            throw new Exception($"Файл {verifyData.Path} не найден");
        }
        var fileToByte = await File.ReadAllBytesAsync(verifyData.Path);

        // Проверка, что файл является XML
        var xmlFile = Encoding.UTF8.GetString(fileToByte);
        using var xr = XmlReader.Create(new StringReader(xmlFile), new XmlReaderSettings { Async = true });
        try
        {
            while (await xr.ReadAsync()) { }
        }
        catch
        {
            throw new Exception($"Содержимое файла {verifyData.Path} не является XML-структурой");
        }

        var result = new XmlSignToXaDesWebModel{ IsValid = false };
        
        var xmlDoc = new XmlDocument{PreserveWhitespace = true};
        xmlDoc.LoadXml(xmlFile);

        try
        {
            await _xaDesService.VerifyXmlFileToXmlDsigAsync(xmlDoc);
        }
        catch (Exception e)
        {
            result.Error = new ErrorWebModel
            {
                Code = "Error",
                Message = e.GetBaseException().Message
            };
        }

        result.IsValid = true;
        return result;
    }


    public async Task<SignResultWebModel> SignDataAsync(SignDataByteRequestWebModel sign)
    {
        var res = await TryCheckAvestAsync();

        if (!res.IsValid && res.Errors != null)
        {
            throw new Exception(string.Join("\n", res.Errors.Select(x => x.Message)));
        }

        var common = await Task.Run(() => GetHashDataAndSignUn(sign.Data, sign.PublicKeyId, sign.Password));

        var result = new SignResultWebModel
        {
            IsValid = true
        };

        if (common.State)
        {
            result.SignData = new SignWebModel
            {
                Data = sign.IsReturnData ? common.Data : null,
                Hash = common.Value,
                IsValid = common.State,
                Signature = common.ExtraValueInBytes
            };

            if (sign.IsReturnData)
            {
                result.SignData.Data = common.Data;
            }
        }
        else
        {
            if (!common.Error.State)
            {
                result.Error = new ErrorWebModel
                {
                    Code = common.Error.Code,
                    Message = common.Error.Result
                };
            }
        }

        result.IsValid = result.Error == null;

        return result;
    }

    public async Task<SignResultWebModel> SignDataAsync(SignDataStringRequestWebModel sign)
    {
        var res = await TryCheckAvestAsync();

        if (!res.IsValid && res.Errors != null)
        {
            throw new Exception(string.Join("\n", res.Errors.Select(x => x.Message)));
        }

        var common = await Task.Run(() => GetHashDataAndSignUn(sign.Data, sign.PublicKeyId, sign.Password));

        var result = new SignResultWebModel
        {
            IsValid = common.State
        };

        if (common.State)
        {
            result.SignData = new SignWebModel
            {
                Data = sign.IsReturnData ? common.Data : null,
                Hash = common.Value,
                IsValid = common.State,
                Signature = common.ExtraValueInBytes
            };

            if (sign.IsReturnData)
            {
                result.SignData.Data = common.Data;
            }
        }
        else
        {
            if (!common.Error.State)
            {
                result.Error = new ErrorWebModel
                {
                    Code = common.Error.Code,
                    Message = common.Error.Result
                };
            }
        }

        //result.IsValid = result.Error == null;

        return result;
    }

    public async Task<CheckResultWebModel> ImportRevocationListCertificates(string filename)
    {
        return await Task.Run(() =>
        {
            try
            {
                if (File.Exists(filename))
                {
                    var fi = new FileInfo(filename);

                    if (fi.Extension.ToLower() == ".crl")
                    {
                        Crypt32Native.AddOrUpdateCRLToStore(filename);
                    }
                    else if(fi.Extension.ToLower() == ".cer")
                    {
                        if (fi.Name == "kuc.cer")
                        {
                            if (!Crypt32Native.AddOrUpdateCertificateToStore(filename))
                            {
                                throw new Exception($"{filename} не добавлен в список доверенных сертификатов");
                            }
                        }
                        else
                        {
                            Crypt32Native.AddOrUpdateCertificateToStore(filename, false);
                        }

                        //throw new Exception($"{filename} не является списком отозванных сертификатов");
                    }
                }

                return new CheckResultWebModel
                {
                    IsValid = true
                };
            }
            catch (Exception ex)
            {
                return new CheckResultWebModel
                {
                    IsValid = false,
                    Errors = new List<ErrorWebModel>
                    {
                        new ErrorWebModel
                        {
                            Code = "0x0",
                            Message = ex.Message
                        }
                    }
                };
            }
        });
    }

    public async Task<CheckResultWebModel> DownloadImportRevocationListCertificates()
    {
        await _certificateRevocationListStoreService.DownloadAsync();

        List<string>? errors = null;

        foreach (var cert in _certificateRevocationListStoreService.DownloadedCertificates)
        {
            var res = await ImportRevocationListCertificates(cert.Path);

            if (!res.IsValid)
            {
                errors ??= new List<string>();

                if (res.Errors != null)
                {
                    errors.AddRange(res.Errors.Select(r => r.Message));
                }
            }
            else
            {
                cert.IsImported = true;
            }
        }

        CheckResultWebModel result;

        if (errors == null)
        {
            result = new CheckResultWebModel
            {
                IsValid = _certificateRevocationListStoreService.DownloadedCertificates.Any(),
            };
        }
        else
        {
            result = new CheckResultWebModel
            {
                IsValid = false,
                Errors = errors.Select(error =>
                    new ErrorWebModel
                    {
                        Code = "0x0",
                        Message = error
                    }).ToList()
            };
        }

        await _certificateRevocationListStoreService.ClearCertFolderAsync();

        return result;
    }

    public bool ReInitLibrary()
    {
        try
        {
            NativeLibrary.SetDllImportResolver(Assembly.GetAssembly(typeof(AvNative))!, DllImportResolver);
            _stateManager.IsReInitLibrary = true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }

        return true;
    }

    public void SetState(bool isBusy) => _stateManager.IsBusy = isBusy;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool GetState() => _stateManager.IsBusy;

    /// <summary>
    /// 
    /// </summary>
    public void Shutdown() => AvNative.Shutdown();
}

class SimpleCrl
{
    /// <summary>
    /// 
    /// </summary>
    public string Subject { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; } = null!;
}