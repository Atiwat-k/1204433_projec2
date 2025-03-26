// // <auto-generated />
// //
// // To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
// //
// //    using MauiApp1.Model;
// //
// //    var user = User.FromJson(jsonString);

// namespace MauiApp1.Model
// {
//     using System;
//     using System.Collections.Generic;

//     using System.Globalization;
//     using Newtonsoft.Json;
//     using Newtonsoft.Json.Converters;

//     public partial class User
//     {
//         [JsonProperty("id")]
//         public long Id { get; set; }

//         [JsonProperty("name")]
//         public string Name { get; set; }

//         [JsonProperty("username")]
//         public string Username { get; set; }

//         [JsonProperty("email")]
//         public string Email { get; set; }

//         [JsonProperty("address")]
//         public Address Address { get; set; }

//         [JsonProperty("phone")]
//         public string Phone { get; set; }

//         [JsonProperty("website")]
//         public string Website { get; set; }

//         [JsonProperty("company")]
//         public Company Company { get; set; }
//     }

//     public partial class Address
//     {
//         [JsonProperty("street")]
//         public string Street { get; set; }

//         [JsonProperty("suite")]
//         public string Suite { get; set; }

//         [JsonProperty("city")]
//         public string City { get; set; }

//         [JsonProperty("zipcode")]
//         public string Zipcode { get; set; }

//         [JsonProperty("geo")]
//         public Geo Geo { get; set; }
//     }

//     public partial class Geo
//     {
//         [JsonProperty("lat")]
//         public string Lat { get; set; }

//         [JsonProperty("lng")]
//         public string Lng { get; set; }
//     }

//     public partial class Company
//     {
//         [JsonProperty("name")]
//         public string Name { get; set; }

//         [JsonProperty("catchPhrase")]
//         public string CatchPhrase { get; set; }

//         [JsonProperty("bs")]
//         public string Bs { get; set; }
//     }

//     public partial class User
//     {
//         public static List<User> FromJson(string json) => JsonConvert.DeserializeObject<List<User>>(json, MauiApp1.Model.Converter.Settings);
//     }

//     public static class Serialize
//     {
//         public static string ToJson(this List<User> self) => JsonConvert.SerializeObject(self, MauiApp1.Model.Converter.Settings);
//     }

//     internal static class Converter
//     {
//         public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
//         {
//             MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
//             DateParseHandling = DateParseHandling.None,
//             Converters =
//             {
//                 new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
//             },
//         };
//     }
// }
