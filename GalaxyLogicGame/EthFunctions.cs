using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyLogicGame
{
    internal class EthFunctions
    {

        public static string GetEthereumContractAddress => "0xA6139A9Bf36c7b3cEA9D6E70eEACa49a3156A402";

        public static string GetEthereumProvider => "https://optimism-kovan.infura.io/v3/d55a983e95774bd9abbaeb2ba7e8f275";
        public static async Task<bool> CheckNFTOwnership(string pubKey) => 0 < await new Web3(GetEthereumProvider).Eth.ERC721.GetContractService(GetEthereumContractAddress).BalanceOfQueryAsync(pubKey);
    }
}
