using RemoteDigitalSignature.Service.Abstractions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RemoteDigitalSignature.Service.Models;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Reflection;
using SignalRSwaggerGen.Attributes;

namespace RemoteDigitalSignature.Hubs;

/// <summary>
/// 
/// </summary>
[SignalRHub]
public class AvestHub : Hub
{
    private readonly IMainSignService _mainSignService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mainSignService"></param>
    public AvestHub(IMainSignService mainSignService)
    {
        _mainSignService = mainSignService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<CertificateResultWebModel>> Certs()
    {
        try
        {
            var result = await _mainSignService.GetCerts();

            return result;
        }
        catch (Exception ex)
        {
            var result = new List<CertificateResultWebModel>
                    {
                        new CertificateResultWebModel
                        {
                            IsValid = false,
                            Error = new ErrorWebModel
                            {
                                Code = "0x0",
                                Message = ex.Message
                            }
                        }
                    };

            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="publicKeyId"></param>
    /// <returns></returns>
    public async Task<CertificateResultWebModel?> Cert(string publicKeyId)
    {
        try
        {
            var result = await _mainSignService.GetCertAsync(publicKeyId);

            return result;
        }
        catch (Exception ex)
        {
            var result = new CertificateResultWebModel
            {
                IsValid = false,
                Error = new ErrorWebModel
                {
                    Code = "0x0",
                    Message = ex.Message
                }

            };

            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<CheckResultWebModel> CheckAvest()
    {
        try
        {
            var res = await _mainSignService.TryCheckAvestAsync();
            return res;
        }
        catch (Exception ex)
        {
            var res = new CheckResultWebModel
            {
                IsValid = false,
                Errors = new List<ErrorWebModel>{
                    new ErrorWebModel
                    {
                        Code = "0x0",
                        Message = ex.Message
                    }
                }
            };

            return res;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string? Version()
    {
        try
        {
            var flv = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return flv.ProductVersion;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="HubException"></exception>
    public async Task<IEnumerable<RevocationListCertificateResultWebModel>?> RevocationListCerts()
    {
        try
        {
            var result = await _mainSignService.GetRevocationListCerts();
            return result;
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("Error", ex.Message);
            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<CheckResultWebModel?> ImportRevocationListCerts()
    {
        try
        {
            var result = await _mainSignService.DownloadImportRevocationListCertificates();

            if (!result.IsValid)
            {
                return result;
            }

            return result;
        }
        catch (Exception ex)
        {
            var result = new CheckResultWebModel
            {
                IsValid = false,
                Errors = new List<ErrorWebModel>{
                    new ErrorWebModel
                    {
                        Code = "0x0",
                        Message = ex.Message
                    }
                }
            };

            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    public async Task<SignResultWebModel?> SignStringData(SignDataStringRequestWebModel sign)
    {
        try
        {
            var result = await _mainSignService.SignDataAsync(sign);
            return result;
        }
        catch (Exception ex)
        {
            var result = new SignResultWebModel
            {
                IsValid = false,
                Error = new ErrorWebModel
                {
                    Code = "0x0",
                    Message = ex.Message
                }
            };

            return result;
        }
    }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    public async Task<SignResultWebModel?> SignBytesData(SignDataByteRequestWebModel sign)
    {
        try
        {
            var result = await _mainSignService.SignDataAsync(sign);
            return result;
        }
        catch (Exception ex)
        {
            var result = new SignResultWebModel
            {
                IsValid = false,
                Error = new ErrorWebModel
                {
                    Code = "0x0",
                    Message = ex.Message
                }
            };

            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    public async Task<HashResultWebModel?> HashBytesData(SignDataByteRequestWebModel sign)
    {
        try
        {
            var result = await _mainSignService.GetDataHashAsync(sign);
            return result;
        }
        catch (Exception ex)
        {
            var result = new HashResultWebModel
            {
                IsValid = false,
                Error = new ErrorWebModel
                {
                    Code = "0x0",
                    Message = ex.Message
                }
            };

            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    public async Task<SignFileResultWebModel?> SignFile(SignFileRequestWebModel sign)
    {
        try
        {
            var result = await _mainSignService.SignFileAsync(sign);
            return result;
        }
        catch (Exception ex)
        {
            var result = new SignFileResultWebModel
            {
                IsValid = false,
                Error = new ErrorWebModel
                {
                    Code = "0x0",
                    Message = ex.Message
                }
            };

            return result;
        }
    }

    /// <summary>
    /// Подписание файла в формате XML
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    public async Task<XmlSignToXaDesWebModel> SignXmlFileInXmlDsig(SignFileRequestWebModel sign)
    {
        try
        {
            var result = await _mainSignService.SignXmlFileInXmlDsigAsync(sign);
            return result;
        }
        catch (Exception ex)
        {
            var result = new XmlSignToXaDesWebModel
            {
                IsValid = false,
                Error = new ErrorWebModel
                {
                    Code = "0x0",
                    Message = ex.Message
                }
            };

            return result;
        }
    }


    /// <summary>
    /// Верификация XML файла подписанного в XaDes
    /// </summary>
    /// <param name="verifyData"></param>
    /// <returns></returns>
    public async Task<XmlSignToXaDesWebModel> VerifySignXmlFileInXmlDsig(SignFileRequestWebModel verifyData)
    {
        try
        {
            var result = await _mainSignService.VerifySignXmlFileInXmlDsig(verifyData);
            return result;
        }
        catch (Exception ex)
        {
            var result = new XmlSignToXaDesWebModel
            {
                IsValid = false,
                Error = new ErrorWebModel
                {
                    Code = "0x0",
                    Message = ex.Message
                }
            };

            return result;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    public async Task<HashFileResultWebModel?> HashFile(SignFileRequestWebModel sign)
    {
        try
        {
            var result = await _mainSignService.GetFileHashAsync(sign);
            return result;
        }
        catch (Exception ex)
        {
            var result = new HashFileResultWebModel
            {
                IsValid = false,
                Error = new ErrorWebModel
                {
                    Code = "0x0",
                    Message = ex.Message
                }
            };

            return result;
        }
    }
}