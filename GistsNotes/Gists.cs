using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Newtonsoft.Json;
using RestSharp;
using Method = RestSharp.Method;
using Object = Java.Lang.Object;

namespace GistsNotes
{
    public static class Gists
    {
        private static IRestResponse _response;
        private static RestClient _restClient;

        private static TaskCompletionSource<IRestResponse> _tcs;
        private const string BaseUrl = "http://api.github.com:80";

        public static List<GistPreview> GetPublicGists()
        {
            var p = new Parameter
            {
                Name = "per_page",
                Value = 500
            };

            var res = ExecuteRequest(BaseUrl, "/gists/public", Method.GET, p)
                .Content;

            return JsonConvert.DeserializeObject<List<GistPreview>>(res);
        }

        public static DetailedGist GetFullInfo(this GistPreview gp)
        {
            var res = ExecuteRequest(BaseUrl, "/gists/" + gp.Id, Method.GET, null);
            if (res.StatusCode == HttpStatusCode.OK)
            {
                var gist = JsonConvert.DeserializeObject<DetailedGist>(res.Content);
                gist.Img = GetImg(gist);
                gist.Notes = gp.Notes;
                return gist;
            }
            return null;
        }

        private static IRestResponse ExecuteRequest(string baseUrl, string reqest, Method method, Parameter parametres)
        {
            _restClient = new RestClient(baseUrl) { PreAuthenticate = true };

            _tcs = new TaskCompletionSource<IRestResponse>();

            var req = new RestRequest(reqest, method)
            {
                OnBeforeDeserialization = resp =>
                {
                    var byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                    if (resp.Content.StartsWith(byteOrderMarkUtf8))
                        resp.Content = resp.Content.Remove(0, byteOrderMarkUtf8.Length);
                },
                UseDefaultCredentials = true
            };

            req.AddHeader("User-Agent", "Foo");
            req.AddHeader("Accept", "*/*");

            if (parametres != null)
                req.AddParameter(parametres);

            Task.Run(async () =>
            {
                _response = await GetResponseContentAsync(_restClient, req);
            }).Wait();

            return _response;
        }

        private static Bitmap GetImg(this DetailedGist dg)
        {
            try
            {
                var uri = new Uri(dg.History[0].User.AvatarUrl);
                var op = uri.GetLeftPart(UriPartial.Authority);
                _restClient = new RestClient(op);
                var req = new RestRequest(dg.History[0].User.AvatarUrl.Remove(0, op.Length), Method.GET);
                var img = _restClient.Execute(req).RawBytes;
                var bmp = BitmapFactory.DecodeByteArray(img, 0, img.Length);

                return bmp;
            }
            catch (Exception e)
            {
                return null;
            }
          
        }

        private static Task<IRestResponse> GetResponseContentAsync(IRestClient theClient, IRestRequest theRequest)
        {
            theClient.ExecuteAsync(theRequest, response =>
            {
                _tcs.SetResult(response);
            });
            return _tcs.Task;
        }
    }
}