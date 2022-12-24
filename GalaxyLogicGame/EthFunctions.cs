﻿using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyLogicGame
{
    internal class EthFunctions
    {
        public static string GetAtomicBombContractAddress => "0x8F4B20a030401369B2e051829F2B81afACC3d0Bc";

        public static string GetEthereumProvider => "https://goerli.optimism.io";

        public static async Task<bool> CheckAtomicBombNFTOwnership(string pubKey)
        {
            bool ownership = 0 < await new Web3(GetEthereumProvider).Eth.ERC721.GetContractService(GetAtomicBombContractAddress).BalanceOfQueryAsync(pubKey);
            if (ownership) Preferences.Set(GetAtomicBombContractAddress, ownership);
            return ownership;
        }
    }
}
