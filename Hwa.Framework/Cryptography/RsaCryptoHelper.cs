using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hwa.Framework.Cryptography
{
    class RasKeyCache
    {
        public Hashtable CacheHashTable = Hashtable.Synchronized(new Hashtable());

        public RasKeyCache()
        {
            //从运营平台获取商户证书时，内部加解密
            CacheHashTable["Certificate"] = new RsaKeyInfo()
            {
                Modn = "258A46C981A4387967FC27B5A79D64F909882DEE2BAF5FC45C688B3175C431E9A4648DBF24927107AE8A0DD69748E1731BF9A1ED1E6B00558E9C04FA313D0C3D",
                PrivateKey = "2159487D8B7966CB751A8930895C86752F008F8195BC438560C2E48446D2BAF996BCE74DDA6045E3A670A142E7BDDEAB021F8C983DE7980256F9346970EB199D",
                PublicKey = "2EE0493361D3DA25A2B8EEA39F5569A5217BFBF14D71780A37B7024C27AA37832D84135CE11ECD31"
            };
            
            //本地配置文件字串加解密
            CacheHashTable["ConfigFile"] = new RsaKeyInfo()
            {
                Modn = "258A46C981A4387967FC27B5A79D64F909882DEE2BAF5FC45C688B3175C431E9A4648DBF24927107AE8A0DD69748E1731BF9A1ED1E6B00558E9C04FA313D0C3D",
                PrivateKey = "2159487D8B7966CB751A8930895C86752F008F8195BC438560C2E48446D2BAF996BCE74DDA6045E3A670A142E7BDDEAB021F8C983DE7980256F9346970EB199D",
                PublicKey = "2EE0493361D3DA25A2B8EEA39F5569A5217BFBF14D71780A37B7024C27AA37832D84135CE11ECD31"
            };

            //商户注册激活字串加解密
            CacheHashTable["Register"] = new RsaKeyInfo()
            {
                Modn = "51BF663A9C037CBE9B82ACE71FE87A515F2A4EA713C9EDCADA453A721731D2BB36FAD365B505094CC8F03AA818C6460C226478AC2E95FE0986E1DF64BC769689",
                PrivateKey = "200C4A4DD70548A74E5EF51AA3DF192730B8D92023BE8BA64B0FEE23AC9B86F65DDD41FF3C898DE9A9EE5388A96B9973EE016446D0A8E420370482E78973816D",
                PublicKey = "42EE5C6C9A01EFFFC18A8AFF3FB7CE215E86B5ADE7EDC7F7F0C79109E44A9"
            };

            //供应商平台字串加解密
            CacheHashTable["SupplyChain"] = new RsaKeyInfo()
            {
                Modn = "11E828CF83045F0E093EB97CBDCD91E01AE75908F9F5826C82BB8AB9845AEAAFCE253012F106EDBF7060DB8F2FBBCB04622AEB00F9CAAF4C3066A223F774D31",
                PrivateKey = "E7935C869F6CBFF9FE80D579FEE2FB3F52922F69113CCB87EADF76A79A589ACC70A5B555E972F803E435234F3A1E69B88DB2E32CC2DF380D4599AE366CF845",
                PublicKey = "75EDF54BBC4FB250880A2E9CAB370054CA5C2A4A14A11C3EA8D8AE84172C8C9839D4B85380F95729"
            };

//#if DEBUG
            #region  开发时，本地使用的PKV -----> zhangh 2014.08.26
            //PKV
            CacheHashTable["20130618"] = new RsaKeyInfo() 
            {
                Modn       = "258A46C981A4387967FC27B5A79D64F909882DEE2BAF5FC45C688B3175C431E9A4648DBF24927107AE8A0DD69748E1731BF9A1ED1E6B00558E9C04FA313D0C3D",
                PrivateKey = "2159487D8B7966CB751A8930895C86752F008F8195BC438560C2E48446D2BAF996BCE74DDA6045E3A670A142E7BDDEAB021F8C983DE7980256F9346970EB199D",
                PublicKey  = "2EE0493361D3DA25A2B8EEA39F5569A5217BFBF14D71780A37B7024C27AA37832D84135CE11ECD31"
            };

            //PKV
            CacheHashTable["20130619"] = new RsaKeyInfo() 
            { 
                Modn       = "3A7A841BFFE7D6EF9A5BB82889711E1178E6E1B6D89C35526621978882F26FB785B809BA5FC23C445E6649C25DB3FD0D39F3F8BBD8247694785B935E657ED1C9",
                PrivateKey = "38F100A426EFF93B91F19A69F970757237DFC2E1353A7B62D01C097C96DAA074CE18DD7C23238B6F1C6B98661F96B15FC15C29B741C976E278EB5B185AE441D1",
                PublicKey  = "7E1553D42A75BF2E7808102913859F38EA48FEFD14398426E63D9B4F7863046D8E9E642BE8C57B6860B4924382D235C6CED564A37558AB49"
            };
            #endregion
//#endif

            //天店助手使用
            CacheHashTable["20140822"] = new RsaKeyInfo()
            {
                Modn = "C8E0313EF65284718F49E76A0FFCC2BFE07BE4047589A6C3A1F6F124E0021494D89E4C8130963F8AA54A8649C2635F14A4A6472B7A667F9163D3BFC88530BD79",
                PrivateKey = "406F5529F0BA2F9773FBE605F72536C62B5FD7DC29172CDAA46487EFD3EC893D63990976D1AC267A17EDBCE7CCD86CB4BDE8CB2083A5F9FBF1A6688FCE4B15D",
                PublicKey = "F27CB15CACE358859E806CA72C4E7756144E53063B529F0B6A335BBF49D1"
            };

            //当前Android端POS在使用
            CacheHashTable["20140823"] = new RsaKeyInfo()
            {
	            Modn = "D12D65B068740BC39C3999C402262FAEBA2799E72B4C99921366F9733B72E3BF34E8CD2EB0146F9404BF0E7C3185EFCC8FD97E562DEA350F763C91DD3D37319",
	            PrivateKey = "5AC554ABAE23814FAB8982B6D272F2D74D3D2154E19E9A6B3DEFDB9A8634A8569F40B061C10539A47ACBE00482A07C248FC6BDDE467CE28A77322F1423BCBB5",
	            PublicKey="6A59B75F4603BAD26F14B5DDAA37F7A8B76D279C994C2A5785FF99155F943C153CB3A3444B6E4DD"
            };

            //当前PC端POS在使用
            CacheHashTable["20140824"] = new RsaKeyInfo()
            {
                Modn = "4E504AEDE140E42BDEA03DCAA08C5CA9B4361B11828EEE43EF451F50992591AE3C3D4BE3AB495F8432795E25DE44534C526A0BCFD5A1FCEAC314D2BCEEC628F9",
                PrivateKey = "A1C0404BCACB6EF72A32B194CC16E8DB5BA1F21D5DE8EC9A2EA2F2744DD9F87AABA01ABC72DFBB6F79C9AD6D80CA5D81D231BF734364E955A69E8AC6273D7BD",
                PublicKey = "121F7E9FCB628D3F06DFA9386C66AE2156F98E2F1CF8D4952A3E6F68144E5EDCA6CA0B054F2D89948439A11A27E565A9"
            };

            //当前Win8端在使用
            CacheHashTable["20141017"] = new RsaKeyInfo()
            {
                Modn = "31C094726F5464900EA96405C73CBCB6FB013BD4BB69754556A5BCDDA9D27EEA59720E521BB72C8AA687AA3FFFF4C417D79C489ED9A7743CD71833B5B71CA649",
                PrivateKey = "2A78966D98A407BD0CB2608153BE2C40D73D0E102AF9ED938125DCB9AAA48D47F05FD75A796BE0D3FF62FB9E22689E182C4F4488FDE00A9A6ABF8DCCADF2C5D5",
                PublicKey = "45DEB064346AD6989E46A95D5C46AC9859C33B5C71E214A92D7D550F7265F71"
            };
            
            CacheHashTable["20141018"] = new RsaKeyInfo()
            {
                Modn = "1D41E24DC35EABE5BC355291F485152548DC97D568DA63C81986339E0DD45EDFCB4D8182AB7CFCB95B9E211C02DE014CA99DBDDEF8BF4A206EDB64DB1CFA3E1D",
                PrivateKey = "14C2E9C58CB3D1F4914E61F35AA5FA72C0FCF22F19E98AE7CAB541071A60BD2EA9AA3506FDF6B22007A2EAE0FBDA3CD0F7E4CFDB3EEE232430E35BCCC17FD5B1",
                PublicKey = "291179C5EBE654075C930309252764F3D9A5F16FC3097345C19B9D328EBCA21"
            };

        }
    }

    public class RsaCryptoHelper
    {
        /// <summary>
        /// 生成公私密钥对
        /// </summary>
        [DllImport("libcloud.dll", CharSet = CharSet.Ansi)]
        private static extern bool createRsaKeyPair(
            StringBuilder lpszPriKey, Int32 dwSizePriKey,
            StringBuilder lpszPubKey, Int32 dwSizePubKey,
            StringBuilder lpszModN, Int32 dwSizeModN);

        /// <summary>
        /// 用公钥或私加密
        /// </summary>
        [DllImport("libcloud.dll", CharSet = CharSet.Ansi)]
        private static extern bool RsaEncrypt(string lpszKey, string lpszModN,
                                         ref byte lpbData, Int32 dwDataLength,
                                         StringBuilder lpszCipherText, Int32 dwCipherTextSize);

        /// <summary>
        /// 用公钥或私解密
        /// </summary>
        [DllImport("libcloud.dll", CharSet = CharSet.Ansi)]
        private static extern bool RsaDecrypt(string lpszKey, string lpszModN,
                                         string lpszCipherText, ref byte lpbData, ref Int32 dwDataSize);

        private static RasKeyCache rkcKeysCache = new RasKeyCache();

        /// <summary>
        /// 创建公私密钥对
        /// </summary>
        public static RsaKeyInfo CreateKeyPair()
        {
            //try
            //{
                StringBuilder sbPriKey = new StringBuilder(1024);
                StringBuilder sbPubKey = new StringBuilder(1024);
                StringBuilder sbModN = new StringBuilder(1024);
                
                bool rtn = createRsaKeyPair(sbPriKey, 1024, sbPubKey, 1024, sbModN, 1024);
                if (!rtn) throw new ApplicationException("生成RSA密钥对失败!");
                
                RsaKeyInfo key = new RsaKeyInfo();

                key.Modn = sbModN.ToString();
                key.PublicKey = sbPubKey.ToString();
                key.PrivateKey = sbPriKey.ToString();

                return key;
            //}
            //catch (Exception ex)
            //{
            //}
            //return null;
        }

        /// <summary>
        /// 密钥版本是否存在
        /// </summary>
        /// <param name="PKV"></param>
        /// <returns></returns>
        public static bool ExistsKey(string PKV)
        {
            try
            {
                return rkcKeysCache.CacheHashTable.ContainsKey(PKV);
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        /// RSA私钥解密
        /// </summary>
        /// <param name="PKV">密钥版本</param>
        /// <param name="lpszCipherText">密文</param>
        /// <returns></returns>
        public static string Decrypt(string PKV, string lpszCipherText)
        {
            RsaKeyInfo rsaKey = rkcKeysCache.CacheHashTable[PKV] as RsaKeyInfo;
            if (rsaKey == null) throw new ApplicationException("PKV已失效,请您升级更新程序!");

            if (lpszCipherText.Length % 128 != 0 || string.IsNullOrEmpty(lpszCipherText))
            {
                throw new ApplicationException("数据解密失败!");
            }

            int dwDataSize = lpszCipherText.Length / 128 * 64;
            byte[] byteData = new byte[dwDataSize];

            if (!RsaDecrypt(rsaKey.PrivateKey, rsaKey.Modn, lpszCipherText, ref byteData[0], ref dwDataSize))
            {
                throw new ApplicationException("数据解密失败!");
            }

            string data = Encoding.UTF8.GetString(byteData, 0, dwDataSize);
            data = data.TrimEnd('\0');
            return data;
        }

        /// <summary>
        /// 使用指定的KEY和模数加密字符串
        /// </summary>
        public static string EncryptString(string Modn, string Key, string data)
        {
            byte[] lpbData = Encoding.UTF8.GetBytes(data);
            Int32 dwDataSize = (lpbData.Length / 60 + 1) * 128 + 1;
            StringBuilder sbText = new StringBuilder(dwDataSize);

            if (!RsaEncrypt(Key, Modn, ref lpbData[0], lpbData.Length, sbText, dwDataSize))
            {
                throw new ApplicationException("加密数据失败!");
            }

            return sbText.ToString();
        }


        /// <summary>
        /// RSA解密字串
        /// </summary>
        /// <param name="Modn">模数</param>
        /// <param name="Key">私钥/公钥</param>
        /// <param name="lpszCipherText"></param>
        /// <returns></returns>
        public static string DecryptString(string Modn, string Key, string lpszCipherText)
        {
            if (string.IsNullOrEmpty(Modn) || string.IsNullOrEmpty(Key))
                return string.Empty;

            if (lpszCipherText.Length % 128 != 0 || string.IsNullOrEmpty(lpszCipherText))
                return string.Empty;

            int dwDataSize = lpszCipherText.Length / 128 * 64;
            byte[] byteData = new byte[dwDataSize];

            if (RsaDecrypt(Key, Modn, lpszCipherText, ref byteData[0], ref dwDataSize))
            {
                string data = Encoding.UTF8.GetString(byteData, 0, dwDataSize);
                return data.TrimEnd('\0');
            }

            return string.Empty;
        }
        
        /// <summary>
        /// 使用PKV版本的私钥加密
        /// </summary>
        /// <param name="PKV"></param>
        /// <param name="data"></param>
        /// <param name="lpszCipherText"></param>
        /// <returns>0-成功,1-失败,2-密钥版本PKV过期(不存在)</returns>
        public static string Encrypt(string PKV, string data)
        {
            RsaKeyInfo rsaKey = rkcKeysCache.CacheHashTable[PKV] as RsaKeyInfo;
            if (rsaKey == null) throw new ApplicationException("PKV已失效,请您升级更新程序!");

            if (data == null || data.Length == 0)
            {
                throw new ApplicationException("加密数据为空!");
            }

            byte[] lpbData = Encoding.UTF8.GetBytes(data);
            Int32 dwDataSize = (lpbData.Length / 60 + 1) * 128 + 1;
            StringBuilder sbCipherText = new StringBuilder(dwDataSize);

            if (!RsaEncrypt(rsaKey.PrivateKey, rsaKey.Modn, ref lpbData[0], lpbData.Length, sbCipherText, dwDataSize))
            {
                throw new ApplicationException("加密数据失败!");
            }

            //lpszCipherText = sbCipherText.ToString();

            return sbCipherText.ToString();
        }
                
        /// <summary>
        /// 命名用PKV的私钥来加密用户的独立密钥
        /// </summary>
        public static string EncryptUserKey(string PKV, string Modn, string Key)
        {
            DateTime dt2013 = new DateTime(2013, 1, 1);
            TimeSpan ts = DateTime.Now - dt2013; //无需时区处理
            string encryptData = string.Format("000000{0:x}", (Int64)(ts.TotalMilliseconds * 1000));
            encryptData = encryptData.Substring(encryptData.Length - 6); //前置6位随机
            encryptData = encryptData + Modn.PadLeft(128, '0') + Key.PadLeft(128, '0');
            return Encrypt(PKV, encryptData);
        }

        public static string DecryptTenantCode(string PKV, string TenantCode)
        {
            if (string.IsNullOrEmpty(TenantCode))
            {
                throw new ApplicationException("TenantCode值为不能为空!");
            }

            string tmpTenantCode = Decrypt(PKV, TenantCode);

            string[] tmpArray = tmpTenantCode.Split('\t');

            if (tmpArray.Length != 2)
            {
                throw new ApplicationException("请求数据(TenantCode)异常!");
            }

            return tmpArray[1];
        }
                
        public static void DecryptSessoinKey(string tenantModn, string tenantPriKey, string SessionKey, ref string LoginCode, ref string OperatorCode, ref long OperatorCheck)
        {
            LoginCode = string.Empty;
            OperatorCode = string.Empty;
            OperatorCheck = 0;

            if (string.IsNullOrEmpty(SessionKey))
            {
                throw new ApplicationException("SessoinKey值为不能为空!");
            }

            string tempString = DecryptString(tenantModn, tenantPriKey, SessionKey);

            byte[] tempArray = Encoding.Default.GetBytes(tempString);
            foreach (byte b in tempArray)
            {
                if (!(char.IsLetterOrDigit((char)b) || (b == '\t') || (b == '_') || (b == '-') || (b == '@') || (b == '.')))
                {
                   throw new ApplicationException("请求数据(SessoinKey)异常#1!");
                }
            }

            string[] dataArray = tempString.Split('\t'); //随机数 + 登录验证码 + 操作员Code
            if (dataArray.Length < 3)
            {
                throw new ApplicationException("请求数据(SessoinKey)异常#2!");
            }
            else
            {
                try
                {
                    OperatorCheck = Convert.ToInt64(dataArray[0], 16);
                }
                catch { }
                LoginCode = dataArray[1];
                OperatorCode = dataArray[2];
            }
        }

        #region  加解密系统配置文件字串

        /// <summary>
        /// 加密系统配置文件字串
        /// </summary>
        public static string EncryptConfigString(string str)
        {
            return Encrypt("ConfigFile", str);
        }
        
        /// <summary>
        /// 解密系统配置文件字串
        /// </summary>
        public static string DecryptConfigString(string str)
        {
            const string PKV = "ConfigFile";

            RsaKeyInfo rsaKey = rkcKeysCache.CacheHashTable[PKV] as RsaKeyInfo;
            if (rsaKey == null) throw new ApplicationException("PKV已失效,请您升级更新程序!");

            if (str.Length % 128 != 0 || string.IsNullOrEmpty(str))
            {
                throw new ApplicationException("数据解密失败!");
            }

            int dwDataSize = str.Length / 128 * 64;
            byte[] byteData = new byte[dwDataSize];

            if (!RsaDecrypt(rsaKey.PublicKey, rsaKey.Modn, str, ref byteData[0], ref dwDataSize))
            {
                throw new ApplicationException("数据解密失败!");
            }

            string data = Encoding.UTF8.GetString(byteData, 0, dwDataSize);
            data = data.TrimEnd('\0');
            return data;
        }

        #endregion

        #region 加解密商户证书字串

        /// <summary>
        /// 加密商户证书字串
        /// </summary>
        public static string EncryptCertificateString(string str)
        {
            return Encrypt("Certificate", str);
        }

        /// <summary>
        /// 解密商户证书字串
        /// </summary>
        public static string DecryptCertificateString(string str)
        {
            const string PKV = "Certificate";

            RsaKeyInfo rsaKey = rkcKeysCache.CacheHashTable[PKV] as RsaKeyInfo;
            if (rsaKey == null) throw new ApplicationException("PKV已失效,请您升级更新程序!");

            if (str.Length % 128 != 0 || string.IsNullOrEmpty(str))
            {
                throw new ApplicationException("数据解密失败!");
            }

            int dwDataSize = str.Length / 128 * 64;
            byte[] byteData = new byte[dwDataSize];

            if (!RsaDecrypt(rsaKey.PublicKey, rsaKey.Modn, str, ref byteData[0], ref dwDataSize))
            {
                throw new ApplicationException("数据解密失败!");
            }

            string data = Encoding.UTF8.GetString(byteData, 0, dwDataSize);
            data = data.TrimEnd('\0');
            return data;
        }

        #endregion

        #region 加解密注册激活字串

        /// <summary>
        /// 加密商户证书字串
        /// </summary>
        public static string EncryptRegisterString(string str)
        {
            return Encrypt("Register", str);
        }

        /// <summary>
        /// 解密商户证书字串
        /// </summary>
        public static string DecryptRegisterString(string str)
        {
            const string PKV = "Register";

            RsaKeyInfo rsaKey = rkcKeysCache.CacheHashTable[PKV] as RsaKeyInfo;
            if (rsaKey == null) throw new ApplicationException("PKV已失效,请您升级更新程序!");

            if (str.Length % 128 != 0 || string.IsNullOrEmpty(str))
            {
                throw new ApplicationException("数据解密失败!");
            }

            int dwDataSize = str.Length / 128 * 64;
            byte[] byteData = new byte[dwDataSize];

            if (!RsaDecrypt(rsaKey.PublicKey, rsaKey.Modn, str, ref byteData[0], ref dwDataSize))
            {
                throw new ApplicationException("数据解密失败!");
            }

            string data = Encoding.UTF8.GetString(byteData, 0, dwDataSize);
            data = data.TrimEnd('\0');
            return data;
        }

        #endregion

        #region 加解密供应商平台字串

        /// <summary>
        /// 加密商户证书字串
        /// </summary>
        public static string EncryptSupplyChainString(string str)
        {
            return Encrypt("SupplyChain", str);
        }

        /// <summary>
        /// 解密商户证书字串
        /// </summary>
        public static string DecryptSupplyChainString(string str)
        {
            const string PKV = "SupplyChain";

            RsaKeyInfo rsaKey = rkcKeysCache.CacheHashTable[PKV] as RsaKeyInfo;
            if (rsaKey == null) throw new ApplicationException("PKV已失效,请您升级更新程序!");

            if (str.Length % 128 != 0 || string.IsNullOrEmpty(str))
            {
                throw new ApplicationException("数据解密失败!");
            }

            int dwDataSize = str.Length / 128 * 64;
            byte[] byteData = new byte[dwDataSize];

            if (!RsaDecrypt(rsaKey.PublicKey, rsaKey.Modn, str, ref byteData[0], ref dwDataSize))
            {
                throw new ApplicationException("数据解密失败!");
            }

            string data = Encoding.UTF8.GetString(byteData, 0, dwDataSize);
            data = data.TrimEnd('\0');
            return data;
        }

        #endregion
    }
}


