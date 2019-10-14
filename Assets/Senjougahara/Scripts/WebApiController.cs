using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Senjougahara;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace Shejougahara
{
    public class WebApiController : MonoBehaviour
    {
        /// <summary>
        /// 显示Json字符串
        /// </summary>
        public Text textJson;
        /// <summary>
        /// 显示结果
        /// </summary>
        public Text textObj;
        /// <summary>
        /// 输入框
        /// </summary>
        public InputField input;

        /// <summary>
        /// 查找
        /// </summary>
        public void Search()
        {
            textJson.text = "start get...";
            textObj.text = "start get...";

            StartCoroutine(GetWeather());
        }
        /// <summary>
        /// 获取天气
        /// </summary>
        /// <returns></returns>
        IEnumerator GetWeather()
        {
            //开始api访问
            using (UnityWebRequest webRequest = new UnityWebRequest())
            {
                //网址
                webRequest.url = "http://apis.juhe.cn/simpleWeather/query?city="
                + input.text + "&key=053c5a42626e28e5ae01ea5ba5e28cf9";
                //访问方法
                webRequest.method = UnityWebRequest.kHttpVerbGET;
                //获取内容
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                yield return webRequest.SendWebRequest();

                #region Post提交
                // webRequest.url = "http://localhost:2572/api/account/login";
                // webRequest.method = UnityWebRequest.kHttpVerbPOST;

                // webRequest.SetRequestHeader("Cookie", cookie);

                // byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"Email\": \"" + inputLoginEmail.text + "\",\"password\": \"" + inputLoginPassword.text + "\",\"remeberme\": false }");
                // webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                // webRequest.SetRequestHeader("Content-Type", "application/json");
                #endregion


                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    //报错
                    textJson.text = webRequest.downloadHandler.text;
                    textObj.text = "";
                }
                else
                {
                    //正确时显示字符串，如果接收到的不是UTF-8需要转换
                    textJson.text = webRequest.downloadHandler.text;

                    //字符串转对象
                    Weather weather = JsonConvert.DeserializeObject<Weather>(webRequest.downloadHandler.text);
                    //显示结果
                    textObj.text = "查询城市：" + weather.result.city
                     + "；当前温度：" + weather.result.realtime.temperature
                     + "℃；天气情况：" + weather.result.realtime.info + "。";
                }
            }
        }
    }
}